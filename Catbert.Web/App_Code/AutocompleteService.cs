using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CAESDO.Catbert.BLL;

/// <summary>
/// Summary description for AutocompleteService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AutocompleteService : System.Web.Services.WebService
{

    public AutocompleteService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Unoptimized method for getting units by query
    /// </summary>
    /// <param name="q"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [WebMethod]
    public List<AutoCompleteData> GetUnitsAuto(string q /*query*/, int limit)
    {
        List<AutoCompleteData> auto = new List<AutoCompleteData>();

        foreach (var unit in UnitBLL.GetAll())
        {
            if (unit.FISCode.ToLower().Contains(q) || unit.ShortName.ToLower().Contains(q))
            {
                auto.Add(
                    new AutoCompleteData(
                        unit.FISCode, //Result
                        string.Empty, //Value
                        new //Additional Info Anonymous Type
                        {
                            Name = unit.ShortName,
                            FIS = unit.FISCode
                        }
                    ));
            }
        }

        return auto;
    }

    [WebMethod]
    public List<AutoCompleteData> GetUsers(string application, string q /*query*/, int limit)
    {
        var users = UserBLL.GetByApplication(application, q, 1 /* Start at the first record */, limit);

        List<AutoCompleteData> auto = new List<AutoCompleteData>();

        foreach (var user in users)
        {
            auto.Add(new AutoCompleteData(user.LoginID, string.Empty,
                new
                {
                    Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                    Login = user.LoginID,
                    Email = user.Email
                }
            ));
        }

        return auto;
    }

}

public class AutoCompleteData
{
    public string result { get; set; } //like key
    public string value { get; set; }
    public object data { get; set; }

    public AutoCompleteData(string result, string value, object data)
    {
        this.result = result;
        this.value = value;
        this.data = data;
    }

    public AutoCompleteData()
    {

    }
}