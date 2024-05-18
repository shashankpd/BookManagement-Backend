using Microsoft.Extensions.Configuration;
using ModelLayer.Entity;
using ModelLayer.RequestBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRepo
    {
        public Task<int> createUser(UserBody entity);

        public Task<string>Login(string email, string password, IConfiguration configuration);

        public Task<User> GetUserByEmail(string email);

        public Task ResetPassword(string email, string otp, string newPassword);

    }
}
