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
            setID(int.Parse(DR["ID"].ToString()));

             if (DR["doneFlag"].ToString() == "1")
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


    public int getID()
    {
        return ID;
    }

    public string getTaskName()
    {
        return taskName;
    }

    public string getTaskDescription()
    {
        return taskDescription;
    }

    public DateTime getExpectedStart()
    {
        return expectedStart;
    }

    public DateTime getExpectedStop()
    {
        return expectedStop;
    }

    public DateTime getActualStart()
    {
        return actualStart;
    }

    public DateTime getActualStop()
    {
        return actualStop;
    }

    public bool getDoneFlag()
    {
        return doneFlag;
    }

    public int getOwnerID()
    {
        return ownerID;
    }

    public int getBoardID()
    {
        return boardID;
    }

    public int getProjectSize()
    {
        return projectSize;
    }

    public int getPercentComplete()
    {
        return percentComplete;
    }

    public User getProjectOwner()
    {
        return projectOwner;
    }

    public List<int> getMilestones()
    {
        return milestones;
    }


    private void setID(int i)
    {
        ID = i;
    }

    private void setTaskName(string s)
    {
        taskName = s;
    }

    private void setTaskDescription(string s)
    {
        taskDescription = s;
    }

    private void setExpectedStart(DateTime d)
    {
        expectedStart = d;
    }

    private void setExpectedStop(DateTime d)
    {
        expectedStop = d;
    }

    private void setActualStart(DateTime d)
    {
        actualStart = d;
    }

    private void setActualStop(DateTime d)
    {
        actualStop = d;
    }

    private void setDoneFlag(bool b)
    {
        doneFlag = b;
    }

    private void setOwnerID(int i)
    {
        ownerID = i;
    }

    private void setBoardID(int i)
    {
        boardID = i;
    }

    private void setProjectSize(int i)
    {
        projectSize = i;
    }

    private void setPercentComplete(int i)
    {
        percentComplete = i;
        if (i == 100)
            setDoneFlag(true);
        else
            setDoneFlag(false);

        updateProjectPercentComplete();
    }

    private void setProjectOwner(User u)
    {
        projectOwner = u;
    }


    //Get Tasks and Feature functions
    public Project getTask(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Projects] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = TTDB.TTQuery(cmd);

        if (DT.Rows.Count == 1)
        {
            return new Project(int.Parse(DT.Rows[0]["ID"].ToString()));
        }
        else
        {
            return null;
        }

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
    
    //Get User's Tasks functions
    public static List<Project> getCompletedTasks(string userName)
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

    public static List<Project> getWipTasks(string userName)
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

    public static List<Project> getReadyTasks(string userName)
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

    public static List<Project> getUpcomingTasks(string userName)
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

   
    //Get Shared Task fucntions
    public static List<Project> getSharedCompletedTasks(int userID)
    {
		List<Project> sharedTask = new List<Project>();
	
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
		
		        foreach (DataRow DR in DT.Rows)
                {
			        sharedTask.Add(new Project(int.Parse(DR["ID"].ToString())));
                }
		
	        }
	    }
        return sharedTask;     
    }

    public static List<Project> getSharedWipTasks(int userID)
    {
        List<Project> sharedWip = new List<Project>();

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

                foreach (DataRow DR in DT.Rows)
                {
                    sharedWip.Add(new Project(int.Parse(DR["ID"].ToString())));
                }
            }
        }
        return sharedWip;
    }

    public static List<Project> getSharedReadyTasks(int userID)
    {
        List<Project> sharedReady = new List<Project>();

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
				
				foreach (DataRow DR in DT.Rows)
				{
					sharedReady.Add(new Project(int.Parse(DR["ID"].ToString())));
				}
			}
        }
        return sharedReady;
	}

    public static List<Project> getSharedUpcomingTasks(int userID)
    {
        List<Project> sharedUpcoming = new List<Project>();

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
                
				foreach (DataRow DR in DT.Rows)
				{
					sharedUpcoming.Add(new Project(int.Parse(DR["ID"].ToString())));
				}
			}
        }
        return sharedUpcoming;
    }


    //Task Control functions
    public void completeTask()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [doneFlag] = '1', [actualStop] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", getID());

        TTDB.TTQuery(cmd);
    }

    public void reOpenTask()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [doneFlag] = '0', [actualStop] = NULL WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", getID());

        TTDB.TTQuery(cmd);
    }

    public void startTask()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [actualStart] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", getID());

        TTDB.TTQuery(cmd);
    }

    private void updateProjectPercentComplete()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [percentComplete] = @perComplete WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@perComplete", getPercentComplete());
        cmd.Parameters.AddWithValue("@ID", getID());

        TTDB.TTQuery(cmd);
    }

    public void deleteTask()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [db_forum].[dbo].[TrackingTool_Board_Threads] WHERE [boardID] = @boardID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", getBoardID());
        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "DELETE FROM [db_forum].[dbo].[TrackingTool_Board_Posts] WHERE [threadID] = @threadID";
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@threadID", DR["threadID"].ToString());
            TTDB.TTQuery(cmd2);
        }

        cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [db_forum].[dbo].[TrackingTool_Board_Threads] WHERE [boardID] = @boardID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", getBoardID());
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [db_forum].[dbo].[TrackingTool_Board_Main] WHERE [boardID] = @boardID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", getBoardID());
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [db_forum].[dbo].[TrackingTool_Features] WHERE [projectID] = @projectID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@projectID", getID());
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [TrackingTool_Projects] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", getID());

        TTDB.TTQuery(cmd);
    }

    public void updateProjectBaseInfo(string Name, string Description)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [taskName] = @Name, [taskDescription] = @Description WHERE [ID] = @taskID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@taskID", getID());
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@Description", Description);

        TTDB.TTQuery(cmd);
    }

    public void increaseProjectSize(int ID, int userID)
    {

        Project proj = getTask(ID);
        int newBoardID = Board.addNewBoard("General Discussion", userID, 0);
        Thread.addNewThread(boardID, "General Discussion", "Generic Thread for the Board", userID);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Projects] SET [projectSize] = '1', [boardID] = @boardID WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@boardID", newBoardID);

        TTDB.TTQuery(cmd);   
    }

    //Add new task functions
     public int addNewTask(string taskName, string taskDescription, DateTime expectedStart, DateTime expectedStop, string userName)
    {
        int userID = User.getUserID(userName);

        if (userID == -1)
        {
            userID = User.addNewUser(userName);
        }


        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Projects] VALUES(@taskName, @taskDescription, @expectedStart, @expectedStop, NULL, NULL, 0, @ownerID, NULL, 0, 0)";
        cmd.Parameters.AddWithValue("@taskName", taskName);
        cmd.Parameters.AddWithValue("@taskDescription", taskDescription);
        cmd.Parameters.AddWithValue("@expectedStart", expectedStart);
        cmd.Parameters.AddWithValue("@expectedStop", expectedStop);
        cmd.Parameters.AddWithValue("@ownerID", userID);
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Projects] WHERE [taskName] = @taskName AND [taskDescription] = @taskDescription AND [expectedStart] = @expectedStart AND [expectedStop] = @expectedStop and [ownerID] = @ownerID";
        cmd.Parameters.AddWithValue("@taskName", taskName);
        cmd.Parameters.AddWithValue("@taskDescription", taskDescription);
        cmd.Parameters.AddWithValue("@expectedStart", expectedStart);
        cmd.Parameters.AddWithValue("@expectedStop", expectedStop);
        cmd.Parameters.AddWithValue("@ownerID", userID);
        DataTable DT = TTDB.TTQuery(cmd);

        int taskID = Int32.Parse(DT.Rows[0]["ID"].ToString());

        TTDB.addNewPermission(taskID, userName, userName, 1, 1, 1, 1);

        if (DT.Rows.Count > 0)
        {
            return taskID;
        }
        else
        {
            return -1;
        }
    }

    /*
    //Need to check what it is returning
    public DataTable getProjectPermissions()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [projectID] = @projectID ORDER BY [updatedTimestamp]";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@projectID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }
    */

}
