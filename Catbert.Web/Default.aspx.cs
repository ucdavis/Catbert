﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.BLL;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //var schools = GenericBLL<School, string>.GetAll();

        //Response.Write(schools.Count);
        CAESDO.CatbertService.CatbertWebServiceSoapClient c = new CAESDO.CatbertService.CatbertWebServiceSoapClient();

        //var q = GenericBLL<TrackingType, int>.GetAll();
        //var q = UserBLL.GetByApplicationRole(1, 1);
    }
}
