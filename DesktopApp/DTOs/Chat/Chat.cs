using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesktopApp.DTOs.Chat
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public List<int> UserIDs { get; set; }
        public List<Message> Messages { get; set; }
    }
}
