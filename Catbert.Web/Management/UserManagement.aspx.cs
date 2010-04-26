using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Management_UserManagement : System.Web.UI.Page
{
    public string Application { get; set; }
    /// <summary>
    /// Do the permission checks necessary to ensure that the current user has access to this application,
    /// as given in the querystring parameter app
    /// </summary>
    protected void Page_Init(object sender, EventArgs e)
    {
        ///TODO: For testing only, set application to Catbert
        Application = Request.QueryString["app"] ?? "Catbert";

        ClientScript.RegisterHiddenField("app", Application); //Register a hidden field with the application name in it for use from JS

        //Check permissions for the current user to the application Application
        ///TODOL Implement check
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
