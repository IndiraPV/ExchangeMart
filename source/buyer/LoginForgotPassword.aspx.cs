using Exchangebase.Com.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Exchangebase.Com.Buyer
{
	public partial class LoginForgotPassword : System.Web.UI.Page
	{
		private EXBUser user;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Get the email address for the account to reset
			string usrEmail = WebConvert.ToString(Request.QueryString["email"], "");

			if (usrEmail.Length == 0)
			{
				Response.Write("Not Found!");
				return;
			}

			// Look up the user by email address
			user = new EXBUser(usrEmail);

			// Verify the user exists
			if (user.UsrID < 1)
			{
				Response.Write("Not Found!");
				return;
			}

			// Reset the user password
			// Send an email to the user
			EmailObject email = new EmailObject();
			email.SendMailMessage("customerservice@exchangebase.com", user.UsrEmail, "", "", "ExchangeBase Password Reset Request", GenerateMessage(user.SetPassword(false)));

			Response.Write("Success!");
		}

		private string GenerateMessage(string password)
		{
			UtilityTools ut = new UtilityTools();
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
			message += "				You can log in to your account with your new password at " + UtilityTools.GetWebsiteBasePath() + "Buyer/Login.aspx.</p>" + Environment.NewLine;
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