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

        public static async Task<PuppeteerHttpService> CreateAsync(string? baseUrl = "", LaunchOptions? launchOptions = null)
        {
            var browserFetcher = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            return new PuppeteerHttpService(launchOptions ?? new LaunchOptions {Headless = true}, baseUrl);
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
                var navigation = new NavigationOptions
                {
                    WaitUntil = new[] {
                    WaitUntilNavigation.DOMContentLoaded }
                };
                await page.GoToAsync(_baseUrl + url, navigation);
                
                string content = await page.EvaluateExpressionAsync<string>("document.body.innerText"); //get inner text/text as puppeteer serializes html content

                await page.DisposeAsync();
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

                string content = await page.EvaluateFunctionAsync<string>("() => document.documentElement.textContent");

                await page.DisposeAsync();
                await browser.DisposeAsync();

                return content;
            }
        }
        
    }