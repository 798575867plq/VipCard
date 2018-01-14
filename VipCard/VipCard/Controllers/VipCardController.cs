using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VipCard.Dal.Util;
using VipCard.Models;

namespace VipCard.Controllers
{
    public class VipCardController : Controller
    {
        public ActionResult Query(VipCardModel m)
        {
            try
            {
                string sql = @"select * from TbVipCard";
                IList<IDictionary<string, object>> data = DBHelper.QueryDicRows(sql);
                m.Datas.Add("list", data);
                m.Ok("查询完成");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }
            return Json(m);
        }

        public ActionResult Add(VipCardModel m)
        {
            try
            {
                m.Cardno = Guid.NewGuid().ToString();
                string sql = @"insert into TbVipCard(cardno,username,password,phone,info,sex) values(@p0,@p1,@p2,@p3,@p4,@p5)";
                DBHelper.Update(sql, m.Cardno, m.Username, m.Password, m.Phone, m.Info, m.Sex);
                m.Ok("开卡成功");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }
            return Json(m);
        }

    }
}
