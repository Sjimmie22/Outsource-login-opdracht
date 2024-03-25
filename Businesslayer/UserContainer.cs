using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Businesslayer
{
    public class UserContainer
    {
        public IUserRepository irepo;

        public UserContainer(IUserRepository irepo)
        {
            this.irepo = irepo;
        }

        public bool AuthenticateUser(User u)
        {
            return irepo.ValidateUser(u.ToDto()); ;
        }
    }
}
