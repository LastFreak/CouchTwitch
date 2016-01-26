using CouchTwitch.CouchTwitch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CouchTwitch
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Client : Page
    {
        private CTClient server = new CTClient();
        public Irc irc;

        private bool PlaySound = false;

        public Client()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += Server_BackRequested;
            

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("token"))
            {
                Request();
            }

            base.OnNavigatedTo(e);
        }

        private void Server_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }

        private async void Request()
        {

            

            Debug.WriteLine((string)ApplicationData.Current.LocalSettings.Values["token"]);
            User user = (User)await ApiHelper.apiRequest<User>("https://api.twitch.tv/kraken/user?oauth_token=" + ApplicationData.Current.LocalSettings.Values["token"]);
            FollowStream streams = (FollowStream)await ApiHelper.apiRequest<FollowStream>("https://api.twitch.tv/kraken/streams/followed?oauth_token=" + ApplicationData.Current.LocalSettings.Values["token"]);
            List<CouchTwitch.FollowStreamJsonTypes.Stream> list = streams.Streams.ToList();
            List<StreamItem> items = new List<StreamItem>();
            foreach(CouchTwitch.FollowStreamJsonTypes.Stream item in list)
            {
                StreamItem siitem = new StreamItem();
                siitem.Logo = new BitmapImage(new Uri(item.Channel.Logo));
                siitem.Name = item.Channel.Name;
                siitem.DisplayName = item.Channel.DisplayName;
                items.Add(siitem);
            }
            lstFollows.ItemsSource = items;

            #region IRC
            irc = new Irc();
            irc.credentails = new Credentails(user.Name, "oauth:" + ApplicationData.Current.LocalSettings.Values["token"]);
            irc.Hostname = "irc.twitch.tv";
            irc.Port = 6667;
            await irc.Connect();
            #endregion
            lstChat.ItemsSource = irc.List;

            ContentDialogResult result = await cdIP.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                server.hostname = new HostName(txtIP.Text);
            }
            
            try
            {
                await server.socket.ConnectAsync(server.hostname, "56789");
            }
            catch
            {

            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var startUri = new Uri("https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=npmoawh06nmx4f0yjozbtq05vttbwmn&redirect_uri=http://localhost&scope=chat_login+user_read");
            var endUri = new Uri("http://localhost");

            var pattern = string.Format("{0}#access_token={1}&scope={2}", endUri, "(?<access_token>.+)", "(?scope.+)");

            var asyncOperation = await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                 Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                 startUri,
                 endUri);
            switch (asyncOperation.ResponseStatus)
            {
                case Windows.Security.Authentication.Web.WebAuthenticationStatus.ErrorHttp:
                    break;
                case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
                    asyncOperation.ResponseData.ToString();
                    var data = asyncOperation.ResponseData.Substring(asyncOperation.ResponseData.IndexOf('#'));
                    var values = data.Split('&');
                    var token = values[0].Split('=')[1];
                    var container = ApplicationData.Current.LocalSettings.Values;
                    container["token"] = token;
                    Debug.WriteLine("Token: " + token);
                    Request();
                    break;
            }

            //webViewLogin.Navigate(new Uri("https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=npmoawh06nmx4f0yjozbtq05vttbwmn&redirect_uri=http://localhost&scope=chat_login"));
            //webViewLogin.Visibility = Visibility.Visible;
        }

        private async void lstFollows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            StreamItem stream = lstFollows.SelectedItem as StreamItem;
            string channelname = stream.Name;
            irc.SwitchChannel(channelname);
            AuthToken token = (AuthToken)await ApiHelper.apiRequest<AuthToken>("http://api.twitch.tv/api/channels/" + channelname + "/access_token");
            string url = "http://usher.twitch.tv/api/channel/hls/" + channelname + ".m3u8?player=twitchweb&token=" + token.Token + "&sig=" + token.Sig + "&$allow_audio_only=true&allow_source=true";
            Debug.WriteLine(url);
            mediaAudioStream.Source = new Uri(url);
            if (PlaySound == true)
            {
                mediaAudioStream.Play();
            }

            #region sendURI
            string message = "SURI:" + url;
            server.ClientSendMessage(message);
            #endregion 
        }

        

        private void txtToken_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                
            }
        }

        private void cdIP_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {

        }


        private void btnHamburger_Click(object sender, RoutedEventArgs e)
        {
            menu.IsPaneOpen = !menu.IsPaneOpen;
        }

        //Eventuell ausbauen:

        //private void tglPlaySound_Click(object sender, RoutedEventArgs e)
        //{
        //    PlaySound = tglPlaySound.IsChecked.Value;
        //    if(tglPlaySound.IsChecked.Value)
        //    {
        //        mediaAudioStream.Play();
        //    }
        //    else
        //    {
        //        mediaAudioStream.Stop();
        //    }
        //}

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            server.ClientSendMessage("FULL:");
        }

        private void lstChat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Ab hier UI-spezifischer Code


        private void ChangePCPhone_Click(object sender, RoutedEventArgs e)
        {
            if (PCLogo.Visibility == Visibility.Visible)
            {
                PCLogo.Visibility = Visibility.Collapsed;
                PhoneLogo.Visibility = Visibility.Visible;
                mediaAudioStream.Visibility = Visibility.Visible;
            }
            else
            {
                PCLogo.Visibility = Visibility.Visible;
                PhoneLogo.Visibility = Visibility.Collapsed;
                mediaAudioStream.Visibility = Visibility.Collapsed;
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            server.ClientSendMessage("PLAY:");
            if (Pause.Visibility == Visibility.Visible)
            {
                Pause.Visibility = Visibility.Collapsed;
                Play.Visibility = Visibility.Visible;
                
            }
            else
            {
                Pause.Visibility = Visibility.Visible;
                Play.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)  //Zeige followed Streams
        {
            menu.IsPaneOpen = !menu.IsPaneOpen;
        }

        private void sldVolume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            server.ClientSendMessage("VOL:" + (e.NewValue/100) .ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Irc Message
            Debug.WriteLine(txtToken.Text);
            irc.SendChatMessage(txtToken.Text);
            txtToken.Text = "";
        }
    }
}
