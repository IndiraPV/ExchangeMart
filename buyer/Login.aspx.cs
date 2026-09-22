using Exchangebase.Com.Bll;
using Exchangebase.Com.Web;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Exchangebase.Com
{
    public partial class Login : System.Web.UI.Page
    {
        EXBUser user = null;
        private const string CookieName = "EXB_USREMAIL";
        public static string pageTitle
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("BuyerLoginBanner");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // This page requires SSL in production, so tell the master page to add HTTPS
            Master.ForceSSL();

            // Wire events
            OkButton.Click += OkButton_Click;

            if (!IsPostBack)
            {
                RetrieveRememberMeCookieSettings();
            }

        }

        void OkButton_Click(object sender, EventArgs e)
        {
            // Log the user in and verify credentials
            if (LoginUser(UsrEmailTextBox.Text.Trim(), UsrPasswordTextBox.Text.Trim()))
            {
                EXBUsrActivityLog activityLog = new EXBUsrActivityLog();
                activityLog.Action = EXBActionLookup.Login;
                activityLog.ObjectType = EXBObjectLookup.User;
                activityLog.TgtID = user.UsrID;
                activityLog.TgtData = EXBChangeLog.GenerateTgtData(null, "Login User : " + user.UsrID);
                activityLog.Save();

                if (user.CusID == 0)
                {
                    // Invalid login
                    LoginError("The Email address or password you entered are incorrect");
                }

                if (!user.ValidateCustomerUser())
                {
                    // Invalid login
                    LoginError("The Email address or password you entered are incorrect");
                }

                Session["User"] = user;
                EXBCustomer loggedInCustomer = new EXBCustomer(user.CusID);
                Session["Customer"] = loggedInCustomer;

                SetRememberMeCookieSettings();

                // Go to the buyer home page
                Response.Redirect("~/Buyer/MyProjectsNew.aspx");
            }

            // Invalid login
            LoginError("The Email address or password you entered are incorrect");
            WoopraReportAction("login error");
        }

        private bool LoginUser(string usrEmail, string usrPassword)
        {
            user = new EXBUser(usrEmail);

            if (!user.ValidateCredentials(usrPassword))
            {
                LoginError("The Email address or password you entered is incorrect");
                return false;
            }

            // Record the ip address if it has changed
            user.CaptureIPAddress(Request.UserHostAddress);

            return true;
        }

        private void LoginError(string errorText)
        {
            PanelLoginError.Visible = true;
            lblLoginError.Text = errorText;
        }

        private void RetrieveRememberMeCookieSettings()
        {
            if (Request.Cookies[CookieName] != null)
            {
                ChkRememberMe.Checked = Request.Cookies[CookieName].Value.Length > 0;
                UsrEmailTextBox.Text = Request.Cookies[CookieName].Value;
            }
        }

        private void SetRememberMeCookieSettings()
        {
            if (ChkRememberMe.Checked)
            {
                if (Response.Cookies[CookieName] == null)
                {
                    Response.Cookies.Set(new HttpCookie(CookieName));
                }
                Response.Cookies[CookieName].Value = UsrEmailTextBox.Text.Trim();
                Response.Cookies[CookieName].Expires = DateTime.Now.AddYears(10);
            }
            else
            {
                if (Response.Cookies[CookieName] != null)
                {
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }
        private void WoopraReportAction(string description)
        {
            if (AnalyticsInjection.IsWoopraTrackEnabled())
            {
                string script = ("<script>woopra.track('userportal', {page: 'Login', description: '" + description
                     + "'});</script>");

                Literal analyticsInjection = new Literal();
                analyticsInjection.Text = script.ToString();
                Page.Header.Controls.Add(analyticsInjection);
            }
        }

        [WebMethod]
        public static string ResetPassword(string email)
        {
            if (email.Trim().Length < 1)
            {
                return "1";
            }

            // Look up the user by email address
            EXBUser user = new EXBUser(HttpUtility.UrlDecode(email));

            // Verify the user exists
            if (user.UsrID < 1)
            {
                return "2";
            }

            // Verify the user account is Active
            if (!user.UsrIsActive)
            {
                return "2";
            }

            // Reset the user password send an email to the user
            EmailObject eo = new EmailObject();
            eo.SendMailMessage("customerservice@exchangebase.com", user.UsrEmail, "", "", "ExchangeBase Password Reset Request", GenerateMessage(user));

            return "0";
        }

        private static string GenerateMessage(EXBUser user)
        {
            string password = user.SetPassword(false);
            string message = "";

            message += "		<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" + Environment.NewLine;
            message += "		<html xmlns=\"http://www.w3.org/1999/xhtml\">" + Environment.NewLine;
            message += "		<head>" + Environment.NewLine;
            message += "			<title></title>" + Environment.NewLine;
            message += "			<style type=\"text/css\">" + Environment.NewLine;
            message += "				p.MsoNormal" + Environment.NewLine;
            message += "				{" + Environment.NewLine;
            message += "					margin-top: 0in;" + Environment.NewLine;
            message += "					margin-right: 0in;" + Environment.NewLine;
            message += "					margin-bottom: 10.0pt;" + Environment.NewLine;
            message += "					margin-left: 0in;" + Environment.NewLine;
            message += "					line-height: 115%;" + Environment.NewLine;
            message += "					font-size: 11.0pt;" + Environment.NewLine;
            message += "					font-family: Calibri;" + Environment.NewLine;
            message += "				}" + Environment.NewLine;
            message += "				a:link" + Environment.NewLine;
            message += "				{" + Environment.NewLine;
            message += "					font-family: \"Times New Roman\";" + Environment.NewLine;
            message += "					color: blue;" + Environment.NewLine;
            message += "					text-decoration: underline;" + Environment.NewLine;
            message += "					text-underline: single;" + Environment.NewLine;
            message += "				}" + Environment.NewLine;
            message += "			</style>" + Environment.NewLine;
            message += "		</head>" + Environment.NewLine;
            message += "		<body>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				Dear " + user.UsrFirstName + " " + user.UsrLastName + ",</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				ExchangeBase has received a request to reset the password associated with your login. Please see your new password below.</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				Your Password: &nbsp;&nbsp;&nbsp; " + password + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				You can log in to your account with your new password at " + UtilityTools.GetWebsiteBasePath() + "/Buyer/Login.aspx.</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				Sincerely,</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				ExchangeBase </p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal\">" + Environment.NewLine;
            message += "				<o:p>&nbsp;</o:p>" + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "			<p class=\"MsoNormal\" style=\"margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal; font-size:10pt;\">" + Environment.NewLine;
            message += "				ExchangeBase will never e-mail you and ask you to disclose or verify your login, password, or payment information. If you receive a suspicious e-mail, report the e-mail to ExchangeBase for investigation." + Environment.NewLine;
            message += "			</p>" + Environment.NewLine;
            message += "		</body>" + Environment.NewLine;
            message += "		</html>";

            return message;
        }
    }
}