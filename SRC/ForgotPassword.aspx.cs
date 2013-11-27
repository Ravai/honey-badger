using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        DataBase cake = new DataBase();
        string newPass=cake.resetPassword(username.Text, email.Text);
        if (!newPass.Equals("", StringComparison.Ordinal))
        {
            formField.Visible = false;
            statusMessage.Font.Bold = true;
            statusMessage.Text = "Your new password: "+newPass+"<br /> Please Change it when you sucessfully logged in";
            btn_return.Visible = true;
        }
        else
        {
            statusMessage.ForeColor = System.Drawing.Color.Red;
            statusMessage.Font.Bold = true;
            statusMessage.Text = "Your Username is not found or your email does not match with that username";
        }
    }
}