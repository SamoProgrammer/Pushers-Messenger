using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ChatResponse
    {
        public List<SimpleContact> Contacts{ get; set; }
        public List<SimpleMessage> Messages { get; set; }

        public int AllMessagesCount { get; set; }
    }
}
