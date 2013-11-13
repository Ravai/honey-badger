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
/// Summary description for Project
/// </summary>
public class Project
{
    private int ID;
    private string taskName;
    private string taskDescription;
    private DateTime expectedStart;
    private DateTime expectedStop;
    private DateTime actualStart;
    private DateTime actualStop;
    private bool doneFlag;
    private int ownerID;
    private int boardID;
    private int projectSize;
    private int percentComplete;
    private User projectOwner;
    private List<int> milestones;

    public Project(int id)
    {
       SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Projects] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", id);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT.Rows.Count == 1)
        {
            //Set Current Data

            DataRow DR = DT.Rows[0];

            setBoardID(int.Parse(DR["boardID"].ToString()));
            setTaskName(DR["taskName"].ToString());
            setTaskDescription(DR["taskDescription"].ToString());
            setOwnerID(int.Parse(DR["ownerID"].ToString()));
            setProjectSize(int.Parse(DR["projectSize"].ToString()));
            setPercentComplete(int.Parse(DR["percentComplete"].ToString()));

             if (DR["doneCompleted"].ToString() == "1")
                setDoneFlag(true);
            else
                setDoneFlag(false);

             if (DR["expectedStart"].ToString() != "")
                setExpectedStart(DateTime.Parse(DR["expectedStop"].ToString()));
            else
                 setExpectedStart(DateTime.MinValue);

             if (DR["expectedStop"].ToString() != "")
                setExpectedStop(DateTime.Parse(DR["expectedStop"].ToString()));
            else
                setExpectedStop(DateTime.MinValue);

            if (DR["actualStart"].ToString() != "")
                setActualStart(DateTime.Parse(DR["actualStart"].ToString()));
            else
                setActualStart(DateTime.MinValue);

            if (DR["actualStop"].ToString() != "")
                setActualStop(DateTime.Parse(DR["actualStop"].ToString()));
            else
                setActualStop(DateTime.MinValue);
        }
        else
        {
            //Shouldnt happen
        }

