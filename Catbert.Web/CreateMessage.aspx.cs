using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAESArch.BLL;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;

public partial class CreateMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime start, end;
        DateTime.TryParse(tbBeginDisplayDate.Text, out start);
        DateTime.TryParse(tbEndDisplayDate.Text, out end);


      
        //Response.Write(end.ToString());

        var messageRecord = new Message()
        {
            //if application is not == -1
            MessageText = tbMessage.Text,
            BeginDisplayDate = start.ToString(),
            EndDisplyDate = end.ToString()
        };

        using (var ts = new TransactionScope())
        {
            MessageBLL.Insert(messageRecord);
            ts.CommitTransaction();
        }
    }
}
