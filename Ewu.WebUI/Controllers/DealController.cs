﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Concrete;
using Ewu.Domain.Db;
using Ewu.Domain.Entities;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models.ViewModel;
using Ewu.WebUI.API;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Linq.SqlClient;

namespace Ewu.WebUI.Controllers
{
    [Authorize]
    public class DealController : Controller
    {
        private ITreasuresRepository repository;
        private IAuthProvider authProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treasuresRepository"></param>
        /// <param name="auth"></param>
        public DealController(ITreasuresRepository treasuresRepository,IAuthProvider auth)
        {
            repository = treasuresRepository;
            authProvider = auth;
        }

        #region 订单创建
        /// <summary>
        /// 创建交易
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateDeal(string TreasureUID = "")
        {
            //根据id生成对象的GUID
            Guid TreasureGuid = Guid.Parse(TreasureUID);

            if (!string.IsNullOrEmpty(TreasureUID))
            {
                //获取当前选择物品的所属人
                string holderid= repository.Treasures
                                        .Where(t => t.TreasureUID == TreasureGuid)
                                        .FirstOrDefault().HolderID;

                if (!string.IsNullOrEmpty(holderid))
                {
                    //当前登录用户所有的物品
                    var userTrea = repository.Treasures.Where(t => t.HolderID == CurrentUser.Id);

                    //生成对应视图模型
                    DealInfo dealInfo = new DealInfo
                    {
                        //当前物品的所属人对象
                        Holder = UserManager.FindById(holderid),
                        //当前物品对象
                        DealTreasure = repository.Treasures.Where(t => t.TreasureUID == TreasureGuid).FirstOrDefault(),
                        //交易-当前登录用户物品集合-模型
                        DealMyTreasureModel = new DealMyTreasureModel
                        {
                            DealTreasureGuid = TreasureGuid,
                            UserTreasures = userTrea
                        }
                    };
                    return View(dealInfo);
                }
            }
            return View();
        }

