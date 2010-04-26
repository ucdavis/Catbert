using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserAdministration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        frame.Attributes["src"] = string.Format("Management/UserManagement.aspx?app={0}", dlistApplications.SelectedValue);
    }
}
