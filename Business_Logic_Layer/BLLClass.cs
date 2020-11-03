using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public interface IBLLClass
    {
        void AddUser(UserDTO userDTO);
        IEnumerable<UserDTO> GetAllUsers();
    }

    public class BLLClass : IBLLClass
    {
        private IDALClass _dal = null;

        public BLLClass()
        {
            _dal = new DALClass();
        }

        public void AddUser(UserDTO userDTO)
        {
            _dal.AddUser(new User()
            {
                Login = userDTO.Login,
                Password = userDTO.Password,
            });
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _dal.GetAllUsers().Select(user => new UserDTO()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
            }).ToList();
        }
    }

}
