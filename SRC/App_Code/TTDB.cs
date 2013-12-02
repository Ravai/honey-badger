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
            return new DataTable();
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
        client.Send(msg);
        return;
    }





    public static bool checkMaintenance()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [Value] FROM [TrackingTool_Flags] WHERE [Type] = 'Maintenance'";
        cmd.Parameters.Clear();

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT.Rows[0]["Value"].ToString() == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static int addNewPermission(int projectID, string newUserAlias, string GivingUserAlias, int PR, int PW, int BR, int BW, string role)
    {
        int newID = userClass.getUserID(newUserAlias);
        if (newID == -1)
        {
            newID = userClass.addNewUser(newUserAlias);
        }


        int giverID = userClass.getUserID(GivingUserAlias);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_ProjectPermissions] VALUES(@projectID, @projectTitle, @projectTitleDescription, @userGivenTo, @userGivenBy, @PR, @PW, @BR, @BW, CURRENT_TIMESTAMP, NULL, 0)";
        cmd.Parameters.AddWithValue("@projectID", projectID);
        cmd.Parameters.AddWithValue("@projectTitle", role);
        cmd.Parameters.AddWithValue("@projectTitleDescription", role);
        cmd.Parameters.AddWithValue("@userGivenTo", newID);
        cmd.Parameters.AddWithValue("@userGivenBy", giverID);
        cmd.Parameters.AddWithValue("@PR", PR);
        cmd.Parameters.AddWithValue("@PW", PW);
        cmd.Parameters.AddWithValue("@BR", BR);
        cmd.Parameters.AddWithValue("@BW", BW);
        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        if (DT.Rows.Count > 0)
        {
            return Int32.Parse(DT.Rows[0]["ID"].ToString());
        }
        else
        {
            return -1;
        }
    }


}