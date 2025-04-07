using System.Threading.Tasks;
using MediaTracker.API.Models;

namespace MediaTracker.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}