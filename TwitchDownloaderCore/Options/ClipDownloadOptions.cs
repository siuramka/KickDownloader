﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchDownloaderCore.Options
{
    public class ClipDownloadOptions
    {
        public string Id { get; set; }
        public string Filename { get; set; }
        public int ThrottleKib { get; set; }
    }
}
