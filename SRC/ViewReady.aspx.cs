﻿using System;
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

public partial class ViewReady : System.Web.UI.Page
{
    DataBase theCake = new DataBase();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        MaintainScrollPositionOnPostBack = true;
        fillReadyTable();
    }

    protected void fillReadyTable()
    {
        string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
        DataTable DT = theCake.getReadyTasks(theCake.getActiveUserName(IP));
        DataTable DT2 = theCake.getSharedReadyTasks(theCake.getActiveUserName(IP));

        if (DT.Rows.Count > 0)
        {
            foreach (DataRow DR in DT.Rows)
            {
                ReadyList.Text += "<li>" + "<p class=\"title\"><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\">" + DR["taskName"].ToString() + "</a>" +
                    "<p class=\"info\">" + DR["taskDescription"].ToString() + "</p></li>";
            }
            foreach (DataRow DR in DT2.Rows)
            {
                ReadyList.Text += "<li>" + "<p class=\"title\"><a href=\"ViewTask.aspx?ID=" + DR["ID"].ToString() + "\"><strong>[SHARED]</strong>" + DR["taskName"].ToString() + "</a>" +
                    "<p class=\"info\">" + DR["taskDescription"].ToString() + "</p></li>";
            }
            //lit_totReady.Text = (DT.Rows.Count + DT2.Rows.Count).ToString();
        }
        else
        {
            ReadyList.Text += "You have no tasks Ready";
            //lit_totReady.Text = "0";
        }

    }
}