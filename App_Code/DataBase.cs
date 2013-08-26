﻿using System;
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
public class DataBase
{
    private SqlConnection conn;
    private SqlDataReader SQLR;

    public DataBase()
    {

    }

    // ==================================================================================================
    // ==================================================================================================
    //                                             ACCESSORS                                             
    // ==================================================================================================
    // ==================================================================================================

    public string getActiveUserName(string IP)
    {
        DataTable DT = getActiveUserData(IP);
        if (DT.Rows.Count == 1)
            return DT.Rows[0]["ownerAlias"].ToString();
        else
            return "";
    }

    public  DataTable getActiveUserData(string IP)
    {
        //string IP = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null) ? HttpContext.Current.Request.UserHostAddress : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM viewTrackingTool_Active_Users WHERE [User_IP] = @IP";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@IP", IP);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        if (DT != null)
        {
            if (DT.Rows.Count == 1)
            {
                user_UpdateActive(IP, DT.Rows[0]["ID"].ToString());
                return DT;
            }
            else
                return new DataTable();
        }
        else
            return new DataTable();
    }

    private  void user_UpdateActive(string IP, string User)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE TrackingTool_Users_Active SET [Last_Active] = CURRENT_TIMESTAMP WHERE [User_IP] = @IP AND [uniqID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@IP", IP);
        cmd.Parameters.AddWithValue("@ID", User);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public  DataTable getAllActiveUserData()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM view_Active_Users WHERE [Last_Active] > DateADD(mi, -15, Current_TimeStamp)";
        cmd.Parameters.Clear();

        return Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public  void Logout_User(string IP)
    {
        DataTable DT = getActiveUserData(IP);

        if (DT.Rows.Count == 1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM TrackingTool_Users_Active WHERE [User_IP] = @IP";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@IP", DT.Rows[0]["User_IP"].ToString());
            Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        }
    }

