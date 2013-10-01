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

public partial class MyReport : System.Web.UI.Page
{
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            calendar_date1.SelectedDate = DateTime.Today;
            calendar_date2.SelectedDate = DateTime.Today;
        }
    }

    protected void fillTable(DateTime startDate, DateTime endDate)
    {
        DateTime StartofMonth = startDate;
        DateTime EndofMonth = (endDate.AddDays(1)).AddSeconds(-1);
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getComments(theCake.getActiveUserName(IP), StartofMonth, EndofMonth);

        if (DT.Rows.Count > 0)
        {
            bool newTask = true;
            int curTaskID = 0;
            foreach (DataRow DR in DT.Rows)
            {
                if (curTaskID.ToString() != DR["taskID"].ToString())
                {
                    newTask = true;
                }

                if (newTask)
                {
                    TableRow TR = new TableRow();
                    TableCell TC1 = new TableCell();
                    TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["taskID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u><br />" + DR["taskDescription"].ToString() + "</strong>";
                    TR.Cells.Add(TC1);
                    tbl_CurrentMonth.Rows.Add(TR);
                    newTask = false;
                    curTaskID = Int32.Parse(DR["taskID"].ToString());
                }

                TableRow TR1 = new TableRow();
                TableCell TC2 = new TableCell();
                TC2.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- " + DR["comment"].ToString() + "<br /><i><font color=red>posted on " + DR["createdTimestamp"].ToString() + "</font></i>";
                TR1.Cells.Add(TC2);
                tbl_CurrentMonth.Rows.Add(TR1);
                newTask = false;
                curTaskID = Int32.Parse(DR["taskID"].ToString());
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no tasks for this month.";
            TR.Cells.Add(TC1);
            tbl_CurrentMonth.Rows.Add(TR);
        }
    }

    protected void btn_ViewReport_OnClick(object sender, EventArgs e)
    {
        DateTime StartofMonth = calendar_date1.SelectedDate;
        DateTime EndofMonth = (calendar_date2.SelectedDate.AddDays(1)).AddSeconds(-1);
        date1.Text = StartofMonth.ToString();
        date2.Text = EndofMonth.ToString();
        fillTable(StartofMonth, EndofMonth);

        mv_Reports.ActiveViewIndex = 1;
    }
}