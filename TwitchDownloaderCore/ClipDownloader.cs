using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TwitchDownloaderCore.Options;
using TwitchDownloaderCore.Tools;
using TwitchDownloaderCore.TwitchObjects.Api;
using TwitchDownloaderCore.TwitchObjects.Gql;

namespace TwitchDownloaderCore
{
    public sealed class ClipDownloader
    {
        private readonly ClipDownloadOptions downloadOptions;
        private static HttpClient httpClient = new HttpClient();

        public ClipDownloader(ClipDownloadOptions DownloadOptions)
        {
            downloadOptions = DownloadOptions;
        }

        public async Task DownloadAsync(CancellationToken cancellationToken = new())
        {
            ClipsResponse clipInfo  = await KickHelper.GetClipInfo(downloadOptions.Id);

            string downloadUrl = clipInfo.clip.video_url;
            cancellationToken.ThrowIfCancellationRequested();

            var clipDirectory = Directory.GetParent(Path.GetFullPath(downloadOptions.Filename))!;
            if (!clipDirectory.Exists)
            {
                KickHelper.CreateDirectory(clipDirectory.FullName);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (downloadOptions.ThrottleKib == -1)
            {
                await using var fs = new FileStream(downloadOptions.Filename, FileMode.Create, FileAccess.Write, FileShare.Read);
                await response.Content.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await using var throttledStream = new ThrottledStream(await response.Content.ReadAsStreamAsync(cancellationToken), downloadOptions.ThrottleKib);
                await using var fs = new FileStream(downloadOptions.Filename, FileMode.Create, FileAccess.Write, FileShare.Read);
                await throttledStream.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
