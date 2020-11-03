using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public interface IDALClass
    {
        void AddUser(User newUser);
        IQueryable<User> GetAllUsers();
    }

    public class DALClass : IDALClass
    {
        private Model1 _ctx = new Model1();

        public void AddUser(User newUser)
        {
            _ctx.Users.Add(newUser);
            _ctx.SaveChanges();
        }

        public IQueryable<User> GetAllUsers()
        {
            return _ctx.Users;
        }
    }
}
