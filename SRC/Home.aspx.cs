using System;
using System.Collections;
using System.Collections.Generic;
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

public partial class Home : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    int sharedTaskTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;

        if (!IsPostBack)
        {
            Calendar_ExpectedStart.SelectedDate = DateTime.Today;
            Calendar_ExpectedStop.SelectedDate = DateTime.Today;
            fillWipTable();
            fillReadyTable();
            //fillCompletedTable();
            fillUpcomingTable();
            

        }
        getMessages();
    }

    private void getMessages()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.CheckNewAcknowledgements(IP);
        if (DT.Rows.Count > 0)
        {
            pnl_Messages.Visible = true;

            foreach (DataRow DR in DT.Rows)
            {
                Project aProj = new Project(int.Parse(DR["projectID"].ToString()));
                //DataTable DT2 = theCake.getTask(Int32.Parse(DR["projectID"].ToString()));
                TableRow TR = new TableRow();
                TableCell TC = new TableCell();
                Label L1 = new Label();
                L1.Text = "A project has been shared with you!  Check out ";
                LinkButton newLink = new LinkButton();
                newLink.Text = aProj.getTaskName();
                //newLink.Text = DT2.Rows[0]["taskName"].ToString();
                newLink.Click += new EventHandler(btn_AcknowledgeMessage);
                newLink.CommandArgument = DR["ID"].ToString();
                newLink.CommandName = DR["projectID"].ToString();
                Label L2 = new Label();
                L2.Text = "!";
                TC.Controls.Add(L1);
                TC.Controls.Add(newLink);
                TC.Controls.Add(L2);
                TR.Cells.Add(TC);
                tbl_Messages.Rows.Add(TR);
            }
        }
    }

    protected void btn_AcknowledgeMessage(object sender, EventArgs e)
    {
        theCake.acknowledgeMessage(Int32.Parse(((LinkButton)sender).CommandArgument));
        Response.Redirect("ViewTask.aspx?ID=" + ((LinkButton)sender).CommandName);
    }

    protected void btn_AddNewTask_OnClick(object sender, EventArgs e)
    {
        if (txt_addNew_Task_Description.Text == "" || txt_addNew_Task_Name.Text == "")
        {
            lbl_Error.Text = "Please fill in all the fields.";
            lbl_Error.Visible = true;

            if (txt_addNew_Task_Description.Text == "") lbl_DescError.Visible = true;
            else lbl_DescError.Visible = false;
            if (txt_addNew_Task_Name.Text == "") lbl_NameError.Visible = true;
            else lbl_NameError.Visible = false;
        }
        else
        {
            string Description = txt_addNew_Task_Description.Text;
            Description = Description.Replace("\r\n", "<br />");

            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

            
            int projectID = Project.addNewTask(txt_addNew_Task_Name.Text, Description, Calendar_ExpectedStart.SelectedDate, Calendar_ExpectedStop.SelectedDate, theCake.getActiveUserName(IP));
            theCake.increaseProjectSize(projectID, theCake.getUserID(theCake.getActiveUserName(IP)));
            Response.Redirect("Home.aspx");
        }
    }

    protected void fillWipTable()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

        List<Project> myWipTasks = Project.getWipTasks(theCake.getActiveUserName(IP));
        List<Project> mySharedWipTasks = Project.getSharedWipTasks(theCake.getUserID(theCake.getActiveUserName(IP)));

        ProgressList.Text += "<ul class=\"cards\">";
        if ((myWipTasks.Count + mySharedWipTasks.Count) > 0)
        {
            foreach (Project proj in myWipTasks)
            {
                ProgressList.Text += "<li><p class=\"title\"><a href=\"ViewTask.aspx?ID=" + proj.getID() + "\">" + proj.getTaskName() +"</a>" +
                    "<p><progress value=\"" + ((decimal)(proj.getPercentComplete())) / 100 + "\" ></progress></p>" + "</li>";
            }
            foreach (Project proj in mySharedWipTasks)
            {
                ProgressList.Text += "<li><p class=\"title\"><a href=\"ViewTask.aspx?ID=" + proj.getID() + "\"><strong>[SHARED]</strong> " + proj.getTaskName() + "</a>" +
                    "<p><progress value=\"" + ((decimal)(proj.getPercentComplete())) / 100 + "\" ></progress></p>" + "</li>";
            }
            lit_totInProgress.Text = (myWipTasks.Count + mySharedWipTasks.Count).ToString();
        }
        else
        {
            ProgressList.Text += "You have no tasks in Progress";
            lit_totInProgress.Text = "0";
        }
        ProgressList.Text += "</ul";

    }

    protected void fillReadyTable()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];

        List<Project> myReadyTasks = Project.getReadyTasks(theCake.getActiveUserName(IP));
        List<Project> mySharedReadyTasks = Project.getSharedReadyTasks(theCake.getUserID(theCake.getActiveUserName(IP)));

        if ((myReadyTasks.Count + mySharedReadyTasks.Count) > 0)
        {
            foreach (Project proj in myReadyTasks)
            {
                ReadyList.Text += "<li><p class=\"pull-right\"><progress value=\"" + ((decimal)(proj.getPercentComplete())) / 100 + "\" /></p>" +
                    "<p class=\"title\"><a href=\"ViewTask.aspx?ID=" + proj.getID() + "\">" + proj.getTaskName() + "</a>" +
                    "<p class=\"info\">" + proj.getTaskDescription() + "</p></li>";
            }
            foreach (Project proj in mySharedReadyTasks)
            {
                ReadyList.Text += "<li><p class=\"title\"><a href=\"ViewTask.aspx?ID=" + proj.getID() + "\"><strong>[SHARED]</strong> " + proj.getTaskName() + "</a>" +
                    "<p><progress value=\"" + ((decimal)(proj.getPercentComplete())) / 100 + "\" ></progress></p>" + "</li>";
            }
            lit_totReady.Text = (myReadyTasks.Count + mySharedReadyTasks.Count).ToString();
        }
        else
        {
            ReadyList.Text += "You have no tasks Ready";
            lit_totReady.Text = "0";
        }

    }


    protected void fillUpcomingTable()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        List<Project> myUpcomingTasks = Project.getUpcomingTasks(theCake.getActiveUserName(IP));
        List<Project> mySharedUpcomingTasks = Project.getSharedUpcomingTasks(theCake.getUserID(theCake.getActiveUserName(IP)));

        if ((myUpcomingTasks.Count + mySharedUpcomingTasks.Count) > 0)
        {
            foreach (Project proj in myUpcomingTasks)
            {
                ReadyList.Text += "<li><p class=\"pull-right\"><progress value=\"" + ((decimal)(proj.getPercentComplete())) / 100 + "\" /></p>" +
                    "<p class=\"title\"><a href=\"ViewTask.aspx?ID=" + proj.getID() + "\">" + proj.getTaskName() + "</a>" +
                    "<p class=\"info\">" + proj.getTaskDescription() + "</p></li>";
            }
            foreach (Project proj in mySharedUpcomingTasks)
            {
                ReadyList.Text += "<li><p class=\"title\"><a href=\"ViewTask.aspx?ID=" + proj.getID() + "\"><strong>[SHARED]</strong> " + proj.getTaskName() + "</a>" +
                    "<p><progress value=\"" + ((decimal)(proj.getPercentComplete())) / 100 + "\" ></progress></p>" + "</li>";
            }
            lit_totReady.Text = (myUpcomingTasks.Count + mySharedUpcomingTasks.Count).ToString();
        }
        else
        {
            ReadyList.Text += "You have no tasks Ready";
            lit_totUpcoming.Text = "0";
        }

    }
}