    public bool Login_User(string UserName, string PW, string IP)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM TrackingTool_Users WHERE [ownerAlias] = @UserName AND [user_PW] = @PW";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@UserName", UserName);
        cmd.Parameters.AddWithValue("@PW", PW);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT.Rows.Count == 1)
        {
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM TrackingTool_Users_Active WHERE [uniqID] = @uniqID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@uniqID", DT.Rows[0]["ID"].ToString());

            Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

            cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO TrackingTool_Users_Active VALUES(@uniqID, @userIP, CURRENT_TIMESTAMP)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@uniqID", Int32.Parse(DT.Rows[0]["ID"].ToString()));
            cmd.Parameters.AddWithValue("@userIP", IP);

            Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            return true;
        }
        else
        {
            return false;
        }
    }

    public  bool Register_User(string UserName, string DisplayName, string PW, string IP)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM TrackingTool_Users WHERE [ownerAlias] = @UserName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@UserName", UserName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT.Rows.Count == 0)
        {
            //try
            //{
            cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO TrackingTool_Users VALUES(@UserName, @user_PW, @IP, @IP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @Active, @userLevel, @Email, @Preferred_Name, @DisplayName, @DisplayImage, @User_Status, @Footer)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@user_PW", PW);
            cmd.Parameters.AddWithValue("@IP", IP);
            cmd.Parameters.AddWithValue("@Active", 1);
            cmd.Parameters.AddWithValue("@userLevel", 0);
            cmd.Parameters.AddWithValue("@Email", "");
            cmd.Parameters.AddWithValue("@Preferred_Name", "");
            cmd.Parameters.AddWithValue("@DisplayName", DisplayName);
            cmd.Parameters.AddWithValue("@DisplayImage", "");
            cmd.Parameters.AddWithValue("@User_Status", "");
            cmd.Parameters.AddWithValue("@Footer", "");

            Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }
        else
        {
            return false;
        }
    }







































    public bool checkMaintenance()
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

    public DataTable getProjectPermissions(int projectID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [projectID] = @projectID ORDER BY [updatedTimestamp]";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@projectID", projectID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getUserProjectPermissions(int userID, int projectID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @userID AND [projectID] = @projectID ORDER BY [updatedTimestamp]";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@projectID", projectID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable CheckNewAcknowledgements(string IP)
    {
        DataTable DT = getActiveUserData(IP);
        if (DT.Rows.Count == 1)
        {
            int ID = Int32.Parse(DT.Rows[0]["ID"].ToString());

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @ID AND [userAcknowledged] = 0";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", ID);

            DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            if (DT == null)
            {
                DT = new DataTable();
            }
            return DT;
        }
        else
        {
            return new DataTable();
        }
    }

    public DataTable getTasks(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getCompletedTasks(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '1' ORDER BY [actualStop] DESC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getSharedCompletedTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                string inString = "(";
                bool first = true;
                foreach (DataRow DR in DT.Rows)
                {
                    if (!first) inString += ", ";
                    inString += "'" + DR["projectID"].ToString() + "'";
                    first = false;
                }
                inString += ")";

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] IN " + inString + " AND [doneFlag] = '1' ORDER BY [actualStop] ASC";
                cmd.Parameters.Clear();

                DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
                if (DT == null)
                {
                    DT = new DataTable();
                }
                return DT;
            }
            else
                return new DataTable();
        }
        else
            return new DataTable();
    }

    public DataTable getWipTasks(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '0' AND [expectedStart] < CURRENT_TIMESTAMP AND [actualStart] IS NOT NULL ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getSharedWipTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                string inString = "(";
                bool first = true;
                foreach (DataRow DR in DT.Rows)
                {
                    if (!first) inString += ", ";
                    inString += "'" + DR["projectID"].ToString() + "'";
                    first = false;
                }
                inString += ")";

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] IN " + inString + " AND [expectedStart] < CURRENT_TIMESTAMP AND [actualStart] IS NOT NULL ORDER BY [expectedStart] ASC";
                cmd.Parameters.Clear();

                DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
                if (DT == null)
                {
                    DT = new DataTable();
                }
                return DT;
            }
            else
                return new DataTable();
        }
        else
            return new DataTable();
    }

    public DataTable getReadyTasks(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '0' AND [expectedStart] < CURRENT_TIMESTAMP AND [actualStart] IS NULL ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getSharedReadyTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                string inString = "(";
                bool first = true;
                foreach (DataRow DR in DT.Rows)
                {
                    if (!first) inString += ", ";
                    inString += "'" + DR["projectID"].ToString() + "'";
                    first = false;
                }
                inString += ")";

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] IN " + inString + " AND [expectedStart] < CURRENT_TIMESTAMP AND [actualStart] IS NULL ORDER BY [expectedStart] ASC";
                cmd.Parameters.Clear();

                DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
                if (DT == null)
                {
                    DT = new DataTable();
                }
                return DT;
            }
            else
                return new DataTable();
        }
        else
            return new DataTable();
    }

    public DataTable getUpcomingTasks(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '0' AND [expectedStart] > CURRENT_TIMESTAMP ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getSharedUpcomingTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                string inString = "(";
                bool first = true;
                foreach (DataRow DR in DT.Rows)
                {
                    if (!first) inString += ", ";
                    inString += "'" + DR["projectID"].ToString() + "'";
                    first = false;
                }
                inString += ")";

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] IN " + inString + " AND [doneFlag] = '0' AND [expectedStart] > CURRENT_TIMESTAMP ORDER BY [expectedStart] ASC";
                cmd.Parameters.Clear();

                DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
                if (DT == null)
                {
                    DT = new DataTable();
                }
                return DT;
            }
            else
                return new DataTable();
        }
        else
            return new DataTable();
    }

    public DataTable getTask(int ID, string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] = @ID and [ownerAlias] = @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT.Rows.Count == 0)
        {
            int userID = getUserID(userName);

            cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [projectID] = @ID and [user_GivenTo] = @userID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@userID", userID);

            DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            if (DT.Rows.Count == 1)
            {
                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] = @ID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ID", ID);
                DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            }
        }
        return DT;
    }

    public DataTable getTask(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getComments(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Comments] WHERE [taskID] = @ID ORDER BY [createdTimestamp] DESC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getComments(string userName, DateTime startDate, DateTime endDate)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Comments] WHERE [ownerAlias] = @userName AND [createdTimestamp] BETWEEN @startDate AND @endDate ORDER BY [taskID], [createdTimestamp] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userName", userName);
        cmd.Parameters.AddWithValue("@startDate", startDate);
        cmd.Parameters.AddWithValue("@endDate", endDate);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getMilestones(string userName, int taskID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Milestones] WHERE [ownerAlias] = @userName AND [taskID] = @taskID ORDER BY [weight] DESC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userName", userName);
        cmd.Parameters.AddWithValue("@taskID", taskID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT.Rows.Count == 0)
        {
            int userID = getUserID(userName);

            cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [projectID] = @ID and [user_GivenTo] = @userID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", taskID);
            cmd.Parameters.AddWithValue("@userID", userID);

            DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            if (DT.Rows.Count == 1)
            {
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Milestones] WHERE [taskID] = @taskID ORDER BY [weight] DESC";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@taskID", taskID);
                DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
            }
        }
        return DT;
    }

    public DataTable updateMilestone(int milestoneID, int perComplete)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [viewTrackingTool_Milestones] SET [completed] = @perComplete, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @milestoneID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@perComplete", perComplete);
        cmd.Parameters.AddWithValue("@milestoneID", milestoneID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable updateMilestone(int milestoneID, string milestoneName, string milestoneDescription, int weight)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [viewTrackingTool_Milestones] SET [milestoneName] = @milestoneName, [milestoneDescription] = @milestoneDescription, [weight] = @weight WHERE [ID] = @milestoneID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@milestoneName", milestoneName);
        cmd.Parameters.AddWithValue("@milestoneDescription", milestoneDescription);
        cmd.Parameters.AddWithValue("@weight", weight);
        cmd.Parameters.AddWithValue("@milestoneID", milestoneID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getFeatures(int milestoneID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE [MilestoneID] = @milestoneID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@milestoneID", milestoneID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public int getMilestoneID_fromFeatureID(int featureID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [MilestoneID] FROM [TrackingTool_Features] WHERE [ID] = @featureID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featureID", featureID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            return -1;
        }
        else
        {
            return Int32.Parse(DT.Rows[0]["MilestoneID"].ToString());
        }
    }

    public DataTable getMilestone(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Milestones] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            return new DataTable();
        }
        else
        {
            return DT;
        }
    }

    public void removeComments(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [TrackingTool_Comments] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public int getUserID(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Users] WHERE [ownerAlias] = @userName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userName", userName);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null || DT.Rows.Count == 0)
        {
            return -1;
        }
        else
        {
            return Int32.Parse(DT.Rows[0]["ID"].ToString());
        }
    }

    public string getUserAlias(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Users] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null || DT.Rows.Count == 0)
        {
            return "";
        }
        else
        {
            return DT.Rows[0]["ownerAlias"].ToString();
        }
    }

    public DataTable getBoard(int boardID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Master] WHERE [boardID] = @boardID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", boardID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getBoards(int taskID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Master] WHERE [projectID] = @taskID ORDER BY [weight] ASC, [createdTimestamp] DESC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@taskID", taskID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getThread(int threadID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Threads] WHERE [threadID] = @threadID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@threadID", threadID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getThreads(int boardID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Threads] WHERE [boardID] = @boardID ORDER BY [createdTimestamp] DESC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", boardID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getPosts(int threadID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Posts] WHERE [threadID] = @threadID ORDER BY [createdTimestamp] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@threadID", threadID);

        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }


    /// <summary>
    /// Get Data From CDIS using an employee WWID
    /// </summary>
    /// <param name="WWID"></param>
    /// <returns>DataTable</returns>
    public DataTable getfromCDIS(string WWID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [WWID], [FirstName], [MiddleInitial], [LastName], [NickName], [BadgeType], [DomainAddress], [LongID], [ShortID], [Title], [MgrName], [MgrWWID], [ccMailPO] FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE [WWID] = @WWID";
        //cmd.CommandText = "SELECT * FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE WWID = @WWID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@WWID", WWID);

        return Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);
    }

    public DataTable getfromCDIS(string FirstName, string LastName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [WWID], [FirstName], [MiddleInitial], [LastName], [NickName], [BadgeType], [DomainAddress], [LongID], [ShortID], [Title], [MgrName], [MgrWWID], [ccMailPO] FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName";
        //cmd.CommandText = "SELECT * FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@FirstName", FirstName);
        cmd.Parameters.AddWithValue("@LastName", LastName);

        return Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);
    }

    public DataTable getfromCDIS(string FirstName, string LastName, string MI)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [WWID], [FirstName], [MiddleInitial], [LastName], [NickName], [BadgeType], [DomainAddress], [LongID], [ShortID], [Title], [MgrName], [MgrWWID], [ccMailPO] FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName AND MiddleInitial LIKE @MI";
        //cmd.CommandText = "SELECT * FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@FirstName", FirstName);
        cmd.Parameters.AddWithValue("@LastName", LastName);
        cmd.Parameters.AddWithValue("@MI", MI);

        return Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);
    }

    /// <summary>
    /// Get Data From CDIS using an employee email
    /// </summary>
    /// <param name="WWID"></param>
    /// <returns>DataTable</returns>
    public DataTable getfromCDISbyEmail(string email)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [WWID], [FirstName], [MiddleInitial], [LastName], [NickName], [BadgeType], [DomainAddress], [LongID], [ShortID], [Title], [MgrName], [MgrWWID] FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE [DomainAddress] = @email";
        //cmd.CommandText = "SELECT * FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE [DomainAddress] = @email";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@email", email);

        return Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);
    }

    /// <summary>
    /// Get the name of an employee based on WWID
    /// </summary>
    /// <param name="WWID"></param>
    /// <returns></returns>
    public DataTable getNamefromCDIS(string WWID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT [FirstName] + ' ' + [LastName] as Name FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE WWID = @WWID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@WWID", WWID);

        return Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);
    }

    /// <summary>
    /// Get a list of all employees underneath a particular manager, by username
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public DataTable getManagerEmployeesFromCDIS(string userName)
    {
        string[] tmp = userName.Split('\\');
        userName = tmp[tmp.Length - 1];

        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT [WWID] FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE [ShortID] LIKE @user";
        cmd.Parameters.AddWithValue("@user", userName);
        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);

        if (DT.Rows.Count > 0)
        {
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT [WWID], [FirstName], [MiddleInitial], [LastName], [NickName], [DomainAddress], [ShortID], [Title], [MgrName], [MgrWWID] FROM [CDIS].[dbo].[WorkerPublicExtended] WHERE [MgrWWID] = @MgrWWID and ([TermDate] is null OR [TermDate] > CURRENT_TIMESTAMP) and [BadgeType] = 'BB' ORDER BY [LastName]";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@MgrWWID", DT.Rows[0]["WWID"].ToString());

            return Query(cmd, ConfigurationManager.ConnectionStrings["CDISConnectionString"].ConnectionString);
        }
        else
        {
            return new DataTable();
        }
    }

    // ==================================================================================================
    // ==================================================================================================
    //                                             UPDATERS                                              
    // ==================================================================================================
    // ==================================================================================================

    public void increaseProjectSize(int ID, int userID)
    {
        DataTable DT = getTask(ID);
        int newBoardID = addNewBoard("General Discussion", userID, 0);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [projectSize] = '1', [boardID] = @boardID WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@boardID", newBoardID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        DT = getThreads(newBoardID);
        int threadID = Int32.Parse(DT.Rows[0]["threadID"].ToString());

        DT = getComments(ID);

        foreach (DataRow DR in DT.Rows)
        {
            addNewPost(threadID, DR["comment"].ToString(), userID, DR["createdTimestamp"].ToString());
        }
    }

    public void markTaskDone(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [doneFlag] = '1', [actualStop] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public void markTaskWip(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [doneFlag] = '0', [actualStop] = NULL WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public void startTask(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [actualStart] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public void updateTaskBaseInfo(int taskID, string Name, string Description)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [taskName] = @Name, [taskDescription] = @Description WHERE [ID] = @taskID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@taskID", taskID);
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@Description", Description);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);
    }

    public void completeFeature(int featureID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [Completed] = 1, [dateCompleted] = CURRENT_TIMESTAMP WHERE [ID] = @featureID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featureID", featureID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        UpdateMilestoneCompleteRate(getMilestoneID_fromFeatureID(featureID));
    }

    public void UpdateMilestoneCompleteRate(int milestoneID)
    {
        DataTable DT = getFeatures(milestoneID);

        decimal total = DT.Rows.Count;
        decimal acc = 0;
        foreach (DataRow DR in DT.Rows)
        {
            if (DR["Completed"].ToString() == "1")
                acc++;
        }

        updateMilestone(milestoneID, (int)((acc / total) * 100));
    }

    public void deleteFeature(int featureID)
    {
        int milestoneID = getMilestoneID_fromFeatureID(featureID);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [TrackingTool_Features] WHERE [ID] = @featureID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featureID", featureID);

        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        UpdateMilestoneCompleteRate(milestoneID);
    }

    public void acknowledgeMessage(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_ProjectPermissions] SET [userAcknowledged] = 1, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        TTDB.TTQuery(cmd);
    }


    // ==================================================================================================
    // ==================================================================================================
    //                                         INSERT FUNCTIONS                                          
    // ==================================================================================================
    // ==================================================================================================

    public int addNewTask(string taskName, string taskDescription, DateTime expectedStart, DateTime expectedStop, string userName)
    {
        int userID = getUserID(userName);
        if (userID == -1)
        {
            userID = addNewUser(userName);
        }


        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Projects] VALUES(@taskName, @taskDescription, @expectedStart, @expectedStop, NULL, NULL, 0, @ownerID, NULL, 0)";
        cmd.Parameters.AddWithValue("@taskName", taskName);
        cmd.Parameters.AddWithValue("@taskDescription", taskDescription);
        cmd.Parameters.AddWithValue("@expectedStart", expectedStart);
        cmd.Parameters.AddWithValue("@expectedStop", expectedStop);
        cmd.Parameters.AddWithValue("@ownerID", userID);
        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Projects] WHERE [taskName] = @taskName AND [taskDescription] = @taskDescription AND [expectedStart] = @expectedStart AND [expectedStop] = @expectedStop and [ownerID] = @ownerID";
        cmd.Parameters.AddWithValue("@taskName", taskName);
        cmd.Parameters.AddWithValue("@taskDescription", taskDescription);
        cmd.Parameters.AddWithValue("@expectedStart", expectedStart);
        cmd.Parameters.AddWithValue("@expectedStop", expectedStop);
        cmd.Parameters.AddWithValue("@ownerID", userID);
        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        int taskID = Int32.Parse(DT.Rows[0]["ID"].ToString());

        addNewPermission(taskID, userName, userName, 1, 1, 1, 1);

        if (DT.Rows.Count > 0)
        {
            return taskID;
        }
        else
        {
            return -1;
        }
    }

    public int addNewUser(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Users] VALUES(@userName)";
        cmd.Parameters.AddWithValue("@userName", userName);
        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT [ID] FROM [TrackingTool_Users] WHERE [ownerAlias] = @userName";
        cmd.Parameters.AddWithValue("@userName", userName);
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

    public int addNewComment(int taskID, string Comment)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Comments] VALUES(@taskID, @Comment, CURRENT_TIMESTAMP, NULL)";
        cmd.Parameters.AddWithValue("@taskID", taskID);
        cmd.Parameters.AddWithValue("@Comment", Comment);
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

    public int addNewMilestone(int taskID, string Name, string Description, int Weight, int boardID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Milestones] VALUES(@taskID, @MilestoneName, @MilestoneDesc, @Weight, @boardID, CURRENT_TIMESTAMP, NULL, 0, NULL)";
        cmd.Parameters.AddWithValue("@taskID", taskID);
        cmd.Parameters.AddWithValue("@MilestoneName", Name);
        cmd.Parameters.AddWithValue("@MilestoneDesc", Description);
        cmd.Parameters.AddWithValue("@Weight", Weight);
        cmd.Parameters.AddWithValue("@boardID", boardID);
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

    public int addNewFeature(int MilestoneID, string Feature)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Features] VALUES(@MilestoneID, @Feature, 0, NULL)";
        cmd.Parameters.AddWithValue("@MilestoneID", MilestoneID);
        cmd.Parameters.AddWithValue("@Feature", Feature);
        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE [MilestoneID] = @MilestoneID AND [Feature] = @Feature";
        cmd.Parameters.AddWithValue("@MilestoneID", MilestoneID);
        cmd.Parameters.AddWithValue("@Feature", Feature);
        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        int featureID = Int32.Parse(DT.Rows[0]["ID"].ToString());

        UpdateMilestoneCompleteRate(getMilestoneID_fromFeatureID(featureID));
        
        if (DT.Rows.Count > 0)
        {
            return featureID;
        }
        else
        {
            return -1;
        }
    }

    public int addNewBoard(string boardName, int userID, int importance)
    {
        // importance is 1 if its a milestone, 0 if its a general board.
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Board_Main] VALUES(@boardName, @importance, CURRENT_TIMESTAMP, @user)";
        cmd.Parameters.AddWithValue("@boardName", boardName);
        cmd.Parameters.AddWithValue("@importance", importance);
        cmd.Parameters.AddWithValue("@user", userID);
        Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT [boardID] FROM [TrackingTool_Board_Main] WHERE [board_CategoryName] = @boardName AND [createdBy] = @user ORDER BY [createdTimestamp] DESC";
        cmd.Parameters.AddWithValue("@boardName", boardName);
        cmd.Parameters.AddWithValue("@user", userID);
        DataTable DT = Query(cmd, ConfigurationManager.ConnectionStrings["TTConnectionString"].ConnectionString);

        int boardID = -1;
        if (DT.Rows.Count > 0)
        {
            boardID =  Int32.Parse(DT.Rows[0]["boardID"].ToString());
        }

        addNewThread(boardID, "General Discussion", "Generic Thread for the Board", userID);

        return boardID;
    }

    public int addNewThread(int boardID, string threadName, string threadDescription, int userID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Board_Threads] VALUES(@boardID, @threadName, @threadDescription, CURRENT_TIMESTAMP, @user)";
        cmd.Parameters.AddWithValue("@boardID", boardID);
        cmd.Parameters.AddWithValue("@threadName", threadName);
        cmd.Parameters.AddWithValue("@threadDescription", threadDescription);
        cmd.Parameters.AddWithValue("@user", userID);
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

    public int addNewPost(int threadID, string post_Full, int userID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Board_Posts] VALUES(@threadID, @post_Full, @user, CURRENT_TIMESTAMP, NULL, NULL)";
        cmd.Parameters.AddWithValue("@threadID", threadID);
        cmd.Parameters.AddWithValue("@post_Full", post_Full);
        cmd.Parameters.AddWithValue("@user", userID);
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

    public int addNewPost(int threadID, string post_Full, int userID, string timestamp)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Board_Posts] VALUES(@threadID, @post_Full, @user, @timestamp, NULL, NULL)";
        cmd.Parameters.AddWithValue("@threadID", threadID);
        cmd.Parameters.AddWithValue("@post_Full", post_Full);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@timestamp", timestamp);
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

    public int addNewPermission(int projectID, string newUserAlias, string GivingUserAlias, int PR, int PW, int BR, int BW)
    {
        int newID = getUserID(newUserAlias);
        if (newID == -1)
        {
            newID = addNewUser(newUserAlias);
        }


        int giverID = getUserID(GivingUserAlias);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_ProjectPermissions] VALUES(@projectID, @userGivenTo, @userGivenBy, @PR, @PW, @BR, @BW, CURRENT_TIMESTAMP, NULL, 0)";
        cmd.Parameters.AddWithValue("@projectID", projectID);
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


    // ==================================================================================================
    // ==================================================================================================
    //                                          BASE FUNCTIONS                                           
    // ==================================================================================================
    // ==================================================================================================

    /// <summary>
    /// Base Query function.
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="connstring"></param>
    /// <returns></returns>
    private DataTable Query(SqlCommand cmd, string connstring)
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
            //mailException(S);
            return null;
        }
    }

    private DataSet Query2(SqlCommand cmd, string connstring)
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
            DS.Tables.Add(DT);
            return DS;
        }
        catch (SqlException S)
        {
            //mailException(S);
            return null;
        }
    }

    /// <summary>
    /// Exception mailer
    /// </summary>
    /// <param name="ex"></param>
