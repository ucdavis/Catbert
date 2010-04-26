using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAESArch.BLL;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;

public partial class MessageEdit : System.Web.UI.Page
{
   private const string StrMessageIdQueryString = "mid";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && Request != null)
        {
            Populate(MessageId());
        }
    }


    private int MessageId ()
    {
        if (Request != null)
        {
            return Convert.ToInt32(Request.QueryString[StrMessageIdQueryString]);
        }

        return 0;
    }


    protected void Populate(int messageId)
    {
        var m = MessageBLL.GetByID(messageId);

        tbMessage.Text = m.MessageText;
        tbBeginDisplayDate.Text = m.BeginDisplayDateString;
        tbEndDisplayDate.Text = m.EndDisplayDateString;
  
        string appStr;
        if (m.Application == null){
            appStr = "-1";
        }
        else{
            appStr = m.Application.ID.ToString();

        }
        ddApplications.SelectedValue = appStr;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

         DateTime startDate, endDateNotNull;
         DateTime? endDate;
         var message = MessageBLL.GetByID(MessageId());

         //If = -1: Apply to all apps and leave app null in message table
         var app = ddApplications.SelectedValue == "-1" ? null : ApplicationBLL.GetByID(Convert.ToInt32(ddApplications.SelectedValue));

         DateTime.TryParse(tbBeginDisplayDate.Text, out startDate);
         
        if (string.IsNullOrEmpty(tbEndDisplayDate.Text) || tbEndDisplayDate.Text == "N/A")
         {
             endDate = null;                       //No End Date
         }
         else
         {
             DateTime.TryParse(tbEndDisplayDate.Text, out endDateNotNull);
             endDate = (DateTime?)endDateNotNull; //Has End Date
         }

        //Response.Write(startDate.ToString());
        message.MessageText = tbMessage.Text;
        message.Application = app;
        message.BeginDisplayDate = startDate;
        message.EndDisplayDate = endDate;
        message.IsActive = true;

        
        using (var ts = new TransactionScope())
        {
            MessageBLL.Update(message);
             ts.CommitTransaction();
        }
    }
}











