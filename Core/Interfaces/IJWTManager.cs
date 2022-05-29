using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IJWTManager
    {
        Task<JwtToken> Authenticate(LoginCommand users);
    }
}
