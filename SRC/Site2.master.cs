using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Site2 : System.Web.UI.MasterPage
{
    public bool LoggedIn = false;
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

        DataTable DT = theCake.getActiveUserData(IP);
        if (DT.Rows.Count == 1)
        {
            LoggedIn = true;
            lbl_userName.Text = DT.Rows[0]["Display_Name"].ToString();
        }
        else
        {
            if (!(Request.Url.ToString().Contains("Login.aspx") || Request.Url.ToString().Contains("Register.aspx")))
                Response.Redirect("Login.aspx");
        }

        if (IsPostBack || !IsPostBack)
        {
            //if (theCake.checkMaintenance())
            //    Response.Redirect("Maintenance.aspx");
        }
    }

    protected void btn_Logout_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        theCake.Logout_User(IP);
        Response.Redirect("Home.aspx");
    }
}
