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

public partial class MasterPage : System.Web.UI.MasterPage
{
    //private System.Type myType;
    public bool LoggedIn = false;
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        //string IP = Request.UserHostAddress;
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

        DataTable DT = theCake.getActiveUserData(IP);
        if (DT.Rows.Count == 1)
        {
            LoggedIn = true;
            lbl_userName.Text = DT.Rows[0]["Display_Name"].ToString();
            pnl_LoggedIn.Visible = true;
            pnl_LoggedOut.Visible = false;
        }
        else
        {
            if (!(Request.Url.ToString().Contains("Login.aspx") || Request.Url.ToString().Contains("Register.aspx")))
                Response.Redirect("Login.aspx");
        }

        // insert code here
        WriteLastUpdated();
        //string user = theCake.getActiveUserName(Request.UserHostAddress);
        //lbl_userName.Text = user;

        if (IsPostBack || !IsPostBack)
        {
            //if (theCake.checkMaintenance())
            //    Response.Redirect("Maintenance.aspx");
        }

        
    }

    // Found the information for this at http://www.heybo.com/weblog/posts/288.aspx
    // There are many other items that can be accessed through this method, refer to page for those.
    // *NOTE* In websites it does not appear that you can have a version information resource.  This prevents
    // us from getting an actual version number.  Translation: It appears as 0.0.0.0 when displayed on the page.

    protected void WriteLastUpdated()
    {
        DateTime dt;

        try
        {
            string filePath = MapPathSecure(Page.Request.CurrentExecutionFilePath);
            dt = new FileInfo(filePath).LastWriteTime;
        }
        catch
        { // "design-time" or current path doesn't resolve to a file
            dt = DateTime.Now;
        }
        templateLastUpdated.Text = "<p>Last updated: " + dt.ToString() + "</p>";
    }

    protected void btn_Logout_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        theCake.Logout_User(IP);
        Response.Redirect("Home.aspx");
    }
}
