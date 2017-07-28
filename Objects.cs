using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Discord
{
    public class LoginResponse
    {
        public string token { get; set; }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class Post
	{
		public string content { get; set; }
		public string nonce { get; set; }
		public bool tts { get; set; }
	}

    public class MessageCollection
    {
        public Message[] Messages { get; set; }
    }

    public class Message
    {
        public object[] attachments { get; set; }
        public bool tts { get; set; }
        public Embed[] embeds { get; set; }
        public DateTime timestamp { get; set; }
        public bool mention_everyone { get; set; }
        public string id { get; set; }
        public object edited_timestamp { get; set; }
        public Author author { get; set; }
        public string content { get; set; }
        public string channel_id { get; set; }
        public object[] mentions { get; set; }
    }

    public class Author
    {
        public string username { get; set; }
        public string discriminator { get; set; }
        public string id { get; set; }
        public string avatar { get; set; }
    }

    public class Embed
    {
        public string url { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public Thumbnail thumbnail { get; set; }
        public Author1 author { get; set; }
        public Video video { get; set; }
        public Provider provider { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public string proxy_url { get; set; }
        public int height { get; set; }
    }

    public class Author1
    {
        public string url { get; set; }
        public string name { get; set; }
    }

    public class Video
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Provider
    {
        public string url { get; set; }
        public string name { get; set; }
    }

}
