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
                string sql = @"select *
	,case when lbalance<=200 then '普通卡' when lbalance<=500 then '黄金卡' when lbalance<=1000 then '白金卡' else '钻石卡' end 'level'
 from
 (
 select vc.cardno,vc.username,vc.phone,vc.info,
 case vc.sex when 'm' then '男' when 'f' then '女' else '保密' end 'sex',
 (select SUM(amount*rtype) from TbVipCardRecord where vcid=vc.vcid) 'balance',
 (select SUM(amount*rtype) from TbVipCardRecord where vcid=vc.vcid and rtype=1) 'lbalance',
  CONVERT(int,(select SUM(amount*rtype*-1) from TbVipCardRecord where vcid=vc.vcid and rtype=-1)) 'integral',
 CONVERT(varchar,createdDate,120) 'createdDate'
 from TbVipCard vc
 ) a";
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


        public ActionResult QueryRecord(VipCardModel m)
        {
            try
            {
                string sql = @"select vcr.info,vcr.amount,
 CONVERT(varchar,vcr.rtime,120) 'rtime',
 vc.username,vc.cardno
 from TbVipCardRecord vcr
 inner join TbVipCard vc on vcr.vcid=vc.vcid";
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

    }
}
