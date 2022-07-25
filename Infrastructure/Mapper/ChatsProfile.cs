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
    public class ChatsProfile : Profile
    {
        public ChatsProfile()
        {
            CreateMap<ChatContact, ChatsResponse>().ForMember(c=>c.Title,cc=>cc.MapFrom(c=>c.ChatTitle)).ForMember(c => c.ChatId, c => c.MapFrom(ch => ch.ChatId));
        }
    }
}
