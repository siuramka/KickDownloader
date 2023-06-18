# Kick Vod Downloader

# This is a fork of the lay295/KickDownloader

I have noticed that there is currently no reliable method to download VODs from Kick.com. As a result, I have created a solution to address this issue.

To overcome the Cloudflare bot protection implemented by Kick.com, I have used Puppeteer Chromium automation for API calls. Initially, I attempted to use cookies copied from the first request in subsequent requests. However, I encountered difficulties in making TLS and Fingerprinting work, both in C# and Golang. Despite Golang offering advanced features for this purpose, I was unable to bypass the bot protection using cookies alone.

To tackle this problem, I utilized Puppeteer stealth plugins in conjunction with Puppeteer itself. In theory, these plugins should enable bypassing Cloudflare's bot protection even in headless mode. However, in practice, I discovered that if I made multiple requests to the same URL, it would still trigger Cloudflare's bot protection and result in being flagged. The solution only works when making Puppeteer requests to different pages.

Due to these limitations, the current implementation uses the **non-headless mode** of Chromium to make all requests to the Kick API.
***This is why you will see browser popups***

If Kick introduces proper API support in the future, this repository will be updated accordingly.

Credits go to lay295 and collaborators for the KickDownloader code.

## Currently only VOD downloading works wia KickDownloaderWPF.
## In Progress
- CLI
- Clip download

Since kick doesnt save chats, no chat feature will be available.

#
#
#
#
#
#