        /// <summary>
        /// 生成交易记录
        /// 交易记录状态：
        /// 1.待确认：发起交易阶段---订单未结束
        /// 2.拒绝：对方拒绝交易---订单已结束
        /// 3.接受：对方接受交易---订单未结束
        /// 4.交易中：双方交易阶段---订单未结束
        /// 5.交易失败：交易双方中有一方无法完成交易---订单已结束
        /// 6.交易成功：交易成功，进入评价阶段---订单已结束
        /// 7.交易取消：发起人在接受人 接受/拒绝 之前取消交易---订单已结束
        /// </summary>
        /// <param name="TreasureSponsorID">发起人物品UID</param>
        /// <param name="TreasureRecipientID">接收人物品UID</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult MakeDeal(string TreasureSponsorID, string TreasureRecipientID)
        {
            //参数为空，报错
            if (string.IsNullOrEmpty(TreasureSponsorID) || string.IsNullOrEmpty(TreasureRecipientID))
            {
                return View("Error","出错");
            }

            //首先要先验证该订单是不是出现过
            using (var db=new LogDealDataContext())
            {
                //获取与当前交易相同的记录
                var deallogs = db.LogDeal.Where(d => d.TreasureSponsorID == TreasureSponsorID && d.TreasureRecipientID == TreasureRecipientID);
                //遍历记录
                foreach(var dlog in deallogs)
                {
                    //当记录中出现以下哞种情况（即订单未完成），说明当前交易订单还在处理，这不允许创建订单，返回Error页面
                    if (dlog.DealStatus == "待确认" || dlog.DealStatus == "接受" || dlog.DealStatus == "交易中")
                    {
                        return View("Error","订单已存在，请勿重复申请！");
                    }
                }
            }
            
            //当前登录用户UID
            string CurId = CurrentUser.Id;

            //这里验证当前发起人的物品是当前用户的，并且验证接收人的物品不是当前用户的
            var treaS = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(TreasureSponsorID)).FirstOrDefault();
            var treaR = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(TreasureRecipientID)).FirstOrDefault();
            if (treaR != null & treaS != null)
            {
                //确保发起物品是登录用户的，接受物品不是当前用户的
                if(treaS.HolderID == CurId && treaR.HolderID != CurId)
                {
                    //发起人id-当前登录人
                    string TraderSponsorID = CurId;
                    //接收人id-从物品获取
                    string TraderRecipientID = repository.Treasures
                                                        .Where(t => t.TreasureUID == Guid.Parse(TreasureRecipientID))
                                                        .FirstOrDefault().HolderID;
                    DealLogCreate dealLog = new DealLogCreate();

                    #region 生成视图模型
                    dealLog.DealInTreasure = treaR;
                    dealLog.DealOutTreasure = treaS;
                    dealLog.QQ = (string.IsNullOrEmpty(CurrentUser.OICQ)) ? "TA没有完善该信息" : CurrentUser.OICQ;
                    dealLog.WeChat = (string.IsNullOrEmpty(CurrentUser.WeChat)) ? "TA没有完善该信息" : CurrentUser.WeChat;
                    dealLog.Email = CurrentUser.Email;
                    dealLog.PhoneNum = CurrentUser.PhoneNumber;
                    #endregion

                    return View(dealLog);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// 生成交易记录[HttpPost]
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult MakeDeal(DealLogCreate dealLogCreate)
        {
            //验证不为空
            if (string.IsNullOrEmpty(dealLogCreate.DealInTreasure.TreasureUID.ToString()) || string.IsNullOrEmpty(dealLogCreate.DealOutTreasure.TreasureUID.ToString()))
            {
                return View("Error");
            }
            else
            {
                Guid guid = Guid.NewGuid();
                //插入数据库
                using (var db = new LogDealDataContext())
                {
                    LogDeal logDeal = new LogDeal
                    {
                        DealBeginTime = DateTime.Now,
                        DealStatus = "待确认",
                        DLogUID = guid,
                        //备注-发起人对接收人
                        RemarkSToR = dealLogCreate.Remark,
                        //交易接收人ID
                        TraderRecipientID = dealLogCreate.DealInTreasure.HolderID,
                        //交易发起人ID
                        TraderSponsorID = dealLogCreate.DealOutTreasure.HolderID,
                        //交易给出物品ID
                        TreasureSponsorID = dealLogCreate.DealOutTreasure.TreasureUID.ToString(),
                        //交易接受物品ID
                        TreasureRecipientID = dealLogCreate.DealInTreasure.TreasureUID.ToString()
                    };
                    try
                    {
                        db.LogDeal.InsertOnSubmit(logDeal);
                        //保存操作
                        db.SubmitChanges();

                        //更新当前物品交易记录
                        var treasure = repository.Treasures.Where(t => t.TreasureUID == dealLogCreate.DealOutTreasure.TreasureUID).FirstOrDefault();
                        treasure.DLogUID = guid.ToString();
                        repository.SaveTreasure(treasure);
                    }
                    catch(Exception ex)
                    {
                        return View("Error", ex.Message);
                    }
                }
                return RedirectToAction("InitiateDealLog", "Account");
            }
        }
        #endregion


        #region 订单进行状态
        /// <summary>
        /// 修改备注信息-发起的申请
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditRemarks(string DLogUID = "")
        {
            if (string.IsNullOrEmpty(DLogUID))
            {
                return View("Error");
            }
            //获取当前交易信息
            LogDeal deal = new LogDeal();
            using(var db=new LogDealDataContext())
            {
                deal = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
            }

            //换入物品
            var treaR = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID))
                                .FirstOrDefault();
            //换出物品
            var treaS = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID))
                                .FirstOrDefault();
            if (treaR != null && treaS != null)
            {
                return View(new DealLogCreate
                {
                    DealInTreasure = treaR,
                    DealOutTreasure = treaS,
                    Remark = deal.RemarkSToR,
                    DealLogID = DLogUID
                });
            }

            return View("Error");
        }

        /// <summary>
        /// 修改备注信息-发起的申请[HttpPost]
        /// </summary>
        /// <param name="dealLogCreate"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult EditRemarks(DealLogCreate dealLogCreate)
        {
            using(var db = new LogDealDataContext())
            {
                //修改备注信息
                var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(dealLogCreate.DealLogID)).FirstOrDefault();
                if (log != null)
                {
                    log.RemarkSToR = dealLogCreate.Remark;
                }
                db.SubmitChanges();
            }
            return RedirectToAction("InitiateDealLog", "Account");
        }

        /// <summary>
        /// 取消交易-发起的交易
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult CancelDeal(string DealLogUID = "")
        {
            string result = "Fail";
            if (!string.IsNullOrEmpty(DealLogUID))
            {
                using(var db=new LogDealDataContext())
                {
                    //更改订单状态
                    var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DealLogUID)).FirstOrDefault();
                    if (log != null)
                    {
                        log.DealStatus = "交易取消";
                        log.DealEndTime = DateTime.Now;
                        db.SubmitChanges();

                        //删除物品中的订单号
                        var treaS = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureSponsorID)).FirstOrDefault();
                        treaS.DLogUID = null;
                        repository.SaveTreasure(treaS);
                        result = "OK";
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 接受申请
        /// </summary>
        /// <returns></returns>
        public ActionResult AgreeDeal(string DLogUID = "")
        {
            //首先改变订单状态
            using (var db = new LogDealDataContext())
            {
                var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
                if (log != null)
                {
                    log.DealStatus = "交易中";
                    db.SubmitChanges();

                    //接受申请后，两个物品需要加入订单号
                    var treasureR = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureRecipientID)).FirstOrDefault();
                    var treasureS = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureSponsorID)).FirstOrDefault();
                    if (treasureR != null)
                    {
                        treasureR.DLogUID = log.DLogUID.ToString();
                        repository.SaveTreasure(treasureR);
                    }
                    if (treasureS != null)
                    {
                        treasureS.DLogUID = log.DLogUID.ToString();
                        repository.SaveTreasure(treasureS);
                    }

                }
            }
            //使其他包含这两样物品的交易申请失效


            //在物流信息中添加一项
            using (var db=new trackingDataContext())
            {
                //当前订单号不存在时
                if (db.Tracking.Where(t => t.DLogUID == DLogUID).FirstOrDefault() == null)
                {
                    db.Tracking.InsertOnSubmit(new Tracking
                    {
                        DLogUID = DLogUID
                    });
                    db.SubmitChanges();
                }
            }

            return RedirectToAction("DealingLog", "Account");
        }


        /// <summary>
        /// 拒绝申请-我收到的交易申请
        /// </summary>
        /// <returns></returns>
        public ActionResult DisagreeDeal(string DLogUID="")
        {
            if (string.IsNullOrEmpty(DLogUID))
            {
                return View("Error");
            }
            //获取当前交易信息
            using (var db = new LogDealDataContext())
            {
                var deal = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();

                //换入物品-对于接收人来说，换入物品是本次申请的发起人物品
                var treaR = repository.Treasures
                                    .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID))
                                    .FirstOrDefault();
                //换出物品-对于接收人来说，换出物品是本次申请的接收人物品
                var treaS = repository.Treasures
                                    .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID))
                                    .FirstOrDefault();

                //相似推荐
                //获取相似物品
                var Moretreas = repository.Treasures
                                        .Where(t => t.TreasureType == treaR.TreasureType).OrderBy(t => t.Favorite).Take(3);

                //过滤是当前用户的
                Moretreas.Where(t => t.HolderID != CurrentUser.Id);

                //根据七天的收藏量
                using (var db2 = new FavoriteDataContext())
                {
                    var FavoriteTrea = db2.Favorite.Where(f => (SqlMethods.DateDiffDay(f.FavoriteTime, DateTime.Now) <= 7)).Select(f => f.TreasureID);
                    if (treaR != null && treaS != null)
                    {
                        return View(new DealLogCreate
                        {
                            DealInTreasure = treaR,
                            DealOutTreasure = treaS,
                            Remark = deal.RemarkSToR,
                            DealLogID = DLogUID,
                            MoreTreasures = Moretreas.AsEnumerable()
                        });
                    }
                }
            }
            return View("Error");
        }

        /// <summary>
        /// 拒绝申请[HttpPost]
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult DisagreeDeal(DealLogCreate dealLogCreate)
        {
            //更新物品信息
            //发起人的物品-因为发起人的物品在发起申请时就会记录订单号，所以拒绝后清空订单号，以便可以进行其他交易
            //接收人的物品-接收人在接受申请时，物品不会被记录订单号，即原来就可以自由交易，只有在接受交易或者自己发起交易后才会记录订单号
            var treaS = repository.Treasures.Where(t => t.TreasureUID == dealLogCreate.DealInTreasure.TreasureUID).FirstOrDefault();
            treaS.DLogUID = null;
            repository.SaveTreasure(treaS);

            //更新本次订单的记录
            using (var db = new LogDealDataContext())
            {
                var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(dealLogCreate.DealLogID)).FirstOrDefault();
                log.RemarkRToS = dealLogCreate.Remark;
                log.DealStatus = "拒绝";
                log.DealEndTime = DateTime.Now;
                db.SubmitChanges();
            }

            return RedirectToAction("AllDealLog", "Account");
        }
        #endregion


        #region 订单交易阶段
        /// <summary>
        /// 选择收货信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseDeliveryAddress(string DLogUID = "", string CurrentRole = "")
        {
            if (!string.IsNullOrEmpty(DLogUID) && !string.IsNullOrEmpty(CurrentRole)) 
            {
                //获取当前用户ID
                string CurrentID = CurrentUser.Id;

                //获取当前用户的收货地址
                using(var db2 = new LogDealDataContext())
                {
                    //获取当前交易订单
                    var logdeal = db2.LogDeal.Where(l => l.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
                    if (logdeal != null)
                    {
                        MyDeliveryAddress myDeliveryAddress = new MyDeliveryAddress
                        {
                            CurrentLogDeal = logdeal,
                            CurrentRole = CurrentRole,
                            NewdeliveryAddress=new DeliveryAddress()
                        };

                        //返回视图模型
                        return View(myDeliveryAddress);
                    }
                }
            }
            return View("Error");
        }

        /// <summary>
        /// 设置收货地址
        /// </summary>
        /// <returns></returns>
        public JsonResult SetDeliveryAddress()
        {
            //获取信息
            string role = Request["CurrentRole"];
            string LogDealUID = Request["DLogUID"];
            string DeliveryAddressUID = Request["DeliveryAddressUID"];
            
            //初始化结果
            string result = "Error";

            if (!string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(LogDealUID) && !string.IsNullOrEmpty(DeliveryAddressUID))
            {
                //添加收货信息
                using (var db = new LogDealDataContext())
                {
                    var log = db.LogDeal.Where(l => l.DLogUID == Guid.Parse(LogDealUID)).FirstOrDefault();
                    if (log != null)
                    {
                        //当前登录用户是接收人
                        if(role== "Recipient")
                        {
                            log.DeliveryAddressRecipientID = DeliveryAddressUID;
                            db.SubmitChanges();
                            result = "OK";
                        }
                        //当前登录用户是发起人
                        else if(role== "Sponsor")
                        {
                            log.DeliveryAddressSponsorID = DeliveryAddressUID;
                            db.SubmitChanges();
                            result = "OK";
                        }
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 废除收货地址
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteDeliveryAddress()
        {
            //获取信息
            string DeliveryAddressUID = Request["DeliveryAddressUID"];

            //初始化结果
            string result = "Error";

            if (!string.IsNullOrEmpty(DeliveryAddressUID))
            {
                using (var db = new DeliveryAddressDataContext())
                {
                    var address = db.DeliveryAddress.Where(a => a.DeliveryAddressUID == DeliveryAddressUID).FirstOrDefault();
                    if (address != null)
                    {
                        address.IsRepeal = true;
                        db.SubmitChanges();
                        result = "OK";
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 增加收货地址
        /// </summary>
        /// <param name="deliveryAddress"></param>
        /// <returns></returns>
        public ActionResult AddDeliveryAddress(MyDeliveryAddress myDeliveryAddress)
        {
            //首先判断信息是否为空
            if((!string.IsNullOrEmpty(myDeliveryAddress.NewdeliveryAddress.LocationCity))&& (!string.IsNullOrEmpty(myDeliveryAddress.NewdeliveryAddress.LocationDistrict)) && (!string.IsNullOrEmpty(myDeliveryAddress.NewdeliveryAddress.LocationProvince)) && (!string.IsNullOrEmpty(myDeliveryAddress.NewdeliveryAddress.MoreLocation)) && (!string.IsNullOrEmpty(myDeliveryAddress.CurrentRole)))
            {
                //获取当前用户ID,联系方式,真实姓名
                string userid = CurrentUser.Id;
                string phoneNum = CurrentUser.PhoneNumber;
                string realName = CurrentUser.RealName;
                //增加收货地址
                using(var db = new DeliveryAddressDataContext())
                {
                    db.DeliveryAddress.InsertOnSubmit(new DeliveryAddress
                    {
                        DeliveryAddressUID = Guid.NewGuid().ToString(),
                        LocationCity = myDeliveryAddress.NewdeliveryAddress.LocationCity,
                        LocationDistrict = myDeliveryAddress.NewdeliveryAddress.LocationDistrict,
                        LocationProvince = myDeliveryAddress.NewdeliveryAddress.LocationProvince,
                        MoreLocation = myDeliveryAddress.NewdeliveryAddress.MoreLocation,
                        PhoneNum = phoneNum,
                        RealName = realName,
                        UserUID = userid,
                        IsRepeal = false
                    });
                    db.SubmitChanges();
                    return View("ChooseDeliveryAddress");
                }
            }

            return View("Error","错误");
        }

        /// <summary>
        /// 收货地址列表
        /// </summary>
        /// <returns></returns>
        public PartialViewResult DeliveryAddressList()
        {
            //获取当前用户ID
            string uid = CurrentUser.Id;
            using(var db = new DeliveryAddressDataContext())
            {
                var lists = db.DeliveryAddress.Where(d => (d.UserUID == uid) && (d.IsRepeal == false)).ToList();
                return PartialView(lists.AsEnumerable());
            }
        }
        #endregion


        #region 物流信息
        /// <summary>
        /// 填写物流单号
        /// </summary>
        /// <param name="DLogUID">订单号</param>
        /// <param name="CurrentRole">当前角色</param>
        /// <returns></returns>
        public ActionResult FillDeliveryNum(string DLogUID = "", string CurrentRole = "")
        {
            //信息不为空
            if (!string.IsNullOrEmpty(DLogUID) && !string.IsNullOrEmpty(CurrentRole))
            {
                return View(new DeliveryNum
                {
                    DLogUID = DLogUID,
                    CurrentRole = CurrentRole
                });
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 查看收货地址
        /// </summary>
        /// <param name="DLogUID"></param>
        /// <returns></returns>
        public ActionResult ViewDeliveryInfo(string DLogUID = "")
        {
            return View();
        }

        /// <summary>
        /// 查询物流单号
        /// </summary>
        /// <returns></returns>
        public JsonResult InquireDeliveryNum()
        {
            string DeliveryNum = Request["DeliveryNum"];
            string DLogUID = Request["DLogUID"];
            string CurrentRole = Request["CurrentRole"];

            Delivery delivery = new Identity().GetDeliveryInfo(DeliveryNum);

            //查询成功
            if (delivery.msg == "查询成功")
            {
                //保存物流信息
                using (var db = new trackingDataContext())
                {
                    var log = db.Tracking.Where(l => l.DLogUID == DLogUID).FirstOrDefault();
                    if (log != null)
                    {
                        //角色身份
                        if (CurrentRole == "Recipient")
                        {
                            log.RecipientTrackingNum = delivery.No;
                            log.RecipientTrackingDate = DateTime.Now;
                        }
                        else if (CurrentRole == "Sponsor")
                        {
                            log.SponsorTrackingNum = delivery.No;
                            log.SponsorTrackingDate = DateTime.Now;
                        }
                    }
                    db.SubmitChanges();
                }
            }

            return Json(delivery.msg,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看物流信息
        /// </summary>
        /// <param name="DLogUID">订单号</param>
        /// <param name="Role">查看物流的角色</param>
        /// <returns></returns>
        public ActionResult DeliveryInfo(string DLogUID="", string Role="")
        {
            //获取当前登录人的ID
            string userId = CurrentUser.Id;

            //当前物流单号
            string DeliveryNum = string.Empty;

            //获取订单信息
            using(var db = new LogDealDataContext())
            {
                var log = db.LogDeal.Where(l => l.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
                if (log != null)
                {
                    using(var db2 = new trackingDataContext())
                    {
                        //获取物流对象
                        var tracking = db2.Tracking.Where(t => t.DLogUID == DLogUID).FirstOrDefault();

                        //从订单信息中获取当前用户在本次交易中的角色
                        //是接收人
                        if (log.TraderRecipientID == userId)
                        {
                            //从Role中判断，用户要查询自己的还是对方的物流信息
                            if (Role == "Ta")
                            {
                                DeliveryNum = tracking.SponsorTrackingNum ?? "";
                            }
                            else if (Role == "My")
                            {
                                DeliveryNum = tracking.RecipientTrackingNum ?? "";
                            }
                        }
                        //发起人
                        else if (log.TraderSponsorID == userId)
                        {
                            //从Role中判断，用户要查询自己的还是对方的物流信息
                            if (Role == "Ta")
                            {
                                DeliveryNum = tracking.RecipientTrackingNum ?? "";
                            }
                            else if (Role == "My")
                            {
                                DeliveryNum = tracking.SponsorTrackingNum ?? "";
                            }
                        }
                    }
                    
                }
                else
                {
                    return View("Error", "错误");
                }
            }

            //查询物流信息
            if (!string.IsNullOrEmpty(DeliveryNum))
            {
                Delivery delivery = new Identity().GetDeliveryInfo(DeliveryNum);
                return View(delivery);
            }

            return View("Error","错误");
        }

        #endregion


        #region 评价阶段
        /// <summary>
        /// 评价-确认收货
        /// </summary>
        /// <param name="DLogUID"></param>
        /// <returns></returns>
        public ActionResult Signing(string DLogUID = "")
        {
            //获取当前用户的uid
            string userId = CurrentUser.Id;

            //获取本次交易订单对象
            if (!string.IsNullOrEmpty(DLogUID))
            {
                using(var db = new LogDealDataContext())
                {
                    var log = db.LogDeal.Where(l => l.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();

                    if (log != null)
                    {
                        //获取对方在本次交易中的对象
                        AppUser SideUser = UserManager.FindById(log.TraderSponsorID == userId ? log.TraderRecipientID : log.TraderSponsorID);
                        //获取对方在本次交易中的物品对象
                        Treasure treasure = repository.Treasures
                                                      .Where(t => t.TreasureUID == Guid.Parse(log.TraderSponsorID == userId ? log.TreasureRecipientID : log.TreasureSponsorID))
                                                      .FirstOrDefault();
                        if (SideUser != null && treasure != null)
                        {
                            return View(new Score
                            {
                                CurTreasure = treasure,
                                LogDeal = log,
                                SideUser = new BasicUserInfo
                                {
                                    RealName = SideUser.RealName,
                                    UserID = SideUser.Id,
                                    UserName = SideUser.UserName,
                                    HeadImg = SideUser.HeadPortrait
                                },
                                NowEvaluation = new NowEvaluation { IsRecommend = true },
                            });
                        }
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        /// 评价-确认收货[httppost]
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Signing(Score score)
        {
            if (!string.IsNullOrEmpty(score.LogDeal.DLogUID.ToString()))
            {
                //首先判断当前用户在本次订单中的身份
                string userid = CurrentUser.Id;

                //增加评论记录
                using (var db = new EvaluationDataContext())
                {
                    using(var db2 = new LogDealDataContext())
                    {
                        //获取记录对象
                        var log = db2.LogDeal.Where(l => l.DLogUID == score.LogDeal.DLogUID).FirstOrDefault();
                        //首先判断当前订单号的记录是否已经存在
                        var eveluation = db.Evaluation.Where(e => e.DLogUID == score.LogDeal.DLogUID.ToString()).FirstOrDefault();

                        if (log != null)
                        {
                            //根据当前用户的身份插入评论记录
                            if (userid == log.TraderRecipientID)
                            {
                                //不存在，则插入新的数据
                                if (eveluation == null)
                                {
                                    db.Evaluation.InsertOnSubmit(new Evaluation
                                    {
                                        DLogUID = score.LogDeal.DLogUID.ToString(),
                                        EvaluationRToS = score.NowEvaluation.EvaluationInfo,
                                        EvaTimeRToS = DateTime.Now,
                                        IsRecommendRToS = score.NowEvaluation.IsRecommend
                                    });
                                }
                                //存在，则更新
                                else
                                {
                                    eveluation.EvaluationRToS = score.NowEvaluation.EvaluationInfo;
                                    eveluation.EvaTimeRToS = DateTime.Now;
                                    eveluation.IsRecommendRToS = score.NowEvaluation.IsRecommend;
                                }
                            }
                            else if (userid == log.TraderSponsorID)
                            {
                                //不存在
                                if (eveluation == null)
                                {
                                    db.Evaluation.InsertOnSubmit(new Evaluation
                                    {
                                        DLogUID = score.LogDeal.DLogUID.ToString(),
                                        EvaluationSToR = score.NowEvaluation.EvaluationInfo,
                                        EvaTimeSToR = DateTime.Now,
                                        IsRecommendSToR = score.NowEvaluation.IsRecommend
                                    });
                                }
                                //存在更新
                                else
                                {
                                    eveluation.EvaluationSToR = score.NowEvaluation.EvaluationInfo;
                                    eveluation.EvaTimeSToR = DateTime.Now;
                                    eveluation.IsRecommendSToR = score.NowEvaluation.IsRecommend;
                                }
                                
                            }

                            //判断评价信息是否双方都完成-如果评价信息原本就存在，说明双方都评价完成了
                            if (eveluation != null)
                            {
                                log.DealEndTime = DateTime.Now;
                                log.DealStatus = "交易成功";
                                db2.SubmitChanges();
                            }
                        }
                    }
                    //保存数据
                    db.SubmitChanges();
                    return RedirectToAction("DealingLog", "Account");
                }
            }
            return View("Error","出错");
        }
        #endregion

        #region 订单详情
        /// <summary>
        /// 订单详情信息
        /// </summary>
        /// <returns></returns>
        public ActionResult DealAllInfo(string DLogUID = "")
        {
            if (!string.IsNullOrEmpty(DLogUID))
            {
                //获取订单对象
                using(var db = new LogDealDataContext())
                {
                    var log = db.LogDeal.Where(l => l.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
                    if (log != null)
                    {
                        #region 获取物品对象
                        var TreasureS = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureSponsorID)).FirstOrDefault();
                        var TreasureR = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureRecipientID)).FirstOrDefault();
                        #endregion

                        #region 获取收货信息
                        DeliveryAddress deliveryAddressS = new DeliveryAddress();
                        DeliveryAddress deliveryAddressR = new DeliveryAddress();
                        using (var db2 = new DeliveryAddressDataContext())
                        {
                            deliveryAddressS = db2.DeliveryAddress.Where(d => d.DeliveryAddressUID == log.DeliveryAddressSponsorID).FirstOrDefault();
                            deliveryAddressR = db2.DeliveryAddress.Where(d => d.DeliveryAddressUID == log.DeliveryAddressRecipientID).FirstOrDefault();
                        }
                        #endregion

                        #region 用户信息
                        BasicUserInfo basicUserInfoS = new BasicUserInfo();
                        BasicUserInfo basicUserInfoR = new BasicUserInfo();
                        var userS = UserManager.FindById(log.TraderSponsorID);
                        var userR = UserManager.FindById(log.TraderRecipientID);
                        if (userS != null && userR != null)
                        {
                            basicUserInfoS.HeadImg = userS.HeadPortrait;
                            basicUserInfoS.RealName = userS.RealName;
                            basicUserInfoS.PhoNum = userS.PhoneNumber;
                            basicUserInfoS.Email = userS.Email;

                            basicUserInfoR.HeadImg = userR.HeadPortrait;
                            basicUserInfoR.RealName = userR.RealName;
                            basicUserInfoR.PhoNum = userR.PhoneNumber;
                            basicUserInfoR.Email = userR.Email;
                        }
                        #endregion

                        #region 评价信息
                        Evaluation evaluation = new Evaluation();
                        using(var db3 = new EvaluationDataContext())
                        {
                            evaluation = db3.Evaluation.Where(e => e.DLogUID == log.DLogUID.ToString()).FirstOrDefault();
                        }
                        #endregion

                        //返回视图模型
                        return View(new DealAllInfo
                        {
                            BasicUserInfoR = basicUserInfoR,
                            BasicUserInfoS = basicUserInfoS,
                            DeliveryAddressR = deliveryAddressR,
                            DeliveryAddressS = deliveryAddressS,
                            Evaluation = evaluation,
                            TreasureR = TreasureR,
                            TreasureS = TreasureS
                        });
                    }
                }
            }
            return View("Error");
        }
        #endregion

        /// <summary>
        /// 获取当前用户
        /// </summary>
        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }

        /// <summary>
        /// 因为在实现不用的管理功能时，会反复使用APpUserManager类。所以定义UserManager以方便
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                //Microsoft.Owin.Host.SystemWeb程序集为HttpContext类添加了一些扩展方法，其中之一便是GetOwinContext
                //GetOwinContext通过IOwinContext对象，将基于请求的上下文对象提供给OWIN API
                //在这其中有一个扩展方法GetUserManager<T>，可以用来得到用户管理器类实例
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}