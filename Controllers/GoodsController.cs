using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VipCard.Dal.Util;
using VipCard.Models;

namespace VipCard.Controllers
{
    public class GoodsController : Controller
    {
        public ActionResult Query(GoodsModel m)
        {
            try
            {
                string gsql = "select * from TbGoods where amount>0";
                string vsql = "select * from TbVipCard";
                string sql = @"select bg.bgid,bg.gid,bg.amount,bg.vcrid,
 CONVERT(varchar,bg.btime,120) 'btime',
 g.gname,g.price,vcr.amount 'vamount',vc.cardno,vc.username
 from TbBuyGoods bg
 inner join TbGoods g on bg.gid=g.gid
 inner join TbVipCardRecord vcr on bg.vcrid=vcr.vcrid
 inner join TbVipCard vc on vcr.vcid=vc.vcid";
                IList<IDictionary<string, object>> gdata = DBHelper.QueryDicRows(gsql);
                IList<IDictionary<string, object>> vdata = DBHelper.QueryDicRows(vsql);
                IList<IDictionary<string, object>> data = DBHelper.QueryDicRows(sql);
                m.Datas.Add("glist", gdata);
                m.Datas.Add("vlist", vdata);
                m.Datas.Add("list", data);
                m.Ok("查询完成");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }

            return Json(m);
        }


        public ActionResult Add(GoodsModel m)
        {
            try
            {
                string sql = "select price,amount from TbGoods where gid=@p0";
                IDictionary<string, object> goods = DBHelper.QueryOneDicRow(sql, m.Gid);
                decimal price = (decimal)goods["price"];
                decimal vamount = price * m.Amount;
                int gamount = ((int)goods["amount"]) - m.Amount;
                sql = @"insert into TbVipCardRecord(vcid,rtype,amount,info) values(@p0,-1,@p1,'商品购买')";
                DBHelper.Update(sql, m.Vcid, vamount);
                sql = @"select top 1 vcrid from TbVipCardRecord order by vcrid desc";
                object vcrid = DBHelper.QueryOne(sql);
                sql = @"insert into TbBuyGoods(gid,amount,vcrid) values(@p0,@p1,@p2)";
                DBHelper.Update(sql, m.Gid, m.Amount, vcrid);
                sql = @"update TbGoods set amount=@p0 where gid=@p1";
                DBHelper.Update(sql, gamount, m.Gid);
                m.Ok("添加完成");
            }
            catch (Exception ex)
            {
                m.Fail(ex);
            }
            return Json(m);
        }
    }
}
