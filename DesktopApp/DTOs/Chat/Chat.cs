using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
