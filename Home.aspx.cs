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

public partial class Home : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    int sharedTaskTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;
        fillWipTable();
        fillReadyTable();
        fillCompletedTable();
        fillUpcomingTable();
        getMessages();
        if (sharedTaskTotal == 0) pnl_SharedTasks.Visible = false;

        if (!IsPostBack)
        {
            Calendar_ExpectedStart.SelectedDate = DateTime.Today;
            Calendar_ExpectedStop.SelectedDate = DateTime.Today;
        }
    }

    private void getMessages()
    {
        DataTable DT = theCake.CheckNewAcknowledgements(Request.UserHostAddress);
        if (DT.Rows.Count > 0)
        {
            pnl_Messages.Visible = true;

            foreach (DataRow DR in DT.Rows)
            {
                DataTable DT2 = theCake.getTask(Int32.Parse(DR["projectID"].ToString()));
                TableRow TR = new TableRow();
                TableCell TC = new TableCell();
                Label L1 = new Label();
                L1.Text = "A project has been shared with you!  Check out ";
                LinkButton newLink = new LinkButton();
                newLink.Text = DT2.Rows[0]["taskName"].ToString();
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

            theCake.addNewTask(txt_addNew_Task_Name.Text, Description, Calendar_ExpectedStart.SelectedDate, Calendar_ExpectedStop.SelectedDate, theCake.getActiveUserName(Request.UserHostAddress));
            Response.Redirect("Home.aspx");
        }
    }

    protected void fillWipTable()
    {
        DataTable DT = theCake.getWipTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoWip.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;
                
                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_WipTaskList.Rows.Add(TR);
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no WIP tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_WipTaskList.Rows.Add(TR);
        }

        DT = theCake.getSharedWipTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoWip.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_Shared_WIP.Rows.Add(TR);
                sharedTaskTotal++;
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no WIP tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_Shared_WIP.Rows.Add(TR);
        }
    }

    protected void fillReadyTable()
    {
        DataTable DT = theCake.getReadyTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoReady.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_ReadyTaskList.Rows.Add(TR);
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no Ready tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_ReadyTaskList.Rows.Add(TR);
        }

        DT = theCake.getSharedReadyTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoReady.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_Shared_Ready.Rows.Add(TR);
                sharedTaskTotal++;
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no Ready tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_Shared_Ready.Rows.Add(TR);
        }
    }

    protected void fillCompletedTable()
    {
        DataTable DT = theCake.getCompletedTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoComplete.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_CompletedTaskList.Rows.Add(TR);
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no completed tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_CompletedTaskList.Rows.Add(TR);
        }

        DT = theCake.getSharedCompletedTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoComplete.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_Shared_Completed.Rows.Add(TR);
                sharedTaskTotal++;
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no completed tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_Shared_Completed.Rows.Add(TR);
        }
    }

    protected void fillUpcomingTable()
    {
        DataTable DT = theCake.getUpcomingTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoUpcoming.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_UpcomingTaskList.Rows.Add(TR);
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no upcoming tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_UpcomingTaskList.Rows.Add(TR);
        }

        DT = theCake.getSharedUpcomingTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            if (DT.Rows.Count > 3) btn_gotoUpcoming.Visible = true;

            int counter = 0;
            foreach (DataRow DR in DT.Rows)
            {
                // Only showing top 3 if there are more than three
                if (counter == 3) break;
                counter++;

                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TC1.VerticalAlign = VerticalAlign.Top;
                TR.Cells.Add(TC1);
                tbl_Shared_Upcoming.Rows.Add(TR);
                sharedTaskTotal++;
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no upcoming tasks.";
            TC1.VerticalAlign = VerticalAlign.Top;
            TR.Cells.Add(TC1);
            tbl_Shared_Upcoming.Rows.Add(TR);
        }
    }

    protected void lnkbtn_ShowAddTask_OnClick(object sender, EventArgs e)
    {
        pnl_AddTask1.Visible = false;
        pnl_AddTask2.Visible = true;
    }

    protected void btn_CancelNewTask_OnClick(object sender, EventArgs e)
    {
        pnl_AddTask1.Visible = true;
        pnl_AddTask2.Visible = false;
    }
}