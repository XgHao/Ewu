﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.Domain.Db;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Models.ViewModel;
using Ewu.WebUI.HtmlHelpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 物品控制器
    /// </summary>
    public class TreasureController : Controller
    {
        private ITreasuresRepository repository;    //定义的物品储存库
        private IAuthProvider authProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treasuresRepository">传入储存库</param>
        public TreasureController(ITreasuresRepository treasuresRepository,IAuthProvider auth)
        {
            repository = treasuresRepository;
            authProvider = auth;
        }

        /// <summary>
        /// 物品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Authorize]
        public ViewResult List(string category, int page = 1, int PageSize = 12)
        {
            //1.首先获取当前条件下的所有物品集合
            var Treasures = repository.Treasures
                                //筛选-1.当前类或者类型为空的 2.不能选择图片为空的(图片为空当作未完成项)
                                .Where(t => (category == null || t.TreasureType == category) && (t.Cover != null && t.DetailPic != null))
                                .OrderBy(t => t.TreasureName)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize);

            //新建一个List
            List<TreasureAndHolderInfo> treasureAndHolders = new List<TreasureAndHolderInfo>();
            //遍历物品集合，填充数据
            foreach (var trea in Treasures)
            {
                AppUser holder = UserManager.FindById(trea.HolderID);
                treasureAndHolders.Add(new TreasureAndHolderInfo
                {
                    Treasure = trea,
                    Holder = holder
                });
            }


            //生成一个具体的列表视图模型
            TreasureListViewModel model = new TreasureListViewModel
            {
                //物品用户信息
                TreasureAndHolderInfos = treasureAndHolders,
                //分页信息
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //总页数，无选择分类这全部，否则按当前的分类
                    TotalItem = category == null
                                          ? repository.Treasures.Count()
                                          : repository.Treasures.Where(e => e.TreasureType == category).Count()
                },
                //当前分类
                CurrentCate = category,
                //当前用户信息
                CurrentUserInfo = CurrentUser
            };
            return View(model);
        }

        /// <summary>
        /// 个人物品页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MyList(string category, int page = 1, int PageSize = 6)
        {
            //根据页码以及分类来确定具体要显示的物品列表
            var Treasures = repository.Treasures
                                //筛选-1.类型为空或者当前类 2.是当前登录用户的物品 3.图片为空不显示
                                .Where(t => (category == null || t.TreasureType == category) && t.HolderID == CurrentUser.Id && (t.Cover != null && t.DetailPic != null))
                                .OrderBy(t => t.TreasureName)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize);

            //新建一个List
            List<TreasureAndHolderInfo> treasureAndHolders = new List<TreasureAndHolderInfo>();
            //遍历物品集合，填充数据
            foreach (var trea in Treasures)
            {
                AppUser holder = UserManager.FindById(trea.HolderID);
                treasureAndHolders.Add(new TreasureAndHolderInfo
                {
                    Treasure = trea,
                    Holder = holder
                });
            }

            //生成一个具体的列表视图模型
            TreasureListViewModel model = new TreasureListViewModel
            {
                TreasureAndHolderInfos = treasureAndHolders,
                //分页信息
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //总页数，无选择分类这全部，否则按当前的分类
                    TotalItem = category == null
                                          ? repository.Treasures.Count()
                                          : repository.Treasures.Where(e => e.TreasureType == category).Count()
                },
                //当前分类
                CurrentCate = category,
                //当前用户信息
                CurrentUserInfo = CurrentUser
            };
            return View(model);
        }

        /// <summary>
        /// 物品详情页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TreasureInfo(string TreasureUID = "")
        {
            if (!string.IsNullOrEmpty(TreasureUID))
            {
                Guid Treasureguid = Guid.Parse(TreasureUID);
                Treasure treasure = repository.Treasures.Where(t => t.TreasureUID == Treasureguid).FirstOrDefault();
                var imgs = treasure.DetailPic.Split('|');
                if (treasure != null)
                {
                    //定义一个视图模型
                    TreaInfo treaInfo = new TreaInfo
                    {
                        HolderInfo = GetLoginUserInfo(treasure.HolderID),
                        LoginUserInfo = CurrentUser,
                        treasureInfo = treasure,
                        //108是生成图片路径的固定的长度
                        DetailImgs = imgs.Where(t => t.Length == 108)
                    };
                    return View(treaInfo);
                }
            }
            return View("Error");
        }


        /// <summary>
        /// 发布新的物品
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UploadItem()
        {
            #region 物品类别集合
            IEnumerable<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="网络设备",Value="网络设备" },
                new SelectListItem(){ Text="电脑配件",Value="电脑配件" },
                new SelectListItem(){ Text="图书画册",Value="图书画册" },
                new SelectListItem(){ Text="电子产品",Value="电子产品" },
                new SelectListItem(){ Text="其他",Value="其他" }
            };
            types = DropListHelper.SetDefault(types, "其他");
            Session["Types"] = types;
            #endregion

            #region 物品成色集合
            IEnumerable<SelectListItem> damageDegree = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="全新",Value="全新" },
                new SelectListItem(){ Text="九八新",Value="九八新" },
                new SelectListItem(){ Text="九五新",Value="九五新" },
                new SelectListItem(){ Text="九成新",Value="九成新" },
                new SelectListItem(){ Text="八五新",Value="八五新" },
                new SelectListItem(){ Text="八成新",Value="八成新" },
                new SelectListItem(){ Text="七成新",Value="七成新" },
                new SelectListItem(){ Text="七成及以下",Value="七成及以下" },
            };
            damageDegree = DropListHelper.SetDefault(damageDegree, "全新");
            Session["DamageDegrees"] = damageDegree;
            #endregion

            #region 物品交易范围集合
            IEnumerable<SelectListItem> tradeRange = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="市内",Value="市内" },
                new SelectListItem(){ Text="省内",Value="省内" },
                new SelectListItem(){ Text="临近省",Value="临近省" },
                new SelectListItem(){ Text="全国(港澳台除外)",Value="全国" },
                new SelectListItem(){ Text="不限",Value="不限" }
            };
            tradeRange = DropListHelper.SetDefault(tradeRange, "不限");
            Session["TradeRanges"] = tradeRange;
            #endregion

            Treasure treasure = new Treasure()
            {
                TreasureUID = Guid.NewGuid(),
                HolderID = CurrentUser.Id,
            };
            return View(treasure);
        }

        /// <summary>
        /// 发布新的物品[HttpPost]
        /// </summary>
        /// <param name="treasure"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult UploadItem(Treasure treasure)
        {
            if (ModelState.IsValid)
            {
                #region 数据初始化
                treasure.BrowseNum = 0;
                treasure.Favorite = 0;
                treasure.UpdateTime = DateTime.Now;
                treasure.UploadTime = DateTime.Now;
                treasure.EditCount = 0;
                treasure.Link = "/Treasure/TreasureInfo?TreasureUID=" + treasure.TreasureUID.ToString();
                if (string.IsNullOrEmpty(treasure.Remarks))
                {
                    treasure.Remarks = "无";
                }
                #endregion
                repository.SaveTreasure(treasure);
                UploadImgs uploadImgs = new UploadImgs
                {
                    TreasureUID = treasure.TreasureUID.ToString(),
                    UserID = treasure.HolderID,
                    TreasureName = treasure.TreasureName
                };

                //再跳转到上传图片页面前，要先清空原来的图片路径
                if (DropListHelper.DeletePic(treasure.TreasureUID))
                {
                    return View("UpLoadImg", uploadImgs);
                }
            }
            return View(treasure);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpLoadImg()
        {
            return View();
        }

        /// <summary>
        /// 获取图片
        /// FileContentResult将二进制文件的内容发送到响应
        /// </summary>
        /// <param name="treasureUID">操作的物品对象GUID</param>
        /// <returns></returns>
        public FileContentResult GetImage(Guid treasureUID)
        {
            //根据GUID获取对象
            Treasure trea = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            if (trea != null)
            {
                return File(trea.ImageData, trea.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取物品所有者用户信息
        /// </summary>
        /// <param name="holderid">用户ID</param>
        /// <returns></returns>
        public AppUser GetLoginUserInfo(string holderid)
        {
            return UserManager.FindById(holderid);
        }

        /// <summary>
        /// 错误视图
        /// </summary>
        /// <returns></returns>
        public ViewResult Error()
        {
            return View();
        }

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