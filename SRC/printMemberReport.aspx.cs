using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class printMemberReport : System.Web.UI.Page
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

    public void generateReport()
    {
        DataTable DT = currentProject.getProjectPermissions();

        lit_ProjectSum.Text = "Total Members on Project:  " + DT.Rows.Count;

        lit_ProjectDetails.Text = "<ul>";

        foreach (DataRow DR in DT.Rows)
        {
            userClass usr = new userClass(int.Parse(DR["user_GivenTo"].ToString()));

            lit_ProjectDetails.Text += "<li>" + usr.getDisplayName() + " - " + DR["projectTitle"].ToString() + "</li>";
        }

        lit_ProjectDetails.Text += "</ul>";
    }
}