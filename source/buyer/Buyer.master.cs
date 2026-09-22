using Exchangebase.Com.Bll;
using Exchangebase.Com.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Exchangebase.Com.Buyer
{
    public partial class Buyer : System.Web.UI.MasterPage
    {
        private bool isLoggedIn = false;
        private string currentFileName;
        public EXBUser TheUser = null;
        public EXBCustomer TheCustomer = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            currentFileName = Path.GetFileName(Request.Url.AbsolutePath);

            bool onLandingPage = (String.Compare(currentFileName, "LandingPage.aspx", false) == 0);
            bool onLoginPage = (String.Compare(currentFileName, "Login.aspx", false) == 0);
            if (UtilityTools.GetIsEnergyFlg())
            {
                //MyProjectsLiteral.Text = "My Projects";
            }
            else
            {
                //MyProjectsLiteral.Text = "My Projects";
            }
            TheUser = Session["User"] as EXBUser;

            isLoggedIn = CheckLoggedIn();

            if (!isLoggedIn && !onLoginPage && !onLandingPage)
            {
                Response.Redirect("~/Buyer/Login.aspx");
            }

            if (!isLoggedIn && onLoginPage)
            {
                //AccountHeaderBar1.LogoLink.NavigateUrl = UtilityTools.GetWebsiteBasePath();
                //bodyWrapper.Attributes["style"] = "margin-top:20px;padding:0;";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle analytics injection
            AnalyticsInjection.Inject(Page.Header);
            HamburgerButton.Visible = false;
            if (isLoggedIn)
            {
                HamburgerButton.Visible = true;
                mainNav.Visible = true;
                // Wire events

                TheCustomer = HttpContext.Current.Session["Customer"] as EXBCustomer;
                if (TheCustomer == null)
                {
                    return;
                }

                //CurrentInventoryLi.Visible = TheCustomer.CanViewOpenItems;

                //UserNameLabel.Click += SignOutButton_Click;
                logoutButton.Click += SignOutButton_Click;
                settingsButton.Click += settingsButton_Click;
                logoutButton2.Click += SignOutButton_Click;
                settingsButton2.Click += settingsButton_Click;

                if (!TheCustomer.CanViewOpenItems)
                {
                    if (HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains("index")
                        || HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains("inventory"))
                    {
                        Response.Redirect("~/Buyer/MyProjectsNew.aspx");
                    }
                }

                // Highlight the buyer nav
                var filePath = Path.GetFileName(Request.Url.AbsolutePath);
                if (filePath.ToLower().Contains("inventory.aspx"))
                {
                    Response.Redirect("~/Buyer/MyProjectsNew.aspx");
                    //CurrentInventoryLi.Attributes.Add("class", "nav-item active");
                }
                else if (filePath.ToLower().Contains("mywatchlist.aspx"))
                {
                    Response.Redirect("~/Buyer/MyProjectsNew.aspx");
                    //MyWatchlistsLi.Attributes.Add("class", "nav-item active");
                }
                else if (filePath.ToLower().Contains("myprojectsnew.aspx"))
                {
                    //MyProjectsLi.Attributes.Add("class", "nav-item active");
                }
                else if (filePath.ToLower().Contains("buyerusersettings.aspx"))
                {
                    settingsButton2.Attributes.Add("style", "color:white;font-weight:600;");
                }
                else if (filePath.ToLower().Contains("login.aspx"))
                {
                    Response.Redirect("~/Buyer/MyProjectsNew.aspx");
                }
            }
            else
            {
                mainNav.Visible = false;
            }


        }

        private bool CheckLoggedIn()
        {
            if (EXBAdministrator.GetAdministratorSession() == null && EXBCustomer.GetCustomerSession() == null)
            {
                return false;
            }
            if (TheUser == null)
            {
                return false;
            }
            UserNameLabel.Text = TheUser.GetFullName();
            return true;
        }

        public void ForceSSL()
        {
            // Force SSL on the current page if SSL is enabled in global config
            if (WebConvert.ToBoolean(AppConfig.GetValue("forceSSL"), true))
            {
                if (!Request.IsSecureConnection)
                {
                    // Redirect to use https since SSL is enabled and not active on the current page
                    Response.Redirect(Request.Url.ToString().Replace("http:", "https:"));
                }
            }
            else
            {
                if (Request.IsSecureConnection)
                {
                    // Redirect to use http since SSL is disabled and is active on the current page
                    Response.Redirect(Request.Url.ToString().Replace("https:", "http:"));
                }
            }
        }

        void SignOutButton_Click(object sender, EventArgs e)
        {
            TheUser = null;
            TheCustomer = null;

            Session.Remove("User");
            Session.Remove("Customer");

            WoopraReportAction("TopNav - SignOut");
            // Wire events
            Response.Redirect("~/Buyer/Login.aspx");
        }

        void settingsButton_Click(object sender, EventArgs e)
        {
            WoopraReportAction("TopNav - Settings");
            Response.Redirect("~/Buyer/BuyerUserSettings.aspx");
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
                string script = ("<script>woopra.track('userportal', {page: 'Top Navbar', description: '" + description
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
    }
}