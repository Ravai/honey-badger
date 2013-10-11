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

/*NOTES:
 * -Currently you can edit someone elses profile by adding #openEdit to the end of their profile URL
 */
public partial class UserProfile : System.Web.UI.Page
{
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["userID"] != null)
        {
            if (!IsPostBack)
            {
                int uID;
                bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);

                if (result)
                {
                    string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
                    DataTable DTactive = theCake.getActiveUserData(IP);
                    DataTable DT = theCake.getUserData(uID);

                    //Only allow editing for the current user
                    if (DTactive.Rows[0]["ID"].ToString() == DT.Rows[0]["ID"].ToString())
                    {
                        pln_Edit.Visible = true;
                    }
                    else
                    {
                        pln_Edit.Visible = false;
                    }

                    if (DT.Rows.Count == 1)
                    {
                        lbl_DisplayName.Text = "<strong>" + DT.Rows[0]["Display_Name"].ToString() + "</strong>";

                        if (DT.Rows[0]["Display_Image"].ToString() == "")
                        {
                            img_avatar.ImageUrl = "/images/avatars/Common/SampleAvatar.gif";
                        }
                        else
                        {
                            img_avatar.ImageUrl = DT.Rows[0]["Display_Image"].ToString();
                        }

                        lbl_alias.Text = DT.Rows[0]["ownerAlias"].ToString();

                        if (DT.Rows[0]["middleName"] != null)
                        {
                            lbl_name.Text = DT.Rows[0]["firstName"].ToString() + " " + DT.Rows[0]["middleName"].ToString() + " " + DT.Rows[0]["lastName"].ToString();
                        }
                        else
                        {
                            lbl_name.Text = DT.Rows[0]["firstName"].ToString() + " " + DT.Rows[0]["lastName"].ToString();
                        }

                        lbl_email.Text = DT.Rows[0]["eMail"].ToString();
                        lbl_phone.Text = DT.Rows[0]["phoneNumber"].ToString();
                        lbl_dateJoined.Text = DT.Rows[0]["dateCreated"].ToString();

                        //Edit window - populate fields with current data
                        txt_FirstName.Text = DT.Rows[0]["firstName"].ToString();
                        txt_MiddleName.Text = DT.Rows[0]["middleName"].ToString();
                        txt_LastName.Text = DT.Rows[0]["lastName"].ToString();
                        txt_EmailAddress.Text = DT.Rows[0]["eMail"].ToString();
                        txt_PhoneNumber.Text = DT.Rows[0]["phoneNumber"].ToString();
                        txt_DisplayImage.Text = DT.Rows[0]["Display_Image"].ToString();


                        //ERROR CHECKING STUFF
                        if (lbl_FirstNameError.Visible == true || lbl_LastNameError.Visible == true ||
                            lbl_EmailError.Visible == true || lbl_Error.Visible == true)
                        {

                        }
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
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void btn_ApplyChanges_OnClick(object sender, EventArgs e)
    {
        int uID;
        bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);

        //Make sure required fields are filled
        if (txt_FirstName.Text == "" || txt_LastName.Text == "" || txt_EmailAddress.Text == "")
        {
            lbl_Error.Text = "Please fill in all of the required fields (First Name, Last Name, Email Address).";
            lbl_Error.Visible = true;

            if (txt_FirstName.Text == "") lbl_FirstNameError.Visible = true;
            else lbl_FirstNameError.Visible = false;
            if (txt_LastName.Text == "") lbl_LastNameError.Visible = true;
            else lbl_LastNameError.Visible = false;
            if (txt_EmailAddress.Text == "") lbl_EmailError.Visible = true;
            else lbl_EmailError.Visible = false;

            Response.Redirect("UserProfile.aspx?userID=" + uID + "#openEdit");
        }
        else //required fields are filled
        {
            string DisplayName;
            
            if (rdbt_Name.Checked)
            {
                DisplayName = txt_FirstName.Text + " " + txt_MiddleName.Text + " " + txt_LastName.Text;
            }
            else
            {
                DisplayName = lbl_alias.Text;
            }

            theCake.updateUserInfo(uID, txt_FirstName.Text, txt_MiddleName.Text, txt_LastName.Text, DisplayName, txt_EmailAddress.Text, txt_PhoneNumber.Text, txt_DisplayImage.Text);
            Response.Redirect("UserProfile.aspx?userID=" + uID);
        }
            
    }
}