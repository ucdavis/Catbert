using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Emulation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Only Scott can use this testing page
        if (!Roles.IsUserInRole("EmulationUser"))
            Response.Redirect(FormsAuthentication.DefaultUrl);
            //Response.Redirect(RecruitmentConfiguration.ErrorPage(RecruitmentConfiguration.ErrorType.AUTH));
    }

    protected void btnLoginID_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();

        FormsAuthentication.Initialize();

        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
            txtLoginID.Text,
            DateTime.Now,
            DateTime.Now.AddMinutes(15),
            false,
            String.Empty,
            FormsAuthentication.FormsCookiePath);

        string hash = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

        Response.Cookies.Add(cookie);

        Response.Redirect(FormsAuthentication.DefaultUrl);
    }
}
