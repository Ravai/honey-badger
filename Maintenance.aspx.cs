﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Maintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBase theCake = new DataBase();
        if (!theCake.checkMaintenance())
            Response.Redirect("Home.aspx");
    }
}