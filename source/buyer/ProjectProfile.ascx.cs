using Exchangebase.Com.Bll;
using Exchangebase.Com.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

// Code Cleanup - v&e
namespace Exchangebase.Com.Buyer
{
    public partial class ProjectProfile : System.Web.UI.UserControl
    {
        public ProjectProfileViewModel ViewModel { get; set; }
        private EXBUser user;
        public EXBUser TheUser
        {
            get
            {
                user = EXBUser.GetUserSession();
                if (user == null)
                {
                    throw new EXBUserSessionException();
                }
                return user;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["PID"] != null && Request.QueryString["PID"].Contains(","))
            {
                string[] iwaIDString = Request.QueryString["PID"].Split(',');
                List<int> iwaIDS = new List<int>();
                for (int i = 0; i < iwaIDString.Length; i++)
                {
                    iwaIDS.Add(WebConvert.ToInt32(iwaIDString[i],0));
                }

                ViewModel = ProjectProfileViewModel.GetProjectsProfile(iwaIDS, TheUser.UsrID);
            }
            else
            {
                int iwaID = WebConvert.ToInt32(Request.QueryString["PID"], 0);
                ViewModel = ProjectProfileViewModel.GetProjectProfile(iwaID, TheUser.UsrID);
            }
            int itmID = WebConvert.ToInt32(Request.QueryString["IID"], 0);


            if (!IsPostBack)
            {
                PopulateForm();
                SetContactInfo();
                if (UtilityTools.GetIsEnergyFlg())
                {
                    projectNameLiteral.Text = "Project Name";
                    projectNumLiteral.Text = "Project #";
                }
                else
                {
                    projectNameLiteral.Text = "Project Name";
                    projectNumLiteral.Text = "Project #";
                }

            }
        }

        protected void PopulateForm()
        {
            //if(ViewModel.

            if (ViewModel.iwtID == (int)EXBItemsWantedType.LinePipe || ViewModel.iwtID == (int)EXBItemsWantedType.OCTG)
            {
                //ODHeader.Attributes.Add("style", "");
                //ODHeader1.Attributes.Add("style", "");
                //QtyAvailHeader.InnerText = "Qty Available";
                //QtyAvailHeader1.InnerText = "Qty Available";
                //QtyQuotePriceHeader.InnerText = "Quote Price";
                //QtyQuotePriceHeader1.InnerText = "Quote Price";
                //QtyWantedHeader.InnerText = "Qty Wanted";
                //QtyWantedHeader1.InnerText = "Qty Wanted";
                //ODTotalRow.Attributes.Add("style", "");
                //ODTotalRow
                //QuotedItemsRepeater.DataSource = ViewModel.QuotedItems;
                //QuotedItemsRepeater.DataBind();

                BestOptionsRepeater.DataSource = ViewModel.BestOptions;
                BestOptionsRepeater.DataBind();

                //ItemRepeater.DataSource = ViewModel.QuotedItems.Union(ViewModel.BestOptions).ToList();
                //ItemRepeater.DataBind();
            }
            else if (ViewModel.iwtID == (int)EXBItemsWantedType.Strategic) //equipment
            {
                //ODHeader.Attributes.Add("style", "display:none;");
                //ODHeader1.Attributes.Add("style", "display:none;");

                //QuotedItemsRepeater.DataSource = ViewModel.QuotedItems;
                //QuotedItemsRepeater.DataBind();

                BestOptionsRepeater.DataSource = ViewModel.BestOptions;
                BestOptionsRepeater.DataBind();
            }



            if (ViewModel.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough)
            {
                linepipeOverview.Visible = false;
                equipmentOverview.Visible = false;
                additionalInformationDiv.Visible = false;
                BestOptionsLabel.Visible = false;
                lineBreakHR.Visible = false;


            }
            else if (ViewModel.iwtID == (int)EXBAssetType.LinePipe || ViewModel.iwtID == (int)EXBAssetType.OCTG)
            {
                linepipeOverview.Visible = true;
                equipmentOverview.Visible = false;
                additionalInformationDiv.Visible = true;
                BestOptionsLabel.Visible = true;
                lineBreakHR.Visible = true;

            }
            else //equipment currently sourcing
            {
                linepipeOverview.Visible = false;
                equipmentOverview.Visible = true;
                additionalInformationDiv.Visible = true;
                BestOptionsLabel.Visible = true;
                lineBreakHR.Visible = true;

            }

            if (ViewModel.BestOptions == null || !ViewModel.BestOptions.Any())
            {
                noOptionsText.Visible = true;
            }
        }

        private void SetContactInfo()
        {
            if (TheUser != null)
            {
                if (TheUser.PMAdmID > 0)
                {
                    EXBAdministrator admin = new EXBAdministrator(TheUser.PMAdmID);
                    buyerRepLiteral.Text = " <a href='mailTo:" + admin.AdmEmail + "?Subject=Question: Project # " + ViewModel.iwaID + "' class='d-inline-block mx-2 my-2 btn btn-outline-primary'><i class='fa fa-envelope mr-md-2' aria-hidden='true'></i><span class='d-none d-md-inline-block'>" + admin.FullName + "</span></a>";
                    buyerRepExtLiteral.Text = admin.AdmPhoneExt != null && admin.AdmPhoneExt.Length > 0 ? " x" + admin.AdmPhoneExt : "";
                }
                else
                {
                    buyerRepLiteral.Text = " <a href='mailTo:customerservice@exchangebase.com?Subject=Question: Project # " + ViewModel.iwaID + "' class='d-inline-block mx-2 my-2 btn btn-outline-primary'><i class='fa fa-envelope mr-md-2' aria-hidden='true'></i><span class='d-none d-md-inline-block'>Customer Service</span></a>";
                }
            }
        }

