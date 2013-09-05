using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Projects : System.Web.UI.Page
{
    DataBase theCake = new DataBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        WriteLastUpdated();
        fillWipTables();
        fillReadyTables();
    }

    private void WriteLastUpdated()
    {
        DateTime dt;

        try
        {
            string filePath = MapPathSecure(Page.Request.CurrentExecutionFilePath);
            dt = new FileInfo(filePath).LastWriteTime;
        }
        catch
        { // "design-time" or current path doesn't resolve to a file
            dt = DateTime.Now;
        }
        templateLastUpdated.Text = "<p>Last updated: " + dt.ToString() + "</p>";
    }

    private void fillWipTables()
    {
        DataTable DT = theCake.getWipTasks(theCake.getActiveUserName(Request.UserHostAddress));
        string TableString = "";

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                Literal sectionControl = new Literal();
                float percentComplete = float.Parse(DR["Percent_Completed"].ToString()) / 100;
                TableString += "<section><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\"><h3>" + DR["taskName"].ToString() + "</h3><p>Progress: <progress value=\"" + percentComplete + "\" /></p></a></section>";
                sectionControl.Text = TableString;
                pnl_WipTasks.Controls.Add(sectionControl);
            }
        }
        else
        {
            Literal sectionControl = new Literal();
            TableString += "No projects available";
            sectionControl.Text = TableString;
            pnl_WipTasks.Controls.Add(sectionControl);
        }
    }

    private void fillReadyTables()
    {
        DataTable DT = theCake.getReadyTasks(theCake.getActiveUserName(Request.UserHostAddress));

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                string TableString = "";
                Literal sectionControl = new Literal();
                float percentComplete = float.Parse(DR["Percent_Completed"].ToString()) / 100;
                TableString += "<section><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\"><h3>" + DR["taskName"].ToString() + "</h3><p>Progress: <progress value=\"" + percentComplete + "\" /></p></a></section>";
                sectionControl.Text = TableString;
                pnl_ReadyTasks.Controls.Add(sectionControl);
            }
        }
        else
        {
            string TableString = "";
            Literal sectionControl = new Literal();
            TableString += "No projects available";
            sectionControl.Text = TableString;
            pnl_ReadyTasks.Controls.Add(sectionControl);
        }
    }
}