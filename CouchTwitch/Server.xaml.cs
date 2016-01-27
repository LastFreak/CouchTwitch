using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CouchTwitch
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Server : Page
    {
        private StreamSocketListener listener;
        private StreamSocket socket;

        public Server()
        {
            this.InitializeComponent();
        }

        private Uri StreamUri
        {
            get; set;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += Server_BackRequested;

            listener = new StreamSocketListener();
            listener.ConnectionReceived += Listener_ConnectionReceived;
            try
            {
                await listener.BindServiceNameAsync("56789", SocketProtectionLevel.PlainSocket);
                var icp = NetworkInformation.GetInternetConnectionProfile();
                var hostname = NetworkInformation.GetHostNames().SingleOrDefault(hn =>
                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                    == icp.NetworkAdapter.NetworkAdapterId);
                MessageDialog msg = new MessageDialog( "Die IP ist: \r\n"+hostname.CanonicalName);
#pragma warning disable CS4014
                msg.ShowAsync();
#pragma warning restore CS4014
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
            if(!ApplicationView.GetForCurrentView().IsFullScreenMode)
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            base.OnNavigatedTo(e);
        }

        

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("Connected!");
            socket = args.Socket;
            DataReader reader = new DataReader(socket.InputStream);
            try
            {
                while (true)
                {
                    uint MessageSize = await reader.LoadAsync(sizeof(uint));
                    if (MessageSize != sizeof(uint))
                    {
                        return;
                    }
                    uint StringLengh = reader.ReadUInt32();
                    await reader.LoadAsync(StringLengh);
                    string msg = reader.ReadString(StringLengh);
                    Debug.WriteLine(msg);
                    
                    string command = msg.Split(' ')[0];
                    string[] cmd = msg.Split(' ');
                    Debug.WriteLine(msg);
                    switch (command) {
                        case "SURI":
                            StreamUri = new Uri(cmd[1]);
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                Play();
                            });
                            break;
                        case "FULL":
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                if (ApplicationView.GetForCurrentView().IsFullScreenMode == false)
                                {
                                    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                                }
                                mediaStream.IsFullWindow = !mediaStream.IsFullWindow;
                            });
                            break;
                        case "VOL":
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                mediaStream.Volume = Convert.ToDouble(cmd[1]);
                            });
                            break;
                        case "PLAY":
                            
                            break;
                        case "SYNC":
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                Debug.WriteLine(cmd[2]);
                            DateTime sync = Convert.ToDateTime(cmd[2]);
                            sync = sync.AddSeconds(10);
                                Debug.WriteLine(sync.ToString());
                            mediaStream.Stop();
                            while(DateTime.Now.Second != sync.Second)
                            {

                            }
                            mediaStream.Play();
                                Debug.WriteLine("Continued.");
                            });
                            break;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Play()
        {
            mediaStream.Source = StreamUri;
            mediaStream.Play();
        }

        private void Server_BackRequested(object sender, BackRequestedEventArgs e)
        {
            listener.Dispose();
            if(this.Frame.CanGoBack)this.Frame.GoBack();
        }
            
            
    }
}
