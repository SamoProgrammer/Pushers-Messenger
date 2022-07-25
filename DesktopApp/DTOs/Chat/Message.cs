using System;

namespace DesktopApp.DTOs.Chat
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime SentDate { get; set; }
        public Chat Chat { get; set; }


    }
}
