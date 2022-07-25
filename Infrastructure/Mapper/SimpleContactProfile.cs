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
    public class SimpleContactProfile:Profile
    {
        public SimpleContactProfile()
        {
            CreateMap<Contact, SimpleContact>().ForMember(c=>c.Username,sc=>sc.MapFrom(c=>c.Username)).ForMember(c=>c.Id,sc=>sc.MapFrom(c=>c.Id)) ;
        }
    }
}
