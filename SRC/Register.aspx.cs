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
using PasswordHash;

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
        string user_IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

        if (txt_Password1.Text == txt_Password2.Text)
        {
            string displayName = "";
            if (radio_Name.Checked)
            {
                if (txt_MI.Text.Length > 0)
                    displayName = txt_FirstName.Text + " " + txt_MI.Text + " " + txt_LastName.Text;
                else
                    displayName = txt_FirstName.Text + " " + txt_LastName.Text;
            }
            else
            {
                displayName = txt_UserName.Text;
            }
            if (theCake.Register_User(txt_UserName.Text, txt_FirstName.Text, txt_MI.Text, txt_LastName.Text, txt_Email.Text, txt_Phone.Text, displayName, txt_Password1.Text, user_IP))
            {
                theCake.Login_User(txt_UserName.Text, txt_Password1.Text, user_IP);

                TTDB.mailTo(txt_Email.Text, "Registration to - Development Project A", "Thanks " + txt_FirstName.Text + " " + txt_LastName.Text + " for registering to the Development Project A!  This is just a receipt of action for you.<br /><br />Your username:  " + txt_UserName.Text + " <br /><br />Thanks again for registering!");

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