using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Friend
    {
        public Contact Contact { get; set; }
        public int? ContactId { get; set; }

        public string FriendUsername { get; set; }
        public int FriendContactId { get; set; }
    }
}
