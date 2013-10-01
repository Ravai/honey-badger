using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Base Class for Accessing and Setting data
/// 
/// TT DB String:  ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString
/// CDIS String:    ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString
/// </summary>
public static class TTDB
{
    private static SqlConnection conn;
    private static SqlDataReader SQLR;

    // ==================================================================================================
    // ==================================================================================================
    //                                          BASE FUNCTIONS                                           
    // ==================================================================================================
    // ==================================================================================================

    public static DataTable TTQuery(SqlCommand cmd)
    {
        string connstring = ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString;
        return Query(cmd, connstring);
    }

    /// <summary>
    /// Base Query function.
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="connstring"></param>
    /// <returns></returns>
    private static DataTable Query(SqlCommand cmd, string connstring)
    {
        try
        {
            conn = new SqlConnection(connstring);
            conn.Open();
            cmd.Connection = conn;
            SQLR = cmd.ExecuteReader();
            DataSet DS = new DataSet();

            DataTable DT = new DataTable();
            DT.Load(SQLR);

            SQLR.Close();
            cmd.Dispose();
            conn.Close();

            return DT;
        }
        catch (SqlException S)
        {
            mailException(S);
            return null;
        }
    }

    /// <summary>
    /// Exception mailer
    /// </summary>
    /// <param name="ex"></param>
    private static void mailException(Exception ex)
    {
        MailMessage msg = new MailMessage();
        msg.IsBodyHtml = true;
        msg.From = new MailAddress("Exception@noreply.com");
        msg.To.Add(new MailAddress("john.couch@intel.com"));
        msg.Subject = "Tracking Tool - Exception";
        msg.Body = "<br /><br /><b>Shorthand:</b> " + ex.Message + "<br /><br /><b>Trace:</b>  " + ex.StackTrace + "<br /><br />" + ex.InnerException;
        SmtpClient client = new SmtpClient();
        //client.Send(msg);
        return;
    }

    public static void mailTo(string email, string Subject, string Body)
    {
        MailMessage msg = new MailMessage();
        msg.IsBodyHtml = true;
        msg.From = new MailAddress("Exception@noreply.com");
        msg.To.Add(new MailAddress(email));
        msg.Subject = Subject;
        msg.Body = Body;
        SmtpClient client = new SmtpClient();
        //client.Send(msg);
        return;
    }
}