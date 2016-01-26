using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace CouchTwitch
{
    public class CTClient
    {

        public HostName hostname;
        public StreamSocket socket;


        public CTClient()
        {
            socket = new StreamSocket();
        }

        public async void ClientSendMessage(string stringToSend)
        {
            if (socket == null || stringToSend == "") return;

            DataWriter writer = new DataWriter(socket.OutputStream);

            writer.WriteUInt32(writer.MeasureString(stringToSend));
            writer.WriteString(stringToSend);
            Debug.WriteLine(stringToSend);
            try
            {
                await writer.StoreAsync();
                writer.DetachStream();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
