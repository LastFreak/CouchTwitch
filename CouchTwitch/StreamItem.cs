using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace CouchTwitch
{
    public class StreamItem
    {
        public BitmapImage Logo { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public StreamItem()
        {

        }
    }
}
