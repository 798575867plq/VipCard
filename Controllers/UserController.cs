using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VipCard.Dal.Util;
using VipCard.Models;

namespace VipCard.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Login(UserModel m)
        {
            try
            {
                string sql = @"select * from TbUser where username=@p0 and password=@p1";
                IDictionary<string, object> user = DBHelper.QueryOneDicRow(sql, m.Username, m.Password);
                if (user == null || user.Count == 0)
                {
                    throw new Exception("用户名或者密码错误，登陆失败！");
                }
                m.Datas.Add("user", user);
                m.Ok(user["nickname"] + "登陆成功");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }
            return Json(m);
        }

    }
}