        protected void ViewItem_Command(object sender, CommandEventArgs e)
        {
            int iwaID = WebConvert.ToInt32(Request.QueryString["PID"], 0);
            int itmID = WebConvert.ToInt32(Request.QueryString["IID"], 0);
            itmID = WebConvert.ToInt32(e.CommandArgument, 0);
            WoopraReportAction("view quoted item: " + iwaID + "," + itmID);
            Response.Redirect(String.Format("~/Buyer/MyProjectsNew.aspx?PID={0}&IID={1}", iwaID, itmID));
        }

        protected void QuotedItemsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Item item = e.Item.DataItem as Item;

            Label iwbSellPriceLiteral = e.Item.FindControl("iwbSellPriceLiteral") as Label;
            Label iwbSellPriceTotalLiteral = e.Item.FindControl("iwbSellPriceTotalLiteral") as Label;
            decimal iwbSellPrice = item.iwbSellPrice;
            decimal itmQty = item.itmQty;
            int itmTotalFeet = item.itmTotalFeet;
            Panel buttonPanel = e.Item.FindControl("ButtonPanel") as Panel;
            Panel soldPanel = e.Item.FindControl("SoldPanel") as Panel;
            Panel purchasedPanel = e.Item.FindControl("PurchasedPanel") as Panel;
            Label hotLabel = e.Item.FindControl("InterestLevelHot") as Label;
            Label warmLabel = e.Item.FindControl("InterestLevelWarm") as Label;
            Label neutralLabel = e.Item.FindControl("InterestLevelNeutral") as Label;
            Label coldLabel = e.Item.FindControl("InterestLevelCold") as Label;
            EXBAssetType assetType = UtilityTools.ParseAssetType(item.astID);
            Label qtyWanted = e.Item.FindControl("qtyWanted") as Label;
            Label qtyAvailable = e.Item.FindControl("qtyAvailable") as Label;
            Label gradeLabel = e.Item.FindControl("gradeLabel") as Label;
            HtmlContainerControl tableRow = e.Item.FindControl("TableRow") as HtmlContainerControl;
            HtmlContainerControl gradeLabelSpan = e.Item.FindControl("gradeLabelSpan") as HtmlContainerControl;
            HtmlContainerControl QuotePriceAskingPriceLabel = e.Item.FindControl("QuotePriceAskingPriceLabel") as HtmlContainerControl;
            if (QuotePriceAskingPriceLabel == null) { return; }
            //bool isBreakthrough = ViewModel.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough;
            QuotePriceAskingPriceLabel.InnerText = "Quote Price";
            //if (isBreakthrough)
            //{
            //    DatesPanel.Visible = false;
            //}
            DatesPanel.Visible = false;
            if (gradeLabel != null && gradeLabelSpan != null)
            {
                if (item.Grade != "")
                {
                    gradeLabelSpan.Visible = true;
                    gradeLabel.Visible = true;
                    gradeLabel.Text = item.Grade != "" ? item.Grade : "N/A";
                }
                else
                {
                    gradeLabelSpan.Visible = false;
                    gradeLabel.Visible = false;
                    gradeLabel.Text = item.Grade != "" ? item.Grade : "";
                }
            }

            if (assetType == EXBAssetType.Equipment)
            {


                qtyWanted.Text = item.iwbQty == 0 ? "N/A" : String.Format("{0:N0}", item.iwbQty);
                qtyAvailable.Text = item.itmQty == 0 ? "N/A" : String.Format("{0:N0}", item.itmQty);
            }
            else
            {
                qtyAvailable.Text = "N/A";
                HtmlContainerControl odtd = e.Item.FindControl("ODTD") as HtmlContainerControl;
                if (odtd != null)
                {
                    odtd.Attributes.Add("style", "");
                }
                if (item.itmIsQuote && item.vscID > 1 && item.BuyerFeeType == EXBBuyerFeeTypes.FlatFee)
                {
                    qtyWanted.Text = "N/A";
                }
                else if (item.itmIsQuote && item.vscID > 1 && item.BuyerFeeType == EXBBuyerFeeTypes.PerUOM)
                {
                    qtyWanted.Text = item.iwbQty == 0 ? "N/A" : String.Format("{0:N0}", item.iwbQty) + "";
                }
                else
                {
                    qtyWanted.Text = item.iwbTotalFeet == 0 ? "N/A" : String.Format("{0:N0}", item.iwbTotalFeet) + " ft";
                    qtyAvailable.Text = item.itmTotalFeet == 0 ? "N/A" : String.Format("{0:N0}", item.itmTotalFeet) + " ft";
                }


            }

