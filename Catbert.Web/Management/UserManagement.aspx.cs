using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;

public partial class Management_UserManagement : System.Web.UI.Page
{
    public string CurrentApplication { get; set; }
    public List<Unit> Units { get; set; }
    public List<Role> Roles { get; set; }

    /// <summary>
    /// Do the permission checks necessary to ensure that the current user has access to this application,
    /// as given in the querystring parameter app
    /// </summary>
    protected void Page_Init(object sender, EventArgs e)
    {
        ///TODO: For testing only, set application to Catbert
        CurrentApplication = Request.QueryString["app"] ?? "Catbert";

        ClientScript.RegisterHiddenField("app", CurrentApplication); //Register a hidden field with the application name in it for use from JS

        //Check permissions for the current user to the application Application
        ///TODOL Implement check
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Retrieve the units and roles only once, in the same transaction
        using (var ts = new Transaction())
        {
            Units = UnitBLL.GetVisibleByUser(CurrentApplication);
            Roles = RoleBLL.GetVisibleByUser(CurrentApplication);

            ts.CommittTransaction();
        }

        //Assign the units and roles to the different data bound objects as necessary
        
        //Units
        lviewUnits.DataSource = Units;
        lviewFilterUnits.DataSource = Units;
        lviewUserUnits.DataSource = Units;

        lviewUnits.DataBind();
        lviewFilterUnits.DataBind();
        lviewUserUnits.DataBind();

        //Roles
        lviewRoles.DataSource = Roles;
        lviewFilterRoles.DataSource = Roles;
        lviewUserRoles.DataSource = Roles;

        lviewRoles.DataBind();
        lviewFilterRoles.DataBind();
        lviewUserRoles.DataBind();
    }
}
