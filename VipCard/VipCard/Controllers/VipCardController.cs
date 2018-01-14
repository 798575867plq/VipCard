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
                string sql = @"select vc.cardno,vc.username,vc.phone,
 case vc.sex when 'm' then '男' when 'f' then '女' else '保密' end 'sex',
 (select SUM(amount*rtype) from TbVipCardRecord where vcid=vc.vcid) 'balance',
 CONVERT(varchar,createdDate,120) 'createdDate'
 from TbVipCard vc";
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
                if (String.IsNullOrWhiteSpace(m.Cardno))
                {
                    m.Cardno = Guid.NewGuid().ToString();
                }
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

        public ActionResult Change(VipCardModel m)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(m.Cardno))
                {
                    m.Cardno = Guid.NewGuid().ToString();
                }
                string sql = @"update TbVipCard set cardno=@p0 where cardno=@p1";
                DBHelper.Update(sql, m.Cardno, m.OldCardno);
                m.Ok("换卡成功");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }
            return Json(m);
        }

        public ActionResult Charge(VipCardModel m)
        {
            try
            {
                string sql = @"select vcid from TbVipCard where cardno=@p0";
                object vcid = DBHelper.QueryOne(sql, m.Cardno);
                if (vcid == null)
                {
                    throw new Exception("会员卡不存在");
                }
                sql = @"insert into TbVipCardRecord(vcid,rtype,amount,info) values(@p0,@p1,@p2,@p3)";
                DBHelper.Update(sql, vcid, 1, m.Amount, "会员充值");
                m.Ok("充值成功");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }
            return Json(m);
        }

    }
}
