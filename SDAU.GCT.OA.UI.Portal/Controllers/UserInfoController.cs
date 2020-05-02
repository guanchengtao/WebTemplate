﻿
using SDAU.GCT.OA.BLL;
using SDAU.GCT.OA.Common;
using SDAU.GCT.OA.IBLL;
using SDAU.GCT.OA.Model;
using SDAU.GCT.OA.UI.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SDAU.GCT.OA.UI.Portal.Controllers
{
    [TimingActionFilter]
    public class UserInfoController : Controller
    {
        public IUserInfoService UserInfoService { get; set; }

        [HttpGet]
        public ActionResult GetAllUser()
        {
            var olddata = UserInfoService.GetEntities(u => u.DelFlag == 1);
            var data = olddata.Select(u => new
            {
                u.Id,
                u.UserName,
                u.UserPwd
            });

            int count = data.Count();
            var jsondata = new
            {
                msg = string.Empty,
                code = Status.success,
                count,
                data
            };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

       
        [HttpGet]
        public ActionResult GetUser(int id)
        {
            var data = UserInfoService.GetEntities(u => u.Id == id);
            var jsondata = new { Status.code, data };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserByPage()
        {
            int pageSize = int.Parse(Request["limit"] ?? "10");
            int pageIndex = int.Parse(Request["page"] ?? "1");
            int count = 0;
            //条件
            Expression<Func<UserInfo, bool>> WhereLambda = u => u.DelFlag == 1;
            //排序条件
            Expression<Func<UserInfo, DateTime>> OrderbyLambda = u => u.SubTime;
            var data = UserInfoService.GetEntitiesByPage(pageSize, pageIndex, out count, WhereLambda, OrderbyLambda, false);
            //为防止序列化类型为“”的对象时检测到循环引用的情况出现。需要对查询结果进行筛选
            var newdata = data.Select(u => new { u.Id, u.UserName, u.UserPwd, u.Remark, u.SubTime, u.DelFlag }).AsQueryable();
            var jsondata = new { Status.code, count, newdata };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            var nameList = UserInfoService.GetEntities(x => x.UserName == userInfo.UserName
            && x.DelFlag == 1);
            if(nameList.Count() > 0)
            {
                return Json(new
                {
                    msg = "用户名已存在，换个吧~~",
                    code = Status.error
                }
                , JsonRequestBehavior.AllowGet);
            }
            userInfo.SubTime = DateTime.Now;
            userInfo.DelFlag = 1;
            string userName = userInfo.UserName;
            string userPwd = userInfo.UserPwd;
            string tempPwd = $"{userName}{userPwd}";
            userInfo.UserName = userName;
            userInfo.UserPwd = MD5Helper.GenerateMD5(tempPwd);
            userInfo.Remark = userInfo.UserPwd;
            UserInfoService.Add(userInfo);
            var jsondata = new { Status.code };
            return Json(jsondata, JsonRequestBehavior.AllowGet);

        }

       
        [HttpPost]
        public ActionResult DeleteSingle(int id)
        {
            bool delflag = UserInfoService.DeleteSingle(id);
            var jsondata = new { delflag, Status.code };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMultiple(List<int> ids)
        {
            UserInfoService.DeleteMultiple(ids);
            var jsondata = new { count = ids.Count(), Status.code };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(UserInfo userInfo)
        {
            //同一个上下文不能缓存两个同一个主键的对象
            UserInfo oldUser = UserInfoService.GetEntities(u => u.Id == userInfo.Id).FirstOrDefault();
            oldUser.UserName = userInfo.UserName;
            oldUser.UserPwd = userInfo.UserPwd;
            oldUser.Remark = userInfo.Remark;
            bool updateflag = UserInfoService.Update(oldUser);
            var jsondata = new { updateflag, Status.code };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
    }
}