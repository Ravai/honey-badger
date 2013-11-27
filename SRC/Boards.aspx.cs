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

        if (!IsPostBack)
        {
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

                if (!BoardWrite)
                {
                    pnl_addThread.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
    }

    public void getThreads()
    {
        DataTable DT = theCake.getThreads(Int32.Parse(Request.QueryString["ID"].ToString()));

        foreach (DataRow DR in DT.Rows)
        {
            Thread thrd = new Thread(Int32.Parse(DR["threadID"].ToString()));

            string threadItem = "";

            threadItem += "<div style=\"border:1px solid #646469;\">" +
                "<div align=\"center\">" +
                "<section style=\"display:table; width:90%; margin:10px; text-align:left;\">" +
                    "<section style=\"display:inline-block; width:90%; height:100%; vertical-align:top;\">" +
                        "<div style=\"font-variant:small-caps; font-size:1.2em;\">" +
                            "<a href=\"Threads.aspx?ID=" + thrd.get_threadID().ToString() + "\">" + thrd.get_thread_Name() + "</a>" +
                        "</div>" +
                        "<hr style=\"margin:2px;\" />" +
                        "<div style=\"font-size:.9em;\">" +
                            thrd.get_thread_Description() +
                        "</div>" +
                        "<br />" +
                        "<div style=\"font-size:.6em;\">" +
                            "created on " + thrd.get_createdTimestamp().ToShortDateString() + " by " + theCake.getUserAlias(thrd.get_createdBy()) + "." +
                        "</div>" +
                    "</section>" +
                    "<section style=\"display:inline-block; width:10%; height:100%;\">" +
                        "<div class=\"widget pull-right\" style=\"width:100%;\">" +
                            "<div align=\"center\">" +
                                "<p><strong>Post Count</strong></p>" +
                                thrd.count().ToString() +
                            "</div>" +
                        "</div>" +
                    "</section>" +
                "</section>" +
                "</div>" +
            "</div>";

            lit_ThreadList.Text += threadItem;
        }
    }

    protected void btn_newThread_OnClick(object sender, EventArgs e)
    {
        if (txt_Description.Text.Length > 0 && txt_Subject.Text.Length > 0)
        {
            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            theCake.addNewThread(Int32.Parse(Request.QueryString["ID"].ToString()), txt_Subject.Text, txt_Description.Text, theCake.getUserID(theCake.getActiveUserName(IP)));
            Response.Redirect("Boards.aspx?ID=" + Request.QueryString["ID"].ToString());
        }
        else
        {
            lbl_Error.Visible = true;
        }
    }
}