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

                    // Only allow editing for the current user if they are on their own profile
                    if (DTactive.Rows[0]["ID"].ToString() == DT.Rows[0]["ID"].ToString())
                    {
                        pln_Edit.Visible = true;
                    }
                    else
                    {
                        pln_Edit.Visible = false;
                        pnl_PersonalProjects.Visible = false;
                    }

                    // Display user info
                    if (DT.Rows.Count == 1)
                    {
                        
                        lbl_DisplayName.Text = "<h2>" + DT.Rows[0]["Display_Name"].ToString() + "</h2>";

                        // Check if user has an avatar image
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

                        // Edit window - populate fields with current data
                        txt_FirstName.Text = DT.Rows[0]["firstName"].ToString();
                        txt_MiddleName.Text = DT.Rows[0]["middleName"].ToString();
                        txt_LastName.Text = DT.Rows[0]["lastName"].ToString();
                        txt_EmailAddress.Text = DT.Rows[0]["eMail"].ToString();
                        txt_PhoneNumber.Text = DT.Rows[0]["phoneNumber"].ToString();
                        txt_DisplayImage.Text = DT.Rows[0]["Display_Image"].ToString();


                        // Edit window - ERROR CHECKING STUFF
                        if (lbl_FirstNameError.Visible == true || lbl_LastNameError.Visible == true ||
                            lbl_EmailError.Visible == true || lbl_Error.Visible == true)
                        {
                            
                        }
                    }
                    else
                    {
                        // error currently displays empty fields
                    }
                }
                else
                {
                    // error currently displays empty fields
                }

                fillProjectsTable();
                fillSharedProjectsTable();
            }
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void btn_SearchUsernames_OnClick(object sender, EventArgs e)
    {
        int uID;
        bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);
        DataTable DT = theCake.searchUsersByUserName(txt_usernameSearch.Text.Trim());

        if (DT.Rows.Count == 0) // No results found
        {
            TableRow TR0 = new TableRow();
            TableCell TC0 = new TableCell();
            Label no_res = new Label();
            no_res.Text = "No matches were found.";
            TC0.Controls.Add(no_res);
            TR0.Cells.Add(TC0);
            tbl_searchResults.Rows.Add(TR0);
        }

        if (DT.Rows.Count >= 1) // Results found
        {
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR1 = new TableRow();
                TableCell TC1 = new TableCell();
                Label username_res = new Label();
                username_res.Text = "<a href=\"UserProfile.aspx?userID=" + DR["ID"].ToString() + "\">" + DR["ownerAlias"].ToString() + "</a>";
                TC1.Controls.Add(username_res);
                TR1.Cells.Add(TC1);
                tbl_searchResults.Rows.Add(TR1);
            }
        }
    }

    protected void btn_SearchUsers_OnClick(object sender, EventArgs e)
    {
        int uID;
        bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);
        DataTable DT = theCake.searchUsersByName(txt_first_name.Text.Trim(), txt_middle_name.Text.Trim(), txt_last_name.Text.Trim());

        if (DT.Rows.Count == 0) // No results found
        {
            TableRow TR0 = new TableRow();
            TableCell TC0 = new TableCell();
            Label no_res = new Label();
            no_res.Text = "No matches were found.";
            TC0.Controls.Add(no_res);
            TR0.Cells.Add(TC0);
            tbl_searchResults.Rows.Add(TR0);
        }

        if (DT.Rows.Count >= 1) // Results found
        {
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR1 = new TableRow();
                TableCell TC1 = new TableCell();
                Label name_res = new Label();
                if (DR["middleName"].ToString() == "")
                {
                    name_res.Text = "<a href=\"UserProfile.aspx?userID=" + DR["ID"].ToString() + "\">" + DR["firstName"].ToString() + " " + DR["lastName"].ToString() + "</a>";
                }
                else
                {
                    name_res.Text = "<a href=\"UserProfile.aspx?userID=" + DR["ID"].ToString() + "\">" + DR["firstName"].ToString() + " " + DR["middleName"] + " " + DR["lastName"].ToString() + "</a>";
                }
                TC1.Controls.Add(name_res);
                TR1.Cells.Add(TC1);
                tbl_searchResults.Rows.Add(TR1);
            }
        }
    }

    protected void btn_ApplyChanges_OnClick(object sender, EventArgs e)
    {
        int uID;
        bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);

        // Make sure required fields are filled
        if (txt_FirstName.Text == "" || txt_LastName.Text == "" || txt_EmailAddress.Text == "")
        {
            lbl_Error.Text = "Please fill in all of the required fields (First Name, Last Name, Email Address).";
            lbl_Error.Visible = true;

            if (txt_FirstName.Text == "") 
                lbl_FirstNameError.Visible = true;
            else 
                lbl_FirstNameError.Visible = false;
            if (txt_LastName.Text == "") 
                lbl_LastNameError.Visible = true;
            else 
                lbl_LastNameError.Visible = false;
            if (txt_EmailAddress.Text == "") 
                lbl_EmailError.Visible = true;
            else 
                lbl_EmailError.Visible = false;

            //Response.Redirect("UserProfile.aspx?userID=" + uID + "#openEdit");
        }
        else // required fields are filled
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

    protected void fillProjectsTable()
    {
        int projectFlag = 0;
        int uID;
        bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);
        DataTable DT1 = theCake.getUserData(uID);
        DataTable DT2 = theCake.getTasks(DT1.Rows[0]["ownerAlias"].ToString());

        TableRow TR0 = new TableRow();
        TableCell TCname = new TableCell();
        TableCell TCpercent = new TableCell();
        Label name = new Label();
        Label percent = new Label();
        name.Text = "<u><strong>Project Name</strong></u>";
        percent.Text = "<u><strong>Percent Completed</strong></u>";
        TCname.Controls.Add(name);
        TCpercent.Controls.Add(percent);
        TCpercent.HorizontalAlign = HorizontalAlign.Center;
        TR0.Cells.Add(TCname);
        TR0.Cells.Add(TCpercent);
        tbl_Projects.Rows.Add(TR0);

        // List each project the user has
        if (DT2.Rows.Count > 0)
        {
            foreach (DataRow DR in DT2.Rows)
            {
                DataTable DTp = theCake.getProjectPermissions((int)DR["ID"]);
                if (DTp.Rows.Count == 1) // We only want projects that haven't been shared
                {
                    projectFlag = 1;
                    TableRow TR = new TableRow();
                    TableCell TC = new TableCell();
                    TableCell TC2 = new TableCell();
                    Label L1 = new Label();
                    Label L2 = new Label();
                    L1.Text = DR["taskName"].ToString();
                    //L2.Text = DR["Percent_Completed"].ToString() + "%";
                    if (int.Parse(DR["doneFlag"].ToString()) == 1)
                    {
                        L2.Text = "<p><progress value=\"" + 1 + "\" ></progress></p>";
                        //L2.Text = "COMPLETED";
                    }
                    else
                    {
                        L2.Text = "<p><progress value=\"" + ((decimal)(int.Parse(DR["Percent_Completed"].ToString()))) / 100 + "\" ></progress></p>";
                    }
                    TC.Controls.Add(L1);
                    TC2.Controls.Add(L2);
                    TR.Cells.Add(TC);
                    TR.Cells.Add(TC2);
                    tbl_Projects.Rows.Add(TR);
                }
            }
        }
        else if (projectFlag != 1) // Indicate if the user has no projects.
        {
            TableRow TR = new TableRow();
            TableCell TC = new TableCell();
            Label LE = new Label();
            LE.Text = "This user has no personal projects.";
            TC.Controls.Add(LE);
            TR.Cells.Add(TC);
            tbl_Projects.Rows.Add(TR);
        }
        else // If user has no projects, display a message indicating that
        {
            TableRow TR = new TableRow();
            TableCell TC = new TableCell();
            Label LE = new Label();
            LE.Text = "This user has no personal projects.";
            TC.Controls.Add(LE);
            TR.Cells.Add(TC);
            tbl_Projects.Rows.Add(TR);
        }
    }

    protected void fillSharedProjectsTable()
    {
        int uID;
        bool result = Int32.TryParse(Request.QueryString["userID"].ToString(), out uID);
        DataTable DT1 = theCake.getUserData(uID);
        DataTable DT2 = theCake.getSharedWipTasks(DT1.Rows[0]["ownerAlias"].ToString());
        DataTable DT3 = theCake.getSharedReadyTasks(DT1.Rows[0]["ownerAlias"].ToString());
        DataTable DT4 = theCake.getSharedUpcomingTasks(DT1.Rows[0]["ownerAlias"].ToString());
        DataTable DT5 = theCake.getUserData(uID);
        DataTable DT6 = theCake.getTasks(DT1.Rows[0]["ownerAlias"].ToString());

  
        TableRow TR1 = new TableRow();
        TableCell TC1 = new TableCell();
        TableCell TC2 = new TableCell();
        Label L1 = new Label();
        Label L2 = new Label();
        L1.Text = "<u><strong>Project Name</strong></u>";
        L2.Text = "<u><strong>Role</strong></u>";
        TC1.Controls.Add(L1);
        TC2.Controls.Add(L2);
        TR1.Cells.Add(TC1);
        TR1.Cells.Add(TC2);
        tbl_sharedProjects.Rows.Add(TR1);

        int sharedFlag = 0;

        // List all projects that the user owns and is sharing with others
        if (DT6.Rows.Count > 0)
        {
            foreach (DataRow DR in DT6.Rows)
            {
                DataTable DTp = theCake.getProjectPermissions((int)DR["ID"]);
                if (DTp.Rows.Count > 1) // We only want projects that have been shared
                {
                    sharedFlag = 1;
                    TableRow TR = new TableRow();
                    TableCell TownC1 = new TableCell();
                    TableCell TownC2 = new TableCell();
                    Label Lown1 = new Label();
                    Label Lown2 = new Label();
                    Lown1.Text = DR["taskName"].ToString();
                    Lown2.Text = "Owner"; // Should probably set this to use getUserProjectPermissions projectTitle value
                    TownC1.Controls.Add(Lown1);
                    TownC2.Controls.Add(Lown2);
                    TR.Cells.Add(TownC1);
                    TR.Cells.Add(TownC2);
                    
                    tbl_sharedProjects.Rows.Add(TR);
                }
            }
        }

        // List all projects in progress that are shared with user
        if (DT2.Rows.Count > 0)
        {
            foreach (DataRow DR in DT2.Rows)
            {
                sharedFlag = 1;
                DataTable DTp = theCake.getUserProjectPermissions(uID, (int)DR["ID"]);
                TableRow TR = new TableRow();
                TableCell TCwip1 = new TableCell();
                TableCell TCwip2 = new TableCell();
                Label Lwip1 = new Label();
                Label Lwip2 = new Label();
                Lwip1.Text = DR["taskName"].ToString();
                Lwip2.Text = DTp.Rows[0]["projectTitle"].ToString();
                TCwip1.Controls.Add(Lwip1);
                TCwip2.Controls.Add(Lwip2);
                TR.Cells.Add(TCwip1);
                TR.Cells.Add(TCwip2);
                tbl_sharedProjects.Rows.Add(TR);
            }
        }

        // List all ready projects that are shared with user
        if (DT3.Rows.Count > 0)
        {
            foreach (DataRow DR in DT3.Rows)
            {
                sharedFlag = 1;
                DataTable DTp = theCake.getUserProjectPermissions(uID, (int)DR["ID"]);
                TableRow TR = new TableRow();
                TableCell TCrdy1 = new TableCell();
                TableCell TCrdy2 = new TableCell();
                Label Lrdy1 = new Label();
                Label Lrdy2 = new Label();
                Lrdy1.Text = DR["taskName"].ToString();
                Lrdy2.Text = DTp.Rows[0]["projectTitle"].ToString();
                TCrdy1.Controls.Add(Lrdy1);
                TCrdy2.Controls.Add(Lrdy2);
                TR.Cells.Add(TCrdy1);
                TR.Cells.Add(TCrdy2);
                tbl_sharedProjects.Rows.Add(TR);
            }
        }

        // List all upcoming projects that are shared with user
        if (DT4.Rows.Count > 0)
        {
            foreach (DataRow DR in DT4.Rows)
            {
                sharedFlag = 1;
                DataTable DTp = theCake.getUserProjectPermissions(uID, (int)DR["ID"]);
                TableRow TR = new TableRow();
                TableCell TCup1 = new TableCell();
                TableCell TCup2 = new TableCell();
                Label Lup1 = new Label();
                Label Lup2 = new Label();
                Lup1.Text = DR["taskName"].ToString();
                Lup2.Text = DTp.Rows[0]["projectTitle"].ToString();
                TCup1.Controls.Add(Lup1);
                TCup2.Controls.Add(Lup2);
                TR.Cells.Add(TCup1);
                TR.Cells.Add(TCup2);
                tbl_sharedProjects.Rows.Add(TR);
            }
        }
   
        //If there are no shared projects, display a message
        if (sharedFlag == 0)
        {
            TableRow TR = new TableRow();
            TableCell TC = new TableCell();
            Label LE = new Label();
            LE.Text = "This user has no shared projects.";
            TC.Controls.Add(LE);
            TR.Cells.Add(TC);
            tbl_sharedProjects.Rows.Add(TR);
        }
    }
}