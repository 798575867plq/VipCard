using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VipCard.Models
{
    public class GoodsModel : BaseModel
    {
        public int Vcid { get; set; }
        public int Gid { get; set; }
        public int Amount { get; set; }
    }
}