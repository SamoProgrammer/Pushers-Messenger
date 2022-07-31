using AutoMapper;
using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapper
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<Chat, ChatResponse>().ForMember(c => c.Messages, c => c.MapFrom(cr => cr.Messages.OrderBy(c => c.SentDate).TakeLast(20).Select(m => new SimpleMessage()
            {
                ChatId = m.ChatId,
                Id = m.Id,
                SenderId = m.SenderId,
                SentDate = m.SentDate,
                Text = m.Text,
                IsDeliverd = true
            })));
        }
    }
}
