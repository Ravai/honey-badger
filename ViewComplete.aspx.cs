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

public partial class ViewComplete : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;
        fillCompletedTable();
    }

    protected void fillCompletedTable()
    {
        DataTable DT = theCake.getCompletedTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                TableRow TR = new TableRow();
                TableCell TC1 = new TableCell();
                TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
                TableCell TC2 = new TableCell();
                TC2.Text = "<u>Date Started</u><br /><i><font color=red>" + DateTime.Parse(DR["actualStart"].ToString()).Date + "</font></i>";
                TableCell TC3 = new TableCell();
                TC3.Text = "<u>Date Completed</u><br /><i><font color=red>" + DateTime.Parse(DR["actualStop"].ToString()).Date + "</font></i>";
                TC1.Width = Unit.Percentage(30);
                TC2.Width = Unit.Percentage(20);
                TC3.Width = Unit.Percentage(50);
                TR.Cells.Add(TC1);
                TR.Cells.Add(TC2);
                TR.Cells.Add(TC3);
                tbl_CompletedTaskList.Rows.Add(TR);
            }
        }
        else
        {
            TableRow TR = new TableRow();
            TableCell TC1 = new TableCell();
            TC1.Text = "You have no completed tasks.";
            TR.Cells.Add(TC1);
            tbl_CompletedTaskList.Rows.Add(TR);
        }
    }
}