using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VipCard.Models;

namespace VipCard.Controllers
{
    public class TestController : Controller
    {

        public ActionResult Index(TestModel m)
        {
            m.Ok("服务器测试正常");
            return Json(m, JsonRequestBehavior.AllowGet);
        }

    }
}
