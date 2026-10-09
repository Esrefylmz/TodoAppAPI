using System;
using System.Collections.Generic;
using System.Text;
using TodoAppCore.Models;

namespace TodoAppCore.Repositories
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
        User? GetById(int id);
        void Add(User user);
        bool ExistsByUsername(string username);
    }
}
