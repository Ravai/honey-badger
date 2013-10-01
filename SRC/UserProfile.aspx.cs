using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class UserProfile : System.Web.UI.Page
{
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["userID"] != null)
        {
            int uID;
            bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);

            if (result)
            {
                DataTable DT = theCake.getUserData(uID);

                if (DT.Rows.Count == 1)
                {
                    lbl_userName.Text = "<strong>" + DT.Rows[0]["Display_Name"].ToString() + "</strong>";
                    //lbl_name.Text = DT.Rows[0]["First_Name"].ToString() + " " + DT.Rows[0]["Last_Name"].ToString();
                    lbl_email.Text = DT.Rows[0]["Email"].ToString();
                    //lbl_phone.Text = DT.Rows[0]["Phone_Number"].ToString();
                    lbl_lastOnline.Text = DT.Rows[0]["dateLastLoggedIn"].ToString();
                }
                else
                {
                    //error currently displays empty fields
                }
            }
            else
            {
                //error currently displays empty fields
            }
        }
    }
}