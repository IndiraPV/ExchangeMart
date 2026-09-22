using Exchangebase.Com.Bll;
using Exchangebase.Com.buyer;
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
    public partial class ItemProfileControl : System.Web.UI.UserControl
    {
        public int itmID
        {
            get
            {
                return WebConvert.ToInt32(ItmIDHiddenField.Value, 0);
            }
            set
            {
                ItmIDHiddenField.Value = value.ToString();
            }
        }
        public int iwaID;
        public int iwbID;
        public EXBItem targetItem;
        public EXBItemsWantedBid iwb;
        public EXBItemsWanted iwa;
        public EXBBid targetBid;
        private EXBCustomer customer;
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

        public EXBCustomer TheCustomer
        {
            get
            {
                customer = EXBCustomer.GetCustomerSession();
                if (customer == null)
                {
                    throw new EXBBuyerSessionDNEException();
                }
                return customer;
            }
        }

        public bool UpdateFlag;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //itmID = WebConvert.ToInt32(Request.QueryString["id"], 0);
            //iwaID = WebConvert.ToInt32(Request.QueryString["iwaID"], 0);
            iwbID = WebConvert.ToInt32(Request.QueryString["iwbID"], 0);
            if (itmID < 1)
            {
                return;
            }
            //Wire Events
            IdcRepeater.ItemDataBound += IdcRepeater_ItemDataBound;
            IimRepeater.ItemDataBound += IimRepeater_ItemDataBound;
            IimRepeater2.ItemDataBound += IimRepeater2_ItemDataBound;

            if (itmID == 0 || (TheUser == null && TheCustomer == null && EXBAdministrator.GetAdministratorSession() == null))
            {
                // Item DNE
                Response.Redirect("~/Buyer/MyProjectsNew.aspx");
            }

            // Load the requested item from the database
            targetItem = new EXBItem(itmID);

            // Get the bid that the item belongs to
            targetBid = targetItem.GetAttachedBid();

            if (iwaID > 0 && targetBid != null)
            {
                iwa = new EXBItemsWanted(iwaID);
                iwb = new EXBItemsWantedBid(iwbID);
            }

            //set switch for viewing items and quotes
            ItemImageAndDescriptionPanel1.Visible = (!targetItem.ItmIsQuote || (targetItem.ItmIsQuote && targetItem.VscID <= 1));
            QuotesDetailsPanel.Visible = targetItem.ItmIsQuote && targetItem.VscID > 1;
            ItemTabs.Visible = (!targetItem.ItmIsQuote || (targetItem.ItmIsQuote && targetItem.VscID <= 1));
            QuoteTabs.Visible = targetItem.ItmIsQuote && targetItem.VscID > 1;
            if (targetItem.ItmIsQuote && targetItem.VscID > 1)
            {
                ServiceNameLiteral.Text = targetItem.VscName;
                description.Attributes.Add("class", "tab-pane");
                documentsA.Attributes.Add("class", "nav-link mx-1 active");
                documents.Attributes.Add("class", "tab-pane active");
                GradeDiv.Visible = targetItem.Grade.TagValue != "Unknown";
                serviceDate.Visible = targetBid.BidOpenEndDate != null;
            }
            else
            {
                documents.Attributes.Add("class", "tab-pane");
                description.Attributes.Add("class", "tab-pane active");

            }

            if (!CheckUserAccess())
            {
                // Redirect out
                Response.Redirect("~/Buyer/MyProjectsNew.aspx");
            }

            // Set the page title to show the bid name
            Page.Title = "EXB Asset View: " + targetItem.ItmName;

            // Prepare the slimbox
            //SetSlimboxImages();

            if (UpdateFlag || !IsPostBack)
            {
                PopulateForm();
                SetWoopra();
                UpdateFlag = false;
            }
        }

        private bool CheckUserAccess()
        {
            if (targetBid == null || targetItem == null)
            {
                // Bid not in an acceptable state for public view
                return false;
            }

            if (targetBid.BidStatus == EXBBidStatus.Awareness || targetBid.BidStatus == EXBBidStatus.Lead || targetBid.BidStatus == EXBBidStatus.Upcoming || targetBid.BidStatus == EXBBidStatus.Prospective || targetBid.BidStatus == EXBBidStatus.Operations || targetBid.BidStatus == EXBBidStatus.Cancelled || targetBid.BidStatus == EXBBidStatus.Lost)
            {
                // Bid not in an acceptable state for public view
                return false;
            }

            if (targetBid.CusID == TheUser.CusID)
            {
                // User cannot see assets from his own company
                return false;
            }

            if (targetBid.BidStatus == EXBBidStatus.Closed && iwaID < 1)
            {
                // Bid is Closed and there is no valid project id
                return false;
            }

            if (iwaID > 0)
            {
                // Check if User has access to the Project
                if (!UtilityTools.GetItemsWantedAccess(iwaID, TheUser.UsrID, targetBid.BidID, targetBid.BidStatus))
                {
                    iwaID = 0;
                }
            }

            return true;
        }

        private void PopulateForm()
        {
            if (UtilityTools.GetIsEnergyFlg())
            {
                //viewProjectLiteral.Text = "View Project";
            }
            else
            {
                //viewProjectLiteral.Text = "View Project";
            }
            //populate the first grid
            ItemTitle.Text = "Quote # " + iwb.IwbID + " " + iwb.IwbName;
            itmIDLiteral.Text = iwb.IwbID.ToString();
            itmLocationLiteral.Text = targetItem.FacState.Length > 0 ? targetItem.FacState + ", " + targetItem.FacCountry : targetItem.FacCountry;
            itmLocationLiteral2.Text = targetItem.FacState.Length > 0 ? targetItem.FacState + ", " + targetItem.FacCountry : targetItem.FacCountry;
            QtyWantedLiteral.Text = String.Format("{0:n0}", EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID));
            itmDescriptionLiteral.Text = targetItem.ItmDescription.Length > 0 ? WebConvert.PreserveBreaks(targetItem.ItmDescription) : "No additional information available";
            //commentLiteral.Text = targetitem
            itmPipeGradeLiteral.Text = ItmGradeLiteral.Text = targetItem.Grade.TagValue;
            if (targetItem.ItmConditionDisplay == "New")
            {
                ItmConditionNew.Visible = true;
                ItmCondition1Used.Visible = false;
            }
            else
            {
                ItmConditionNew.Visible = true;
                ItmCondition1Used.Visible = false;
            }
            ItmConditionLiteral.Text = targetItem.ItmConditionDisplay;
            ItmMTRLiteral.Text = targetItem.HasMtr ? "Yes" : "No";
            ItmCountryOfOriginLiteral.Text = targetItem.ItmCountryOfOrigin;
            if (String.IsNullOrEmpty(ItmCountryOfOriginLiteral.Text))
            {
                ItmCountryOfOriginLiteral.Text = "N/A";
            }
            ItmManufactureYearLiteral.Text = WebConvert.ToString(String.Format("{0:yyyy}", targetItem.ItmMfrYear), "");
            if (String.IsNullOrEmpty(ItmManufactureYearLiteral.Text))
            {
                ItmManufactureYearLiteral.Text = "N/A";
            }
            ItmMillLiteral.Text = targetItem.ItmMill;
            if (String.IsNullOrEmpty(ItmMillLiteral.Text))
            {
                ItmMillLiteral.Text = "N/A";
            }
            SetContactInfo();

            //set a few other properties
            itmODLiteral.Text = targetItem.ItmODValueDisplay;
            itmSeamTypeLiteral.Text = targetItem.SeamType == EXBItemSeamType.Unknown ? "N/A" : targetItem.SeamType.ToString();
            itmHasCoatingLiteral.Text = targetItem.HasCoating ? "Yes" : "No";


            bool iwaFlg = false;

            if (iwaID > 0 && iwa != null)
            {
                iwaFlg = UtilityTools.GetItemsWantedAccess(iwaID, TheUser.UsrID, targetBid.BidID, targetBid.BidStatus);

            }
            var pricing = new ItemsWantedBidPricing(iwa, iwb, targetItem, targetBid, removeCapital:false);

            switch (targetItem.AssetType)
            {
                case EXBAssetType.Equipment:
                    if (targetItem.ItmQty == 1)
                    {
                        itmQtyLiteral.Text = targetItem.ItmQty > 0 ? String.Format("{0:n0}", targetItem.ItmQty) : "N/A";
                    }
                    else
                    {
                        itmQtyLiteral.Text = targetItem.ItmQty > 0 ? String.Format("{0:n0}", targetItem.ItmQty) : "N/A";
                    }

                    SetDisplayType(EXBAssetType.Equipment);

                    itmIndexPriceLiteral.Text = targetItem.ItmIndexPrice > 0 ? String.Format("{0:C0}", targetItem.ItmIndexPrice) + " " + targetItem.GetUOMDisplay().ToLower() : "N/A";
                    //ItemIndexPriceLabel.Text = "Original Price: ";
                    if (iwaFlg)
                    {
                        decimal iwbSellPrice = EXBItemsWanted.GetQuotePrice(iwaID, targetBid.BidID);
                        decimal iwbQty = EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID);
                        QtyWantedLiteral.Text = iwbQtyLiteral.Text = iwbQty > 0 ? String.Format("{0:N0}", EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID)) : "N/A";
                        //hide the large green asking price 
                        itmSellPricePanel.Visible = false;
                        //show the little grey asking price
                        itmSellPriceLiteral2.Text = targetItem.ItmSellPrice > 0 ? String.Format("{0:C0}", targetItem.ItmSellPrice) + " " + targetItem.GetUOMDisplay().ToLower() : "N/A";
                        iwbSellPriceLiteral.Text = iwbSellPrice > 0 ? String.Format("{0:C0}", iwbSellPrice) + " " + targetItem.GetUOMDisplay().ToLower() : "N/A";
                        itmSellPricePanel_Small.Visible = true;
                        //show the quote price
                        iwbSellPricePanel.Visible = false;
                        iwbQtyPanel.Visible = true;
                    }
                    else
                    {
                        itmSellPriceLiteral.Text = targetItem.ItmSellPrice > 0 ? String.Format("{0:C0}", targetItem.ItmSellPrice) + " " + targetItem.GetUOMDisplay().ToLower() : "N/A";
                        totalSellPriceLiteral.Text = targetItem.ItmSellPrice > 0 ? String.Format("{0:C0}", Math.Round(targetItem.ItmSellPrice, 3) * targetItem.ItmQty) : "N/A";
                        TotalSellPricePanel.Visible = true;
                        itmSellPricePanel.Visible = true;
                        viewPricing.Visible = true;
                        quotedPricing.Visible = false;
                        iwbSellPricePanel.Visible = false;// !itmSellPricePanel.Visible;
                        iwbQtyPanel.Visible = false;
                    }
                    EquipmentGrade.Visible = true;

                    //if there is attributes show the attributes.
                    var attributeTags = targetItem.AttributeTags;
                    if (attributeTags.Any())
                    {
                        AttributesRepeater.Visible = true;
                        AttributesRepeater.DataSource = attributeTags.Where(x => x.TagValue != "Not Specified");
                        AttributesRepeater.DataBind();
                    }
                    else
                    {
                        AttributesRepeater.Visible = false;
                    }
                    break;
                case EXBAssetType.LinePipe:
                    //ItemIndexPriceLabel.Text = "Index Price: ";
                    SetDisplayType(EXBAssetType.LinePipe);

                    itmODLiteral.Text = targetItem.ItmODValueDisplay;
                    itmWTLiteral.Text = targetItem.ItmIDValueDisplay;
                    itmQtyLiteral.Text = targetItem.ItmTotalFeet > 0 ? String.Format("{0:N0}", targetItem.ItmTotalFeet) + " ft <br />" : "N/A";
                    itmIndexPriceLiteral.Text = targetItem.ItmIndexPriceFT > 0 ? String.Format("{0:C2}", targetItem.ItmIndexPriceFT) + " /ft <br />" + String.Format("{0:C2}", targetItem.ItmIndexPrice) + " /st" : "N/A";
                    EquipmentGrade.Visible = false;
                    itmSellPricePanel_Small.Visible = false;
                    if (iwaFlg)
                    {
                        decimal quotePriceUOM = EXBItemsWanted.GetQuotePrice(iwaID, targetBid.BidID);
                        decimal quotePriceFT = targetItem.ItmTotalFeet > 0 ? WebConvert.ToDecimal(Math.Round((quotePriceUOM * targetItem.ItmQty) / targetItem.ItmTotalFeet, 2, MidpointRounding.AwayFromZero), 0m) : 0;
                        //show the quote price
                        iwbSellPriceLiteral.Text = quotePriceFT > 0 ? String.Format("{0:C2}", quotePriceFT) + " /ft <br />" + String.Format("{0:C2}", quotePriceUOM) + " /st" : "N/A";
                        iwbSellPricePanel.Visible = false;
                        //hide the large green asking price
                        itmSellPricePanel.Visible = false;
                        //show the little grey asking price
                        itmSellPriceLiteral2.Text = targetItem.ItmSellPriceFT > 0 ? String.Format("{0:C2}", targetItem.ItmSellPriceFT) + " /ft <br />" + String.Format("{0:C2}", targetItem.ItmSellPrice) + " /st" : "N/A";
                        itmSellPricePanel_Small.Visible = true;
                        //show quantity info
                        int iwbTotalFeet = EXBItemsWanted.GetQuantityWantedFT(iwaID, targetBid.BidID);
                        QtyWantedLiteral.Text = iwbQtyLiteral.Text = iwbTotalFeet > 0 ? String.Format("{0:N0}", iwbTotalFeet) + " " + " ft" : "N/A";
                        if (targetItem.ItmIsQuote && targetItem.VscID > 1)
                        {
                            decimal iwbQty = EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID);
                            QtyWantedLiteral.Text = iwbQty > 0 ? String.Format("{0:N0}", EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID)) : "N/A";
                        }
                        iwbQtyPanel.Visible = true;
                    }
                    else
                    {
                        itmSellPriceLiteral.Text = targetItem.ItmSellPriceFT > 0 ? String.Format("{0:C2}", targetItem.ItmSellPriceFT) + " /ft <br />" + String.Format("{0:C2}", targetItem.ItmSellPrice) + " /st" : "N/A";
                        totalSellPriceLiteral.Text = targetItem.ItmSellPrice > 0 ? String.Format("{0:C0}", Math.Round(targetItem.ItmSellPriceFT, 3) * targetItem.ItmTotalFeet) : "N/A";
                        TotalSellPricePanel.Visible = true;
                        itmSellPricePanel.Visible = true;
                        iwbSellPricePanel.Visible = false;// !itmSellPricePanel.Visible;
                        iwbQtyPanel.Visible = false;
                        itmSellPricePanel.Visible = true;
                        viewPricing.Visible = true;
                        quotedPricing.Visible = false;
                    }
                    AttributesRepeater.Visible = false;
                    break;
                case EXBAssetType.OCTG:
                    //ItemIndexPriceLabel.Text = "Index Price: ";
                    SetDisplayType(EXBAssetType.OCTG);

                    itmWeightLiteral.Text = targetItem.ItmWeightDisplay;
                    itmEndConnectionLiteral.Text = targetItem.ItmEndConnection;
                    itmQtyLiteral.Text = targetItem.ItmTotalFeet > 0 ? String.Format("{0:N0}", targetItem.ItmTotalFeet) + " ft" : "N/A";
                    itmIndexPriceLiteral.Text = targetItem.ItmIndexPriceFT > 0 ? String.Format("{0:C2}", targetItem.ItmIndexPriceFT) + " /ft <br />" + String.Format("{0:C2}", targetItem.ItmIndexPrice) + " /st" : "N/A";
                    EquipmentGrade.Visible = false;
                    itmSellPricePanel_Small.Visible = false;
                    if (iwaFlg)
                    {
                        decimal quotePriceUOM = EXBItemsWanted.GetQuotePrice(iwaID, targetBid.BidID);
                        decimal quotePriceFT = targetItem.ItmTotalFeet > 0 ? WebConvert.ToDecimal(Math.Round((quotePriceUOM * targetItem.ItmQty) / targetItem.ItmTotalFeet, 2, MidpointRounding.AwayFromZero), 0m) : 0;
                        iwbSellPriceLiteral.Text = quotePriceFT > 0 ? String.Format("{0:C2}", quotePriceFT) + " /ft <br />" + String.Format("{0:C2}", quotePriceUOM) + " /st" : "N/A";
                        int iwbTotalFeet = EXBItemsWanted.GetQuantityWantedFT(iwaID, targetBid.BidID);
                        QtyWantedLiteral.Text = iwbQtyLiteral.Text = iwbTotalFeet > 0 ? String.Format("{0:N0}", iwbTotalFeet) + " " + " ft" : "N/A";
                        if (targetItem.ItmIsQuote && targetItem.VscID > 1)
                        {
                            decimal iwbQty = EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID);
                            QtyWantedLiteral.Text = iwbQty > 0 ? String.Format("{0:N0}", EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID)) : "N/A";
                        }
                        //hide the large green asking price
                        itmSellPricePanel.Visible = false;
                        //show the large green quote price
                        iwbSellPricePanel.Visible = true;
                        iwbQtyPanel.Visible = true;
                        //show the little grey asking price
                        itmSellPriceLiteral2.Text = targetItem.ItmSellPriceFT > 0 ? String.Format("{0:C2}", targetItem.ItmSellPriceFT) + " /ft <br />" + String.Format("{0:C2}", targetItem.ItmSellPrice) + " /st" : "N/A";
                        itmSellPricePanel_Small.Visible = true;
                        int iwbTotalFeetOCTG = EXBItemsWanted.GetQuantityWantedFT(iwaID, targetBid.BidID);
                        QtyWantedLiteral.Text = iwbQtyLiteral.Text = iwbTotalFeetOCTG > 0 ? String.Format("{0:N0}", iwbTotalFeetOCTG) + " " + " ft" : "N/A";
                        if (targetItem.ItmIsQuote && targetItem.VscID > 1)
                        {
                            decimal iwbQty = EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID);
                            QtyWantedLiteral.Text = iwbQty > 0 ? String.Format("{0:N0}", EXBItemsWanted.GetQuantityWantedUOM(iwaID, targetBid.BidID)) : "N/A";
                        }
                        iwbQtyPanel.Visible = true;

                    }
                    else
                    {

                        itmSellPriceLiteral.Text = targetItem.ItmSellPriceFT > 0 ? String.Format("{0:C2}", targetItem.ItmSellPriceFT) + " /ft <br />" + String.Format("{0:C2}", targetItem.ItmSellPrice) + " /st" : "N/A";
                        itmSellPricePanel.Visible = true;
                        totalSellPriceLiteral.Text = targetItem.ItmSellPrice > 0 ? String.Format("{0:C0}", Math.Round(targetItem.ItmSellPriceFT, 3) * targetItem.ItmTotalFeet) : "N/A";
                        TotalSellPricePanel.Visible = true;
                        iwbSellPricePanel.Visible = !itmSellPricePanel.Visible;
                        iwbQtyPanel.Visible = false;
                        itmSellPricePanel.Visible = true;
                        viewPricing.Visible = true;
                        quotedPricing.Visible = false;
                    }
                    AttributesRepeater.Visible = false;
                    break;


            }

            if (iwaFlg)
            {
                basicInfo.Text = EXBEmailUtility.GetItemDetails(targetItem);
                if (pricing.hasFreightCost)
                {
                    decimal productCostTotal = iwb.IwbSellPriceTotal;
                    decimal productCostUOM = iwb.IwbSellPrice;
                    decimal productCostFT = iwb.IwbSellPriceFT;

                    decimal freightCostTotal = iwb.IwbTransportationCostCustomerTotal;
                    decimal freightCostUOM = iwb.IwbTransportationCostCustomerUOM;
                    decimal freightCostFT = iwb.IwbTransportationCostCustomerFT;

                    productCostTotal += iwb.IwbServiceCostCustomerTotal;
                    productCostUOM += iwb.IwbServiceCostCustomerUOM;
                    productCostFT += iwb.IwbServiceCostCustomerFT;

                    if (!iwb.IwbDisplayDeliveryCost)
                    {
                        productCostTotal += iwb.IwbTransportationCostCustomerTotal;
                        productCostUOM += iwb.IwbTransportationCostCustomerUOM;
                        productCostFT += iwb.IwbTransportationCostCustomerFT;

                        pricingDivFreight.Visible = false;
                        pricingDivFreight2.Visible = false;
                        pricingDevFreightSpacer.Visible = false;
                    }

                    if (targetItem.AssetType != EXBAssetType.Equipment)
                    {
                        pricingTotalUOM.Text = pricing.TotalPriceFT;
                        pricingFreightUOM.Text = String.Format("{0:C2}", freightCostFT) + " /FT";
                        pricingProductUOM.Text = String.Format("{0:C2}", productCostFT) + " /FT";
                    }
                    else
                    {
                        pricingTotalUOM.Text = pricing.TotalPriceUOM;
                        pricingFreightUOM.Text = String.Format("{0:C2}", freightCostUOM) + " " + iwb.Item.GetUOMDisplay();
                        pricingProductUOM.Text = String.Format("{0:C2}", productCostUOM) + " " + iwb.Item.GetUOMDisplay();
                    }


                    pricingTotal.Text = pricing.TotalPrice;

                    pricingFreight.Text = String.Format("{0:C2}", freightCostTotal);
                    pricingProduct.Text = String.Format("{0:C2}", productCostTotal);

                }
                else
                {
                    //emily 05/12
                    decimal productCostTotal = iwb.IwbSellPriceTotal;
                    decimal productCostUOM = iwb.IwbSellPrice;
                    decimal productCostFT = iwb.IwbSellPriceFT;


                    productCostTotal += iwb.IwbServiceCostCustomerTotal;
                    productCostUOM += iwb.IwbServiceCostCustomerUOM;
                    productCostFT += iwb.IwbServiceCostCustomerFT;

                    productCostTotal += iwb.IwbTransportationCostCustomerTotal;
                    productCostUOM += iwb.IwbTransportationCostCustomerUOM;
                    productCostFT += iwb.IwbTransportationCostCustomerFT;


                    pricingDivFreight.Visible = false;
                    pricingDivFreight2.Visible = false;
                    pricingDivProduct.Visible = false;
                    pricingDivProduct2.Visible = false;
                    pricingDevFreightSpacer.Visible = false;
                    pricingDivProductSpacer.Visible = false;
                    totalPriceLabelUOM.InnerText = "Quoted Price / UOM";
                    totalPriceLabel.InnerText = "Quoted Price";

                    if (targetItem.AssetType != EXBAssetType.Equipment)
                    {

                        pricingTotalUOM.Text = String.Format("{0:C2}", productCostFT) + " /FT";
                    }
                    else
                    {
                        pricingTotalUOM.Text = String.Format("{0:C2}", productCostUOM) + " " + iwb.Item.GetUOMDisplay();
                    }
                    pricingTotal.Text = String.Format("{0:C2}", productCostTotal);
                }
            }

            SetupWatchlist();

            EXBDbConnection db = new EXBDbConnection();
            if (iwaID == 0)
            {
                itemDocumentCountLabel.Text = "0";
                itemQuoteDocumentCountLabel.Text = "0";

            }
            else
            {
                if (iwaFlg)
                {
                    itemDocumentCountLabel.Text = WebConvert.ToInt32(db.ExecuteScalar("SELECT COUNT(ibdID) FROM InternalBidDocuments WHERE bidID = " + targetBid.BidID + " AND ibdID in (SELECT ibdID FROM ItemsWantedBidInternalDocuments WHERE iwaID = " + iwaID + ")"), 0).ToString();
                    itemQuoteDocumentCountLabel.Text = itemDocumentCountLabel.Text;
                    //show the loadout for purchased items 
                    if (iwa.Status == EXBItemsWantedStatus.Fulfilled || iwa.Status == EXBItemsWantedStatus.OrderFulfillment)
                    {
                        loadoutTab.Visible = loadoutTab2.Visible = true;
                        LoadOutViewModel vm = LoadOutViewModel.GetViewModel(itmID, TheUser.CusID, true);
                        itemLoadOutLabel.Text = itemLoadOutLabel2.Text = itmLoadShippedLabel.Text = vm.LoadOuts.Count().ToString();
                        if (vm.LoadOuts.Count() == 0)
                        {
                            yesLoadoutsDiv.Visible = false;
                            noLoadoutsDiv.Visible = true;
                        }
                        else
                        {
                            yesLoadoutsDiv.Visible = true;
                            noLoadoutsDiv.Visible = false;
                        }
                        if (targetItem.AssetType == EXBAssetType.Equipment)
                        {
                            itmTotalFeetShippedLabel.Text = String.Format("{0:N0}", vm.LoadOuts.Sum(x => x.iblTotalQty));
                        }
                        else if (targetItem.AssetType == EXBAssetType.LinePipe || targetItem.AssetType == EXBAssetType.OCTG)
                        {
                            itmTotalFeetShippedLabel.Text = String.Format("{0:N0} FT", vm.LoadOuts.Sum(x => x.iblTotalFeet).ToString());
                        }
                        LoadoutRepeater.DataSource = vm.LoadOuts;
                        LoadoutRepeater.DataBind();

                    }
                }
                else
                {
                    itemDocumentCountLabel.Text = "0";
                    itemQuoteDocumentCountLabel.Text = "0";
                }
                commentLiteral.Text = iwb.IwbOptionNotes != null && iwb.IwbOptionNotes.Length > 0 ? WebConvert.PreserveBreaks(iwb.IwbOptionNotes) : "No Comments Available";
            }
            itemImageCountLabel.Text = WebConvert.ToInt32(db.ExecuteScalar("SELECT COUNT(iimID) FROM ItemImages WHERE itmID = " + targetItem.ItmID + " AND iitID = " + (int)EXBItemImageTypes.Icon), 0).ToString();

            // Bind the item's documents
            if (iwaID == 0)
            {
                IdcRepeater.DataSource = null;
            }
            else
            {
                if (iwaFlg)
                {
                    IdcRepeater.DataSource = EXBInternalBidDocument.GetInternalBidDocuments(targetBid.BidID, iwaID, true);
                }
                else
                {
                    IdcRepeater.DataSource = null;
                }
            }
            IdcRepeater.DataBind();
            if (IdcRepeater.Items.Count == 0)
            {
                IdcRepeaterEmpty.Visible = true;
            }

            IimRepeater.DataSource = targetItem.Images.Where(target => target.Type == EXBItemImageTypes.Icon);
            IimRepeater.DataBind();

            IimRepeater2.DataSource = targetItem.Images.Where(target => target.Type == EXBItemImageTypes.Thumbnail);
            IimRepeater2.DataBind();
            if (targetItem != null && targetItem.Images != null && targetItem.Images.Any())
            {
                var tryFirstLargeImage = targetItem.Images.OrderBy(x => x.IimSequence).FirstOrDefault(target => target.Type == EXBItemImageTypes.Large);
                if (tryFirstLargeImage != null)
                {
                    mainImg.ImageUrl = tryFirstLargeImage.WebFilePath;
                    mainImgLink.Attributes.Add("href", tryFirstLargeImage.WebFilePath);
                }
                else
                {
                    mainImg.ImageUrl = targetItem.Images.OrderBy(x => x.IimSequence).First().WebFilePath;
                    mainImgLink.Attributes.Add("href", targetItem.Images.First().WebFilePath);
                }
            }
            else
            {
                //the image may not be loaded correctly, null out main image link
                mainImg.ImageUrl = "/Images/image-unavailable2.png";
                mainImgLink.Attributes.Add("href", "");
            }

            //LightGalleryRepeater.DataSource = targetItem.Images.Where(target => target.Type == EXBItemImageTypes.Large);
            //LightGalleryRepeater.DataBind();
            if (targetItem.ItmIsQuote && targetItem.VscID > 1)
            {
                decimal totalPrice = 0;
                if (targetItem.BuyerFeeType == EXBBuyerFeeTypes.FlatFee)
                {
                    ServicePriceLiteral1.Text = String.Format("{0:C0}", EXBItemsWanted.GetQuotePrice(iwaID, targetBid.BidID));
                }
                else if (targetItem.BuyerFeeType == EXBBuyerFeeTypes.PerUOM)
                {
                    if (targetItem.AssetType == EXBAssetType.Equipment)
                    {
                        ServicePriceLiteral1.Text = String.Format("{0:C0}", EXBItemsWanted.GetQuotePrice(iwaID, targetBid.BidID)) + " /ea";
                    }
                    else
                    {
                        ServicePriceLiteral1.Text = String.Format("{0:C0}", EXBItemsWanted.GetQuotePrice(iwaID, targetBid.BidID)) + " /ea";
                    }
                }
                DateLiteral.Text = targetBid.BidOpenEndDate == null ? "" : ((DateTime)targetBid.BidOpenEndDate).ToString("MM-dd-yyyy");
            }

        }

        private void SetDisplayType(EXBAssetType type)
        {
            switch (type)
            {
                case EXBAssetType.LinePipe:
                    LinePipeOCTGPanel.Visible = true;
                    LinePipeRows.Visible = true;
                    OriginalPriceIndexPriceLiteral.Text = "Index Price";
                    break;
                case EXBAssetType.OCTG:
                    LinePipeOCTGPanel.Visible = true;
                    OCTGRows.Visible = true;
                    OriginalPriceIndexPriceLiteral.Text = "Index Price";
                    break;
                case EXBAssetType.Equipment:
                    LinePipeOCTGPanel.Visible = false;
                    LinePipeRows.Visible = false;
                    OriginalPriceIndexPriceLiteral.Text = "Original Price";
                    break;
                default:
                    break;
            }
        }
        void IdcRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (IdcRepeater == null || IdcRepeater.Items == null || IdcRepeater.Items.Count < 1)
            {
                //if (e.Item.ItemType == ListItemType.Footer)
                //{
                //    ((HtmlTableRow)e.Item.FindControl("trEmptyRow")).Visible = true;
                //}
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EXBInternalBidDocument doc = (EXBInternalBidDocument)e.Item.DataItem;
                HyperLink docType = (HyperLink)e.Item.FindControl("hypDocType");
                HyperLink docFilename = (HyperLink)e.Item.FindControl("hypDocFilename");
                HyperLink docSize = (HyperLink)e.Item.FindControl("hypDocSize");
                HyperLink downloadDocumentButton = (HyperLink)e.Item.FindControl("downloadDocumentButton");

                // Set the document filename and download links
                if (docType != null)
                {
                    docType.Text = doc.DccName;
                    docType.NavigateUrl = "~/GetDoc.ashx?ibdID=" + doc.IbdID + "&iwaID=" + iwaID;
                }

                if (docFilename != null)
                {
                    docFilename.Text = doc.DocFilename;
                    docFilename.NavigateUrl = "~/GetDoc.ashx?ibdID=" + doc.IbdID + "&iwaID=" + iwaID;
                }

                if (docSize != null)
                {
                    docSize.Text = (doc.DocSize / 1024) + " KB";
                    docSize.NavigateUrl = "~/GetDoc.ashx?ibdID=" + doc.IbdID + "&iwaID=" + iwaID;
                }

                if (downloadDocumentButton != null)
                {
                    downloadDocumentButton.NavigateUrl = "~/GetDoc.ashx?ibdID=" + doc.IbdID + "&iwaID=" + iwaID;
                }
            }
        }

        void IimRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (IimRepeater.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    ((HtmlTableRow)e.Item.FindControl("trEmptyRow")).Visible = true;
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EXBItemImage image = (EXBItemImage)e.Item.DataItem;
                Image itmImageLogo = (Image)e.Item.FindControl("itmImageLogo");
                HyperLink imgDownload = (HyperLink)e.Item.FindControl("hypImgDownload");
                Literal imgCaption = (Literal)e.Item.FindControl("imgCaption");

                if (itmImageLogo != null)
                {
                    itmImageLogo.ImageUrl = image.WebFilePath;
                }
                if (imgCaption != null)
                {
                    imgCaption.Text = image.IimCaption;
                    if (String.IsNullOrEmpty(imgCaption.Text))
                    {
                        imgCaption.Text = "Image " + (e.Item.ItemIndex + 1).ToString();
                    }
                }

                List<int> iimIDList = targetItem.Images.Where(target => target.Type == EXBItemImageTypes.Large && target.IimSequence == image.IimSequence).Select(target => target.IimID).ToList();

                if (imgDownload != null)
                {
                    imgDownload.NavigateUrl = "~/GetImage.ashx?iimID=" + (iimIDList.Count > 0 ? iimIDList[0] : 0);
                }
            }
        }

        void IimRepeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (IimRepeater.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    ((HtmlTableRow)e.Item.FindControl("trEmptyRow")).Visible = true;
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EXBItemImage image = (EXBItemImage)e.Item.DataItem;
                Image itmImageLogo = (Image)e.Item.FindControl("itmImageLogo");
                HyperLink imgDownload = (HyperLink)e.Item.FindControl("hypImgDownload");
                Literal imgCaption = (Literal)e.Item.FindControl("imgCaption");

                if (itmImageLogo != null)
                {
                    itmImageLogo.ImageUrl = image.WebFilePath;
                }
                if (imgCaption != null)
                {
                    imgCaption.Text = image.IimCaption;
                    if (String.IsNullOrEmpty(imgCaption.Text))
                    {
                        imgCaption.Text = "Image " + e.Item.ItemIndex + 1;
                    }
                }
                List<int> iimIDList = targetItem.Images.Where(target => target.Type == EXBItemImageTypes.Large && target.IimSequence == image.IimSequence).Select(target => target.IimID).ToList();

                if (imgDownload != null)
                {
                    imgDownload.NavigateUrl = "~/GetImage.ashx?iimID=" + (iimIDList.Count > 0 ? iimIDList[0] : 0);
                }
            }
        }

        private void SetContactInfo()
        {
            if (TheUser != null)
            {
                if (TheUser.PMAdmID > 0)
                {
                    EXBAdministrator admin = new EXBAdministrator(TheUser.PMAdmID);
                    buyerRepLiteral.Text = " <a href='mailTo:" + admin.AdmEmail + "?Subject=Question: Asset # " + targetItem.ItmID + "' class='d-inline-block mx-2 mt-1 btn btn-outline-primary'><i class='fa fa-envelope mr-md-2' aria-hidden='true'></i><span class='d-none d-md-inline-block'>" + admin.FullName + "</span></a>";
                    buyerRepExtLiteral.Text = admin.AdmPhoneExt != null && admin.AdmPhoneExt.Length > 0 ? " x" + admin.AdmPhoneExt : "";
                }
                else
                {
                    buyerRepLiteral.Text = " <a href='mailTo:customerservice@exchangebase.com?Subject=Question: Asset # " + targetItem.ItmID + "' class='d-inline-block mx-2 mt-1 btn btn-outline-primary'><i class='fa fa-envelope mr-md-2' aria-hidden='true'></i><span class='d-none d-md-inline-block'>Customer Service</span></a>";
                }
            }
        }

        protected void SetWoopra()
        {
            if (!IsPostBack || UpdateFlag)
            {
                if (AnalyticsInjection.IsWoopraTrackEnabled())
                {
                    // Inject woopra code
                    WoopraInjection();
                }
            }
        }

        public event EventHandler BackButton_Clicked;

        private void WoopraInjection()
        {
            string email = "";

            if (EXBCustomer.GetCustomerSession() != null)
            {
                email = EXBAdministrator.GetAdministratorEmail(TheUser.PMAdmID);
            }

            StringBuilder script = new StringBuilder();
            script.AppendLine("woopra.track('bidview', {bidid: '" + targetItem.ItmID + "', bidname: '" + targetBid.CusName + " - " + targetItem.ItmName.Replace("'", "") + "', domain: '" + UtilityTools.GetDomain() + "', buyerrepemail: '" + email + "'});");
            ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", script.ToString(), true);
        }

        private void SetupWatchlist()
        {
        }

        private void WoopraReportAction(string description)
        {
            if (AnalyticsInjection.IsWoopraTrackEnabled())
            {
                // Inject woopra code
                string script = ("woopra.track('userportal', {page: 'Item Profile', description: '" + description
                    + "'});");
                ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", script.ToString(), true);
            }
        }

        protected void Back_Button_Clicked(object sender, CommandEventArgs e)
        {
            if (BackButton_Clicked != null)
            {
                BackButton_Clicked.Invoke(sender, e);
            }
        }
        
        protected void LoadoutRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlContainerControl row = e.Item.FindControl("Tr1") as HtmlContainerControl;

            if (row != null) //hide the view documents button if there are no documents
            {
                if (e.Item.DataItem is LoadOut)
                {
                    if ((e.Item.DataItem as LoadOut).Documents.Count == 0)
                    {
                        if (row.Controls.Count > 7)
                        {
                            if (row.Controls[7].Controls.Count > 0)
                            {
                                row.Controls[7].Controls[0].Visible = false;
                            }
                        }
                    }
                }
            }
        }


        protected void ServicesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EXBItemsWantedBidServiceCost serviceCost = e.Item.DataItem as EXBItemsWantedBidServiceCost;
            Label name = e.Item.FindControl("name") as Label;
            Label price = e.Item.FindControl("price") as Label;
            Label qty = e.Item.FindControl("qty") as Label;
            Label total = e.Item.FindControl("total") as Label;

            if (name != null)
            {
                name.Text = serviceCost.Item.ItmName;
            }

            if(price != null)
            {
                price.Text = String.Format("{0:C2}", serviceCost.IbsSellPrice) + serviceCost.Item.GetUOMDisplay();
            }

            if (qty != null)
            {
                qty.Text = String.Format("{0:N0}", serviceCost.IbsQty) + " " + serviceCost.Item.UOM.ToString();
            }

            if (total != null)
            {
                total.Text = String.Format("{0:C2}", serviceCost.TotalRevenue);
            }
        }

    }
}