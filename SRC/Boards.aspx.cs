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

public partial class Boards : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    bool BoardWrite = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] == null) Response.Redirect("Home.aspx");

        Board brd = new Board(Int32.Parse(Request.QueryString["ID"].ToString()));
        
        DataTable DT = theCake.getBoard(Int32.Parse(Request.QueryString["ID"].ToString()));
        if (brd.isActive())//(DT.Rows.Count == 1)
        {
            boardName.Text = brd.get_board_CategoryName();// DT.Rows[0]["board_CategoryName"].ToString();
            boardName.PostBackUrl = "Boards.aspx?ID=" + brd.get_BoardID().ToString(); //DT.Rows[0]["boardID"].ToString();
            lnk_ReturnToProject.PostBackUrl = "ViewTask.aspx?ID=" + DT.Rows[0]["projectID"].ToString(); // need to remedy this one

            getThreads();

            DataTable newDT = theCake.getTask(Int32.Parse(DT.Rows[0]["projectID"].ToString()));
            projectName.Text = newDT.Rows[0]["taskName"].ToString();
            projectName.PostBackUrl = "ViewTask.aspx?ID=" + DT.Rows[0]["projectID"].ToString();

            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

            if (theCake.getActiveUserName(IP) != DT.Rows[0]["ownerAlias"].ToString())
            {
                DataTable DT2 = theCake.getUserProjectPermissions(theCake.getUserID(theCake.getActiveUserName(IP)), Int32.Parse(DT.Rows[0]["projectID"].ToString()));
                if (DT2.Rows.Count == 1)
                {
                    if (DT2.Rows[0]["permission_Board_Write"].ToString() == "0") BoardWrite = false;
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }
            }
        }
        else
        {
            Response.Redirect("Home.aspx");
        }

        if (!BoardWrite) pnl_AddThread1.Visible = false;
    }

    public void getThreads()
    {
        DataTable DT = theCake.getThreads(Int32.Parse(Request.QueryString["ID"].ToString()));

        foreach (DataRow DR in DT.Rows)
        {
            Thread thrd = new Thread(Int32.Parse(DR["threadID"].ToString()));

            TableRow TR1 = new TableRow();
            TableCell TC1 = new TableCell();

            Table catTBL = new Table();
            catTBL.CellPadding = 5;
            catTBL.Width = Unit.Percentage(100);
            catTBL.BorderColor = System.Drawing.Color.Black;
            catTBL.BorderStyle = BorderStyle.Solid;
            catTBL.BorderWidth = Unit.Pixel(1);
            catTBL.BackColor = System.Drawing.ColorTranslator.FromHtml("#1b1f27");
            TableRow catTR = new TableRow();
            TableCell catTC = new TableCell();
            catTC.Width = Unit.Percentage(80);
            LinkButton CategoryName = new LinkButton();
            CategoryName.Text = thrd.get_thread_Name();
            CategoryName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0000FF");
            CategoryName.Font.Size = FontUnit.XLarge;
            CategoryName.Font.Bold = true;
            CategoryName.Font.Name = "Courier";
            CategoryName.Style.Add("font-variant", "small-caps");
            CategoryName.PostBackUrl = "Threads.aspx?ID=" + thrd.get_threadID().ToString();
            catTC.Controls.Add(CategoryName);
            Literal hr = new Literal();
            hr.Text = "<br /><hr />";
            catTC.Controls.Add(hr);
            Label Description = new Label();
            Description.Text = thrd.get_thread_Description() + "<br /><br />";
            Description.Font.Size = FontUnit.Small;
            catTC.Controls.Add(Description);
            Label created = new Label();
            created.Text = "created on " + thrd.get_createdTimestamp().ToShortDateString() + " by " + theCake.getUserAlias(thrd.get_createdBy()) + ".";
            created.Font.Size = FontUnit.XXSmall;
            created.Font.Italic = true;
            catTC.Controls.Add(created);
            catTC.CssClass = "CategoryTable";

            catTR.Cells.Add(catTC);
            TableCell catTC2 = new TableCell();
            catTC2.Width = Unit.Percentage(20);
            catTC2.CssClass = "CategoryTable";
            catTC2.HorizontalAlign = HorizontalAlign.Center;
            Label l2 = new Label();
            l2.Text = ":Posts:<br />";
            l2.Font.Bold = true;
            catTC2.Controls.Add(l2);
            Label posts = new Label();
            posts.Text = thrd.count().ToString();
            catTC2.Controls.Add(posts);
            catTR.Cells.Add(catTC2);

            catTBL.Rows.Add(catTR);
            TC1.Controls.Add(catTBL);
            TR1.Cells.Add(TC1);
            tbl_Threads.Rows.Add(TR1);
        }
    }

    protected void btn_newThread_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        theCake.addNewThread(Int32.Parse(Request.QueryString["ID"].ToString()),txt_Subject.Text, txt_Description.Text, theCake.getUserID(theCake.getActiveUserName(IP)));
        Response.Redirect("Boards.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void lnkbtn_ShowAddThread_OnClick(object sender, EventArgs e)
    {
        pnl_AddThread1.Visible = false;
        pnl_AddThread2.Visible = true;
    }

    protected void btn_CancelNewThread_OnClick(object sender, EventArgs e)
    {
        pnl_AddThread1.Visible = true;
        pnl_AddThread2.Visible = false;
    }
}