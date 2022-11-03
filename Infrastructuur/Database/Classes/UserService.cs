using Infrastructuur.Database.Database;
using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Classes
{
    public class UserService : IUserService
    {
        private readonly WeedDatabase _weedDatabase;

        public UserService(WeedDatabase weedDatabase)
        {
            _weedDatabase = weedDatabase;
        }
        public List<UserEntity> GetAllUsers()
        {
            return _weedDatabase.users;
        }
        public UserEntity CreateUser(UserEntity user)
        {
            user.Id = _weedDatabase.users.Max(x => x.Id) + 1;
            user.Role = "User";
            _weedDatabase.users.Add(user);
            return user;
        }

        public UserEntity GetUser(string firstName, string password)
        {
            return _weedDatabase.users.FirstOrDefault(x => x.FirstName == firstName && x.Password == password);
        }

        public UserEntity GetUserById(int id)
        {
            return _weedDatabase.users.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteUser(int id)
        {
            _weedDatabase.users.Remove(_weedDatabase.users.FirstOrDefault(x => x.Id == id));
        }

        public void AddWeedsToUser(int userId, WeedEntity weed)
        {
            _weedDatabase.users.FirstOrDefault(x => x.Id == userId).Weeds.Add(weed);
        }

        public UserEntity GetUserByEmail(string email)
        {
            return _weedDatabase.users.FirstOrDefault(x => x.Email == email);
        }

        public void DeleteWeeds(int userId, int id)
        {
            var weed = _weedDatabase.Weeds.FirstOrDefault(x => x.Id == id);
            var user = _weedDatabase.users.FirstOrDefault(x => x.Id == userId).Weeds.Remove(weed);
        }
    }
}
