using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.IO;
using System.Net.WebSockets;
using ProtoBuf;

namespace DesktopApp.Views
{
    public partial class ChattingWindow : Window
    {
        public ClientWebSocket client { get; set; }
        public ChattingWindow()
        {
            client = new ClientWebSocket();
            InitializeComponent();
            AvaloniaXamlLoader.Load(this);

            var person = new Message
            {
                Text="Test Message"
            };
            using (var memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, person);
                var byteArray = memoryStream.ToArray();
            }

        }
    }
    [ProtoContract]
    class Message
    {
        [ProtoMember(1)]
        public string Text { get; set; }


    }
}
