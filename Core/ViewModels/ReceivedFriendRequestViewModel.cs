using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class ReceivedFriendRequestViewModel
    {
        public int Id { get; set; }
        public int FromContactId { get; set; }
        public string FromContactUsername { get; set; }
        public string Text { get; set; }
    }
}