        milestones = new List<int>();
        getFeatures();
    }


    private int getID()
    {
        return ID;
    }

    private string getTaskName()
    {
        return taskName;
    }

    private string getTaskDescription()
    {
        return taskDescription;
    }

    private DateTime getExpectedStart()
    {
        return expectedStart;
    }

    private DateTime getExpectedStop()
    {
        return expectedStop;
    }

    private DateTime getActualStart()
    {
        return actualStart;
    }

    private DateTime getActualStop()
    {
        return actualStop;
    }

    private bool getDoneFlag()
    {
        return doneFlag;
    }

    private int getOwnerID()
    {
        return ownerID;
    }

    private int getBoardID()
    {
        return boardID;
    }

    private int getProjectSize()
    {
        return projectSize;
    }

    private int getPercentComplete()
    {
        return percentComplete;
    }

    private User getProjectOwner()
    {
        return projectOwner;
    }



    public void setID(int i)
    {
        ID = i;
    }

    public void setTaskName(string s)
    {
        taskName = s;
    }

    public void setTaskDescription(string s)
    {
        taskDescription = s;
    }

    public void setExpectedStart(DateTime d)
    {
        expectedStart = d;
    }

    public void setExpectedStop(DateTime d)
    {
        expectedStop = d;
    }

    public void setActualStart(DateTime d)
    {
        actualStart = d;
    }

    public void setActualStop(DateTime d)
    {
        actualStop = d;
    }

    public void setDoneFlag(bool b)
    {
        doneFlag = b;
    }

    public void setOwnerID(int i)
    {
        ownerID = i;
    }

    public void setBoardID(int i)
    {
        boardID = i;
    }

    public void setProjectSize(int i)
    {
        projectSize = i;
    }

    public void setPercentComplete(int i)
    {
        percentComplete = i;
    }

    public void setProjectOwner(User u)
    {
        projectOwner = u;
    }




    public static List<Project> getTasks(string userName)
    {
        List<Project> tasks = new List<Project>();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = TTDB.TTQuery(cmd);
        foreach (DataRow DR in DT.Rows)
        {
            tasks.Add(new Project(int.Parse(DR["ID"].ToString())));
        }

        return tasks;
    }

    public List<Project> getCompletedTasks(string userName)
    {
        List<Project> completed = new List<Project>();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '1' ORDER BY [actualStop] DESC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = TTDB.TTQuery(cmd);
        foreach (DataRow DR in DT.Rows)
        {
            completed.Add(new Project(int.Parse(DR["ID"].ToString())));
        }

        return completed;
    }

    public List<Project> getWipTasks(string userName)
    {
        List<Project> wip = new List<Project>();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '0' AND [expectedStart] < CURRENT_TIMESTAMP AND [actualStart] IS NOT NULL ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            wip.Add(new Project(int.Parse(DR["ID"].ToString())));
        }

        return wip;
    }

    public List<Project> getReadyTasks(string userName)
    {
        List<Project> ready = new List<Project>();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '0' AND [expectedStart] < CURRENT_TIMESTAMP AND [actualStart] IS NULL ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            ready.Add(new Project(int.Parse(DR["ID"].ToString())));
        }

        return ready;
    }

    public List<Project> getUpcomingTasks(string userName)
    {
        List<Project> upcoming = new List<Project>();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ownerAlias] = @user AND [doneFlag] = '0' AND [expectedStart] > CURRENT_TIMESTAMP ORDER BY [expectedStart] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userName);

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            upcoming.Add(new Project(int.Parse(DR["ID"].ToString())));
        }

        return upcoming;
    }

    public List<Project> getTask(int ID)
    {
        List<Project> task = new List<Project>();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            task.Add(new Project(int.Parse(DR["ID"].ToString())));
        }

        return task;
    }



    public DataTable getSharedCompletedTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = TTDB.TTQuery(cmd);

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

                DT = TTDB.TTQuery(cmd);
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

    public DataTable getSharedWipTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = TTDB.TTQuery(cmd);

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

                DT = TTDB.TTQuery(cmd);
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
    
    public DataTable getSharedReadyTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = TTDB.TTQuery(cmd);

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

                DT = TTDB.TTQuery(cmd);
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

    public DataTable getSharedUpcomingTasks(string userName)
    {
        int userID = getUserID(userName);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT DISTINCT [projectID] FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @user AND [user_GivenBy] != @user";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@user", userID);

        DataTable DT = TTDB.TTQuery(cmd);

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

                DT = TTDB.TTQuery(cmd);
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

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT.Rows.Count == 0)
        {
            int userID = getUserID(userName);

            cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [projectID] = @ID and [user_GivenTo] = @userID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@userID", userID);

            DT = TTDB.TTQuery(cmd);
            if (DT.Rows.Count == 1)
            {
                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] = @ID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ID", ID);
                DT = TTDB.TTQuery(cmd);
            }
        }
        return DT;
    }

    public DataTable getFeatures(int milestoneID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE [MilestoneID] = @milestoneID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@milestoneID", milestoneID);

        DataTable DT = TTDB.TTQuery(cmd);
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

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            return -1;
        }
        else
        {
            return Int32.Parse(DT.Rows[0]["MilestoneID"].ToString());
        }
    }

   
    
    
    private void getFeatures()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE [parentID] is null AND [projectID] = @id ORDER BY [createdTimestamp] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@id", getID());

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            milestones.Add(int.Parse(DR["ID"].ToString()));
        }   
    }

    public int getUserID(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Users] WHERE [ownerAlias] = @userName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userName", userName);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null || DT.Rows.Count == 0)
        {
            return -1;
        }
        else
        {
            return Int32.Parse(DT.Rows[0]["ID"].ToString());
        }
    }






}