using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Services.Interfaces;

namespace MediaTracker.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user != null && user.Password == password)
            {
                // In a real application, we would use proper password hashing
                // For this example, we're using plain text passwords
                return user;
            }

            return null;
        }
    }
}