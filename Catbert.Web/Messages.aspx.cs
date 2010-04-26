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

    protected void BtnDeactivateClick(object sender, EventArgs e)
   {
       
   }

    /// <summary>
    /// Message Update function.  Get Message obj to update
    /// </summary>
    /// <param name="objId", "property", "value", "objType">
    /// </param>
    public string SaveProperty(int objId, string property, string value, string objType)
    {
        try
        {
           SaveProperty(MessageBLL.GetByID(objId), property, value); 

        //I don't understand what is being thrown below --below? LD
           return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //update message 
    protected void SaveProperty(Message message, string property, string value)
    {

        switch (property)
        {
            case "IsActive":
                message.IsActive = Convert.ToBoolean(value);
                break;
        };

        MessageBLL.Update(message);
    }



}
