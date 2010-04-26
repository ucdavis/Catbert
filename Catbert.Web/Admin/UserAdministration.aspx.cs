using System;

public partial class UserAdministration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var applicationName = dlistApplications.SelectedValue;
        
        if (string.IsNullOrEmpty(applicationName) == false)
        {
            frame.Attributes["src"] = string.Format("../Management/UserManagement.aspx?app={0}", applicationName);

            pnlShowUserManagement.Visible = true;
        }
        else
        {
            pnlShowUserManagement.Visible = false;
        }
    }
}
