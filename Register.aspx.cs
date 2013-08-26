using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Register : System.Web.UI.Page
{
    // For generating random numbers.
    private Random random = new Random();
    DataBase theCake = new DataBase();

    private void Page_Load(object sender, System.EventArgs e)
    {

    }

    //
    // Returns a string of six random digits.
    //
    private string GenerateRandomCode()
    {
        string s = "";
        for (int i = 0; i < 6; i++)
            s = String.Concat(s, this.random.Next(10).ToString());
        return s;
    }

    protected void btn_Submit_OnClick(object sender, EventArgs e)
    {
        string user_IP = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null) ? HttpContext.Current.Request.UserHostAddress : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (txt_Password1.Text == txt_Password2.Text)
        {
            if (theCake.Register_User(txt_UserName.Text, txt_DisplayName.Text, txt_Password1.Text, user_IP))
            {
                theCake.Login_User(txt_UserName.Text, txt_Password1.Text, user_IP);

                Response.Redirect("Default.aspx");
            }
            else
            {
                lbl_FailMessage.Text = "The username you have chosen already has been taken.  Please choose another.";
            }
        }
        else
        {
            lbl_FailMessage.Text = "Your passwords do not match.  Please try again.";
        }
    }
}