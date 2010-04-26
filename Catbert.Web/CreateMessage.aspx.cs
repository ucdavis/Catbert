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
        if(!Page.IsPostBack)
        {
            tbBeginDisplayDate.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now).ToString();

            
        }
    
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime start, endNotNull;
        DateTime? end;
        Application app;

        DateTime.TryParse(tbBeginDisplayDate.Text, out start);
        
        if (string.IsNullOrEmpty(tbEndDisplayDate.Text)){
            end = null;
        }
        else{
            DateTime.TryParse(tbEndDisplayDate.Text, out endNotNull);
            end = (DateTime?) endNotNull;
        }
        //Response.Write(start.ToString());
       
        //If = -1: Apply to all apps and leave app null in message table
        app = ddApplications.SelectedValue == "-1" ? null : ApplicationBLL.GetByID(Convert.ToInt32(ddApplications.SelectedValue));

        
        var messageRecord = new Message()
        {
            Application = app,
            MessageText = tbMessage.Text,
            BeginDisplayDate = start,
            EndDisplayDate = end,
            IsActive = true
        }; 

        using (var ts = new TransactionScope())
        {
            MessageBLL.Insert(messageRecord);
            ts.CommitTransaction();
        }
    }
}
