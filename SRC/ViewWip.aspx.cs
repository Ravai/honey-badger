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

public partial class ViewWip : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;
        fillWipTable();
    }

    //protected void fillWipTable()
    //{
    //    string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
    //    DataTable DT = theCake.getWipTasks(theCake.getActiveUserName(IP));

    //    if (DT.Rows.Count > 0)
    //    {
    //        foreach (DataRow DR in DT.Rows)
    //        {

    //            TableRow TR = new TableRow();
    //            TableCell TC1 = new TableCell();
    //            TC1.Text = "<strong><u><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a></u></strong><br />" + DR["taskDescription"].ToString();
    //            TableCell TC2 = new TableCell();
    //            TC2.Text = "<u>Started On</u><br /><i><font color=red>" + DateTime.Parse(DR["actualStart"].ToString()).Date + "</font></i>";
    //            TableCell TC3 = new TableCell();
    //            TC3.Text = "<u>Expected Stop</u><br /><i><font color=red>" + DateTime.Parse(DR["expectedStop"].ToString()).Date + "</font></i>";
    //            TC1.Width = Unit.Percentage(30);
    //            TC2.Width = Unit.Percentage(20);
    //            TC3.Width = Unit.Percentage(50);
    //            TR.Cells.Add(TC1);
    //            TR.Cells.Add(TC2);
    //            TR.Cells.Add(TC3);
    //            tbl_WipTaskList.Rows.Add(TR);
    //        }
    //    }
    //    else
    //    {
    //        TableRow TR = new TableRow();
    //        TableCell TC1 = new TableCell();
    //        TC1.Text = "You have no WIP tasks.";
    //        TR.Cells.Add(TC1);
    //        tbl_WipTaskList.Rows.Add(TR);
    //    }
    //}

    protected void fillWipTable()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getWipTasks(theCake.getActiveUserName(IP));
        DataTable DT2 = theCake.getSharedWipTasks(theCake.getActiveUserName(IP));

        ProgressList.Text += "<ul class=\"cards\">";
        if (DT.Rows.Count > 0 || DT2.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                ProgressList.Text += "<li><p class=\"title\"><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a>" +
                    "<p>" + DR["taskDescription"].ToString() + "</p>" + 
                    "<p><progress value=\"" + ((decimal)(int.Parse(DR["Percent_Completed"].ToString()))) / 100 + "\" ></progress></p>" + "</li>";
            }
            foreach (DataRow DR in DT2.Rows)
            {
                ProgressList.Text += "<li><p class=\"title\"><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\"><strong>[SHARED]</strong> " + DR["taskName"].ToString() + "</a>" +
                    "<p>" + DR["taskDescription"].ToString() + "</p>" + 
                    "<p><progress value=\"" + ((decimal)(int.Parse(DR["Percent_Completed"].ToString()))) / 100 + "\" ></progress></p>" + "</li>";
            }
            //lit_totInProgress.Text = (DT.Rows.Count + DT2.Rows.Count).ToString();
        }
        else
        {
            ProgressList.Text += "You have no tasks in Progress";
            //lit_totInProgress.Text = "0";
        }
        ProgressList.Text += "</ul";

    }
}