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
    string ID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;

        if (Request.QueryString["ID"] != null)
        {
            updateProjectPercent();

            ID = Request.QueryString["ID"].ToString();
            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            DataTable DT = theCake.getTask(Int32.Parse(ID), theCake.getActiveUserName(IP));

            if (DT.Rows.Count == 0)
            {
                Response.Redirect("Home.aspx");
            }

            string ownerAlias = theCake.getUserAlias(Int32.Parse(DT.Rows[0]["ownerID"].ToString()));
            if (theCake.getActiveUserName(IP) != ownerAlias)
            {
                int userID = theCake.getUserID(theCake.getActiveUserName(IP));
                DataTable permissionChecker = theCake.getUserProjectPermissions(userID, Int32.Parse(ID));
                if (permissionChecker.Rows[0]["permission_Project_Write"].ToString() == "0")
                    ProjectWrite = false;
                if (permissionChecker.Rows[0]["permission_Board_Write"].ToString() == "0")
                    BoardWrite = false;
            }

            if (!ProjectWrite)
            {
                pnl_EditOperations.Visible = false;
                pnl_SpecialOptions.Visible = false;
                //btn_AddMilestone.Visible = false;
            }
            if (!BoardWrite)
            {
                btn_AddComment.Visible = false;
            }

            lbl_TaskName.Text = DT.Rows[0]["taskName"].ToString();
            lbl_Description.Text = DT.Rows[0]["taskDescription"].ToString();
            lbl_projectOwner.Text = DT.Rows[0]["Display_Name"].ToString();
            if (!IsPostBack)
            {
                txt_Edit_TaskName.Text = DT.Rows[0]["taskName"].ToString();
                txt_Edit_TaskDescription.Text = DT.Rows[0]["taskDescription"].ToString();
            }
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
                    //pnl_Milestone.Visible = true;
                    FillMilestones();
                    //pnl_Boards.Visible = true;
                    getallBoards();
                }
            }
            else
            {
                if (Request.QueryString["feat"] != null)
                {
                    if (!IsPostBack)
                    {
                        Feature ftr = new Feature(int.Parse(Request.QueryString["feat"].ToString()));
                        lbl_remChildFeature_Name.Text = ftr.getFeatureName();
                        lbl_remChildFeature_Description.Text = ftr.getFeatureDescription();
                        lbl_remChildFeature_PercentComplete.Text = ftr.getPercentComplete().ToString() + "%";
                        btnRemFeature.CommandArgument = ftr.getID().ToString();

                        txt_editChildFeature_Name.Text = ftr.getFeatureName();
                        txt_editChildFeature_Description.Text = ftr.getFeatureDescription();
                        for (int i = 0; i <= 100; i++)
                        {
                            ddl_editChildFeature_PercentComplete.Items.Add(i.ToString());
                        }
                        for (int i = 1; i <= 5; i++)
                        {
                            ddl_editChildFeature_Weight.Items.Add(i.ToString());
                            ddl_addChildFeature_Weight.Items.Add(i.ToString());
                        }
                        ddl_editChildFeature_PercentComplete.SelectedIndex = ftr.getPercentComplete();
                        ddl_editChildFeature_Weight.SelectedIndex = ftr.getWeight() - 1;

                        if (ftr.hasChildren)
                        {
                            ddl_editChildFeature_PercentComplete.Enabled = false;
                        }
                        else
                        {
                            ddl_editChildFeature_PercentComplete.Enabled = true;
                        }

                        lbl_quickComplete_Name.Text = ftr.getFeatureName();
                        lbl_quickComplete_Description.Text = ftr.getFeatureDescription();
                        lbl_quickComplete_PercentComplete.Text = ftr.getPercentComplete().ToString();
                    }
                }

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
                    //pnl_Milestone.Visible = true;
                    FillMilestones();
                    //pnl_Boards.Visible = true;
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
    
    private string getFeatureLines(Feature ftr)
    {
        string retString = "";
        if (ftr.myChildren.Count > 0)
        {
            if (ftr.getPercentComplete() == 100)
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"green\"><i>" + ftr.getPercentComplete() + "%</i></font></td><td align=\"right\"><a class=\"google button blue\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#addChildFeature\" title=\"Add New Sub-Objective\">+</a><a class=\"google button\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#editChildFeature\" title=\"Edit Objective\">E</a></td></tr></table><ul><hr style=\"height:1px; margin:2px;\" />";
            }
            else
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"red\"><i>" + ftr.getPercentComplete() + "%</i></font></td><td align=\"right\"><a class=\"google button blue\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#addChildFeature\" title=\"Add New Sub-Objective\">+</a><a class=\"google button\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#editChildFeature\" title=\"Edit Objective\">E</a></td></tr></table><ul><hr style=\"height:1px; margin:2px;\" />";
            }
            foreach (Feature child in ftr.myChildren)
            {
                retString += getFeatureLines(child);
            }
            retString += "</ul></li>";
        }
        else
        {
            if (ftr.getPercentComplete() == 100)
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"green\"><i>" + ftr.getPercentComplete() + "%</i></font></td><td align=\"right\"><a class=\"google button blue\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#addChildFeature\" title=\"Add New Sub-Objective\">+</a><a class=\"google button\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#editChildFeature\" title=\"Edit Objective\">E</a><a class=\"google button red\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#removeChildFeature\" title=\"Remove Objective\">&#215;</a></li></td></tr></table><hr style=\"height:1px; margin:2px;\" />";
            }
            else
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"red\"><i>" + ftr.getPercentComplete() + "%</i></font></td><td align=\"right\"><a class=\"google button blue\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#addChildFeature\" title=\"Add New Sub-Objective\">+</a><a class=\"google button\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#editChildFeature\" title=\"Edit Objective\">E</a><a class=\"google button red\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#removeChildFeature\" title=\"Remove Objective\">&#215;</a><a class=\"google button green\" href=\"?ID=" + ID + "&feat=" + ftr.getID() + "#quickComplete\" title=\"Complete Objective\">&#x2713;</a></li></td></tr></table><hr style=\"height:1px; margin:2px;\" />";
            }
        }
        return retString;
    }

    public void FillMilestones()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getMilestones(theCake.getActiveUserName(IP), Int32.Parse(Request.QueryString["ID"].ToString()));
        int projectID = int.Parse(Request.QueryString["ID"].ToString());

        string headerControl = "";
        headerControl += "<div class=\"container-fluid\"><div id=\"three_summaries\" class=\"row-fluid\">";
        lit_Milestones.Text = headerControl;

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                Feature headFeature = new Feature(int.Parse(DR["ID"].ToString()));

                string milestoneControl = "";
                milestoneControl += "<div class=\"span6\"><div class=\"widget\"><div style=\"font-size:1.2em; font-family: 'Kite One', sans-serif;\">" +
                    headFeature.getFeatureName() + "</div><progress value=\"" +
                    ((decimal)(headFeature.getPercentComplete()) / 100) + "\"></progress>";

                milestoneControl += getFeatureLines(headFeature);

                milestoneControl += "</div></div>";

                lit_Milestones.Text += milestoneControl;
            }
        }
        else
        {
            lit_Milestones.Text = "No Milestones set up yet!";
        }

        string footerControl = "";
        footerControl += "</div></div>";
        lit_Milestones.Text += footerControl;

        progress_Header.Text = "<progress value=\"" + ((decimal)(theCake.getProjectPercentComplete(projectID)))/100 +"\" />";
    }

    protected void btnDone_OnClick(object sender, EventArgs e)
    {
        theCake.completeFeature(Int32.Parse(((Button)sender).CommandArgument));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btnRem_OnClick(object sender, EventArgs e)
    {
        Feature ftr = new Feature(Int32.Parse(((Button)sender).CommandArgument));
        ftr.deleteFeature();
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
        //pnl_AddMilestone.Visible = true;
        //btn_AddMilestone.Visible = false;
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
        //pnl_Mainheader_Edit.Visible = true;
    }

    protected void btn_Update_NameDescription_OnClick(object sender, EventArgs e)
    {
        string NewProjectName = txt_Edit_TaskName.Text.Replace("\n", "<br/>");
        string NewProjectDescription = txt_Edit_TaskDescription.Text.Replace("\n", "<br/>");
        theCake.updateTaskBaseInfo(Int32.Parse(Request.QueryString["ID"].ToString()), NewProjectName, NewProjectDescription);
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_Edit_Sharing_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PSharing.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_UpdateMilestone_OnClick(object sender, EventArgs e)
    {
        Feature ftr = new Feature(Int32.Parse(((Button)sender).CommandArgument));
        ftr.updateFeatureDetails(txt_Milestone_Name.Text, txt_Milestone_Desc.Text, int.Parse(ddl_Milestone_Weight.SelectedValue), 0);
        //theCake.updateMilestone(Int32.Parse(((Button)sender).CommandArgument), txt_Milestone_Name.Text, txt_Milestone_Desc.Text, Int32.Parse(ddl_Milestone_Weight.SelectedItem.Text));
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_gotoUpdateMilestone(object sender, EventArgs e)
    {
        int featureID = Int32.Parse(((LinkButton)sender).CommandArgument);
        //btn_UpdateMilestone.CommandArgument = featureID.ToString();
        Feature ftr = new Feature(featureID);
        txt_Milestone_Desc.Text = ftr.getFeatureDescription();
        txt_Milestone_Name.Text = ftr.getFeatureName();
        ddl_Milestone_Weight.SelectedValue = ftr.getWeight().ToString();

        //btn_UpdateMilestone.Visible = true;
        //btn_AddMilestone.Visible = false;
        btn_AddMilestone_Final.Visible = false;
        //pnl_AddMilestone.Visible = true;
        
    }

    protected void btn_EditChildFeature_OnClick(object sender, EventArgs e)
    {
        int featureID = int.Parse(Request.QueryString["feat"].ToString());
        string name = txt_editChildFeature_Name.Text;
        string description = txt_editChildFeature_Description.Text;
        int weight = int.Parse(ddl_editChildFeature_Weight.SelectedItem.Value);
        int percentComplete = int.Parse(ddl_editChildFeature_PercentComplete.SelectedItem.Value);

        Feature ftr = new Feature(featureID);
        if (ddl_editChildFeature_PercentComplete.Enabled)
        {
            ftr.updateFeatureDetails(name, description, weight, percentComplete);
        }
        else
        {
            ftr.updateFeatureDetails(name, description, weight);
        }
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    protected void btn_addChildFeature_OnClick(object sender, EventArgs e)
    {
        int featureID = int.Parse(Request.QueryString["feat"].ToString());
        int projectID = int.Parse(Request.QueryString["ID"].ToString());
        Feature.addNewFeature(projectID, featureID, txt_addChildFeature_Name.Text, txt_addChildFeature_Description.Text, 0, -1);
        Response.Redirect("ViewTask.aspx?ID=" + Request.QueryString["ID"].ToString());
    }

    private void updateProjectPercent()
    {
        int projectID = int.Parse(Request.QueryString["ID"].ToString());
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

        DataTable DT = theCake.getMilestones(theCake.getActiveUserName(IP), Int32.Parse(Request.QueryString["ID"].ToString()));
        decimal total = 0;
        decimal counter = 0;
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                Feature ftr = new Feature(int.Parse(DR["ID"].ToString()));
                counter++;
                total += ftr.getPercentComplete();
            }
            theCake.updateProjectPercentComplete((int)(total / counter), projectID);
        }
        else
        {
            theCake.updateProjectPercentComplete(0, projectID);
        }
    }

    protected void btnQuickComplete_OnClick(object sender, EventArgs e)
    {
        string projectID = Request.QueryString["ID"].ToString();
        int featureID = int.Parse(Request.QueryString["feat"].ToString());
        Feature ftr = new Feature(featureID);
        ftr.updatePercentComplete(100);

        Response.Redirect("ViewTask.aspx?ID=" + projectID);
    }
}