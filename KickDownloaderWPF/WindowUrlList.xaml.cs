using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TwitchDownloaderCore;
using TwitchDownloaderCore.TwitchObjects.Gql;
using KickDownloaderWPF.Properties;
using KickDownloaderWPF.TwitchTasks;
using TwitchDownloaderCore.TwitchObjects.Api;
using static KickDownloaderWPF.App;
using Clip = TwitchDownloaderCore.TwitchObjects.Gql.Clip;

namespace KickDownloaderWPF
{
    /// <summary>
    /// Interaction logic for WindowUrlList.xaml
    /// </summary>
    public partial class WindowUrlList : Window
    {
        public WindowUrlList()
        {
            InitializeComponent();
        }

        private async void btnQueue_Click(object sender, RoutedEventArgs e)
        {
            btnQueue.IsEnabled = false;
            List<string> idList = new List<string>();
            List<string> urlList = new List<string>(textList.Text.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
            List<string> invalidList = new List<string>();
            List<string> errorList = new List<string>();
            List<TaskData> dataList = new List<TaskData>();
            Dictionary<string, string> idDict = new Dictionary<string, string>();

            foreach (var url in urlList)
            {
                string id = PageVodDownload.ValidateUrl(url) ;
                if (id == null)
                {
                    id = PageClipDownload.ValidateUrl(url);
                }
                if (id == "")
                {
                    invalidList.Add(url);
                }
                else
                {
                    idList.Add(id);
                    idDict[id] = url;
                }
            }

            if (invalidList.Count > 0)
            {
                MessageBox.Show(Translations.Strings.UnableToParseInputsMessage + Environment.NewLine + string.Join(Environment.NewLine, invalidList.ToArray()), Translations.Strings.UnableToParseInputs, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dictionary<int, string> taskDict = new Dictionary<int, string>();
            List<Task<VideoResponse>> taskVideoList = new List<Task<VideoResponse>>();
            List<Task<ClipsResponse>> taskClipList = new List<Task<ClipsResponse>>();

            foreach (var id in idList)
            {
                if (!id.All(Char.IsNumber))
                {
                    Task<VideoResponse> task = KickHelper.GetVideoInfo(id);
                    taskVideoList.Add(task);
                    taskDict[task.Id] = id;
                }
                else
                {
                    Task<ClipsResponse> task = KickHelper.GetClipInfo(id);
                    taskClipList.Add(task);
                    taskDict[task.Id] = id;
                }
            }

            try
            {
                await Task.WhenAll(taskVideoList);
            }
            catch { }
            try
            {
                await Task.WhenAll(taskClipList);
            }
            catch { }

            for (int i = 0; i < taskVideoList.Count; i++)
            {
                if (taskVideoList[i].IsCompleted)
                {
                    string id = taskDict[taskVideoList[i].Id];
                    if (!taskVideoList[i].IsFaulted)
                    {
                        VideoResponse data = taskVideoList[i].Result;
                        TaskData newData = new TaskData();
                        newData.Id = id;
                        try
                        {
                            string thumbUrl = data.livestream.thumbnail;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.UriSource = new Uri(thumbUrl);
                            bitmapImage.EndInit();
                            newData.Thumbnail = bitmapImage;
                        }
                        catch { }
                        newData.Title = data.livestream.session_title;
                        newData.Streamer = data.livestream.channel.username;
                        newData.Time = Settings.Default.UTCVideoTime ? data.created_at : data.created_at.ToLocalTime();
                        dataList.Add(newData);
                    }
                    else
                    {
                        errorList.Add(idDict[id]);
                    }
                }
            }

            for (int i = 0; i < taskClipList.Count; i++)
            {
                if (taskClipList[i].IsCompleted)
                {
                    string id = taskDict[taskClipList[i].Id];
                    if (!taskClipList[i].IsFaulted)
                    {
                        ClipsResponse data = taskClipList[i].Result;
                        TaskData newData = new TaskData();
                        newData.Id = id;
                        try
                        {
                            string thumbUrl = data.clip.thumbnail_url;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.UriSource = new Uri(thumbUrl);
                            bitmapImage.EndInit();
                            newData.Thumbnail = bitmapImage;
                        }
                        catch { }
                        newData.Title = data.clip.title;
                        newData.Streamer = data.clip.channel.username;
                        newData.Time = Settings.Default.UTCVideoTime ? data.clip.created_at : data.clip.created_at.ToLocalTime();
                        dataList.Add(newData);
                    }
                    else
                    {
                        errorList.Add(idDict[id]);
                    }
                }
            }

            if (errorList.Count > 0)
            {
                MessageBox.Show(Translations.Strings.UnableToGetInfoMessage + Environment.NewLine + string.Join(Environment.NewLine, errorList.ToArray()), Translations.Strings.UnableToGetInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WindowQueueOptions queue = new WindowQueueOptions(dataList);
            bool? queued = queue.ShowDialog();
            if (queued != null && (bool)queued)
                this.Close();

            btnQueue.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
		{
            Title = Translations.Strings.TitleUrlList;
			AppSingleton.RequestTitleBarChange();
		}
    }
}
