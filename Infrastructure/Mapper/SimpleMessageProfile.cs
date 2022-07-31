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
    public class SimpleMessageProfile : Profile
    {
        public SimpleMessageProfile()
        {
            CreateMap<Message, SimpleMessage>().ForMember(m => m.Id, sm => sm.MapFrom(prop => prop.Id))
                .ForMember(m => m.IsDeliverd, sm => sm.MapFrom(prop => true))
            .ForMember(m => m.SentDate, sm => sm.MapFrom(prop => prop.SentDate))
            .ForMember(m => m.SenderId, sm => sm.MapFrom(prop => prop.SenderId))
            .ForMember(m => m.ChatId, sm => sm.MapFrom(prop => prop.ChatId))
            .ForMember(m => m.Text, sm => sm.MapFrom(prop => prop.Text)); ;
        }
    }
}
