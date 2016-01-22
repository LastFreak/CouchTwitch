using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace CouchTwitch
{
    public class Irc
    {
        private readonly StreamSocket socket = new StreamSocket();
        private bool connected;
        private DataReader reader;
        public string lastMessage;
        public ObservableCollection<TwitchIrcChatItem> List = new ObservableCollection<TwitchIrcChatItem>();
        public string Hostname
        {
            get; set;
        }
        public int Port { get; set; }
        public Credentails credentails;
        private string Channel{ get; set; }
        private bool closing;

        public async Task<bool> Connect()
        {
            if (connected) return false;
            var hostname = new HostName(Hostname);
            await socket.ConnectAsync(hostname, Port.ToString());
            connected = true;
            reader = new DataReader(socket.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial
            };
            SendIdentity();
            WaitforData();
            return true;
        }



        public void SwitchChannel(string name)
        {
            //TODO: Channel verlassen
            if(Channel != null)
            {
                SendRawMessage("PART " + Channel);
            }
            SendRawMessage("JOIN #" + name);
            Channel = "#" + name;
        }

        private async void WaitforData()
        {
            if (!connected || socket == null) return;
            uint s = await reader.LoadAsync(2048);
            string data = reader.ReadString(s);
            if (data != null)
            {
                Debug.WriteLine(data);
                if (Channel != null)
                {
                    TwitchIrcChatItem tci = new TwitchIrcChatItem(data, Channel);
                    if(tci.Message != "")
                    List.Add(tci);
                }
            }
            
            if (data.Contains("No ident response")) SendIdentity();
            if (Regex.IsMatch(data, "PING :[0-9]+\\r\\n")) ReplyPong(data);
            
            WaitforData();
        }

        private void ReplyPong(string data)
        {
            var pingCode = Regex.Match(data, "[0-9]+");
            SendRawMessage("PONG " + pingCode);
        }

        

        private void SendIdentity()
        {
            if (credentails.Nickname == string.Empty) credentails.Nickname = credentails.Username;
            if (credentails.Password != String.Empty) SendRawMessage("PASS " + credentails.Password);
            SendRawMessage("NICK " + credentails.Nickname);
            SendRawMessage("USER " + credentails.Username + " " + credentails.Username + " * :" + credentails.Username);
           
        }
        public void SendChatMessage(string message)
        {
            string data = ":" + credentails.Username + "!" + credentails.Username + "@" + credentails.Username + ".tmi.twitch.tv PRIVMSG " + Channel + " :" + message;
            SendRawMessage(data);
            List.Add(new TwitchIrcChatItem(":" + credentails.Username + "!" + Channel + " :" + message, Channel));
        }
        public async void SendRawMessage(string v)
        {
            var writer = new DataWriter(socket.OutputStream);
            writer.WriteString(v + "\r\n");
            await writer.StoreAsync();
            await writer.FlushAsync();
            writer.DetachStream();
            if (!closing) return;
            socket.Dispose();
            connected = false;
        }
        public void Dispose()
        {
            SendRawMessage("QUIT :");
            closing = true;
        }
    }

    public class Credentails
    {
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Credentails(string username, string password = "", string nickname = "")
        {
            Username = username;
            Password = password;
            Nickname = nickname;
        }
    }
    
}
