using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VipCard.Models
{
    public class VipCardModel : BaseModel
    {
        public int Vcid { get; set; }
        public string Cardno { get; set; }
        public string OldCardno { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Info { get; set; }
        public decimal Amount { get; set; }
    }
}