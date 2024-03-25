
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Businesslayer
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {

        }

        public UserDTO ToDto() //zet user naar userDTO
        {
            return new UserDTO() { Email = Email, PasswordSalt = Password };
        }
    }
}
