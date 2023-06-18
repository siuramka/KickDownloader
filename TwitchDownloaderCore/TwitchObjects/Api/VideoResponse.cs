using System;
using System.Collections.Generic;

namespace TwitchDownloaderCore.TwitchObjects.Api;

 public class Livestream
    {
        public int id { get; set; }
        public string slug { get; set; }
        public int channel_id { get; set; }
        public DateTime created_at { get; set; }
        public string session_title { get; set; }
        public bool is_live { get; set; }
        public object risk_level_id { get; set; }
        public object source { get; set; }
        public object twitch_channel { get; set; }
        public int duration { get; set; }
        public string language { get; set; }
        public bool is_mature { get; set; }
        public int viewer_count { get; set; }
        public string thumbnail { get; set; }
        public Channel channel { get; set; }
        public List<Category> categories { get; set; }
    }

    public partial class Channel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string slug { get; set; }
        public bool is_banned { get; set; }
        public string playback_url { get; set; }
        public object name_updated_at { get; set; }
        public bool vod_enabled { get; set; }
        public bool subscription_enabled { get; set; }
        public int followersCount { get; set; }
        public User user { get; set; }
        public bool can_host { get; set; }
        public object verified { get; set; }
    }

    public partial class User
    {
        public string profilepic { get; set; }
        public string bio { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
        public string youtube { get; set; }
        public string discord { get; set; }
        public string tiktok { get; set; }
        public string username { get; set; }
    }

    public partial class Category
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public List<string> tags { get; set; }
        public object description { get; set; }
        public object deleted_at { get; set; }
        public int viewers { get; set; }
        public CategoryDetails category { get; set; }
    }

    public class CategoryDetails
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string icon { get; set; }
    }

    public class VideoResponse
    {
        public int? id { get; set; }
        public int live_stream_id { get; set; }
        public object slug { get; set; }
        public object thumb { get; set; }
        public object s3 { get; set; }
        public object trading_platform_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Guid uuid { get; set; }
        public int views { get; set; }
        public object deleted_at { get; set; }
        public string source { get; set; }
        public Livestream livestream { get; set; }
    }
