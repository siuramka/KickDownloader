# Kick Vod Downloader

# This is a fork of the lay295/TwitchDownloader

About: 
I have noticed that there is currently no reliable method to download VODs from Kick.com. As a result, I have created a solution to address this issue.

To overcome the Cloudflare bot protection implemented by Kick.com, I have used Puppeteer Chromium automation for API calls. Initially, I attempted to use cookies copied from the first request in subsequent requests. However, I encountered difficulties in making TLS and Fingerprinting work, both in C# and Golang. Despite Golang offering advanced features for this purpose, I was unable to bypass the bot protection using cookies alone.

To tackle this problem, I utilized Puppeteer stealth plugins in conjunction with Puppeteer itself. In theory, these plugins should enable bypassing Cloudflare's bot protection even in headless mode. However, in practice, I discovered that if I made multiple requests to the same URL, it would still trigger Cloudflare's bot protection and result in being flagged. The solution only works when making Puppeteer requests to different pages.

**EDIT** I realised that the UA doesn't get set by the stealth plugin, fixing this issue headless=true mode now works.

If Kick introduces proper API support in the future, this repository will be updated accordingly.

## Release Alpha
# Currently only VOD downloading works wia KickDownloaderWPF.
# In Progress
- CLI
- Clip download

Since kick doesnt save chats, no chat feature will be available.


Credits go to lay295 and collaborators for the TwitchDownloader code.
