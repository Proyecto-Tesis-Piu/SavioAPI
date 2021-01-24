using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonetaAPI.Models
{
    public class UserRequest
    {
        public String Token { get; set; }
    }

    public class LoginResponse : UserRequest 
    { 
        public Boolean EmailConfirmed { get; set; }
    }
}
