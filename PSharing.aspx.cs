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
        lnk_ReturnToProject.PostBackUrl = "ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString();
        getPermissions();
        if (IsPostBack)
        {
            SearchNames();
            SearchWWID();
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
        DataTable DT;

        if (txt_MI.Text.Length > 0)
            DT = theCake.getfromCDIS(txt_FirstName.Text.Trim(), txt_LastName.Text.Trim(), txt_MI.Text.Trim());
        else
            DT = theCake.getfromCDIS(txt_FirstName.Text.Trim(), txt_LastName.Text.Trim());

        if (DT.Rows.Count == 0)
        {
            lbl_checkMessages.Text = "No matches found for First and Last name";
            //btn_AddNewPermission.Enabled = false;
            newAlias = "";
        }

        tbl_possibleNames.Rows.Clear();
        if (DT.Rows.Count >= 1)
        {
            btn_AddNewPermission.Enabled = true;
            lbl_checkMessages.Text = "";
            newAlias = "";
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR = new TableRow();
                TableCell TC = new TableCell();
                RadioButton rb = new RadioButton();
                rb.GroupName = "possibles";
                if (DR["MiddleInitial"].ToString() == "")
                {
                    rb.Text = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["LastName"].ToString();
                    rb.ID = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["LastName"].ToString();
                    rb.ID = DR["ccMailPO"].ToString() + "\\" + DR["shortID"].ToString();
                }
                else
                {
                    rb.Text = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["MiddleInitial"].ToString() + " " + DR["LastName"].ToString();
                    rb.ID = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["MiddleInitial"].ToString() + " " + DR["LastName"].ToString();
                    rb.ID = DR["ccMailPO"].ToString() + "\\" + DR["shortID"].ToString();
                }
                TC.Controls.Add(rb);
                TR.Cells.Add(TC);
                tbl_possibleNames.Rows.Add(TR);
            }
        }
    }

    private void SearchWWID()
    {
        DataTable DT;

        DT = theCake.getfromCDIS(txt_WWID.Text.Trim());
        
        if (DT.Rows.Count == 0)
        {
            lbl_checkMessages2.Text = "No matches found for WWID";
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
                if (DR["MiddleInitial"].ToString() == "")
                {
                    rb.Text = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["LastName"].ToString();
                    //rb.ID = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["LastName"].ToString();
                    rb.ID = DR["ccMailPO"].ToString() + "\\" + DR["shortID"].ToString();
                }
                else
                {
                    rb.Text = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["MiddleInitial"].ToString() + " " + DR["LastName"].ToString();
                    //rb.ID = "[" + DR["WWID"].ToString() + "] " + DR["FirstName"].ToString() + " " + DR["MiddleInitial"].ToString() + " " + DR["LastName"].ToString();
                    rb.ID = DR["ccMailPO"].ToString() + "\\" + DR["shortID"].ToString();
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

    protected void btn_CheckWWID_OnClick(object sender, EventArgs e)
    {
        SearchWWID();
    }

    protected void btn_AddNewPermission_OnClick(object sender, EventArgs e)
    {
        btn_AddNewPermission.Enabled = false;
        bool found = false;
        string newAlias = "";
        foreach (TableRow TR in tbl_possibleNames.Rows)
        {
            RadioButton rb = (RadioButton)(TR.Cells[0].Controls[0]);
            if (rb.Checked)
            {
                found = true;
                newAlias = rb.ID;
                break;
            }
        }
        if (!found)
        {
            foreach (TableRow TR in tbl_possibleNames2.Rows)
            {
                RadioButton rb = (RadioButton)(TR.Cells[0].Controls[0]);
                if (rb.Checked)
                {
                    found = true;
                    newAlias = rb.ID;
                    break;
                }
            }
        }

        int PR = 0;
        int PW = 0;
        int BR = 0;
        int BW = 0;

        if (radio_Board_Read.Checked) BR = 1;
        if (radio_Board_Write.Checked) { BW = 1; BR = 1; }
        if (radio_Project_Read.Checked) PR = 1;
        if (radio_Project_Write.Checked) { PW = 1; PR = 1; }

        theCake.addNewPermission(Int32.Parse(Request.QueryString["ID"].ToString()), newAlias, theCake.getActiveUserName(Request.UserHostAddress), PR, PW, BR, BW);
        Response.Redirect("PSharing.aspx?ID=" + Request.QueryString["ID"].ToString());
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

    protected void lnkbtn_AddbyWWID_OnClick(object sender, EventArgs e)
    {
        if (lnkbtn_AddbyWWID.CommandArgument == "0")
        {
            pnl_AddByWWID.Visible = true;
            lnkbtn_AddbyWWID.Text = "[-] Minimize";
            lnkbtn_AddbyWWID.CommandArgument = "1";
        }
        else
        {
            pnl_AddByWWID.Visible = false;
            lnkbtn_AddbyWWID.Text = "[+] Add By WWID?";
            lnkbtn_AddbyWWID.CommandArgument = "0";
        }
    }
}