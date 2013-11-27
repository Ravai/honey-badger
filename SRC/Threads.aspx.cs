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

public partial class Threads : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    bool BoardWrite = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] == null) Response.Redirect("Home.aspx");

        DataTable DT = theCake.getThread(Int32.Parse(Request.QueryString["ID"].ToString()));
        if (DT.Rows.Count == 1)
        {
            lbl_ThreadName.Text = DT.Rows[0]["thread_Name"].ToString();
            lnk_ReturnToBoard.PostBackUrl = "Boards.aspx?ID=" + DT.Rows[0]["boardID"].ToString();

            if (!IsPostBack)
                getPosts();

            DataTable newDT = theCake.getBoard(Int32.Parse(DT.Rows[0]["boardID"].ToString()));
            boardName.Text = newDT.Rows[0]["board_CategoryName"].ToString();
            boardName.PostBackUrl = "Boards.aspx?ID=" + DT.Rows[0]["boardID"].ToString();

            string projectID = newDT.Rows[0]["projectID"].ToString();
            newDT = theCake.getTask(Int32.Parse(projectID));
            projectName.Text = newDT.Rows[0]["taskName"].ToString();
            projectName.PostBackUrl = "ViewTask.aspx?ID=" + projectID;

            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            if (theCake.getActiveUserName(IP) != DT.Rows[0]["ownerAlias"].ToString())
            {
                DataTable DT2 = theCake.getUserProjectPermissions(theCake.getUserID(theCake.getActiveUserName(IP)), Int32.Parse(projectID));
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
                pnl_AddPost.Visible = false;
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    public void getPosts()
    {
        DataTable DT = theCake.getPosts(Int32.Parse(Request.QueryString["ID"].ToString()));

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                Post pst = new Post(Int32.Parse(DR["postID"].ToString()));
                int uID = pst.get_postBy();
                string postBlock = "";
                
                postBlock += "<div class=\"widget\">" + 
                    "<section style=\"display:table;\">" + 
                        "<section style=\"display:inline-block;\">" + 
                            "<div class=\"widget\" align=\"center\">" + 
                                "<a href=\"UserProfile.aspx?userID=" + uID.ToString() + "\">" + pst.get_DisplayName() + "</a>" + "<br />" + 
                                "<img height=\"100px\" width=\"100px\" src=\"" + pst.get_DisplayImage() + "\" />" + 
                            "</div>" + 
                        "</section>" + 
                        "<section style=\"display:inline-block; vertical-align:top; margin:5px; width:100%-100px;\">" + 
                            "<div style=\"font-size:.8em;\">" + 
                                "<strong>Posted:</strong> " + pst.get_createdTimestamp().ToShortDateString() + " " + pst.get_createdTimestamp().ToShortTimeString() + 
                            "</div>" + 
                            "<hr style=\"margin:1px;\" />" + 
                            "<p style=\"font-size:1.1em;\">" + pst.get_post_Full() + "</p>" + 
                        "</section>" + 
                    "</section>" + 
                "</div>";

                lit_Posts.Text += postBlock;

            }
        }
        else
        {
            string postBlock = "";

            postBlock += "<div class=\"widget\">" +
                "<section style=\"display:table;\">" +
                    "<section style=\"display:inline-block;\">" +
                        "<div class=\"widget\" align=\"center\">" +
                            "System Monkey<br />" +
                            "<img height=\"100px\" width=\"100px\" src=\"http://placekitten.com/100/100\" />" +
                        "</div>" +
                    "</section>" +
                    "<section style=\"display:inline-block; vertical-align:top; margin:5px; width:100%-100px;\">" +
                        "<div style=\"font-size:.8em;\">" +
                            "<strong>Posted:</strong> Never Occurred!" + 
                        "</div>" +
                        "<hr style=\"margin:1px;\" />" +
                        "<p style=\"font-size:1.1em;\">No posts have occurred here yet.  Feel free to post something...</p>" +
                    "</section>" +
                "</section>" +
            "</div>";

            lit_Posts.Text += postBlock;
        }
    }

    protected void btn_newPost_OnClick(object sender, EventArgs e)
    {
        if (txt_newPost.Text.Length > 0)
        {
            string newPost = txt_newPost.Text;
            newPost = newPost.Replace("\n", "<br />");

            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            theCake.addNewPost(Int32.Parse(Request.QueryString["ID"].ToString()), newPost, theCake.getUserID(theCake.getActiveUserName(IP)));
            Response.Redirect("Threads.aspx?ID=" + Request.QueryString["ID"].ToString());
        }
        else
        {
            lbl_Error.Visible = true;
        }
    }
}