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
    public List<Unit> Units { get; set; }
    public List<Role> Roles { get; set; }
    public List<Application> Applications { get; set; }

    /// <summary>
    /// Do the permission checks necessary to ensure that the current user has access to this application,
    /// as given in the querystring parameter app
    /// </summary>
    protected void Page_Init(object sender, EventArgs e)
    {
        //ClientScript.RegisterHiddenField("app", CurrentApplication); //Register a hidden field with the application name in it for use from JS
        ClientScript.RegisterHiddenField("user", User.Identity.Name); //Also a hidden field with the user's name in it for use from JS
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Retrieve the units and roles only once, in the same transaction
        using (var ts = new TransactionScope())
        {
            Units = UnitBLL.GetAllUnits();
            Applications = ApplicationBLL.GetAll("Name", true);
            
            ts.CommitTransaction();
        }

        //Assign the units and roles to the different data bound objects as necessary
        
        //Units
        lviewUnits.DataSource = Units;
        lviewUnitsForAssociation.DataSource = Units;

        lviewUnits.DataBind();
        lviewUnitsForAssociation.DataBind();

        //Applications
        lviewFilterApplications.DataSource = Applications;
        lviewApplications.DataSource = Applications;
        lviewApplicationPermissions.DataSource = Applications;
        lviewApplicationUnits.DataSource = Applications;

        lviewFilterApplications.DataBind();
        lviewApplications.DataBind();
        lviewApplicationPermissions.DataBind();
        lviewApplicationUnits.DataBind();
    }
}
