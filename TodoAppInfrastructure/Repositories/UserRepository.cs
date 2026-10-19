using TodoAppCore.Models;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;

namespace TodoAppInfrastructure.Repositories
{
    public class UserRepository
        : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TodoContext dbContext)
            : base(dbContext)
        {
        }

        public User? GetByUsername(string username)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Username == username);
        }

        public User? GetById(int id)
        {
            return _dbContext.Users.Find(id);
        }



        public bool ExistsByUsername(string username)
        {
            return _dbContext.Users
                .Any(u => u.Username == username);
        }
    }
}