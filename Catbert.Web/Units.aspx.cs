using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAESDO.Catbert.BLL;

public partial class Units : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAddUnit_Click(object sender, EventArgs e)
    {
        var unit = new CAESDO.Catbert.Core.Domain.Unit();

        unit.FISCode = txtFIS.Text;
        unit.PPSCode = txtPPS.Text;
        unit.ShortName = txtShortName.Text;
        unit.FullName = txtFullName.Text;

        unit.School = SchoolBLL.GetByID(dlistSchool.SelectedValue);

        bool success = false;

        using (var ts = new Transaction())
        {
            try
            {
                success = UnitBLL.MakePersistent(unit);
                ts.CommittTransaction();
            }
            catch
            {
                success = false;
                ts.RollBackTransaction();
            }
        }

        lviewUnits.DataBind();

        //Show the new unit added dialog
        pnlUnitAdded.Visible = true;

        if (success)
            litUnitMessage.Text = "Unit Added Successfully";
        else
            litUnitMessage.Text = "Unit Was Not Added.  Check the FIS Code is not already in use";
    }
}
