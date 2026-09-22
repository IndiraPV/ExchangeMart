using Exchangebase.Com.Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Exchangebase.Com.Buyer
{
    public partial class MyProjectsNew : EXBBuyerRestrictedPage
    {
        [Serializable]
        public class MyProjectsNewViewModel
        {
            public List<MyProject> MyProjects { get; set; }
            //public List<Item> FeaturedItems { get; set; }
        }
        public MyProjectsNewViewModel ViewModel
        {
            get { return ViewState["ViewModel"] as MyProjectsNewViewModel; }
            set { ViewState["ViewModel"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //ProjectCardsRepeater.ItemDataBound += ProjectCardsRepeater_ItemDataBound;
            if (!Page.IsPostBack)
            {
                //set the project / requests
                if (UtilityTools.GetIsEnergyFlg())
                {
                    myProjectsTitle.Text = "My Projects";
                    ProjectNameLiteral.Text = "Your Sourcing Requests";
                    myprojectsLiteral.Text = "My Projects";
                    mobileNavbarTitle.Text = "Requests";
                    //ViewProjectLiteral.Text = "View Request";
                }
                else
                {
                    mobileNavbarTitle.Text = "Projects";
                    myProjectsTitle.Text = "My Projects";
                    ProjectNameLiteral.Text = "Current Projects";
                    myprojectsLiteral.Text = "My Projects";
                    //ViewProjectLiteral.Text = "View Project";
                }
                ViewModel = new MyProjectsNewViewModel();
                ViewModel.MyProjects = MyProject.GetAll(TheUser.UsrID);
                
                // Code Cleamup - v&e
                EXBDbConnection db = new EXBDbConnection();
                DataTable dt = db.Query("SELECT * FROM vwUserPortalItemsWantedBidLoadOutAll where cusID = " + TheUser.CusID);
                bool isVentures = !UtilityTools.GetIsEnergyFlg();
                var hasSoldItems = dt.Rows != null && dt.Rows.Count > 0 && isVentures; //ventures
                if (hasSoldItems && isVentures) //hide if none or if ventures
                {
                    SoldItemsRepeater.DataSource = dt;
                    SoldItemsRepeater.DataBind();
                }
                else
                {
                    SoldItemsRepeater.Visible = false;
                    soldItemsTitle.Visible = false;
                    soldItemsDiv.Visible = false;
                }

                Func<MyProject, bool> currentProject = (x) => { return x.iwsID >= (int)EXBItemsWantedStatus.Qualifying && x.iwsID <= (int)EXBItemsWantedStatus.ContractReview; };
                Func<MyProject, bool> closedProject = (x) => { return x.iwsID == (int)EXBItemsWantedStatus.Fulfilled || x.iwsID == (int)EXBItemsWantedStatus.ApprovedExecutive || x.iwsID == (int)EXBItemsWantedStatus.ApprovedFinance || x.iwsID == (int)EXBItemsWantedStatus.OrderFulfillment; };
                Func<MyProject, bool> breakthrough = (x) => { return x.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough; };

                var breakthroughProjects = ViewModel.MyProjects.Where(currentProject).Where(breakthrough);
                var currentProjects = ViewModel.MyProjects.Where(currentProject).Where(x => !breakthrough(x));
                var closedProjects = ViewModel.MyProjects.Where(closedProject).Where(x => !breakthrough(x));

                ProjectNamesRepeater.DataSource = currentProjects;
                ProjectNamesRepeaterClosed.DataSource = closedProjects;
                breakThroughProjectsRepeater.DataSource = breakthroughProjects;
                breakThroughProjectsRepeater.DataBind();

                bool hasBreakthrough = breakthroughProjects.Any();
                bool hasCurrentlySourcing = currentProjects.Any();
                bool hasClosedProjects = closedProjects.Any();
                bool hasNone = !hasBreakthrough && !hasCurrentlySourcing && !hasClosedProjects;
                bool onlyBreakthrough = hasBreakthrough && !hasCurrentlySourcing && !hasClosedProjects;

                //there's no projects, show empty state
                ItemsPanel.Visible = false;
                bool isLoadout = WebConvert.ToInt32(Request.QueryString["LoadOutID"], -1) > 0 && isVentures; //isVentures
                if(hasNone && hasSoldItems)
                {
                    if (!isLoadout && WebConvert.ToInt32(Request.QueryString["LoadOutID"], -1) == -1)
                    {
                        Response.Redirect("~/Buyer/MyProjectsNew.aspx?LoadOutID=" + SafeRead.ToInt32(dt.Rows[0], "itmID").ToString());
                    }
                }
                else if (hasNone)
                {
                    SideBarPanel.Visible = false;
                    myProjectEmptyPanel.Visible = true;
                }
                else if (onlyBreakthrough)
                {
                    SideBarPanel.Visible = false;
                    ProjectProfilePanel.Visible = true;
                    if (!isLoadout && WebConvert.ToInt32(Request.QueryString["PID"], -1) == -1)
                    {
                        Response.Redirect("~/Buyer/MyProjectsNew.aspx?PID=" + breakthroughProjects.First().iwaID);
                    }

                }
                else if (hasCurrentlySourcing)
                {
                    SideBarPanel.Visible = true;
                    ProjectProfilePanel.Visible = true;
                    if (!isLoadout && WebConvert.ToInt32(Request.QueryString["PID"], -1) == -1)
                    {
                        Response.Redirect("~/Buyer/MyProjectsNew.aspx?PID=" + currentProjects.First().iwaID);
                    }
                }
                else //doesn't have currently sourcing, only closed
                {
                    //has either closed or currently sourcing
                    SideBarPanel.Visible = true;
                    ProjectProfilePanel.Visible = true;
                    if (!isLoadout && WebConvert.ToInt32(Request.QueryString["PID"], -1) == -1)
                    {
                        Response.Redirect("~/Buyer/MyProjectsNew.aspx?PID=" + closedProjects.First().iwaID);
                    }
                }
                if (hasCurrentlySourcing)
                {
                    SideBarPanel.Visible = true;
                    ProjectNamesRepeater.Visible = true;
                    titleCurrentProjects.Visible = true;
                }
                if (hasClosedProjects)
                {
                    SideBarPanel.Visible = true;
                    ProjectNamesRepeaterClosed.Visible = true;
                    closedProjectTitle.Visible = true;
                }
                if (!hasCurrentlySourcing)
                {
                    titleCurrentProjects.Visible = false;
                }
                //else show the sidepanel

                ////get featured items
                //List<Item> featuredItems = new List<Item>();
                ////ViewModel.FeaturedItems = new List<Item>();
                //foreach (var breakthroughProject in breakthroughProjects)
                //{
                //    var bProjectProfile = ProjectProfileViewModel.GetProjectProfile(breakthroughProject.iwaID, TheUser.UsrID);
                //    foreach (var item in bProjectProfile.BestOptions)
                //    {
                //        item.iwaID = bProjectProfile.iwaID;
                //        if(item.astID == (int)EXBAssetType.Equipment)
                //        {
                //            item.TotalPrice = item.QtyAvailable * item.iwbSellPrice;
                //        }
                //        else
                //        {
                //            item.TotalPrice = item.itmTotalFeet * item.iwbSellPriceFT;
                //        }
                //    }
                //    featuredItems.AddRange(bProjectProfile.BestOptions);
                //}
                //if (featuredItems.Any())
                //{

                //    featuredAssetsRepeater.DataSource = featuredItems;
                //    featuredAssetsRepeater.DataBind();
                //    highlyDiscountedDiv.Visible = true;
                //}
                //else
                //{
                //    highlyDiscountedDiv.Visible = false;
                //    featuredItemsDiv.Visible = false;
                //}



                if (UtilityTools.GetIsEnergyFlg())
                {
                    var energyString = GetEnergyString();
                    welcomeText1.Text = welcomeText2.Text = energyString;
                    //if (breakthroughProjects.Any() || featuredItems.Any())
                    //{
                    //    featuredItemsDiv.Visible = true;
                    //    //welcomeTextLarge.Attributes["class"] = "col-lg-6 col-md-12 d-lg-block d-xl-block ";
                    //}
                    //else
                    //{
                    //    featuredItemsDiv.Visible = false;
                    //   // welcomeTextLarge.Attributes["class"] = "col-lg-6 col-md-12 d-lg-block d-xl-block col-offset-3 ";
                    //}
                }
                else
                {
                    if (!hasBreakthrough)
                    {
                        featuredAssetDiv1.Visible = false;
                        featuredAssetDiv2.Visible = false;
                    }
                    var venturesString = GetVenturesString();
                    welcomeText1.Text = venturesString;
                    welcomeText2.Text = venturesString;
                    //featuredItemsDiv.Visible = false;
                }

                //are you in energy? Show the energy string.
                //do you have highly discounted assets? Show them. 
                //are there no breakthroughs or items? Center the welcome text

                if (!breakthroughProjects.Any())
                {
                    highlyDiscountedDiv1.Visible = false;
                    
                    breakThroughProjectsRepeater.Visible = false;
                }
                else
                {
                    highlyDiscountedDiv1.Visible = true;
                    breakThroughProjectsRepeater.Visible = false;
                    string breakthroughIwaIDs = WebConvert.GetCommaSeparatedValues(breakthroughProjects.Select(target => target.iwaID).ToList());
                    highlyDiscountedLabel1.CommandArgument = breakthroughIwaIDs;

                }
                if (!ViewModel.MyProjects.Any(closedProject))
                {
                    ProjectNamesRepeaterClosed.Visible = false;
                    closedProjectTitle.Visible = false;
                }
                else
                {
                    ProjectNamesRepeaterClosed.Visible = true;
                    closedProjectTitle.Visible = true;
                }

                //ProjectCardsRepeater.DataBind();
                ProjectNamesRepeater.DataBind();
                ProjectNamesRepeaterClosed.DataBind();
                //if (TheUser != null)
                //{
                //    contactNameSpan1.Text = contactNameSpan.Text = ", " + TheUser.UsrFirstName + ", "; //.UsrFullName + ", ";
                //    if (TheUser.PMAdmID > 0)
                //    {
                //        EXBAdministrator admin = new EXBAdministrator(TheUser.PMAdmID);
                //        //adminCell.Text = ", " + admin.ph
                //        admName.Text = admName1.Text = "<b>" + admin.FullName + "</b>";
                //        admEmail.Text = admEmail.Text = " <a href='mailTo:" + admin.AdmEmail + "?subject=user portal asset request for new project" + "'>" + admin.AdmEmail + "</a>";
                //        buyRepLiteral.Text = buyRepExtLiteral2.Text = buyRepExtLiteral1.Text = " <a href='mailTo:" + admin.AdmEmail + "?subject=user portal asset request for new project" + "'>" + admin.FullName + "</a>";
                //        buyRepExtLiteral.Text = buyRepExtLiteral2.Text = buyRepExtLiteral1.Text = admin.AdmPhoneExt != null && admin.AdmPhoneExt.Length > 0 ? " x" + admin.AdmPhoneExt : "";
                //    }
                //    else
                //    {
                //        buyRepLiteral.Text = buyRepLiteral2.Text = buyRepLiteral1.Text = " <a href='mailTo:customerservice@exchangebase.com??subject=user portal asset request for new project'>Concierge Service</a>";
                //    }
                //}
                if (!ViewModel.MyProjects.Any() && !hasSoldItems)
                {
                    myProjectEmptyPanel.Visible = true;
                    SideBarPanel.Visible = false;
                    //MainItemsPanel.Visible = false;
                    ItemsPanel.Visible = false;


                    return;
                }

            }

            int iwaID = WebConvert.ToInt32(Request.QueryString["PID"], 0);
            int itmID = WebConvert.ToInt32(Request.QueryString["IID"], 0);
            int loadOutItemId = WebConvert.ToInt32(Request.QueryString["LoadOutID"], 0);
            if (iwaID > 0 && itmID == 0)
            {
                ShowProjectProfile(iwaID);
            }
            else if (iwaID > 0 && itmID > 0)
            {
                //show item profile
                iwaIDFilterValue.Value = iwaID.ToString();
                SetActiveItem();
                ShowItemProfile(iwaID, itmID);
            }
            else if (loadOutItemId > 0)
            {
                iwaIDFilterValue.Value = loadOutItemId.ToString();
                ShowLoadoutProfile(loadOutItemId);
            }
            else 
            {
                //show all projects
                ProjectProfilePanel.Visible = false;
                ItemsPanel.Visible = true;
                ItemProfileControl.itmID = 0;
                ItemProfileControl.iwaID = 0;
                ItemProfilePanel.Visible = false;
                ItemProfileControl.UpdateFlag = false;
            }

        }

        private string GetVenturesString()
        {
            return GetEnergyString();
        }

        private string GetEnergyString()
        {
            string userName = " ";
            string adminName = "<b>ExchangeBase</b>";
            string adminPhoneNumber = "<b>(440) 331-3600</b>";
            string adminEmail = "<a href='mailTo:customerservice@exchangebase.com??subject=user portal asset request for new project'>Concierge Service</a>";
            if (TheUser != null)
            {
                userName = ", " + TheUser.UsrFirstName + ", ";
                if (TheUser.PMAdmID > 0)
                {
                    EXBAdministrator admin = new EXBAdministrator(TheUser.PMAdmID);
                    //adminCell.Text = ", " + admin.ph
                    adminName = "<b>" + admin.FullName + "</b>";
                    adminEmail = " <a href='mailTo:" + admin.AdmEmail + "?subject=user portal asset request for new project" + "'>" + admin.AdmEmail + "</a>";
                    adminPhoneNumber = admin.AdmPhoneExt != null && admin.AdmPhoneExt.Length > 0 ? " x" + admin.AdmPhoneExt : "";
                    if (adminPhoneNumber.Length < 6) //in case no phone number was found for this person or invalid phone number
                    {
                        adminPhoneNumber = "<b>(440) 331-3600</b>";
                    }
                }
            }
            string sourcingRequests = "sourcing requests";
            string requests = "requests";
            if (UtilityTools.GetIsEnergyFlg())
            {
                sourcingRequests = "sourcing requests";
            }
            else
            {
                sourcingRequests = "projects";
            }
            return "<p>Welcome to your ExchangeBase Data Room where you will find specs, documents, pictures, and quotes associated with your " + sourcingRequests + " and purchased items.</p>"
                + "<p>Please contact your Senior Project Manager " + adminName + " at " + adminPhoneNumber + " or email " + adminEmail + " with any questions or additional sourcing requests.</p>";
        }

        void ProjectCardsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            MyProject project = e.Item.DataItem as MyProject;
            var reviewLabel = e.Item.FindControl("ContractReviewStatusLabel") as Label;
            var completeLabel = e.Item.FindControl("CompleteStatusLabel") as Label;
            //var sourcingLabel = null; // e.Item.FindControl("CurrentlySourcingStatusLabel") as Label;
            var DecisionDateLabel = e.Item.FindControl("DecisionDateLabel") as Label;
            var DeliveryLabel = e.Item.FindControl("DeliveryLabel") as Label;
            if (project.iwsID == (int)EXBItemsWantedStatus.Qualifying || project.iwsID == (int)EXBItemsWantedStatus.Qualified || project.iwsID == (int)EXBItemsWantedStatus.ClearingEvaluation || project.iwsID == (int)EXBItemsWantedStatus.Sourcing || project.iwsID == (int)EXBItemsWantedStatus.SourcingCompleted || project.iwsID == (int)EXBItemsWantedStatus.CustomerReview)
            {
                //sourcingLabel.Visible = true;
            }
            else if (project.iwsID == (int)EXBItemsWantedStatus.Fulfilled || project.iwsID == (int)EXBItemsWantedStatus.ApprovedExecutive || project.iwsID == (int)EXBItemsWantedStatus.ApprovedFinance || project.iwsID == (int)EXBItemsWantedStatus.OrderFulfillment)
            {
                completeLabel.Visible = true;
            }
            else if (project.iwsID == (int)EXBItemsWantedStatus.ContractReview)
            {
                reviewLabel.Visible = true;
            }
            if (String.IsNullOrEmpty(project.DeliveryLocation))
            {
                DeliveryLabel.Visible = false;
            }
            if (String.IsNullOrEmpty(project.DueDate))
            {
                DecisionDateLabel.Visible = false;
            }
            Literal viewProjectLiteral = e.Item.FindControl("viewProjectLiteral") as Literal;
            if (viewProjectLiteral != null)
            {
                if (UtilityTools.GetIsEnergyFlg())
                {
                    viewProjectLiteral.Text = "View Project";
                }
                else
                {
                    viewProjectLiteral.Text = "View Project";
                }
            }
        }

        protected void ShowProjectProfile(int iwaID)
        {
            iwaIDFilterValue.Value = iwaID.ToString();
            SetActiveItem();
            WoopraReportAction("ShowProject: " + iwaID);
            ProjectProfilePanel.Visible = true;
            myLoadoutPanel.Visible = false;
            ItemsPanel.Visible = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            ItemProfilePanel.Visible = false;
            ItemProfileControl.UpdateFlag = false;
        }

        protected void ShowLoadoutProfile(int loadOutID)
        {
            soldItemFilterValue.Value = loadOutID.ToString();
            SetActiveItem();
            WoopraReportAction("Show LoadOut for item : " + loadOutID);
            myLoadoutPanel.Visible = true;
            ProjectProfilePanel.Visible = false;
            ItemsPanel.Visible = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            ItemProfilePanel.Visible = false;
            ItemProfileControl.UpdateFlag = false;
        }

        protected void BackButton_ProjectProfile_Command(object sender, CommandEventArgs e)
        {
            //ProjectProfilePanel.Visible = false;
            //ProjectCardsPanel.Visible = true;
            myLoadoutPanel.Visible = false;
            Response.Redirect("~/Buyer/MyProjectsNew.aspx");
            WoopraReportAction("Project BackButton");
        }

        protected void BackButton_ItemProfile_Command(object sender, CommandEventArgs e)
        {
            int iwaID = WebConvert.ToInt32(Request.QueryString["PID"], 0);
            Response.Redirect("~/Buyer/MyProjectsNew.aspx?PID=" + iwaID);
            WoopraReportAction("ItemProfile BackButton");
        }

        protected void IwaLinkButton_Command(object sender, CommandEventArgs e)
        {
            WoopraReportAction("Sidepanel Clicked to View Project :" + e.CommandArgument);
            Response.Redirect("~/Buyer/MyProjectsNew.aspx?PID=" + e.CommandArgument);
        }

        private void SetActiveItem()
        {
            string activeItem = iwaIDFilterValue.Value;
            foreach (RepeaterItem ri in ProjectNamesRepeater.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField IwaIDHidden = ri.FindControl("IwaIDHidden") as HiddenField;
                    LinkButton li = ri.FindControl("IwaLinkButton") as LinkButton;
                    HtmlContainerControl div = ri.FindControl("divForActive") as HtmlContainerControl;
                    if (IwaIDHidden != null && div != null)
                    {
                        if (IwaIDHidden.Value == activeItem)
                        {
                            div.Attributes["class"] = "d-block active";
                            //li.Attributes["class"] = "sidebar-link active";
                        }
                        else
                        {
                            div.Attributes["class"] = "d-block";
                            //li.Attributes["class"] = "sidebar-link";
                        }
                    }
                }
            }
            bool foundBreakthrough = false;
            foreach (RepeaterItem ri in breakThroughProjectsRepeater.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField IwaIDHidden = ri.FindControl("IwaIDHidden") as HiddenField;
                    LinkButton li = ri.FindControl("IwaLinkButton") as LinkButton;
                    HtmlContainerControl div = ri.FindControl("divForActive") as HtmlContainerControl;
                    if (IwaIDHidden != null && div != null)
                    {
                        if (IwaIDHidden.Value == activeItem)
                        {
                            div.Attributes["class"] = "d-block active";
                            foundBreakthrough = true;
                            //li.Attributes["class"] = "sidebar-link active";
                        }
                        else
                        {
                            div.Attributes["class"] = "d-block";
                            //li.Attributes["class"] = "sidebar-link";
                        }
                    }
                    
                }
            }
            if (foundBreakthrough)
            {
                highlyDiscountedDiv1.CssClass = "d-block active filter-text mb-1 pb-2 d-flex justify-content-between d-none";

            }
            else
            {
                highlyDiscountedDiv1.CssClass = "filter-text mb-1 pb-2 d-flex justify-content-between d-none";

            }
            foreach (RepeaterItem ri in ProjectNamesRepeaterClosed.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField IwaIDHidden = ri.FindControl("IwaIDHidden") as HiddenField;
                    LinkButton li = ri.FindControl("IwaLinkButton") as LinkButton;
                    HtmlContainerControl div = ri.FindControl("divForActive") as HtmlContainerControl;
                    if (IwaIDHidden != null && div != null)
                    {
                        if (IwaIDHidden.Value == activeItem)
                        {
                            div.Attributes["class"] = "d-block active";
                            //li.Attributes["class"] = "sidebar-link active";
                        }
                        else
                        {
                            div.Attributes["class"] = "d-block";
                            //li.Attributes["class"] = "sidebar-link";
                        }
                    }
                }
            }
            foreach (RepeaterItem ri in SoldItemsRepeater.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField IwaIDHidden = ri.FindControl("IwaIDHidden") as HiddenField;
                    LinkButton li = ri.FindControl("SoldItemLinkButton") as LinkButton;
                    HtmlContainerControl div = ri.FindControl("divForActive") as HtmlContainerControl;
                    if (IwaIDHidden != null && div != null)
                    {
                        if (IwaIDHidden.Value == activeItem)
                        {
                            div.Attributes["class"] = "d-block active";
                            //li.Attributes["class"] = "sidebar-link active";
                        }
                        else
                        {
                            div.Attributes["class"] = "d-block";
                            //li.Attributes["class"] = "sidebar-link";
                        }
                    }
                }
            }
        }
        private void ShowItemProfile(int iwaID, int itmID)
        {
            ItemProfileControl.itmID = itmID;
            ItemProfileControl.iwaID = iwaID;
            ItemProfileControl.UpdateFlag = true;
            ItemProfilePanel.Visible = true;
            myLoadoutPanel.Visible = false;
            ProjectProfilePanel.Visible = false;
            ItemsPanel.Visible = false;
        }
        protected void ItemProfileControl_BackButton_Clicked(object sender, EventArgs e)
        {
            int iwaID = WebConvert.ToInt32(Request.QueryString["PID"], 0);
            Response.Redirect("~/Buyer/MyProjectsNew.aspx?PID=" + iwaID);
        }
        private void WoopraReportAction(string description)
        {
            if (HttpContext.Current.Request.Url.Host.Contains("demo.exchangebase.com"))
            { }
            //else if (HttpContext.Current.Request.Url.Host.Contains("dev.exchangebase.com"))
            //{}
            else if (HttpContext.Current.Request.Url.Host.Contains("test.exchangebase.com"))
            { }
            else if (HttpContext.Current.Request.Url.Host.Contains("exchangebase.com"))
            {
                string script = ("<script>woopra.track('userportal', {page: 'Current Inventory', description: '" + description
                     + "'});</script>");

                Literal analyticsInjection = new Literal();
                analyticsInjection.Text = script.ToString();
                Page.Header.Controls.Add(analyticsInjection);
            }
        }

        protected void ProjectNamesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlContainerControl div = e.Item.FindControl("projectNumberLabel") as HtmlContainerControl;
            if (div != null)
            {
                var dataItem = e.Item.DataItem as MyProject;
                if (dataItem.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough)
                {
                    div.Visible = false;
                }
                else
                {
                    div.Visible = true;
                }
            }
        }

        protected void featuredAssetsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                if (item.itmIsQuote && item.BuyerFeeType == EXBBuyerFeeTypes.FlatFee)
                {
                    qtyWanted.Text = "N/A";
                }
                else if (item.itmIsQuote && item.BuyerFeeType == EXBBuyerFeeTypes.PerUOM)
                {
                    qtyWanted.Text = item.iwbQty == 0 ? "N/A" : String.Format("{0:N0}", item.iwbQty) + "";
                }
                else
                {
                    qtyWanted.Text = item.iwbTotalFeet == 0 ? "N/A" : String.Format("{0:N0}", item.iwbTotalFeet) + " ft";
                    qtyAvailable.Text = item.itmTotalFeet == 0 ? "N/A" : String.Format("{0:N0}", item.itmTotalFeet) + " ft";
                }


            }

            if (iwbSellPriceLiteral != null)
            {
                if (assetType == EXBAssetType.Equipment && itmQty <= 1 && item.uomName == "EA")
                {
                    //only if itmUomID == 1
                    iwbSellPriceLiteral.Text = String.Format("{0:C0}", iwbSellPrice);
                    //iwbSellPriceTotalLiteral.Text = String.Format("{0:C0}", iwbSellPrice * item.itmQty);
                }
                else
                {
                    if (iwbSellPrice == 0)
                    {
                        iwbSellPriceLiteral.Text = "N/A";
                        //iwbSellPriceTotalLiteral.Text = "N/A";
                    }
                    else
                    {

                        if (item.itmIsQuote && item.BuyerFeeType == EXBBuyerFeeTypes.PerUOM)
                        {
                            iwbSellPriceLiteral.Text = assetType == EXBAssetType.Equipment ? String.Format("{0:C0}", iwbSellPrice) + "" : String.Format("{0:C2}", item.iwbSellPriceFT) + "";
                        }
                        else if (item.itmIsQuote && item.BuyerFeeType == EXBBuyerFeeTypes.FlatFee)
                        {
                            iwbSellPriceLiteral.Text = assetType == EXBAssetType.Equipment ? String.Format("{0:C0}", iwbSellPrice) + "" + "" : String.Format("{0:C2}", item.iwbSellPriceFT) + "";
                        }
                        else
                        {
                            iwbSellPriceLiteral.Text = assetType == EXBAssetType.Equipment ? String.Format("{0:C0}", iwbSellPrice) + "/" + item.uomName.ToLower() : String.Format("{0:C2}", item.iwbSellPriceFT) + "/ft";
                        }

                        if (item.iwbTotalFeet == 0)
                        {
                            //iwbSellPriceTotalLiteral.Text = "N/A";
                        }
                        else
                        {
                            //iwbSellPriceTotalLiteral.Text = String.Format("{0:C0}", item.TotalPrice);
                        }
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
                    if (item.itmIsQuote)
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
                            iwbSellPriceLabel.Text = String.Format("{0:C0}", iwbSellPrice);
                        }
                        else
                        {
                            iwbSellPriceLabel.Text = String.Format("{0:C0}", iwbSellPrice) + " /ea";
                        }
                    }
                }
                else
                {
                    if (item.itmIsQuote)
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
                        iwbSellPriceLabel.Text = item.iwbSellPriceFT > 0 ? String.Format("{0:C2}", item.iwbSellPriceFT) + " /ft" : "N/A";
                    }
                }
            }

            Image itmImgMain = e.Item.FindControl("itmImgMain") as Image;

            if (itmImgMain != null)
            {
                if (item.imgID > 0) //dr["imgID"], 0) > 0)
                {
                    itmImgMain.ImageUrl = EXBImage.GetImageWebFilePath(item.imgID, item.imgExtension);// WebConvert.ToString(dr.imgExtensiondr["imgExtension"], ".jpg"));
                }
                else
                {
                    itmImgMain.ImageUrl = "~/Images/image-unavailable2.png";
                }
            }
            bool itemIsFeatured = item.itmIsFeatured;// false;// WebConvert.ToBoolean(dr["itmIsFeatured"], false);
            //hide featured panel or not
            Panel featuredPanel = e.Item.FindControl("FeaturedPanel") as Panel;
            if (featuredPanel != null)
            {
                featuredPanel.Visible = itemIsFeatured;
            }
        }

        protected void SoldItemLinkButton_Command(object sender, CommandEventArgs e)
        {
            var loadID = WebConvert.ToInt32(e.CommandArgument, 0);
            Response.Redirect("~/Buyer/MyProjectsNew.aspx?LoadOutID=" + loadID);
          

        }

    }

    [Serializable]
    public class MyProject
    {
        public int ProjectId { get { return iwaID; } }
        public string ProjectName { get { return iwaDisplayName; } }
        public string DeliveryLocation { get; set; }
        public string DueDate { get; set; }
        public int QuotedItems { get; set; }
        public int SelectedItems { get; set; }
        public int BestOptionItemsCount { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public string iodName { get; set; }
        public int iwaID { get; set; }
        public string iwaName { get; set; }
        public string iwaDisplayName { get; set; }
        public string iwsName { get; set; }
        //public int BestOptions { get; set; }
        //public string iod { get; set; }
        public int iwtID { get; set; }
        public int iwsID { get; set; }
        public int isgID { get; set; }

        public MyProject()
        {

        }

        public static List<MyProject> GetAll(int usrID, string andWhere = "")
        {
            List<MyProject> projects = new List<MyProject>();

            List<vwUserPortalItemsWanted> itemsWantedBids = new List<vwUserPortalItemsWanted>();
            itemsWantedBids = vwUserPortalItemsWanted.GetAll(usrID, andWhere);

            foreach (vwUserPortalItemsWanted item in itemsWantedBids)
            {
                if (projects.Any(x => x.iwaID == item.iwaID))
                {
                    //increase the count for selected or quoted, only if there's an item in there (bidid > 0)
                    var selected = projects.First(x => x.iwaID == item.iwaID);
                    if (item.iwbDisplay && item.isPresented && item.iwbIsPrefered)
                    {
                        selected.BestOptionItemsCount++;
                    }
                    else if (item.iwbDisplay && item.isPresented)
                    {
                        selected.QuotedItems++;
                    }
                    else if (item.iwbDisplay)
                    {
                        selected.SelectedItems++;
                    }
                }
                else
                {
                    MyProject project = new MyProject();
                    project.iwaID = item.iwaID;
                    project.iwaName = item.iwaName;
                    project.DeliveryLocation = item.DeliveryLocation;
                    project.DueDate = item.DueDate;
                    project.iwsID = item.iwsID;
                    project.Status = item.iwsName;
                    project.Grade = item.grade;
                    project.iodName = item.iodName;
                    project.iwaDisplayName = item.iwaDisplayName;
                    project.iwaRefName = item.iwaRefName;
                    project.isgID = item.isgID;

                    //do count
                    if (item.iwbDisplay && item.isPresented && item.iwbIsPrefered)
                    {
                        project.BestOptionItemsCount++;
                    }
                    else if (item.iwbDisplay && item.isPresented)
                    {
                        project.QuotedItems++;
                    }
                    else if (item.iwbDisplay)
                    {
                        project.SelectedItems++;
                    }
                    projects.Add(project);
                    //if iwb is null, then there's a project without any selected, quoted, or best option items.
                }
            }


            return projects;
        }

        public string iwaRefName { get; set; }
    }

    public class vwUserPortalItemsWanted
    {
        public int iwaID { get; set; }
        public string iwaName { get; set; }
        public string iwaDisplayName { get; set; }
        public string iwaRefName { get; set; }
        public DateTime iwaDueDate { get; set; }
        public string DueDate { get; set; }
        public string iwaState { get; set; }
        public string iwaCountry { get; set; }
        public string DeliveryLocation { get; set; }
        public int grdID { get; set; }
        public string grade { get; set; }
        public int bidID { get; set; }
        public bool isPresented { get; set; }
        public bool iwbIsPrefered { get; set; }
        public int iodID { get; set; }
        public string iodName { get; set; }
        public int iwsID { get; set; }
        public int iwtID { get; set; }
        public bool iwaActive { get; set; }
        public int isgID { get; set; }
        public string iwsName { get; set; }
        public bool iwbDisplay { get; set; }
        public int iwbSstID { get; set; }


        public static List<vwUserPortalItemsWanted> GetAll(int usrID, string andWhere = "")
        {
            List<vwUserPortalItemsWanted> itemsWanted = new List<vwUserPortalItemsWanted>();
            //string iwaWhere = EXBUser.GetUserActiveCurrentlySourcingIwaCondition(usrID);
            string iwaWhere = EXBUser.GetUserActiveCurrentlySourcingBreakthroughIwaCondition(usrID);
            if (iwaWhere.Length > 0)
            {
                string sqlSelect = "SELECT * from vwItemsWantedBidUserPortalMyProjectsPage  WHERE iwaActive = 1 AND " + iwaWhere + " AND iwbDisplay = 1 " +
                    andWhere + " AND iwsID IN ( " + (int)EXBItemsWantedStatus.Qualifying + "," + (int)EXBItemsWantedStatus.Qualified + "," + (int)EXBItemsWantedStatus.ClearingEvaluation + "," + (int)EXBItemsWantedStatus.Sourcing
                    + "," + (int)EXBItemsWantedStatus.SourcingCompleted + "," + (int)EXBItemsWantedStatus.CustomerReview
                    + "," + (int)EXBItemsWantedStatus.ContractReview + "," + (int)EXBItemsWantedStatus.ApprovedExecutive + "," + (int)EXBItemsWantedStatus.ApprovedFinance + "," + (int)EXBItemsWantedStatus.OrderFulfillment + "," + (int)EXBItemsWantedStatus.Fulfilled + ") ORDER BY isgID  DESC ";
                EXBDbConnection db = new EXBDbConnection();
                DataTable dt = db.Query(sqlSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    vwUserPortalItemsWanted project = new vwUserPortalItemsWanted();
                    project.iwaID = SafeRead.ToInt32(dr, "iwaID");
                    project.iwaName = SafeRead.ToString(dr, "iwaName");
                    project.iwaRefName = SafeRead.ToString(dr, "iwaRefName");
                    project.iwaDisplayName = project.iwaRefName.Length > 0 ? project.iwaRefName : project.iwaName;
                    project.iwaState = SafeRead.ToString(dr, "iwaState");
                    project.iwaCountry = SafeRead.ToString(dr, "iwaCountry");
                    if (project.iwaState.Length > 0 && project.iwaCountry.Length > 0)
                    {
                        project.DeliveryLocation = project.iwaState + ", " + project.iwaCountry;
                    }
                    else if (project.iwaState.Length > 0)
                    {
                        project.DeliveryLocation = project.iwaState;
                    }
                    else if (project.iwaCountry.Length > 0)
                    {
                        project.DeliveryLocation = project.iwaCountry;
                    }
                    else
                    {
                        project.DeliveryLocation = "N/A";
                    }
                    project.iwaDueDate = SafeRead.ToDateTime(dr, "iwaDecisionDate");
                    if (project.iwaDueDate != DateTime.MinValue)
                    {
                        project.DueDate = project.iwaDueDate.ToString("M-dd-yyyy");
                    }
                    else
                    {
                        project.DueDate = "N/A";
                    }
                    project.grdID = SafeRead.ToInt32(dr, "grdID");
                    project.grade = SafeRead.ToString(dr, "grade");
                    project.bidID = SafeRead.ToInt32(dr, "bidID");
                    project.iodID = SafeRead.ToInt32(dr, "iodID");
                    project.isPresented = SafeRead.ToBoolean(dr, "isPresented");
                    project.iwbIsPrefered = SafeRead.ToBoolean(dr, "iwbIsPrefered");
                    project.iwbDisplay = SafeRead.ToBoolean(dr, "iwbDisplay");
                    project.iodName = SafeRead.ToString(dr, "iodName");
                    project.iwsName = SafeRead.ToString(dr, "iwsName");
                    project.iwsID = SafeRead.ToInt32(dr, "iwsID");
                    project.iwtID = SafeRead.ToInt32(dr, "iwtID");
                    project.isgID = SafeRead.ToInt32(dr, "isgID");
                    project.iwbSstID = SafeRead.ToInt32(dr, "iwbSstID");

                    if (project.isgID == (int)EXBItemsWantedSourcingStage.Breakthrough)
                    {
                        switch (project.iwtID)
                        {
                            case (int)EXBAssetType.Equipment:
                                project.iwaRefName = "Equipment";
                                break;
                            case (int)EXBAssetType.LinePipe:
                                project.iwaRefName = "Line Pipe";
                                break;
                            case (int)EXBAssetType.OCTG:
                                project.iwaRefName = "Casing, Tubing, & Drill Pipe";
                                break;
                            default:
                                project.iwaRefName = "Highly Discounted Assets and Emerging Technology";
                                break;
                        }
                    }
                    itemsWanted.Add(project);
                }
            }
            return itemsWanted;
        }
    }
}