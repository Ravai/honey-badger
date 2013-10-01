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
        }
        else
        {
            Response.Redirect("Home.aspx");
        }

        if (!BoardWrite) lnkbtn_ShowAddPost.Visible = false;
    }

    public void getPosts()
    {
        DataTable DT = theCake.getPosts(Int32.Parse(Request.QueryString["ID"].ToString()));

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                Post pst = new Post(Int32.Parse(DR["postID"].ToString()));

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

                TableCell catTC2 = new TableCell();
                catTC2.VerticalAlign = VerticalAlign.Top;
                catTC2.Width = Unit.Percentage(10);
                catTC2.CssClass = "CategoryTable";
                catTC2.HorizontalAlign = HorizontalAlign.Center;
                Label user = new Label();
                user.Text = theCake.getUserAlias(pst.get_postBy()) + "<hr />";
                catTC2.Controls.Add(user);
                Label postedOn = new Label();
                postedOn.Text = "<strong>Created: </strong><br />" + pst.get_createdTimestamp().ToShortDateString() + "<br />" + pst.get_createdTimestamp().ToShortTimeString();
                catTC2.Controls.Add(postedOn);
                catTR.Cells.Add(catTC2);

                TableCell catTC = new TableCell();
                catTC.Width = Unit.Percentage(90);
                catTC.VerticalAlign = VerticalAlign.Top;
                Label CategoryName = new Label();
                CategoryName.Text = pst.get_post_Full();
                CategoryName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0000FF");
                CategoryName.Font.Size = FontUnit.XLarge;
                CategoryName.Font.Name = "Courier";
                catTC.Controls.Add(CategoryName);
                catTC.CssClass = "CategoryTable";
                catTR.Cells.Add(catTC);

                catTBL.Rows.Add(catTR);
                TC1.Controls.Add(catTBL);
                TR1.Cells.Add(TC1);
                tbl_Posts.Rows.Add(TR1);
            }
        }
        else
        {
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
            catTC.Width = Unit.Percentage(90);
            Label CategoryName = new Label();
            CategoryName.Text = "There has been no posts.  Be the first to say something!";
            CategoryName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0000FF");
            CategoryName.Font.Size = FontUnit.XLarge;
            CategoryName.Font.Name = "Courier";
            CategoryName.Style.Add("font-variant", "small-caps");
            catTC.Controls.Add(CategoryName);
            catTC.CssClass = "CategoryTable";
            catTR.Cells.Add(catTC);

            catTBL.Rows.Add(catTR);
            TC1.Controls.Add(catTBL);
            TR1.Cells.Add(TC1);
            tbl_Posts.Rows.Add(TR1);
        }
    }

    protected void btn_newPost_OnClick(object sender, EventArgs e)
    {
        string newPost = txt_newPost.Text;
        newPost = newPost.Replace("\n", "<br />");

        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        theCake.addNewPost(Int32.Parse(Request.QueryString["ID"].ToString()), newPost, theCake.getUserID(theCake.getActiveUserName(IP)));
        Response.Redirect("Threads.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void lnkbtn_ShowAddPost_OnClick(object sender, EventArgs e)
    {
        pnl_AddPost1.Visible = false;
        pnl_AddPost2.Visible = true;
    }

    protected void btn_CancelNewPost_OnClick(object sender, EventArgs e)
    {
        pnl_AddPost1.Visible = true;
        pnl_AddPost2.Visible = false;
    }
}