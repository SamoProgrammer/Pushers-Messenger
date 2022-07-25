﻿using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapper
{
    public class SentFriendRequestProfile : Profile
    {
        public SentFriendRequestProfile()
        {
            CreateMap<FriendRequest, (int Id, string ToContactUsername, int ToContactId, string Text)>()
                .ForMember(f => f.Id, f => f.MapFrom(fr => fr.Id)).ForMember(f => f.ToContactUsername, f => f.MapFrom(fr => fr.ToContact.Username))
                .ForMember(f => f.ToContactId, f => f.MapFrom(fr => fr.ToContactId)).ForMember(f => f.Text, f => f.MapFrom(fr => fr.Text));
        }
    }
}
