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

public partial class ViewTask : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    bool ProjectWrite = true;
    bool BoardWrite = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;

        if (Request.QueryString["ID"] != null)
        {
            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            DataTable DT = theCake.getTask(Int32.Parse(Request.QueryString["ID"].ToString()), theCake.getActiveUserName(IP));

            if (DT.Rows.Count == 0)
            {
                Response.Redirect("Home.aspx");
            }

            string ownerAlias = theCake.getUserAlias(Int32.Parse(DT.Rows[0]["ownerID"].ToString()));
            if (theCake.getActiveUserName(IP) != ownerAlias)
            {
                int userID = theCake.getUserID(theCake.getActiveUserName(IP));
                DataTable permissionChecker = theCake.getUserProjectPermissions(userID, Int32.Parse(Request.QueryString["ID"].ToString()));
                if (permissionChecker.Rows[0]["permission_Project_Write"].ToString() == "0")
                    ProjectWrite = false;
                if (permissionChecker.Rows[0]["permission_Board_Write"].ToString() == "0")
                    BoardWrite = false;
            }

            if (!ProjectWrite)
            {
                pnl_EditOperations.Visible = false;
                pnl_SpecialOptions.Visible = false;
                btn_AddMilestone.Visible = false;
            }
            if (!BoardWrite)
            {
                btn_AddComment.Visible = false;
            }

            lbl_TaskName.Text = DT.Rows[0]["taskName"].ToString();
            lbl_Description.Text = DT.Rows[0]["taskDescription"].ToString();

            lbl_ExpectedStart.Text = DateTime.Parse(DT.Rows[0]["expectedStart"].ToString()).ToShortDateString();
            lbl_ExpectedStop.Text = DateTime.Parse(DT.Rows[0]["expectedStop"].ToString()).ToShortDateString();

            if (DT.Rows[0]["actualStart"].ToString() == "") lbl_ActualStart.Text = "Not Yet Started";
            else lbl_ActualStart.Text = DateTime.Parse(DT.Rows[0]["actualStart"].ToString()).ToShortDateString();

            if (DT.Rows[0]["actualStop"].ToString() == "") lbl_ActualStop.Text = "Not Yet Completed";
            else lbl_ActualStop.Text = DateTime.Parse(DT.Rows[0]["actualStop"].ToString()).ToShortDateString();

            if (DT.Rows[0]["doneFlag"].ToString() == "1")
            {
                btn_markDone.Visible = false;
                btn_markWip.Visible = true;
                btn_UpgradeSize.Visible = false;

                AddComments.Visible = false;

                if (DT.Rows[0]["projectSize"].ToString() == "0")
                {
                    pnl_Comments.Visible = true;
                    FillComments();
                }
                else
                {
                    pnl_Milestone.Visible = true;
                    FillMilestones();
                    pnl_Boards.Visible = true;
                    getallBoards();
                }
            }
            else
            {
                if (DT.Rows[0]["actualStart"].ToString() == "")
                {
                    btn_markDone.Visible = false;
                    btn_markWip.Visible = false;

                    AddComments.Visible = false;
                    btn_startTask.Visible = true;
                }

                if (DT.Rows[0]["projectSize"].ToString() == "0")
                {
                    pnl_Comments.Visible = true;
                    FillComments();
                    btn_UpgradeSize.Visible = true;
                }
                else
                {
                    pnl_Milestone.Visible = true;
                    FillMilestones();
                    pnl_Boards.Visible = true;
                    getallBoards();
                }
            }


        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void btn_startTask_OnClick(object sender, EventArgs e)
    {
        theCake.startTask(Int32.Parse(Request.QueryString["ID"].ToString()));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_AddComment_OnClick(object sender, EventArgs e)
    {
        string comment = txt_addNewComment.Text;
        comment = comment.Replace("\r\n", "<br />");

        theCake.addNewComment(Int32.Parse(Request.QueryString["ID"].ToString()), comment);
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void FillComments()
    {
        DataTable DT = theCake.getComments(Int32.Parse(Request.QueryString["ID"].ToString()));

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR = new TableRow();
                TableCell TC0 = new TableCell();
                LinkButton LB = new LinkButton();
                LB.Text = "X";
                LB.CommandArgument = DR["commentID"].ToString();
                LB.Click += new EventHandler(LB_removeComment_OnClick);
                if (!BoardWrite) LB.Visible = false;
                TC0.Controls.Add(LB);
                TC0.Width = Unit.Pixel(30);
                TR.Cells.Add(TC0);

                TableCell TC1 = new TableCell();
                TC1.Text = "<strong>[<i>" + DR["createdTimestamp"].ToString() + "</i>]</strong>  " + DR["comment"].ToString();
                TR.Cells.Add(TC1);
                tbl_Comments.Rows.Add(TR);

                TR = new TableRow();
                TableCell TC = new TableCell();
                TR.Cells.Add(TC);
                tbl_Comments.Rows.Add(TR);
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC = new TableCell();
            TC.Text = "No comments exist for this task.";
            TR.Cells.Add(TC);
            tbl_Comments.Rows.Add(TR);
        }
    }

    public void FillMilestones()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getMilestones(theCake.getActiveUserName(IP), Int32.Parse(Request.QueryString["ID"].ToString()));

        decimal totalMax = 0;
        decimal countMax = 0;

        Table baseTable = new Table();
        baseTable.CellPadding = 0;
        baseTable.CellSpacing = 3;
        TableRow baseRow = new TableRow();
        int counter = 0;
        int maxPerRow = 4;

        foreach (DataRow DR in DT.Rows)
        {
            Feature topFeature = new Feature(int.Parse(DR["ID"].ToString()));
            totalMax += topFeature.getWeight();
            countMax += (decimal)(topFeature.getWeight()) * ((decimal)(topFeature.getPercentComplete()) / 100);

            TableCell baseCell = new TableCell();
            baseCell.VerticalAlign = VerticalAlign.Top;
            baseCell.Width = Unit.Pixel(300);
            if (counter == maxPerRow)
            {
                baseTable.Rows.Add(baseRow);
                baseRow = new TableRow();
                counter = 0;
            }
            counter++;

            Table tbl = new Table();
            tbl.Width = Unit.Percentage(100);

            TableRow TR = new TableRow();
            TableCell TC = new TableCell();
            Literal hr = new Literal();
            hr.Text = "<hr />";
            Label name = new Label();
            name.Font.Size = FontUnit.Small;
            name.Font.Bold = true;
            name.Text = topFeature.getFeatureName() + " <i>(" + topFeature.getPercentComplete() + "%)</i>";
            LinkButton edit = new LinkButton();
            edit.Text = "[edit]";
            edit.Font.Size = FontUnit.XXSmall;
            edit.CommandArgument = topFeature.getID().ToString();
            edit.Click += new EventHandler(btn_gotoUpdateMilestone);
            TC.HorizontalAlign = HorizontalAlign.Center;
            TC.Controls.Add(hr);
            TC.Controls.Add(name);
            TC.Controls.Add(edit);
            tbl.ToolTip = topFeature.getFeatureDescription();
            TR.Cells.Add(TC);
            tbl.Rows.Add(TR);

            TR = new TableRow();
            TC = new TableCell();
            if (topFeature.getPercentComplete() == 0)
            {
                TC.Text = "<center><table width=\"100%\" style=\"border:1px solid black;\" cellspacing=0><tr><td></td></tr></table></center>";
            }
            else if (topFeature.getPercentComplete() == 100)
            {
                TC.Text = "<center><table width=\"100%\" style=\"border:1px solid black;\" cellspacing=0><tr><td style=\"background-color:Green;\"></td></tr></table></center>";
            }
            else
            {
                TC.Text = "<center><table width=\"100%\" style=\"border:1px solid black;\" cellspacing=0><tr><td width=\"" + DR["completed"].ToString() + "%\" style=\"background-color:Green;\"></td><td></td></tr></table></center>";
            }
            TR.Cells.Add(TC);
            tbl.Rows.Add(TR);

            //DataTable DT2 = theCake.getFeatures(topFeature.getID());
            TableRow TR2 = new TableRow();
            TableCell TC2 = new TableCell();
            foreach (Feature child in topFeature.myChildren)
            //if (DT2.Rows.Count > 0)
            {
                //foreach (DataRow DR2 in DT2.Rows)
                //{
                Button btnDone = new Button();
                btnDone.Text = ((char)(0X2713)).ToString();
                btnDone.CommandArgument = child.getID().ToString();
                btnDone.Click += new EventHandler(btnDone_OnClick);
                if (!ProjectWrite) btnDone.Visible = false;
                if (child.getPercentComplete() == 0) TC2.Controls.Add(btnDone);
                Button btnRem = new Button();
                btnRem.Text = "x";
                btnRem.CommandArgument = child.getID().ToString();
                btnRem.Click += new EventHandler(btnRem_OnClick);
                if (!ProjectWrite) btnRem.Visible = false;
                if (child.getPercentComplete() == 0) TC2.Controls.Add(btnRem);
                Label lbl = new Label();
                lbl.Text = " - " + child.getFeatureName();
                if (child.getPercentComplete() == 100)
                {
                    lbl.ForeColor = System.Drawing.Color.Green;
                    lbl.Font.Italic = true;
                }
                TC2.Controls.Add(lbl);
                Label blnk = new Label();
                blnk.Text = "<br />";
                TC2.Controls.Add(blnk);
                //}
            }
            TR2.Cells.Add(TC2);
            tbl.Rows.Add(TR2);
            

            if (BoardWrite)
            {
                TableRow TR3 = new TableRow();
                TableCell TC3 = new TableCell();

                // Ability to add Feature is the series of buttons here.
                LinkButton addFeature = new LinkButton();
                addFeature.Text = "{+}";
                addFeature.Click += new EventHandler(btn_AddFeature_OnClick);
                TextBox txtFeature = new TextBox();
                txtFeature.Visible = false;
                Button addFeature_Final = new Button();
                addFeature_Final.Text = "Add Feature";
                addFeature_Final.Visible = false;
                addFeature_Final.CommandArgument = DR["ID"].ToString();
                addFeature_Final.Click += new EventHandler(btn_AddFeatureFinal_OnClick);

                TC3.Controls.Add(addFeature);
                TC3.Controls.Add(txtFeature);
                TC3.Controls.Add(addFeature_Final);
                TR3.Cells.Add(TC3);
                tbl.Rows.Add(TR3);
            }

            baseCell.Controls.Add(tbl);
            baseRow.Cells.Add(baseCell);
        }
        baseTable.Rows.Add(baseRow);

        pnl_MileStones.Controls.Add(baseTable);

        if (totalMax > 0)
        {
            lbl_totalPercentComplete.Text = ((int)(100 * ((decimal)countMax) / ((decimal)totalMax))).ToString() + "% Completed";
            lbl_totalPercentComplete.Visible = true;
        }
    }

    protected void btnDone_OnClick(object sender, EventArgs e)
    {
        Feature ftr = new Feature(Int32.Parse(((Button)sender).CommandArgument));
        ftr.updatePercentComplete(100);
        theCake.completeFeature(Int32.Parse(((Button)sender).CommandArgument));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btnRem_OnClick(object sender, EventArgs e)
    {
        theCake.deleteFeature(Int32.Parse(((Button)sender).CommandArgument));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void LB_removeComment_OnClick(object sender, EventArgs e)
    {
        int commentID = Int32.Parse(((LinkButton)sender).CommandArgument);
        theCake.removeComments(commentID);
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_markDone_OnClick(object sender, EventArgs e)
    {
        theCake.markTaskDone(Int32.Parse(Request.QueryString["ID"].ToString()));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_markWip_OnClick(object sender, EventArgs e)
    {
        theCake.markTaskWip(Int32.Parse(Request.QueryString["ID"].ToString()));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_AddMilestone_OnClick(object sender, EventArgs e)
    {
        pnl_AddMilestone.Visible = true;
        btn_AddMilestone.Visible = false;
    }

    protected void btn_AddMilestone_Final_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        int boardID = theCake.addNewBoard(txt_Milestone_Name.Text, theCake.getUserID(theCake.getActiveUserName(IP)), 1);
        //theCake.addNewMilestone(Int32.Parse(Request.QueryString["ID"].ToString()), txt_Milestone_Name.Text, txt_Milestone_Desc.Text, Int32.Parse(ddl_Milestone_Weight.SelectedItem.Value), boardID);
        
        Feature.addNewMilestone(Int32.Parse(Request.QueryString["ID"].ToString()), txt_Milestone_Name.Text, txt_Milestone_Desc.Text, Int32.Parse(ddl_Milestone_Weight.SelectedItem.Value), boardID);
        
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_AddFeatureFinal_OnClick(object sender, EventArgs e)
    {
        string featureName = ((TextBox)((((TableCell)((Button)sender).Parent)).Controls[1])).Text;
        Feature.addNewFeature(Int32.Parse(Request.QueryString["ID"].ToString()), Int32.Parse(((Button)sender).CommandArgument), featureName, "", 1, -1);
        
        //theCake.addNewFeature(Int32.Parse(((Button)sender).CommandArgument), Feature);
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_AddFeature_OnClick(object sender, EventArgs e)
    {
        ((LinkButton)sender).Visible = false;
        ((TextBox)((((TableCell)((LinkButton)sender).Parent)).Controls[1])).Visible = true;
        ((Button)((((TableCell)((LinkButton)sender).Parent)).Controls[2])).Visible = true;
    }

    public void getallBoards()
    {
        DataTable DT = theCake.getBoards(Int32.Parse(Request.QueryString["ID"].ToString()));
        if (DT.Rows.Count > 0)
        {
            lnk_GoToDiscussion.PostBackUrl = "Boards.aspx?ID=" + DT.Rows[0]["boardID"].ToString();
        }

    //    foreach (DataRow DR in DT.Rows)
    //    {
    //        Board brd = new Board(Int32.Parse(DR["boardID"].ToString()));


    //        TableRow TR1 = new TableRow();
    //        TableCell TC1 = new TableCell();

    //        Table catTBL = new Table();
    //        catTBL.CellPadding = 5;
    //        catTBL.Width = Unit.Percentage(100);
    //        catTBL.BorderColor = System.Drawing.Color.Black;
    //        catTBL.BorderStyle = BorderStyle.Solid;
    //        catTBL.BorderWidth = Unit.Pixel(1);
    //        catTBL.BackColor = System.Drawing.ColorTranslator.FromHtml("#1b1f27");
    //        TableRow catTR = new TableRow();
    //        TableCell catTC = new TableCell();
    //        catTC.Width = Unit.Percentage(80);
    //        LinkButton CategoryName = new LinkButton();
    //        CategoryName.Text = brd.get_board_CategoryName();
    //        CategoryName.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0000FF");
    //        CategoryName.Font.Size = FontUnit.XLarge;
    //        CategoryName.Font.Bold = true;
    //        CategoryName.Font.Name = "Courier";
    //        CategoryName.Style.Add("font-variant", "small-caps");
    //        CategoryName.PostBackUrl = "Boards.aspx?ID=" + brd.get_BoardID();
    //        catTC.Controls.Add(CategoryName);
    //        Literal hr = new Literal();
    //        hr.Text = "<br /><hr /><br />";
    //        catTC.Controls.Add(hr);
    //        Label created = new Label();
    //        created.Text = "created on " + brd.get_createdTimestamp().ToShortDateString() + " by " + theCake.getUserAlias(brd.get_createdBy()) + ".";
    //        created.Font.Size = FontUnit.XXSmall;
    //        created.Font.Italic = true;
    //        catTC.Controls.Add(created);
    //        catTC.CssClass = "CategoryTable";

    //        catTR.Cells.Add(catTC);
    //        TableCell catTC2 = new TableCell();
    //        catTC2.Width = Unit.Percentage(20);
    //        catTC2.CssClass = "CategoryTable";
    //        catTC2.HorizontalAlign = HorizontalAlign.Center;
    //        Label l1 = new Label();
    //        l1.Text = ":Threads:<br />";
    //        l1.Font.Bold = true;
    //        catTC2.Controls.Add(l1);
    //        Label threads = new Label();
    //        threads.Text = brd.ThreadCount().ToString();
    //        catTC2.Controls.Add(threads);
    //        Literal hr2 = new Literal();
    //        hr2.Text = "<br />";
    //        catTC2.Controls.Add(hr2);
    //        Label l2 = new Label();
    //        l2.Text = ":Posts:<br />";
    //        l2.Font.Bold = true;
    //        catTC2.Controls.Add(l2);
    //        Label posts = new Label();
    //        posts.Text = brd.PostCount().ToString();
    //        catTC2.Controls.Add(posts);
    //        catTR.Cells.Add(catTC2);

    //        catTBL.Rows.Add(catTR);
    //        TC1.Controls.Add(catTBL);
    //        TR1.Cells.Add(TC1);
    //        tbl_Boards.Rows.Add(TR1);
    //    }
    }

    protected void btn_UpgradeSize_OnClick(object sender, EventArgs e)
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        theCake.increaseProjectSize(Int32.Parse(Request.QueryString["ID"].ToString()), theCake.getUserID(theCake.getActiveUserName(IP)));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_Edit_NameDescription_OnClick(object sender, EventArgs e)
    {
        txt_Edit_TaskDescription.Text = lbl_Description.Text;
        txt_Edit_TaskName.Text = lbl_TaskName.Text;

        pnl_MainHeader.Visible = false;
        pnl_Mainheader_Edit.Visible = true;
    }

    protected void btn_Update_NameDescription_OnClick(object sender, EventArgs e)
    {
        theCake.updateTaskBaseInfo(Int32.Parse(Request.QueryString["ID"].ToString()), txt_Edit_TaskName.Text, txt_Edit_TaskDescription.Text);
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_Edit_Sharing_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PSharing.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_UpdateMilestone_OnClick(object sender, EventArgs e)
    {
        Feature ftr = new Feature(Int32.Parse(((Button)sender).CommandArgument));
        ftr.updateFeatureDetails(txt_Milestone_Name.Text, txt_Milestone_Desc.Text, int.Parse(ddl_Milestone_Weight.SelectedValue));
        //theCake.updateMilestone(Int32.Parse(((Button)sender).CommandArgument), txt_Milestone_Name.Text, txt_Milestone_Desc.Text, Int32.Parse(ddl_Milestone_Weight.SelectedItem.Text));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_gotoUpdateMilestone(object sender, EventArgs e)
    {
        int featureID = Int32.Parse(((LinkButton)sender).CommandArgument);
        btn_UpdateMilestone.CommandArgument = featureID.ToString();
        Feature ftr = new Feature(featureID);
        txt_Milestone_Desc.Text = ftr.getFeatureDescription();
        txt_Milestone_Name.Text = ftr.getFeatureName();
        ddl_Milestone_Weight.SelectedValue = ftr.getWeight().ToString();

        btn_UpdateMilestone.Visible = true;
        btn_AddMilestone.Visible = false;
        btn_AddMilestone_Final.Visible = false;
        pnl_AddMilestone.Visible = true;
        
    }
}