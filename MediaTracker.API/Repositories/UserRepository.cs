using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTracker.API.Models;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Utils;

namespace MediaTracker.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JsonFileManager<User> _fileManager;

        public UserRepository()
        {
            _fileManager = new JsonFileManager<User>("Data/users.json");
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _fileManager.ReadAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var users = await GetAllAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var users = await GetAllAsync();
            return users.FirstOrDefault(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var users = await GetAllAsync();
            return users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User> AddAsync(User user)
        {
            var users = await GetAllAsync();
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            user.CreatedAt = DateTime.Now;

            users.Add(user);
            await _fileManager.WriteAllAsync(users);

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var users = await GetAllAsync();
            var index = users.FindIndex(u => u.Id == user.Id);

            if (index >= 0)
            {
                users[index] = user;
                await _fileManager.WriteAllAsync(users);
                return user;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var users = await GetAllAsync();
            var index = users.FindIndex(u => u.Id == id);

            if (index >= 0)
            {
                users.RemoveAt(index);
                await _fileManager.WriteAllAsync(users);
                return true;
            }

            return false;
        }
    }
}