﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ewu.Domain.Entities
{
    public class Treasure
    {
        [Key]
        public Guid TreasureUID { get; set; }     //物品唯一标识

        public string HolderID { get; set; }        //所有者ID
        public string TreasureName { get; set; }    //物品名称
        public string TreasureType { get; set; }    //物品类别
        public string DamageDegree { get; set; }    //损坏程度
        public string TradeRange { get; set; }      //交易范围
        public string Cover { get; set; }           //封面图片
        public string DetailPic { get; set; }       //补充图片
        public string DetailContent { get; set; }   //补充说明
        public DateTime UploadTime { get; set; }    //上传时间
        public DateTime UpdateTime { get; set; }    //最后更新时间
        public int Favorite { get; set; }           //收藏数
        public string Link { get; set; }            //物品的详情页
        public int BrowseNum { get; set; }          //物品的浏览量
    }
}
