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

public partial class PSharing : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    string newAlias = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] != null)
        {
            lnk_ReturnToProject.PostBackUrl = "ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString();
            getPermissions();
            PermissionLabel.Text = "Select a role to view permission levels";
            if (IsPostBack)
            {
                SearchNames();
                SearchuserName();
            }
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    private void getPermissions()
    {
        DataTable DT = theCake.getProjectPermissions(Int32.Parse(Request.QueryString["ID"].ToString()));

        tbl_List.Rows.Clear();
        if (DT.Rows.Count == 0)
        {
            TableRow TR = new TableRow();
            TableCell TC = new TableCell();
            TC.Text = "No users have permission";
            TR.Cells.Add(TC);
            tbl_List.Rows.Add(TR);
        }
        else
        {
            foreach (DataRow DR in DT.Rows)
            {
                int ID = Int32.Parse(DR["user_GivenTo"].ToString());
                string Name = theCake.getUserAlias(ID);
                TableRow TR = new TableRow();
                TableCell TC = new TableCell();

                string project = "";
                string board = "";
                if (DR["permission_Project_Read"].ToString() == "1")
                {
                    project = "Read";
                }
                if (DR["permission_Project_Write"].ToString() == "1")
                {
                    if (project.Length > 0) project += "/Write";
                    else project = "Write";
                }
                if (DR["permission_Board_Read"].ToString() == "1")
                {
                    board = "Read";
                }
                if (DR["permission_Board_Write"].ToString() == "1")
                {
                    if (board.Length > 0) board += "/Write";
                    else board = "Write";
                }

                TC.Text = Name + "[Project - " + project + "] [Boards - " + board + "]";
                TR.Cells.Add(TC);
                tbl_List.Rows.Add(TR);
            }
        }
    }

    private void SearchNames()
    {
        DataTable DT = theCake.searchUsersByName(txt_FirstName.Text.Trim(), txt_MiddleName.Text.Trim(), txt_LastName.Text.Trim());
        newAlias = "";
        if (DT.Rows.Count == 0)
        {
            lbl_checkMessages.Text = "No matches found for First and Last name";
        }
        else if (DT.Rows.Count > 10)
        {
            lbl_checkMessages.Text = "Too many results. Add more criteria";
        }
        else
        {
            tbl_possibleNames.Rows.Clear();
            //btn_AddNewPermission.Enabled = true;
            lbl_checkMessages.Text = "";
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR = new TableRow();
                TableCell TC = new TableCell();
                RadioButton rb = new RadioButton();
                rb.GroupName = "possibles";
                if (DR["middleName"].ToString() == "")
                {
                    rb.Text = "[" + DR["ownerAlias"].ToString() + "] " + DR["firstName"].ToString() + " " + DR["lastName"].ToString();
                    rb.ID = DR["ownerAlias"].ToString();
                }
                else
                {
                    rb.Text = "[" + DR["ownerAlias"].ToString() + "] " + DR["firstName"].ToString() + " " + DR["middleName"].ToString() + " " + DR["lastName"].ToString();
                    rb.ID = DR["ownerAlias"].ToString();
                }
                TC.Controls.Add(rb);
                TR.Cells.Add(TC);
                tbl_possibleNames.Rows.Add(TR);
            }
        }
    }

    private void SearchuserName()
    {
        DataTable DT = theCake.searchUsersByUserName(txt_userName.Text.Trim());

        //DT = theCake.getfromCDIS(txt_userName.Text.Trim());
        
        if (DT.Rows.Count == 0)
        {
            lbl_checkMessages2.Text = "No matches found for userName";
            //btn_AddNewPermission.Enabled = false;
            newAlias = "";
        }

        tbl_possibleNames2.Rows.Clear();
        if (DT.Rows.Count >= 1)
        {
            btn_AddNewPermission.Enabled = true;
            lbl_checkMessages2.Text = "";
            newAlias = "";
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR = new TableRow();
                TableCell TC = new TableCell();
                RadioButton rb = new RadioButton();
                rb.GroupName = "possibles";
                if (DR["middleName"].ToString() == "")
                {
                    rb.Text = "[" + DR["ownerAlias"].ToString() + "] " + DR["firstName"].ToString() + " " + DR["lastName"].ToString();
                    rb.ID = DR["ownerAlias"].ToString();
                }
                else
                {
                    rb.Text = "[" + DR["ownerAlias"].ToString() + "] " + DR["firstName"].ToString() + " " + DR["middleName"].ToString() + " " + DR["lastName"].ToString();
                    rb.ID = DR["ownerAlias"].ToString();
                }
                TC.Controls.Add(rb);
                TR.Cells.Add(TC);
                tbl_possibleNames2.Rows.Add(TR);
            }
        }
    }

    protected void btn_CheckName_OnClick(object sender, EventArgs e)
    {
        SearchNames();
    }

    protected void btn_CheckuserName_OnClick(object sender, EventArgs e)
    {
        SearchuserName();
    }

    protected void btn_AddNewPermission_OnClick(object sender, EventArgs e)
    {
        btn_AddNewPermission.Enabled = false;
        bool userFound = false;
        bool roleSelected = false;
        string newAlias = "";
        
        // Check if name/user id selected
        foreach (TableRow TR in tbl_possibleNames.Rows)
        {
            RadioButton rb = (RadioButton)(TR.Cells[0].Controls[0]);
            if (rb.Checked)
            {
                userFound = true;
                newAlias = rb.ID;
                break;
            }
        }
        if (!userFound)
        {
            foreach (TableRow TR in tbl_possibleNames2.Rows)
            {
                RadioButton rb = (RadioButton)(TR.Cells[0].Controls[0]);
                if (rb.Checked)
                {
                    userFound = true;
                    newAlias = rb.ID;
                    break;
                }
            }
        }

        // Check for User role selected
        int index = Convert.ToInt32(roles_DropDownList.SelectedItem.Value);
        if (index >= 0)
            roleSelected = true;

        if (userFound && roleSelected)
        {
            int[] permissions = Roles.getPermissions(Convert.ToInt32(roles_DropDownList.SelectedItem.Value));
            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            theCake.addNewPermission(Int32.Parse(Request.QueryString["ID"].ToString()), newAlias, theCake.getActiveUserName(IP), 
                                     permissions[0], permissions[1], permissions[2], permissions[3]);
            Response.Redirect("PSharing.aspx?ID=" + Request.QueryString["ID"].ToString());
        }
        else
            ValidationLabel.Text = "***Select User and Role to give them permissions***";
    }

    protected void lnkbtn_AddbyName_OnClick(object sender, EventArgs e)
    {
        if (lnkbtn_AddbyName.CommandArgument == "0")
        {
            pnl_AddByName.Visible = true;
            lnkbtn_AddbyName.Text = "[-] Minimize";
            lnkbtn_AddbyName.CommandArgument = "1";
        }
        else
        {
            pnl_AddByName.Visible = false;
            lnkbtn_AddbyName.Text = "[+] Add By Name?";
            lnkbtn_AddbyName.CommandArgument = "0";
        }
    }

    protected void lnkbtn_AddbyUserName_OnClick(object sender, EventArgs e)
    {
        if (lnkbtn_AddbyUserName.CommandArgument == "0")
        {
            pnl_AddByUserName.Visible = true;
            lnkbtn_AddbyUserName.Text = "[-] Minimize";
            lnkbtn_AddbyUserName.CommandArgument = "1";
        }
        else
        {
            pnl_AddByUserName.Visible = false;
            lnkbtn_AddbyUserName.Text = "[+] Add By userName?";
            lnkbtn_AddbyUserName.CommandArgument = "0";
        }
    }
    protected void roles_DropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(roles_DropDownList.SelectedItem.Value);
        PermissionLabel.Text = Roles.retrievePermissions(index);
        if (index >= 0)
            btn_AddNewPermission.Enabled = true;
    }
}