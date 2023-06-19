using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace TwitchDownloaderCore.Tools;
public class PuppeteerHttpService
    {
        private LaunchOptions _launchOptions;
        private PuppeteerExtra _puppeteerExtra;
        private string _baseUrl;

        private PuppeteerHttpService(LaunchOptions launchOptions, string baseUrl)
        {
            _baseUrl = baseUrl;
            _launchOptions = launchOptions;
            _puppeteerExtra = new PuppeteerExtra();
            this.ConfigPuppeteer();
        }
        private void ConfigPuppeteer()
        {
            _puppeteerExtra.Use(new StealthPlugin());
        }

        public static async Task<PuppeteerHttpService> CreateAsync(string? baseUrl = "", LaunchOptions? launchOptions = null, IProgress<ProgressReport>?  progress = null)
        {
            var browserFetcher = new BrowserFetcher();
            if (!IsChromiumInstalled(BrowserFetcher.DefaultChromiumRevision))
            {
                progress.Report(new ProgressReport(ReportType.SameLineStatus, "Downloading chromium [1/2]"));
                await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
                progress.Report(new ProgressReport(ReportType.SameLineStatus, "Downlaoded chromium! [2/2]"));
                progress.Report(new ProgressReport() { ReportType = ReportType.Percent, Data = 100 });

            }

            return new PuppeteerHttpService(launchOptions ?? new LaunchOptions { Headless = true }, baseUrl);
        }

        private static bool IsChromiumInstalled(string revision)
        {
            var browserFetcher = new BrowserFetcher();
            var path = browserFetcher.GetExecutablePath(revision);
            return File.Exists(path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Since going to use only get requests, use simple url param.</param>
        /// <returns></returns>
        public async Task<T> GetJsonAsync<T>(string url)
        {
            // Launch the puppeteer browser with plugins
            using (IBrowser browser = await _puppeteerExtra.LaunchAsync(_launchOptions))
            {
                var page = await browser.NewPageAsync();
                
                var ua = await page.Browser.GetUserAgentAsync();
                ua = ua.Replace("HeadlessChrome", "Chrome");

                var regex = new Regex(@"/\(([^)]+)\)/");
                ua = regex.Replace(ua, "(Windows NT 10.0; Win64; x64)");

                await page.SetUserAgentAsync(ua);
                
                var navigation = new NavigationOptions
                {
                    WaitUntil = new[] {
                    WaitUntilNavigation.DOMContentLoaded }
                };
                
                await page.GoToAsync(_baseUrl + url, navigation);
                
                string content = await page.EvaluateExpressionAsync<string>("document.body.innerText"); //get inner text/text as puppeteer serializes html content

                // await browser.CloseAsync();
                // await page.DisposeAsync();
                await browser.DisposeAsync();
                var jsonObject = JsonConvert.DeserializeObject<T>(content);

                return jsonObject;
            }
        }
        public async Task<string> GetTextAsync(string url)
        {
            // Launch the puppeteer browser with plugins
            using (IBrowser browser = await _puppeteerExtra.LaunchAsync(_launchOptions))
            {
                var page = await browser.NewPageAsync();
                await page.GoToAsync(_baseUrl + url);
                
                var ua = await page.Browser.GetUserAgentAsync();
                ua = ua.Replace("HeadlessChrome", "Chrome");

                var regex = new Regex(@"/\(([^)]+)\)/");
                ua = regex.Replace(ua, "(Windows NT 10.0; Win64; x64)");

                await page.SetUserAgentAsync(ua);

                string content = await page.EvaluateFunctionAsync<string>("() => document.documentElement.textContent");

                await page.DisposeAsync();
                await browser.DisposeAsync();

                return content;
            }
        }
        
    }