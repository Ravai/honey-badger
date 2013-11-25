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
            lnk_MyAccount.PostBackUrl = "UserProfile.aspx?userID=" + DT.Rows[0]["ID"].ToString();

            string menuString = "<ul id=\"dashboard-menu\" class=\"nav nav-list\">";
            if (Request.Url.ToString().Contains("Default.aspx"))
                menuString += "<li class=\"active \"><a href=\"Default.aspx\"><i class=\"icon-home\"></i> <span>Home</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"Default.aspx\"><i class=\"icon-home\"></i> <span>Home</span></a></li>";
            if (Request.Url.ToString().Contains("Home.aspx"))
                menuString += "<li class=\"active \"><a href=\"Home.aspx\"><i class=\"icon-home\"></i> <span>Dashboard</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"Home.aspx\"><i class=\"icon-home\"></i> <span>Dashboard</span></a></li>";
            if (Request.Url.ToString().Contains("ViewWip.aspx"))
                menuString += "<li class=\"active \"><a href=\"ViewWip.aspx\"><i class=\"icon-book\"></i> <span>Projects in Progress</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"ViewWip.aspx\"><i class=\"icon-book\"></i> <span>Projects in Progress</span></a></li>";
            if (Request.Url.ToString().Contains("ViewReady.aspx"))
                menuString += "<li class=\"active \"><a href=\"ViewReady.aspx\"><i class=\"icon-book\"></i> <span>Ready Projects</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"ViewReady.aspx\"><i class=\"icon-book\"></i> <span>Ready Projects</span></a></li>";
            if (Request.Url.ToString().Contains("ViewUpcoming.aspx"))
                menuString += "<li class=\"active \"><a href=\"ViewUpcoming.aspx\"><i class=\"icon-book\"></i> <span>Upcoming Projects</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"ViewUpcoming.aspx\"><i class=\"icon-book\"></i> <span>Upcoming Projects</span></a></li>";
            if (Request.Url.ToString().Contains("ViewComplete.aspx"))
                menuString += "<li class=\"active \"><a href=\"ViewComplete.aspx\"><i class=\"icon-book\"></i> <span>Completed Projects</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"ViewComplete.aspx\"><i class=\"icon-book\"></i> <span>Completed Projects</span></a></li>";
            menuString += "</ul>";

            lit_Menu.Text = menuString;
        }
        else
        {
            if (!(Request.Url.ToString().Contains("Login.aspx") || Request.Url.ToString().Contains("Register.aspx") || Request.Url.ToString().Contains("Default.aspx")))
                Response.Redirect("Default.aspx");

            string menuString = "<ul id=\"dashboard-menu\" class=\"nav nav-list\">";
            if (Request.Url.ToString().Contains("Default.aspx"))
                menuString += "<li class=\"active \"><a href=\"Default.aspx\"><i class=\"icon-home\"></i> <span>Home</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"Default.aspx\"><i class=\"icon-home\"></i> <span>Home</span></a></li>";
            if (Request.Url.ToString().Contains("Login.aspx"))
                menuString += "<li class=\"active \"><a href=\"Login.aspx\"><i class=\"icon-home\"></i> <span>Log-In</span></a></li>";
            else
                menuString += "<li class=\" \"><a href=\"Login.aspx\"><i class=\"icon-home\"></i> <span>Log-In</span></a></li>";
            menuString += "</ul>";
            lit_Menu.Text = menuString;
        }

        

        if (IsPostBack || !IsPostBack)
        {
            if (theCake.checkMaintenance())
                Response.Redirect("Maintenance.aspx");
        }
    }

    protected void btn_Logout_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        theCake.Logout_User(IP);
        Response.Redirect("Home.aspx");
    }
}
