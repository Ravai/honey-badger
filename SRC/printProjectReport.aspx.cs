using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class printProjectReport : System.Web.UI.Page
{
    Project currentProject;
    userClass currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] != null)
        {
            currentProject = new Project(int.Parse(Request.QueryString["ID"].ToString()));
            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            currentUser = new userClass(userClass.getUserIDByIP(IP));

            lbl_ProjectDescription.Text = currentProject.getTaskDescription();
            lbl_ProjectName.Text = currentProject.getTaskName();

            generateReport();
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    private string getFeatureLines(Feature ftr)
    {
        string retString = "";
        if (ftr.myChildren.Count > 0)
        {
            if (ftr.getPercentComplete() == 100)
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"green\"><i>" + ftr.getPercentComplete() + "%</i></font></td></tr></table><ul><hr style=\"height:1px; margin:2px;\" />";
            }
            else
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"red\"><i>" + ftr.getPercentComplete() + "%</i></font></td></tr></table><ul><hr style=\"height:1px; margin:2px;\" />";
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
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"green\"><i>" + ftr.getPercentComplete() + "%</i></font></td></tr></table><hr style=\"height:1px; margin:2px;\" />";
            }
            else
            {
                retString += "<li title=\"" + ftr.getFeatureDescription() + "\"><table width=\"100%\"><tr><td align=\"left\">" + ftr.getFeatureName() + "&nbsp;&nbsp;<font color=\"red\"><i>" + ftr.getPercentComplete() + "%</i></font></td></tr></table><hr style=\"height:1px; margin:2px;\" />";
            }
        }
        return retString;
    }

    public void generateReport()
    {
        List<int> myMilestones = currentProject.getMilestones();

        string headerControl = "";
        headerControl += "<div class=\"container-fluid\"><div id=\"three_summaries\" class=\"row-fluid\">";
        lit_ProjectDetails.Text = headerControl;

        if (myMilestones.Count > 0)
        {
            foreach (int milestoneID in myMilestones)
            {
                Feature headFeature = new Feature(milestoneID);

                string milestoneControl = "";
                milestoneControl += "<div class=\"span6\"><div class=\"widget\"><div style=\"font-size:1.2em; font-family: 'Kite One', sans-serif;\">" +
                    headFeature.getFeatureName() + "</div><progress value=\"" +
                    ((decimal)(headFeature.getPercentComplete()) / 100) + "\"></progress>";

                milestoneControl += getFeatureLines(headFeature);

                milestoneControl += "</div></div>";

                lit_ProjectDetails.Text += milestoneControl;
            }
        }
        else
        {
            lit_ProjectDetails.Text = "No Milestones set up yet!";
        }

        string footerControl = "";
        footerControl += "</div></div>";
        lit_ProjectDetails.Text += footerControl;

        lit_ProjectTotalComplete.Text = "Total Project Completion:  <strong>" + currentProject.getPercentComplete() + "%</strong>";
    }
}