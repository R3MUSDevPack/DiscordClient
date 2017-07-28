using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace R3MUS.Devpack.Discord
{
    public class Client
    {
        private const string getURI = "https://discordapp.com/api/channels/{0}/messages?limit=50";
        private const string postURI = "https://discordapp.com/api/v6/channels/{0}/messages";
        private const string LOGIN_URL = "https://discordapp.com/api/auth/login";
        private const string LOGOUT_URL = "https://discordapp.com/api/auth/logout";
        private string authToken;
        
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool Logon()
        {
            if((UserName != string.Empty) && (Password != string.Empty))
            {
                var encoding = new UTF8Encoding();

                try
                {
                    using (var client = new WebClient())
                    {
                        var reqObj = new LoginRequest() { email = UserName, password = Password };
                        var coll = new NameValueCollection();
                        
                        client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";

                        coll["credentials"] = JsonConvert.SerializeObject(reqObj);

                        //var response = encoding.GetString(client.UploadValues(LOGIN_URL, "POST", coll));
                        var response = encoding.GetString(client.UploadData(LOGIN_URL, "POST", Encoding.Default.GetBytes(coll["credentials"])));

                        authToken = JsonConvert.DeserializeObject<LoginResponse>(response).token;

                        if(authToken != string.Empty)
                        {
                            return true;
                        }
                    }
                }
                catch(Exception ex)
                {
                }
            }
            return false;
        }

        public void LogOut()
        {
            var encoding = new UTF8Encoding();

            try
            {
                using (var client = new WebClient())
                {
                    var reqObj = new LoginRequest() { email = UserName, password = Password };
                    var coll = new NameValueCollection();

                    client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    
                    
                    var response = encoding.GetString(client.UploadData(LOGOUT_URL, "POST", Encoding.Default.GetBytes(
                        JsonConvert.SerializeObject(
                            new 
                            {
                                token = authToken
                            }))));

                    authToken = string.Empty;                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        public List<Message> GetMessages(long channelId, long messageId = 0)
        {
            if(authToken != string.Empty)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.Authorization] = authToken;

                        var text = System.Text.Encoding.Default.GetString(client.DownloadData(string.Format(getURI, channelId.ToString())));

                        var messages = JsonConvert.DeserializeObject<List<Message>>(text);
                        
                        if (messageId > 0)
                        {
                            var msgText = new List<Message>();
                            messages.Where(msg => Convert.ToInt64(msg.id) > messageId).ToList().ForEach(msg =>
                                msgText.Add(msg)
                            );
                            messages = msgText;
                        }
                        return messages;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new List<Message>();
        }

        public void PostMessage(long channelId, Post message)
        {
            if (authToken != string.Empty)
            {
                try
                {
                    var msgString = JsonConvert.SerializeObject(message);
                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.Authorization] = authToken;
                        client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";

                        var result = client.UploadData(string.Format(postURI, channelId.ToString()), Encoding.Default.GetBytes(msgString));                        
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