/*    public void mailException(Exception ex)
    {
        MailMessage msg = new MailMessage();
        msg.IsBodyHtml = true;
        msg.From = new MailAddress("Exception@noreply.com");
        msg.To.Add(new MailAddress("john.couch@intel.com"));
        msg.Subject = "Tracking Tool - Exception";
        msg.Body = "<br /><br /><b>Shorthand:</b> " + ex.Message + "<br /><br /><b>Trace:</b>  " + ex.StackTrace + "<br /><br />" + ex.InnerException;
        SmtpClient client = new SmtpClient();
        client.Send(msg);
        return;
    }

    public void mailException(string s)
    {
        MailMessage msg = new MailMessage();
        msg.IsBodyHtml = true;
        msg.From = new MailAddress("Exception@noreply.com");
        msg.To.Add(new MailAddress("john.couch@intel.com"));
        msg.Subject = "Tracking Tool - Mailer";
        msg.Body = "<br /><br /><b>Message:</b> " + s + "<br />";
        SmtpClient client = new SmtpClient();
        client.Send(msg);
        return;
    }*/

    public void mailTo(string email, string Subject, string Body)
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

    /// <summary>
    /// Why would you want this function?  It doesn't do anything here!
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "Why you trying to print this class?";
    }

    /// <summary>
    /// Equals?  This function returns true just to screw with you.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        return true;
    }
}