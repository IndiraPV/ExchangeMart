using Exchangebase.Com.Bll;
using Exchangebase.Com.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Exchangebase.Com.Buyer
{
    public partial class MyWatchList : EXBBuyerRestrictedPage
    {
        protected class GrdIdNameCount
        {
            public int linkID { get; set; }
            public string grdID { get; set; }
            public string iwaID { get; set; }
            public string Grade { get; set; }
            public string itmIDs { get; set; }
            public int iodID { get; set; }
            public bool IsEquipment = false;
            public string cmdArgument
            {
                get
                {
                    return iwaID + ":" + itmIDs;
                }
            }
            public int Count { get; set; }
        }

        public static string itmLink;


        protected void Page_Load(object sender, EventArgs e)
        {

            var controlName = Request.Params.Get("__EVENTTARGET");
            var argument = Request.Params.Get("__EVENTARGUMENT");
            if (controlName == "card")
            {
                //card clicked, go to the item profile view
                ShowItemProfile(argument);
            }
            //Wire Events
            ItmRepeater.ItemDataBound += ItmRepeater_ItemDataBound;


            //if (!IsPostBack)
            //{
            SetUpPage();
            //}

        }

        void SetUpPage()
        {
            EXBDbConnection db = new EXBDbConnection();
            string sql = "SELECT astID, itmID, iwaID, iwaRefName, grdID, iodID, Grade, iodName FROM vwItemsWantedBids WHERE isPresented = 1 AND iwbDisplay = 1 " +
                " AND buyID = " + TheBuyer.BuyID + " AND iwaActive = 1 AND isgID = " + (int)EXBItemsWantedSourcingStage.Breakthrough + " AND bstID = " + (int)EXBBidStatus.Open + "ORDER BY astID desc, iodID, Grade";
            DataTable dt = db.Query(sql);

            //group the list and get counts
            List<GrdIdNameCount> grdNames = new List<GrdIdNameCount>();
            string iwaIDList = "";
            string itmIDList = "";
            foreach (DataRow dr in dt.Rows)
            {
                itmIDList += dr["itmID"] + ", ";
               
                
                iwaIDList += dr["iwaID"] + ", ";
                if (UtilityTools.ParseAssetType(WebConvert.ToInt32(dr["astID"], 0)) == EXBAssetType.LinePipe)
                {
                    var gradeName = new GrdIdNameCount() { grdID = dr["grdID"].ToString(), Grade = dr["iodName"].ToString() + ", " + dr["Grade"].ToString(), Count = 1, iwaID = dr["iwaID"].ToString(), IsEquipment = false, iodID = SafeRead.ToInt32(dr, "iodID") };
                    if (grdNames.Any(x => x.Grade == gradeName.Grade))
                    {
                        grdNames.First(x => x.Grade == gradeName.Grade).Count++;
                        grdNames.First(x => x.Grade == gradeName.Grade).itmIDs += "," + dr["itmID"].ToString();
                    }
                    else
                    {
                        grdNames.Add(gradeName);
                        grdNames.First(x => x.Grade == gradeName.Grade).itmIDs = dr["itmID"].ToString();
                    }
                }
                else
                {
                    var gradeName = new GrdIdNameCount() { grdID = dr["grdID"].ToString(), Grade = dr["Grade"].ToString(), Count = 1, iwaID = dr["iwaID"].ToString(), IsEquipment = true };
                    if (grdNames.Any(x => x.Grade == gradeName.Grade))
                    {
                        grdNames.First(x => x.Grade == gradeName.Grade).Count++;
                        grdNames.First(x => x.Grade == gradeName.Grade).itmIDs += "," + dr["itmID"].ToString();
                    }
                    else
                    {
                        grdNames.Add(gradeName);
                        grdNames.First(x => x.Grade == gradeName.Grade).itmIDs = dr["itmID"].ToString();
                    }
                }

                
            }
            iwaIDList = iwaIDList.Length > 2 ? iwaIDList.Remove(iwaIDList.Length - 2, 2) : iwaIDList;
            itmIDList = itmIDList.Length > 2 ? itmIDList.Remove(itmIDList.Length - 2, 2) : itmIDList;
           

            //order by equipment grade first, then order by line pipe OD

            grdNames = grdNames.OrderByDescending(x => x.IsEquipment).ThenBy(x=>x.iodID).ThenBy(x => x.Grade).ToList();
            if (grdNames.Count() > 1)
            {
                grdNames.Insert(0, new GrdIdNameCount() { grdID = "0", Count = grdNames.Sum(x => x.Count), Grade = "All", iwaID = iwaIDList, itmIDs = itmIDList });
            }
            grdRepeater.DataSource = grdNames;
            grdRepeater.DataBind();

            if (grdNames.Count > 0)
            {

                GrdIdNameCount dr = grdNames.First();
                if (grdNames.FirstOrDefault(x => x.cmdArgument == cmdArgumentFilterValue.Value) != null)
                {
                    dr = grdNames.First(x => x.cmdArgument == cmdArgumentFilterValue.Value);
                }
                itmIDsFilterValue.Value = dr.itmIDs;
                cmdArgumentFilterValue.Value = dr.cmdArgument;
                //grdIDFilterValue.Value = dr.grdID;
                GradeTitleLabel.Text = dr.Grade;
                watchListEmptyPanel.Visible = false;
                watchListMainPanel.Visible = true;
                //new panel visible false
                MyWatchlistEmptyCantViewItems.Visible = false;
                GetItemData();
            }
            else if (TheCustomer.CanViewOpenItems)
            {
                watchListEmptyPanel.Visible = true;
                watchListMainPanel.Visible = false;
                MyWatchlistEmptyCantViewItems.Visible = false;
            }
            else
            {
                watchListEmptyPanel.Visible = false;
                watchListMainPanel.Visible = false;
                MyWatchlistEmptyCantViewItems.Visible = true;
            }

            if (TheUser != null)
            {
                if (TheUser.PMAdmID > 0)
                {
                    EXBAdministrator admin = new EXBAdministrator(TheUser.PMAdmID);
                    buyRepLiteral.Text = " <a href='mailTo:" + admin.AdmEmail + "?subject=user portal: asset request for new project" + "'>" + admin.FullName + "</a>";
                    buyRepExtLiteral.Text = admin.AdmPhoneExt != null && admin.AdmPhoneExt.Length > 0 ? " x" + admin.AdmPhoneExt : "";
                }
                else
                {
                    buyRepLiteral.Text = " <a href='mailTo:customerservice@exchangebase.com??subject=user portal asset request for new project'>Concierge Service</a>";
                }
            }
        }

        void ItmRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow dr = (DataRow)e.Item.DataItem;
                int itmID = WebConvert.ToInt32(dr["itmID"], 0);

                int iwaID = WebConvert.ToInt32(dr["iwaID"], 0);
                bool itemIsFeatured = WebConvert.ToBoolean(dr["itmIsFeatured"], false);
                //hide featured panel or not
                Panel featuredPanel = e.Item.FindControl("FeaturedPanel") as Panel;
                if (featuredPanel != null)
                {
                    featuredPanel.Visible = itemIsFeatured;
                }

                Literal itmNameLiteral = e.Item.FindControl("itmNameLiteral") as Literal;
                if (itmNameLiteral.Text != null)
                {
                    itmNameLiteral.Text = WebConvert.ToString(dr["itmName"], "");
                }


                decimal itmIndexPrice = WebConvert.ToDecimal(dr["itmIndexPrice"], 0);
                decimal itmSellPrice = WebConvert.ToDecimal(dr["itmSellPrice"], 0);
                decimal itmQty = WebConvert.ToDecimal(dr["itmQty"], 0);
                int itmTotalFeet = WebConvert.ToInt32(dr["itmTotalFeet"], 0);
                decimal itmIndexPriceFT = itmTotalFeet > 0 ? Math.Round(itmIndexPrice * itmQty / itmTotalFeet, 2, MidpointRounding.AwayFromZero) : 0;
                decimal itmSellPriceFT = itmTotalFeet > 0 ? Math.Round(itmSellPrice * itmQty / itmTotalFeet, 2, MidpointRounding.AwayFromZero) : 0;
                string uomName = WebConvert.ToString(dr["uomName"], "");
                string facLocationName = WebConvert.ToString(dr["facState"], "") != null ? WebConvert.ToString(dr["facState"], "") + ", " + WebConvert.ToString(dr["facCountry"], "") : WebConvert.ToString(dr["facCountry"], "");
                string itmCond = WebConvert.ToString(dr["icdName"], "");
                itmLink = itmID.ToString() + "," + iwaID.ToString();
                int linkID = itmID;

                Panel itmCondPanel = e.Item.FindControl("itmCondPanel") as Panel;
                if (itmCondPanel != null)
                {
                    if (itmCond == "New")
                    {
                        itmCondPanel.CssClass = "badge badge-success";
                        itmCondPanel.Controls.Add(new LiteralControl(itmCond));
                    }
                    if (itmCond == "Used")
                    {
                        itmCondPanel.CssClass = "badge badge-warning";
                        itmCondPanel.Controls.Add(new LiteralControl(itmCond));
                    }
                }

                Panel gradeNamePanel = e.Item.FindControl("gradeNamePanel") as Panel;
                if (gradeNamePanel != null)
                {
                    gradeNamePanel.CssClass = "badge badge-primary";
                    gradeNamePanel.Controls.Add(new LiteralControl(WebConvert.ToString(dr["Grade"], "")));
                }

                Panel itmIDPanel = e.Item.FindControl("itmIDPanel") as Panel;
                if (itmIDPanel != null)
                {
                    itmIDPanel.CssClass = "badge badge-secondary";
                    itmIDPanel.Controls.Add(new LiteralControl(WebConvert.ToString("#" + dr["itmID"], "")));
                }

                Literal facLocationNameLiteral = e.Item.FindControl("facLocationNameLiteral") as Literal;
                if (facLocationNameLiteral != null)
                {
                    facLocationNameLiteral.Text = facLocationName.ToString();
                }

                EXBAssetType assetType = UtilityTools.ParseAssetType(WebConvert.ToInt32(dr["astID"], 0));
                HtmlContainerControl askPriceSpan = e.Item.FindControl("askPriceSpan") as HtmlContainerControl;

                Literal itmQtyLiteral = e.Item.FindControl("itmQtyLiteral") as Literal;
                if (itmQtyLiteral != null)
                {
                    if (assetType == EXBAssetType.Equipment)
                    {
                        itmQtyLiteral.Text = String.Format("{0:N0}", itmQty);
                    }
                    else
                    {
                        itmQtyLiteral.Text = itmTotalFeet > 0 ? String.Format("{0:N0}", itmTotalFeet) + " ft" : "N/A";
                    }
                }

                Literal itmIndexPriceLiteral = e.Item.FindControl("itmIndexPriceLiteral") as Literal;
                if (itmIndexPriceLiteral.Text != null)
                {
                    if (assetType == EXBAssetType.Equipment)
                    {
                        if (itmQty == 1)
                        {
                            itmIndexPriceLiteral.Text = itmIndexPrice > 0 ? String.Format("{0:C0}", itmIndexPrice) : "N/A";
                        }
                        else
                        {
                            itmIndexPriceLiteral.Text = itmIndexPrice > 0 ? String.Format("{0:C0}", itmIndexPrice) + " /" + uomName.ToLower() : "N/A";
                        }
                    }
                    else
                    {
                        itmIndexPriceLiteral.Text = itmIndexPrice > 0 ? String.Format("{0:C2}", itmIndexPriceFT) + " /ft" : "N/A";
                    }
                }
                Literal IndexOriginalPriceLiteral = e.Item.FindControl("IndexOriginalPriceLiteral") as Literal;
                if (IndexOriginalPriceLiteral != null)
                {
                    IndexOriginalPriceLiteral.Text = assetType == EXBAssetType.Equipment ? "Original Price" : "Index Price";
                }

                Literal itmSellPriceLiteral = e.Item.FindControl("itmSellPriceLiteral") as Literal;
                if (itmSellPriceLiteral.Text != null)
                {
                    if (assetType == EXBAssetType.Equipment)
                    {
                        if (itmQty == 1)
                        {
                            itmSellPriceLiteral.Text = itmSellPrice > 0 ? String.Format("{0:C0}", itmSellPrice) : "N/A";
                        }
                        else
                        {
                            itmSellPriceLiteral.Text = itmSellPrice > 0 ? String.Format("{0:C0}", itmSellPrice) + " /" + uomName.ToLower() : "N/A";
                        }
                    }
                    else
                    {
                        itmSellPriceLiteral.Text = itmSellPriceFT > 0 ? String.Format("{0:C2}", itmSellPriceFT) + " /ft" : "N/A";
                    }

                }

                Image itmImgMain = e.Item.FindControl("itmImgMain") as Image;

                if (itmImgMain != null)
                {

                    if (WebConvert.ToInt32(dr["imgID"], 0) > 0)
                    {
                        itmImgMain.ImageUrl = EXBImage.GetImageWebFilePath(WebConvert.ToInt32(dr["imgID"], 0), WebConvert.ToString(dr["imgExtension"], ".jpg"));
                    }
                    else
                    {
                        itmImgMain.ImageUrl = "/Images/image-unavailable2.png";
                    }

                    itmImgMain.AlternateText = WebConvert.ToString(dr["itmName"], "");
                }

                LinkButton itmIDLink = e.Item.FindControl("itmIDLink") as LinkButton;
                if (itmIDLink != null)
                {
                    itmIDLink.CommandArgument = itmID.ToString() + "," + iwaID.ToString();
                }

                Button itmIDLink2 = e.Item.FindControl("itmIDLink2") as Button;
                if (itmIDLink2 != null)
                {
                    itmIDLink2.CommandArgument = itmID.ToString() + "," + iwaID.ToString();
                }

                LinkButton watchListRemoveButton = e.Item.FindControl("watchListRemoveButton") as LinkButton;
                if (watchListRemoveButton != null)
                {
                    watchListRemoveButton.Visible = true;
                    watchListRemoveButton.Attributes["itmID"] = itmID.ToString();
                    watchListRemoveButton.CommandArgument = WebConvert.ToInt32(dr["bidID"], 0).ToString() + "," + WebConvert.ToInt32(dr["astID"], 0) + "," + (WebConvert.ToBoolean(dr["iwbAddedByUser"], false) ? 1 : 0);
                }
                Label gradeLabel = e.Item.FindControl("GradeLabel") as Label;
                if (gradeLabel != null)
                {
                    gradeLabel.Text = WebConvert.ToString(dr["Grade"], "");
                }
                Label ODLabel = e.Item.FindControl("ODLabel") as Label;
                if (ODLabel != null)
                {
                    if (assetType == EXBAssetType.LinePipe)
                    {
                        ODLabel.Visible = true;
                        ODLabel.Text = WebConvert.ToString(dr["iodName"], "");
                    }
                }
                HtmlContainerControl mainDiv = e.Item.FindControl("mainCardDiv") as HtmlContainerControl;
                if (mainDiv != null)
                {
                    mainDiv.Attributes.Add("cmdargument", itmID.ToString() + "," + iwaID.ToString());
                }
                Literal itmIDLiteral = e.Item.FindControl("ItmIDLiteral") as Literal;
                if (itmIDLiteral != null)
                {
                    itmIDLiteral.Text = WebConvert.ToInt32(dr["itmID"], 0).ToString();
                }
            }
        }

        private void GetItemData()
        {
            SetBreakthroughActiveItem();
            EXBDbConnection db = new EXBDbConnection();

            string cmdArgument = WebConvert.ToString(cmdArgumentFilterValue.Value, "0");
            string[] cmdArguments = cmdArgument.Split(':');
            if (cmdArguments.Length < 2)
            {
                return;
            }
            if (cmdArguments[1].Length == 0)
            {
                cmdArguments[1] = "0";
            }
            string sqlString = "SELECT * FROM vwItemsWantedBidUserPortal WHERE isPresented = 1 AND iwaID IN ( " + cmdArguments[0] + " ) AND itmID IN ( " + cmdArguments[1] + " ) AND iwbDisplay = 1" + " AND bstID = " + (int)EXBBidStatus.Open + " ORDER BY (CASE WHEN loiSequence IS NULL THEN 99 ELSE loiSequence END)";
            DataTable dt = db.Query(sqlString);
            ItmRepeater.DataSource = dt.Rows;
            ItmRepeater.DataBind();
            if (dt.Rows.Count < 1)
            {
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void GrdLinkButton_Command(object sender, CommandEventArgs e)
        {
            //grdIDFilterValue.Value = e.CommandArgument.ToString();
            cmdArgumentFilterValue.Value = e.CommandArgument.ToString();
            GradeTitleLabel.Text = (sender as LinkButton).Text;
            (sender as LinkButton).Attributes.Add("class", "d-flex active");
            //ItmGradeFilterPanel.Visible = false;
            ItemProfilePanel.Visible = false;
            ItemCardsPanel.Visible = true;
            ItemProfileControl.UpdateFlag = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            GetItemData();
            WoopraReportAction("WatchListGrade Clicked: " + cmdArgumentFilterValue.Value + ":" + (sender as LinkButton).Text); //get grade name for watchlist eh?
        }

        private void SetBreakthroughActiveItem()
        {
            string activeItem = cmdArgumentFilterValue.Value;
            foreach (RepeaterItem ri in grdRepeater.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField IwaIDHidden = ri.FindControl("cmdArgument") as HiddenField;
                    LinkButton lb = ri.FindControl("grdLinkButton") as LinkButton;
                    if (IwaIDHidden != null && lb != null)
                    {
                        if (IwaIDHidden.Value == activeItem)
                        {
                            lb.Attributes["class"] = "d-block active";
                        }
                        else
                        {
                            lb.Attributes["class"] = "d-block";
                        }
                    }
                }
            }
        }

        protected void watchListRemoveButton_Command(object sender, CommandEventArgs e)
        {
            EXBDbConnection db = new EXBDbConnection();
            string commandArgVal = e.CommandArgument.ToString();
            string[] value = commandArgVal.Split(',');
            WoopraReportAction("Remove from Watchlist: " + grdIDFilterValue.Value); //itm id or itm name
            EXBItemsWanted.UserPortalRemoveItemFromWatchList(TheBuyer.BuyID, WebConvert.ToInt32(value[0], 0), UtilityTools.ParseAssetType(WebConvert.ToInt32(value[1], (int)EXBItemsWantedType.LinePipe)));
            string itmID = (sender as LinkButton).Attributes["itmID"].ToString();
            cmdArgumentFilterValue.Value = cmdArgumentFilterValue.Value.Replace("," + itmID, "").Replace(itmID, "").Replace(":,", ":");
            SetUpPage();
            SetBreakthroughActiveItem();
            GetItemData();
        }

        private void WoopraReportAction(string description)
        {
            if (AnalyticsInjection.IsWoopraTrackEnabled())
            {
                bool trackWatchlist = false;
                if (description != null && description.Contains("Watch"))
                {
                    trackWatchlist = true;
                }
                string script = ("<script>woopra.track('userportal', {page: 'Watchlist', description: '" + description
                     + "'});</script>");
                if (trackWatchlist)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", script, false);
                }
                else
                {
                    Literal analyticsInjection = new Literal();
                    analyticsInjection.Text = script.ToString();
                    Page.Header.Controls.Add(analyticsInjection);
                }
            }
        }

        protected void Back_Button_OnCommand(object sender, CommandEventArgs e)
        {
            ItemProfilePanel.Visible = false;
            ItemCardsPanel.Visible = true;
            ItemProfileControl.UpdateFlag = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            //Response.Redirect(Request.RawUrl);
            //ItemRepeaterPanel.Update();
            //ItemProfilePanel.Update();
            SetUpPage();
            WoopraReportAction("Item Profile - Back Button Pressed.");
        }

        protected void ShowItemProfile(string commandArgument)
        {
            ItemProfilePanel.Visible = true;
            ItemCardsPanel.Visible = false;
            var commandArgs = commandArgument.Split(',');
            var itmID = WebConvert.ToInt32(commandArgs[0], 0);
            if (commandArgs.Length > 1)
            {
                var iwaID = WebConvert.ToInt32(commandArgs[1], 0);
                ItemProfileControl.iwaID = iwaID;
            }
            ItemProfileControl.itmID = itmID;
            ItemProfileControl.UpdateFlag = true;
            //ItemRepeaterPanel.Update();
            WoopraReportAction("Show Item Profile: " + itmID);
        }

        protected void ItemProfileControl_BackButton_Clicked(object sender, EventArgs e)
        {
            ItemProfilePanel.Visible = false;
            ItemCardsPanel.Visible = true;
            ItemProfileControl.UpdateFlag = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            //Response.Redirect(Request.RawUrl);
            //ItemRepeaterPanel.Update();
            //ItemProfilePanel.Update();
            SetUpPage();
            WoopraReportAction("Item Profile - Back Button Pressed.");
        }
    }
}