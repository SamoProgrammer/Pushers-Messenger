using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.DTOs;
namespace Infrastructure.Mapper
{
    public class FriendProfile:Profile
    {
        public FriendProfile()
        {
            CreateMap<Friend, FriendDTO>().ForMember(c => c.Id, f => f.MapFrom(c => c.FriendContactId))
                .ForMember(c => c.Username, f => f.MapFrom(c => c.FriendUsername));
        }
    }
}
