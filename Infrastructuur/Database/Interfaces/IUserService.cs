using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Interfaces
{
    public  interface IUserService
    {
        UserEntity GetUser(string firstName, string password);

        UserEntity CreateUser(UserEntity user);
        List<UserEntity> GetAllUsers();

        UserEntity GetUserById(int id);

        void DeleteUser(int id);    

        void AddWeedsToUser(int userId,WeedEntity weed);
        UserEntity GetUserByEmail(string email);
        void DeleteWeeds(int userId, int id);

    }
}
