namespace TwitchDownloaderCore.TwitchObjects.Api;

using System;

public class Creator
{
    public int id { get; set; }
    public string username { get; set; }
    public string slug { get; set; }
    public string profile_picture { get; set; }
}

public partial class Channel
{
    public string username { get; set; }
    public string profile_picture { get; set; }
}

public partial class Category
{
    public string responsive { get; set; }
    public string banner { get; set; }
    public string parent_category { get; set; }
}

public class Clip
{
    public int id { get; set; }
    public bool is_mature { get; set; }
    public string title { get; set; }
    public int duration { get; set; }
    public string thumbnail_url { get; set; }
    public string video_url { get; set; }
    public int view_count { get; set; }
    public int likes_count { get; set; }
    public bool liked { get; set; }
    public DateTime created_at { get; set; }
    public Creator creator { get; set; }
    public Channel channel { get; set; }
    public Category category { get; set; }
}

public class ClipsResponse
{
    public Clip clip { get; set; }
}
