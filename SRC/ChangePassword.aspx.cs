using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : System.Web.UI.Page
{
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Success.Visible = false;
        lbl_FailMessage.Visible = false;
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getActiveUserData(IP);
        if (DT.Rows.Count == 0)
            Response.Redirect("Home.aspx");
    }

    protected void btn_SubmitPWChange_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getActiveUserData(IP);


        string hash = DT.Rows[0]["user_PW"].ToString();

        // User entered their password correctly
        if (PasswordHash.PasswordHash.ValidatePassword(txt_currentPassword.Text, hash))
        {
            // New passwords match
            if (txt_newPassword.Text == txt_newPasswordVerify.Text)
            {
                int uID;
                string userID = DT.Rows[0]["ID"].ToString();
                bool result = Int32.TryParse(userID, out uID);

                // Update user password
                userClass.updateUserPassword(uID, txt_newPassword.Text);

                // Redirect back to user profile
                Response.Redirect("UserProfile.aspx?userID=" + uID);
            }
            else // New passwords do not match
            {
                lbl_FailMessage.Text = "Your passwords do not match.";
                lbl_FailMessage.Visible = false;
            }
        }
        else // Entered password does not match active user's password
        {
            lbl_FailMessage.Text = "Incorrect password.";
            lbl_FailMessage.Visible = true;
        } 
    }
}