            if (buttonPanel != null && soldPanel != null && purchasedPanel != null)
            {
                EXBItemsWantedStatus status = UtilityTools.ParseItemsWantedStatus(ViewModel.iwsID);
                bool statusFlg = (status == EXBItemsWantedStatus.ApprovedFinance || status == EXBItemsWantedStatus.ApprovedExecutive || status == EXBItemsWantedStatus.OrderFulfillment || status == EXBItemsWantedStatus.Fulfilled); //the project is approved and the item is purchased/sold

                //if it's not open, then it's either sold to someone else or purchased by this user
                if (!(EXBBid.ParseBidStatus(item.bstID) == EXBBidStatus.Open))
                {
                    //if the logged in user is not the preferred buyer of this item(someone else bought the item)
                    if (!(statusFlg && item.iwbIsPreferred))
                    {
                        buttonPanel.Visible = false;
                        soldPanel.Visible = true;
                    }
                    else if (statusFlg && item.iwbIsPreferred) //else if this user project is approved and user is the preferred buyer
                    {
                        buttonPanel.Visible = false;
                        purchasedPanel.Visible = true;
                    }
                }
            }


            //set the interest status
            if (hotLabel != null && warmLabel != null && neutralLabel != null && coldLabel != null)
            {
                hotLabel.Visible = false;
                warmLabel.Visible = false;
                neutralLabel.Visible = false;
                coldLabel.Visible = false;
                if (item.loiID == (int)EXBLevelOfInterest.Hot)
                {
                    hotLabel.Visible = true;
                }
                else if (item.loiID == (int)EXBLevelOfInterest.Warm)
                {
                    warmLabel.Visible = true;
                }
                else if (item.loiID == (int)EXBLevelOfInterest.Cold)
                {
                    coldLabel.Visible = true;
                }
                else if (item.loiID == (int)EXBLevelOfInterest.Neutral)
                {
                    neutralLabel.Visible = true;
                }

            }
            Label iwbSellPriceLabel = e.Item.FindControl("IwbSellPriceLabel") as Label;
            if (iwbSellPriceLabel != null)
            {
                if (assetType == EXBAssetType.Equipment)
                {
                    //if itm is price each, then show each (if quantity greater than 0?)
                    if (item.itmIsQuote && item.vscID > 1)
                    {
                        //if(item.
                        switch (item.BuyerFeeType)
                        {
                            case EXBBuyerFeeTypes.FlatFee:
                                iwbSellPriceLabel.Text = String.Format("{0:C0}", iwbSellPrice) + "";
                                break;
                            case EXBBuyerFeeTypes.PerUOM:
                                iwbSellPriceLabel.Text = item.iwbQty <= 1 ? String.Format("{0:C0}", iwbSellPrice) : String.Format("{0:C0}", iwbSellPrice) + " /ea";
                                break;
                            case EXBBuyerFeeTypes.Percentage:
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (itmQty <= 1)
                        {
                            iwbSellPriceLabel.Text = String.Format("{0:C0}", item.ProductCostUOM);
                        }
                        else
                        {
                            iwbSellPriceLabel.Text = String.Format("{0:C0}", item.ProductCostUOM) + " /ea";
                        }
                    }
                }
                else
                {
                    if (item.itmIsQuote && item.vscID > 1)
                    {
                        if (item.BuyerFeeType == EXBBuyerFeeTypes.FlatFee)
                        {
                            iwbSellPriceLabel.Text = String.Format("{0:C0}", item.iwbSellPrice) + "";
                        }
                        //else if (assetType == EXBAssetType.Equipment)
                        //{
                        //    iwbSellPriceLabel.Text = String.Format("{0:C0}", item.iwbSellPrice) + "/ea";
                        //}
                        else
                        {
                            iwbSellPriceLabel.Text = item.iwbQty <= 1 ? String.Format("{0:C0}", iwbSellPrice) : String.Format("{0:C0}", iwbSellPrice) + " /ea";
                        }
                    }
                    else
                    {
                        //if (isBreakthrough)
                        //{
                        //    var itmFTPrice = item.itmTotalFeet > 0 ? String.Format("{0:C2}", ((item.itmSellPrice * item.itmQty) / item.itmTotalFeet)) + " /ft" : "N/A";
                        //    iwbSellPriceLabel.Text = item.itmTotalFeet > 0 ? String.Format("{0:C2} /ft", Math.Round(item.itmSellPriceFT, 3)) : "N/A";
                        //}
                        //else
                        //{
                        //    iwbSellPriceLabel.Text = item.ProductCostFT > 0 ? String.Format("{0:C2}", item.ProductCostFT) + " /ft" : "N/A";
                        //}

                        iwbSellPriceLabel.Text = item.ProductCostFT > 0 ? String.Format("{0:C2}", item.ProductCostFT) + " /ft" : "N/A";
                    }
                }
            }

            Image itmImgMain = e.Item.FindControl("itmImgMain") as Image;

            if (itmImgMain != null)
            {
                if (item.imgID > 0)
                {
                    itmImgMain.ImageUrl = EXBImage.GetImageWebFilePath(item.imgID, item.imgExtension);
                }
                else
                {
                    if (item.vscID > 1)
                    {
                        itmImgMain.ImageUrl = "~/buyer/images/noimage-services.png";
                    }
                    else
                    {
                        switch (item.astID)
                        {
                            case (int)EXBAssetType.OCTG:
                            case (int)EXBAssetType.LinePipe:
                                itmImgMain.ImageUrl = "~/buyer/images/noimage-linepipe.png";
                                break;
                            case (int)EXBAssetType.Equipment:
                                itmImgMain.ImageUrl = "~/buyer/images/noimage-upstream.png";
                                break;
                            default:
                                itmImgMain.ImageUrl = "~/Images/image-unavailable2.png";
                                break;
                        }
                    }
                }
            }
            bool itemIsFeatured = item.itmIsFeatured;// false;// WebConvert.ToBoolean(dr["itmIsFeatured"], false);
            //bool itmIsFeaturedAndNotQuoted = item.itmIsFeatured && ViewModel.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough;
            //bool itmIsQuoted = ViewModel.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough;
            bool itmIsFeaturedAndNotQuoted = item.itmIsFeatured;
            //hide featured panel or not
            Panel featuredPanel = e.Item.FindControl("FeaturedPanel") as Panel;
            Panel askPricePanel = e.Item.FindControl("AskPricePanel") as Panel;
            HtmlContainerControl quotePriceDiv = e.Item.FindControl("quotePriceDiv") as HtmlContainerControl;
            HtmlContainerControl itmSellPriceDiv = e.Item.FindControl("itmSellPriceDiv") as HtmlContainerControl;
            HtmlContainerControl totalPriceDiv = e.Item.FindControl("TotalPriceDiv") as HtmlContainerControl;
            HtmlContainerControl itmSellPriceLabel = e.Item.FindControl("itmSellPriceLabel") as HtmlContainerControl;
            //if (quotePriceDiv != null)
            //{
            //    quotePriceDiv.Visible = itmIsQuoted;
            //}
            //if (itmSellPriceDiv != null)
            //{
            //    itmSellPriceDiv.Visible = !itmIsQuoted;
            //}
            if (itmSellPriceLabel != null)
            {
                itmSellPriceLabel.InnerText = String.Format("{0:C2}", item.itmSellPrice);
            }
            if (featuredPanel != null)
            {
                featuredPanel.Visible = itemIsFeatured;
            }
            //if (askPricePanel != null)
            //{
            //    askPricePanel.Visible = !itmIsFeaturedAndNotQuoted;
            //}
            //if (totalPriceDiv != null)
            //{
            //    totalPriceDiv.Visible = !itmIsFeaturedAndNotQuoted;
            //}
            if (itmSellPriceDiv != null)
            {

            }

            if(soldPanel.Visible)
            {
                HtmlContainerControl mainCardDiv = e.Item.FindControl("mainCardDiv") as HtmlContainerControl;
                mainCardDiv.Attributes["onClick"] = "";
            }
        }
        private void WoopraReportAction(string description)
        {
            if (AnalyticsInjection.IsWoopraTrackEnabled())
            {
                // Inject woopra code
                string script = ("woopra.track('userportal', {page: 'Project Profile', description: '" + description
                    + "'});");
                ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", script.ToString(), true);
            }
        }
    }
    [Serializable]
    public class ProjectProfileViewModel
    {
        public int ProjectId { get { return iwaID; } }
        public string ProjectName { get { return iwaRefName; } }
        public string Grade { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime SourceDateStart { get; set; }
        public DateTime SourceDateEnd { get; set; }
        public DateTime DecisionDate { get; set; }
        public string DeliveryDateDisplay { get { return DeliveryDate != DateTime.MinValue ? String.Format("{0:MM/dd/yy}", DeliveryDate) : "&nbsp;"; } }
        public string SourceDateStartDisplay { get { return SourceDateStart != DateTime.MinValue ? String.Format("{0:MM/dd/yy}", SourceDateStart) : "&nbsp;"; } }
        public string SourceDateEndDisplay { get { return SourceDateEnd != DateTime.MinValue ? String.Format("{0:MM/dd/yy}", SourceDateEnd) : "&nbsp;"; } }
        public string DecisionDateDisplay { get { return DecisionDate != DateTime.MinValue ? String.Format("{0:MM/dd/yy}", DecisionDate) : "&nbsp;"; } }
        public string DueDate { get; set; }
        public string DeliveryLocation { get; set; }
        public string AdditionalInformation { get; set; }
        public string BasicInformation { get; set; }
        public string AdminName { get; set; }
        public string AdminContactInfo { get; set; }
        public string AdminEmail { get; set; }
        public List<Item> SelectedItems { get; set; }
        public List<Item> BestOptions { get; set; }
        public List<Item> AvailableItems { get; set; }
        public List<Item> QuotedItems { get; set; }
        public int SelectedItemsCount { get; set; }
        public int QuotedItemsCount { get; set; }
        public int BestOptionsCount { get; set; }
        public int iwaID { get; set; }
        public int iwaGrdID { get; set; }
        public int iwaManagerAdmID { get; set; }
        public int iwsID { get; set; }
        public string iwaName { get; set; }
        public string iwaRefName { get; set; }
        public string iwaState { get; set; }
        public string iwaCountry { get; set; }
        public DateTime iwaDueDate { get; set; }
        public int isgID { get; set; }
        public int iwtID { get; set; }
        public string iodName { get; private set; }
        public int iwaTotalFeet { get; set; }
        public string isgName { get; set; }
        public string iwaDisplayName { get; set; }
        public decimal iwaQty { get; set; }
        public int catID { get; set; }
        public string Category { get; set; }
        public int icdID { get; set; }
        public string icdName { get; set; }
        public int imgID { get; set; }
        public string imgExtension { get; set; }
        public bool itmIsFeatured { get; set; }

        public ProjectProfileViewModel()
        {

        }
        public static ProjectProfileViewModel GetProjectsProfile(List<int> iwaIDS, int usrID)
        {
            ProjectProfileViewModel vm = new ProjectProfileViewModel();

            //check access to this profile before doing the query
            string iwaSourcingCondigion = EXBUser.GetUserActiveCurrentlySourcingBreakthroughIwaCondition(usrID);
            foreach (int iwaID in iwaIDS)
            {
                if (!EXBUser.HasAccessToProject(usrID, iwaID))
                {
                    return vm;
                }
            }

            string selectProject = "SELECT * from vwItemsWantedUserPortal WHERE iwaID IN (" + WebConvert.GetCommaSeparatedValues(iwaIDS) + ")";
            EXBDbConnection db = new EXBDbConnection();
            DataTable dt = db.Query(selectProject);
            foreach (DataRow dr in dt.Rows)
            {
                vm.isgName = SafeRead.ToString(dr, "isgName");
                vm.isgID = SafeRead.ToInt32(dr, "isgID");
                vm.iwtID = SafeRead.ToInt32(dr, "iwtID");
                vm.itmIsFeatured = SafeRead.ToBoolean(dr, "itmIsFeatured");
                vm.imgID = SafeRead.ToInt32(dr, "imgID");
                vm.imgExtension = SafeRead.ToString(dr, "imgExtension");
                vm.iwaID = SafeRead.ToInt32(dr, "iwaID");
                vm.catID = SafeRead.ToInt32(dr, "catID");
                vm.iwaName = SafeRead.ToString(dr, "iwaName");
                vm.Category = SafeRead.ToString(dr, "Category");
                vm.iwaDisplayName = SafeRead.ToString(dr, "iwaDisplayName");
                vm.iwaRefName = SafeRead.ToString(dr, "iwaRefName");

                vm.iodName = SafeRead.ToString(dr, "iodName");
                vm.AdditionalInformation = WebConvert.PreserveBreaks(SafeRead.ToString(dr, "iwaNotes"));
                if (vm.AdditionalInformation.Length == 0)
                {
                    vm.AdditionalInformation = "No Additional Information Available.";
                }
                vm.BasicInformation = EXBEmailUtility.GetProjectDetails(new EXBItemsWanted(vm.iwaID));
                vm.iwsID = SafeRead.ToInt32(dr, "iwsID");
                vm.Grade = SafeRead.ToString(dr, "grade");
                vm.iwaState = SafeRead.ToString(dr, "iwaState");
                vm.iwaCountry = SafeRead.ToString(dr, "iwaCountry");
                vm.iwaTotalFeet = SafeRead.ToInt32(dr, "iwaTotalFeet");
                vm.iwaQty = SafeRead.ToDecimal(dr, "iwaQty");
                vm.isgName = SafeRead.ToString(dr, "isgName");

                if (vm.iwaState.Length > 0 && vm.iwaCountry.Length > 0)
                {
                    vm.DeliveryLocation = vm.iwaState + ", " + vm.iwaCountry;
                }
                else if (vm.iwaState.Length > 0)
                {
                    vm.DeliveryLocation = vm.iwaState;
                }
                else if (vm.iwaCountry.Length > 0)
                {
                    vm.DeliveryLocation = vm.iwaCountry;
                }
                else
                {
                    vm.DeliveryLocation = "&nbsp;";
                }
                vm.iwaDueDate = SafeRead.ToDateTime(dr, "iwaDecisionDate");
                if (vm.iwaDueDate != DateTime.MinValue)
                {
                    vm.DueDate = vm.iwaDueDate.ToString("M-dd-yyyy");
                }
                else
                {
                    vm.DueDate = "&nbsp;";
                }
                vm.DecisionDate = SafeRead.ToDateTime(dr, "iwaDecisionDate");
                vm.DeliveryDate = SafeRead.ToDateTime(dr, "iwaPickupDate");
                vm.SourceDateStart = SafeRead.ToDateTime(dr, "iwaStartDate");
                vm.SourceDateEnd = SafeRead.ToDateTime(dr, "iwaDueDate");

                break;
            }

            //next get the items
            string selectItemsWantedBids = "select iwaID,iwbSstID,iwbTransportationCostCustomerTotal, iwbServiceCostCustomerTotal, iwbDisplayServiceCost, iwbDisplayDeliveryCost, iwbName, iwbID, vscID, iwbSequence, itmID, itmIsEstimate, loiID, itmName, isPresented, itmQty, uomName, itmTotalFeet, iwaNotLoiName,itmIsFeatured,"
            + " facState, facCountry, iwbTotalFeet, iwbSellPrice, bstID, iwbQty, astID, iwaNotLoiID, iwbDisplay, iwbIsPrefered, iodvalue, Grade, iwbQty, iwbSaleTotal, iwbSellPriceTotal,iwbSellPriceFT, itmIsQuote, bftID, itmSellPriceFT, itmSellPrice "
            + " from vwInternalItemsWantedBids where iwaID IN (" + WebConvert.GetCommaSeparatedValues(iwaIDS) + ") and iwbDisplay = 1 AND itmIsEstimate = 0 ";
            DataTable items = db.Query(selectItemsWantedBids);
            List<Item> allItems = new List<Item>();
            foreach (DataRow dr in items.Rows)
            {
                allItems.Add(new Item(dr));
            }
            vm.BestOptions = new List<Item>();
            vm.QuotedItems = new List<Item>();
            foreach (var item in allItems.Where(x => x.isPresented).OrderBy(x => x.iwbSequence))
            {
                if (vm.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough)
                {
                    if (item.iwbIsPreferred && item.iwbSstID >= (int)EXBSalesStatus.ReadyToPresent)
                    {
                        vm.BestOptions.Add(item);
                    }
                }
                else
                {
                    vm.BestOptions.Add(item);
                }
            }

            vm.SelectedItems = allItems.Where(x => !x.isPresented && !x.iwbIsPreferred).ToList();
            vm.SelectedItemsCount = vm.SelectedItems.Count();
            //vm.BestOptions = allItems.Where(x => x.isPresented && x.iwbIsPreferred).ToList();
            vm.BestOptionsCount = vm.BestOptions.Count();
            //vm.QuotedItems = allItems.Where(x => x.isPresented && !x.iwbIsPreferred).ToList();
            vm.QuotedItemsCount = vm.QuotedItems.Count();
            return vm;
        }

        public static ProjectProfileViewModel GetProjectProfile(int iwaID, int usrID)
        {
            ProjectProfileViewModel vm = new ProjectProfileViewModel();

            //check access to this profile before doing the query
            string iwaSourcingCondigion = EXBUser.GetUserActiveCurrentlySourcingBreakthroughIwaCondition(usrID);
            if (!EXBUser.HasAccessToProject(usrID, iwaID))
            {
                return vm;
            }

            string selectProject = "SELECT * from vwItemsWantedUserPortal WHERE iwaID = " + iwaID;
            EXBDbConnection db = new EXBDbConnection();
            DataTable dt = db.Query(selectProject);
            foreach (DataRow dr in dt.Rows)
            {
                vm.isgName = SafeRead.ToString(dr, "isgName");
                vm.isgID = SafeRead.ToInt32(dr, "isgID");
                vm.iwtID = SafeRead.ToInt32(dr, "iwtID");
                vm.itmIsFeatured = SafeRead.ToBoolean(dr, "itmIsFeatured");
                vm.imgID = SafeRead.ToInt32(dr, "imgID");
                vm.imgExtension = SafeRead.ToString(dr, "imgExtension");
                vm.iwaID = SafeRead.ToInt32(dr, "iwaID");
                vm.catID = SafeRead.ToInt32(dr, "catID");
                vm.iwaName = SafeRead.ToString(dr, "iwaName");
                vm.Category = SafeRead.ToString(dr, "Category");
                vm.iwaDisplayName = SafeRead.ToString(dr, "iwaDisplayName");
                vm.iwaRefName = SafeRead.ToString(dr, "iwaRefName");

                vm.iodName = SafeRead.ToString(dr, "iodName");
                vm.AdditionalInformation = WebConvert.PreserveBreaks(SafeRead.ToString(dr, "iwaNotes"));
                if (vm.AdditionalInformation.Length == 0)
                {
                    vm.AdditionalInformation = "No Additional Information Available.";
                }
                vm.BasicInformation = EXBEmailUtility.GetProjectDetails(new EXBItemsWanted(vm.iwaID));
                vm.iwsID = SafeRead.ToInt32(dr, "iwsID");
                vm.Grade = SafeRead.ToString(dr, "grade");
                vm.iwaState = SafeRead.ToString(dr, "iwaState");
                vm.iwaCountry = SafeRead.ToString(dr, "iwaCountry");
                vm.iwaTotalFeet = SafeRead.ToInt32(dr, "iwaTotalFeet");
                vm.iwaQty = SafeRead.ToDecimal(dr, "iwaQty");
                vm.isgName = SafeRead.ToString(dr, "isgName");

                if (vm.iwaState.Length > 0 && vm.iwaCountry.Length > 0)
                {
                    vm.DeliveryLocation = vm.iwaState + ", " + vm.iwaCountry;
                }
                else if (vm.iwaState.Length > 0)
                {
                    vm.DeliveryLocation = vm.iwaState;
                }
                else if (vm.iwaCountry.Length > 0)
                {
                    vm.DeliveryLocation = vm.iwaCountry;
                }
                else
                {
                    vm.DeliveryLocation = "&nbsp;";
                }
                vm.iwaDueDate = SafeRead.ToDateTime(dr, "iwaDecisionDate");
                if (vm.iwaDueDate != DateTime.MinValue)
                {
                    vm.DueDate = vm.iwaDueDate.ToString("M-dd-yyyy");
                }
                else
                {
                    vm.DueDate = "&nbsp;";
                }
                vm.DecisionDate = SafeRead.ToDateTime(dr, "iwaDecisionDate");
                vm.DeliveryDate = SafeRead.ToDateTime(dr, "iwaPickupDate");
                vm.SourceDateStart = SafeRead.ToDateTime(dr, "iwaStartDate");
                vm.SourceDateEnd = SafeRead.ToDateTime(dr, "iwaDueDate");

                break;
            }

            //next get the items
            string selectItemsWantedBids = "select iwaID,iwbSstID,iwbTransportationCostCustomerTotal, iwbServiceCostCustomerTotal, iwbDisplayServiceCost, iwbDisplayDeliveryCost, iwbName, iwbID, vscID, iwbSequence, itmID, itmIsEstimate, loiID, itmName, isPresented, itmQty, uomName, itmTotalFeet, iwaNotLoiName,itmIsFeatured,"
            + " facState, facCountry, iwbTotalFeet, iwbSellPrice, bstID, iwbQty, astID, iwaNotLoiID, iwbDisplay, iwbIsPrefered, iodvalue, Grade, iwbQty, iwbSaleTotal, iwbSellPriceTotal,iwbSellPriceFT, itmIsQuote, bftID, itmSellPriceFT, itmSellPrice "
            + " from vwInternalItemsWantedBids where iwaID = " + iwaID + " and iwbDisplay = 1 AND itmIsEstimate = 0 ";
            DataTable items = db.Query(selectItemsWantedBids);
            List<Item> allItems = new List<Item>();
            foreach (DataRow dr in items.Rows)
            {
                allItems.Add(new Item(dr));
            }
            vm.BestOptions = new List<Item>();
            vm.QuotedItems = new List<Item>();
            foreach (var item in allItems.Where(x => x.isPresented).OrderBy(x => x.iwbSequence))
            {
                if (vm.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough)
                {
                    if (item.iwbIsPreferred && item.iwbSstID >= (int)EXBSalesStatus.ReadyToPresent)
                    {
                        vm.BestOptions.Add(item);
                    }
                }
                else
                {
                    vm.BestOptions.Add(item);
                }
            }

            vm.SelectedItems = allItems.Where(x => !x.isPresented && !x.iwbIsPreferred).ToList();
            vm.SelectedItemsCount = vm.SelectedItems.Count();
            //vm.BestOptions = allItems.Where(x => x.isPresented && x.iwbIsPreferred).ToList();
            vm.BestOptionsCount = vm.BestOptions.Count();
            //vm.QuotedItems = allItems.Where(x => x.isPresented && !x.iwbIsPreferred).ToList();
            vm.QuotedItemsCount = vm.QuotedItems.Count();
            return vm;
        }


    }
    public class Item
    {
        public decimal itmIndexPrice { get; set; }
        public Item(DataRow dr)
        {
            iwbID = SafeRead.ToInt32(dr, "iwbID");
            icdID = SafeRead.ToInt32(dr, "icdID");
            iwbSequence = SafeRead.ToInt32(dr, "iwbSequence");
            icdName = SafeRead.ToString(dr, "icdName");
            facLocation = SafeRead.ToString(dr, "facLocation");
            itmID = SafeRead.ToInt32(dr, "itmID");
            itmName = SafeRead.ToString(dr, "itmName");
            iwbName = SafeRead.ToString(dr, "iwbName");
            iwbDisplayServiceCost = SafeRead.ToBoolean(dr, "iwbDisplayServiceCost");
            iwbDisplayDeliveryCost = SafeRead.ToBoolean(dr, "iwbDisplayDeliveryCost");
            iwbServiceCostCustomerTotal = SafeRead.ToDecimal(dr, "iwbServiceCostCustomerTotal");
            iwbTransportationCostCustomerTotal = SafeRead.ToDecimal(dr, "iwbTransportationCostCustomerTotal");
            isPresented = SafeRead.ToBoolean(dr, "isPresented");
            iwbIsPreferred = SafeRead.ToBoolean(dr, "iwbIsPrefered");
            itmQty = SafeRead.ToInt32(dr, "itmTotalFeet") != null && SafeRead.ToInt32(dr, "itmTotalFeet") > 0 ? SafeRead.ToDecimal(dr, "itmTotalFeet") : SafeRead.ToDecimal(dr, "itmQty");
            uomName = SafeRead.ToString(dr, "uomName");
            iwbQty = SafeRead.ToDecimal(dr, "iwbQty");
            itmIodOD = SafeRead.ToInt32(dr, "itmIodID");
            itmIodValue = SafeRead.ToString(dr, "iodvalue");
            itmGrdID = SafeRead.ToInt32(dr, "itmGrdID");
            itmTotalFeet = SafeRead.ToInt32(dr, "itmTotalFeet");
            Grade = SafeRead.ToString(dr, "Grade");
            iwbTotalFeet = SafeRead.ToInt32(dr, "iwbTotalFeet");
            iwbSellPriceFT = SafeRead.ToDecimal(dr, "iwbSellPriceFT");
            bstID = SafeRead.ToInt32(dr, "bstID");
            astID = SafeRead.ToInt32(dr, "astID");
            iwbSellPrice = SafeRead.ToDecimal(dr, "iwbSellPrice");
            InterestLevel = SafeRead.ToString(dr, "iwaNotLoiName");
            itmSellPrice = SafeRead.ToDecimal(dr, "itmSellPrice");
            itmSellPriceFT = SafeRead.ToDecimal(dr, "itmSellPriceFT");

            //itmQty = SafeRead.ToInt32(dr, "itmQty");
            loiID = SafeRead.ToInt32(dr, "iwaNotLoiID");
            TotalPrice = SafeRead.ToDecimal(dr, "iwbSellPriceTotal");
            itmIsQuote = SafeRead.ToBoolean(dr, "itmIsQuote");
            vscID = SafeRead.ToInt32(dr, "vscID");
            bftID = SafeRead.ToInt32(dr, "bftID");
            iwaID = SafeRead.ToInt32(dr, "iwaID");
            BuyerFeeType = UtilityTools.ParseBuyerFeeType(bftID);
            itmBuyerFee = SafeRead.ToDecimal(dr, "itmBuyerFee");
            itmBuyerFeeFlat = SafeRead.ToDecimal(dr, "itmBuyerFeeFlat");
            itmIsFeatured = SafeRead.ToBoolean(dr, "itmIsFeatured");
            iwbSstID = SafeRead.ToInt32(dr, "iwbSstID");
            if (String.IsNullOrEmpty(InterestLevel))
            {
                InterestLevel = "Neutral";
            }
        }

