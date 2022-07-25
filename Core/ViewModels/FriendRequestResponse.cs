using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.ViewModels
{
    public class FriendRequestResponse
    {
        public List<ReceivedFriendRequestViewModel> ReceivedFriendRequests { get; set; }
        public List<(int Id, string ToContactUsername, int ToContactId, string Text)> SentFriendRequests { get; set; }

    }
}
