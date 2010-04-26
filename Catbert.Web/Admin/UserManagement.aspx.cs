using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CAESArch.BLL;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;
using CAESArch.Core.Utils;

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
        CurrentApplication = "Catbert";

        ClientScript.RegisterHiddenField("app", CurrentApplication); //Register a hidden field with the application name in it for use from JS
        ClientScript.RegisterHiddenField("user", User.Identity.Name); //Also a hidden field with the user's name in it for use from JS
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Retrieve the units and roles only once, in the same transaction
        using (var ts = new TransactionScope())
        {
            Units = UnitBLL.GetAllUnits();
            Roles = RoleBLL.GetAll();
            
            ts.CommitTransaction();
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
