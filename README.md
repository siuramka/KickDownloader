# This is a fork of the lay295/TwitchDownloader

# Kick Vod Downloader

About: 

I utilized Puppeteer chromium headless mode to bypass cf, meaning the requests take more time than it should.
It's also possible to use Golang with CycleTLS instead.

## Latest Alpha
- Download VODs
- Download Clips
- Task Queue
- Download from URL list parallel

Haven't got rate limited even once while testing lol


Preview:
- ![Figure 1.1](KickDownloaderWPF/Images/vodExample.png)


Since kick doesnt save chats, no chat feature will be available.

## Build Instructions
Windows only for now...
GUI

1. Clone the repository:
```
git clone https://github.com/siuramka/KickDownloader.git
```
2. Navigate to the solution folder:
```
cd KickDownloader
```
3. Restore the solution:
```
dotnet restore
```
4. Build the GUI:
```
dotnet publish KickDownloaderWPF -p:PublishProfile=Windows -p:DebugType=None -p:DebugSymbols=false
```
5. Navigate to the GUI build folder:
```
cd KickDownloaderWPF/bin/Release/net6.0-windows/publish/win-x64
```




Credits go to lay295 and collaborators for the TwitchDownloader code.
