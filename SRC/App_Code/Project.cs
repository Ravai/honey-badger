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
    }

    private void setProjectOwner(User u)
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

    public static List<Project> getTask(int ID)
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



   /* public DataTable getTask(int ID, string userName)
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
    } */

   /*  public DataTable getFeatures(int milestoneID)
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
    } */

   /*  public int getMilestoneID_fromFeatureID(int featureID)
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
    } */
   
   /* public int getUserID(string userName)
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
    } */






}