        public int iwbID { get; set; }
        public int icdID { get; set; }
        public string icdName { get; set; }
        public string facLocation { get; set; }
        public int itmTotalFeet { get; set; }
        public decimal itmQty { get; set; }
        public int itmGrdID { get; set; }
        public int itmIodOD { get; set; }
        public string itmIodValue { get; set; }
        public int itmID { get; set; }
        public string itmName { get; set; }
        public string iwbName { get; set; }
        public bool iwbIsPreferred { get; set; }
        public bool iwbDisplayDeliveryCost { get; set; }
        public bool iwbDisplayServiceCost { get; set; }
        public decimal iwbTransportationCostCustomerTotal { get; set; }
        public decimal iwbServiceCostCustomerUOM
        {
            get
            {
                return (iwbQty > 0 ? Math.Round(iwbServiceCostCustomerTotal / iwbQty, 2, MidpointRounding.AwayFromZero) : 0);
            }
        }
        public decimal iwbServiceCostCustomerFT
        {
            get { return (iwbTotalFeet > 0 ? Math.Round(iwbServiceCostCustomerTotal / iwbTotalFeet, 2, MidpointRounding.AwayFromZero) : 0); }
        }
        public decimal iwbTransportationCostCustomerUOM
        {
            get { return (iwbQty > 0 ? Math.Round(iwbTransportationCostCustomerTotal / iwbQty, 2, MidpointRounding.AwayFromZero) : 0); }
        }
        public decimal iwbTransportationCostCustomerFT
        {
            get { return (iwbTotalFeet > 0 ? Math.Round(iwbTransportationCostCustomerTotal / iwbTotalFeet, 2, MidpointRounding.AwayFromZero) : 0); }
        }
        public decimal iwbServiceCostCustomerTotal { get; set; }
        public decimal iwbQty { get; set; }
        public int iwbTotalFeet { get; set; }
        public bool isPresented { get; set; }
        public decimal iwbSellPrice { get; set; }
        public int ItemId { get; set; }
        public string Type { get { return this.itmIsQuote && this.vscID > 1 ? "Service" : "Asset"; } }
        public string Name { get { return iwbName; } }
        public string Grade { get; set; }
        public string IODName { get; set; }
        public int QtyWanted { get; set; }
        public int QtyAvailable { get; set; }
        public string InterestLevel { get; set; }
        public string UoM { get; set; }
        public decimal TotalPrice { get; set; }
        //emily 05/12
        public decimal ProductCostTotal
        {
            get
            {
                decimal TotalPriceUpdated = TotalPrice;

                TotalPriceUpdated += iwbServiceCostCustomerTotal;

                TotalPriceUpdated += iwbTransportationCostCustomerTotal;


                return TotalPriceUpdated;
            }
        }
        public decimal ProductCostUOM
        {
            get
            {
                decimal value = iwbSellPrice;

                value += iwbServiceCostCustomerUOM;

                value += iwbTransportationCostCustomerUOM;

                return value;
            }
        }
        public decimal ProductCostFT
        {
            get
            {
                decimal value = iwbSellPriceFT;

                value += iwbServiceCostCustomerFT;

                value += iwbTransportationCostCustomerFT;


                return value;
            }
        }
        public int QuotedPrice { get; set; }
        public string uomName { get; set; }
        public int astID { get; set; }
        public int bstID { get; set; }
        public int loiID { get; set; }
        public decimal iwbSellPriceFT { get; set; }
        public bool itmIsQuote { get; set; }
        public int bftID { get; set; }
        public int iwaID { get; set; }
        public EXBBuyerFeeTypes BuyerFeeType { get; set; }
        public decimal itmBuyerFeeFlat { get; set; }
        public decimal itmBuyerFee { get; set; }

        public Item()
        {

        }


        public bool itmIsFeatured { get; set; }

        public decimal itmSellPrice { get; set; }

        public int imgID { get; set; }

        public string imgExtension { get; set; }

        public string bidID { get; set; }

        public int vscID { get; set; }

        public int iwbSequence { get; set; }

        public decimal itmSellPriceFT { get; set; }
        public int iwbSstID { get; set; }
    }
}