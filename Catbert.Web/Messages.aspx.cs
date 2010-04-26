using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAESArch.BLL;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;

public partial class Messages : System.Web.UI.Page
{
    //private const string STR_MessageIDQueryString = "mid";

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    
   protected void BtnDeactivateClick(object sender, CommandEventArgs e)
   {
       int id = Convert.ToInt16(e.CommandArgument);
       DeactivateMsg(id);
   }

    protected  void DeactivateMsg(int id)
    {
        var m = MessageBLL.GetByID(id);
        if (m == null) throw new NotImplementedException();
        m.IsActive = false;

        using (var ts = new TransactionScope())
        {
            MessageBLL.Update(m);
            ts.CommitTransaction();
        }
    }
   

}
