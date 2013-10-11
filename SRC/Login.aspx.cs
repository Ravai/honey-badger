using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getActiveUserData(IP);
        if (DT.Rows.Count == 1)
            Response.Redirect("Home.aspx");
    }

    protected void btn_Submit_OnClick(object sender, EventArgs e)
    {
        string user_IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        //string user_IP = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null) ? HttpContext.Current.Request.UserHostAddress : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (theCake.Login_User(txt_UserName.Text, txt_password.Text, user_IP))
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            lbl_FailMessage.Text = "Login Failed.  Please check your username and password again.";
        }
    }
}