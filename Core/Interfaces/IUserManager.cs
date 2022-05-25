using System.Threading.Tasks;
using Core.ViewModels;

namespace Core.Interfaces
{
    public interface IUserManager
    {
        Task<RegisterationResponse> Register(RegisterationCommand command);
        Task<bool> ConfirmEmail(string username,string token);
    }
}
