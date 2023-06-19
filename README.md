# Kick Vod Downloader

# This is a fork of the lay295/TwitchDownloader

About: 

To overcome the Bot protection I attempted to use cookies copied from the first request in subsequent requests. However, I encountered difficulties in making TLS and Fingerprinting work, both in C# and Golang. Despite Golang offering advanced features for this purpose, I was unable to bypass the bot protection using cookies alone.

To tackle this problem, I utilized Puppeteer chromium headless mode, meaning the requests take more time than it should.

If Kick introduces proper API support in the future, this repository will be updated accordingly.

## Latest Alpha
- Download VODs
- Download Clips
- 
![Figure 1.1](KickDownloaderWPF/Images/vodExample.png)


Since kick doesnt save chats, no chat feature will be available.


Credits go to lay295 and collaborators for the TwitchDownloader code.
