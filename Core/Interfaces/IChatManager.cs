using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IChatManager
    {
        Task<Message> InsertMessage(Message message);
        public Task SeenChat(SeenCommand seenCommand);
    }
}
