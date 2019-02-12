using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VipCard.Models
{
    public class UserModel : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}