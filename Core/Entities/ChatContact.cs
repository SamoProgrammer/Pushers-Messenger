using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ChatContact
    {
        public int ContactId { get; set; }
        public Contact Contact { get; set; }



        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public string ChatTitle { get; set; }
    }
}
