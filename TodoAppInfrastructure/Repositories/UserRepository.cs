using TodoAppCore.Models;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;

namespace TodoAppInfrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext _dbContext;

        public UserRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
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

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public bool ExistsByUsername(string username)
        {
            return _dbContext.Users
                .Any(u => u.Username == username);
        }
    }
}