using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppCore.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
