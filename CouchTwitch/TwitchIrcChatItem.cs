using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchTwitch
{
    public class TwitchIrcChatItem
    {
        public string User { get; set;}
        public string Message { get; set; }
        public TwitchIrcChatItem(string raw, string channel)
        {
            if (raw.Contains("!") && !raw.Contains("JOIN"))
            {
                User = raw.Substring(1, raw.IndexOf('!') - 1);
                Message = raw.Substring(raw.IndexOf(channel + " :") + channel.Length + 2);
            }
            else
            {
                Message = "";
            }
        }
    }
}
