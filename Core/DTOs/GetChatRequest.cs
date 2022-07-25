using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class GetChatRequest
    {
        public int ChatId { get; set; }

        public int SkipCount { get; set; }
    }
}
