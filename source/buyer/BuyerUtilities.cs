using Exchangebase.Com.Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Exchangebase.Com.Buyer
{
    public static class BuyerUtilities
    {
        public static string GetgrdIDFilterValue(List<int> grdIDList)
        {
            string grdIDFilterValue = "";
            foreach (int grdID in grdIDList)
            {
                grdIDFilterValue += grdID + ",";
            }

            if (grdIDFilterValue.Length > 0)
            {
                grdIDFilterValue = grdIDFilterValue.Remove(grdIDFilterValue.Length - 1);
            }
            return grdIDFilterValue;
        }

        public static string GetgrdIDFilterValueWhere(string grdIDFilterValue)
        {
            string where = "";

            if (grdIDFilterValue.Length > 0)
            {
                where = " AND grdID IN (" + grdIDFilterValue + ")";
            }
            return where;
        }

        public static List<ActiveBuyerProject> GetActiveBuyerProjects(EXBAssetType exbAssetType, int usrID, int buyID = 0)
        {
            List<ActiveBuyerProject> activeBuyerProjects = new List<ActiveBuyerProject>();
            EXBDbConnection db = new EXBDbConnection();
            string sql = "SELECT distinct(itmID), iwaID, isgID, iwbAddedByUser, iwbSellPrice, itmQty, itmTotalFeet, astID FROM vwItemsWantedBids WHERE iwaActive = 1 AND astID = "
                + (int)exbAssetType + " AND iwbDisplay = 1 AND isPresented = 1 AND " + EXBUser.GetUserActiveCurrentlySourcingBreakthroughIwaCondition(usrID);
            DataTable dtWatchList = db.Query(sql);
            foreach (DataRow dr in dtWatchList.Rows)
            {
                var sourcingStage = UtilityTools.ParseItemsWantedSourcingStage(WebConvert.ToInt32(dr["isgID"], (int)EXBItemsWantedSourcingStage.Breakthrough));

                //only add item if it's a breakthrough project and item is added by user, or if it's item from currently sourcing.
                if (sourcingStage == EXBItemsWantedSourcingStage.CurrentlySourcing || WebConvert.ToBoolean(dr["iwbAddedByUser"], false))
                {
                    activeBuyerProjects.Add(new ActiveBuyerProject(
                        WebConvert.ToInt32(dr["itmID"], 0),
                        WebConvert.ToInt32(dr["iwaID"], 0),
                        sourcingStage, WebConvert.ToDecimal(dr["iwbSellPrice"], 0), WebConvert.ToDecimal(dr["itmQty"], 0), WebConvert.ToInt32(dr["itmTotalFeet"], 0),
                        UtilityTools.ParseAssetType(WebConvert.ToInt32(dr["astID"], (int)EXBAssetType.Equipment))));
                }
            }
            return activeBuyerProjects;
        }

        public static List<ActiveBuyerProject> GetAllFeaturedActiveBuyerProjects(int usrID, int buyID)
        {
            List<ActiveBuyerProject> activeBuyerProjects = new List<ActiveBuyerProject>();
            EXBDbConnection db = new EXBDbConnection();
            string sql = "SELECT distinct(itmID), iwaID, isgID, iwbAddedByUser, iwbSellPrice, itmQty, itmTotalFeet, astID FROM vwItemsWantedBids WHERE iwaActive = 1 "
                 + " AND iwbDisplay = 1 AND isPresented = 1 AND itmIsFeatured = 1 AND " + EXBUser.GetUserActiveCurrentlySourcingBreakthroughIwaCondition(usrID);
            DataTable dtWatchList = db.Query(sql);
            foreach (DataRow dr in dtWatchList.Rows)
            {
                var sourcingStage = UtilityTools.ParseItemsWantedSourcingStage(WebConvert.ToInt32(dr["isgID"], (int)EXBItemsWantedSourcingStage.Breakthrough));

                //only add item if it's a breakthrough project and item is added by user, or if it's item from currently sourcing.
                if (sourcingStage == EXBItemsWantedSourcingStage.CurrentlySourcing || WebConvert.ToBoolean(dr["iwbAddedByUser"], false))
                {
                    activeBuyerProjects.Add(new ActiveBuyerProject(
                        WebConvert.ToInt32(dr["itmID"], 0),
                        WebConvert.ToInt32(dr["iwaID"], 0),
                        sourcingStage, WebConvert.ToDecimal(dr["iwbSellPrice"], 0), WebConvert.ToDecimal(dr["itmQty"], 0), WebConvert.ToInt32(dr["itmTotalFeet"], 0),
                        UtilityTools.ParseAssetType(WebConvert.ToInt32(dr["astID"], (int)EXBAssetType.Equipment))));
                }
            }
            return activeBuyerProjects;
        }

        public static List<ActiveBuyerProject> GetAllActiveBuyerProjects(int usrID, int buyID)
        {
            List<ActiveBuyerProject> activeBuyerProjects = new List<ActiveBuyerProject>();
            EXBDbConnection db = new EXBDbConnection();
            string sql = "SELECT distinct(itmID), iwaID, isgID, iwbAddedByUser, iwbSellPrice, itmQty, itmTotalFeet, astID FROM vwItemsWantedBids WHERE iwaActive = 1 "
                 + " AND iwbDisplay = 1 AND isPresented = 1 AND " + EXBUser.GetUserActiveCurrentlySourcingBreakthroughIwaCondition(usrID);
            DataTable dtWatchList = db.Query(sql);
            foreach (DataRow dr in dtWatchList.Rows)
            {
                var sourcingStage = UtilityTools.ParseItemsWantedSourcingStage(WebConvert.ToInt32(dr["isgID"], (int)EXBItemsWantedSourcingStage.Breakthrough));

                //only add item if it's a breakthrough project and item is added by user, or if it's item from currently sourcing.
                if (sourcingStage == EXBItemsWantedSourcingStage.CurrentlySourcing || WebConvert.ToBoolean(dr["iwbAddedByUser"], false))
                {
                    activeBuyerProjects.Add(new ActiveBuyerProject(
                        WebConvert.ToInt32(dr["itmID"], 0),
                        WebConvert.ToInt32(dr["iwaID"], 0),
                        sourcingStage, WebConvert.ToDecimal(dr["iwbSellPrice"], 0), WebConvert.ToDecimal(dr["itmQty"], 0), WebConvert.ToInt32(dr["itmTotalFeet"], 0),
                        UtilityTools.ParseAssetType(WebConvert.ToInt32(dr["astID"], (int)EXBAssetType.Equipment))));
                }
            }
            return activeBuyerProjects;
        }
    }

    [Serializable]
    public class ActiveBuyerProject
    {
        public int ItmID { get; set; }
        public int IwaID { get; set; }
        public decimal IwbSellPrice { get; set; }
        public decimal ItmQty { get; set; }
        public int ItmTotalFeet { get; set; }
        public decimal IwbSellPriceFT { get; private set; }
        public EXBAssetType AssetType { get; set; }
        public EXBItemsWantedSourcingStage SourcingStage { get; set; }

        public ActiveBuyerProject(int itmID, int iwaID, EXBItemsWantedSourcingStage sourcingStage, decimal iwbSellPrice, decimal itmQty, int itmTotalFeet, EXBAssetType assetType)
        {
            ItmID = itmID;
            IwaID = iwaID;
            IwbSellPrice = iwbSellPrice;
            ItmQty = itmQty;
            ItmTotalFeet = itmTotalFeet;
            AssetType = assetType;
            IwbSellPriceFT = assetType == EXBAssetType.Equipment ? 0 : ItmTotalFeet > 0 ? Math.Round(itmQty * IwbSellPrice / itmTotalFeet, 2, MidpointRounding.AwayFromZero) : 0;
            SourcingStage = sourcingStage;

        }
    }
    
  
}