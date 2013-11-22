using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Summary description for Thread
/// </summary>
public class Thread
{
    // private variables declaration
    private int threadID;
    private int boardID;
    private string thread_Name;
    private string thread_Description;
    private DateTime createdTimestamp;
    private int createdBy;
    private ArrayList PostList = new ArrayList();

    // Constructors
	public Thread(int ID)
	{
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Board_Threads] WHERE [threadID] = @threadID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@threadID", ID);

        DataTable DT = TTDB.TTQuery(cmd);

        threadID = Int32.Parse(DT.Rows[0]["threadID"].ToString());
        boardID = Int32.Parse(DT.Rows[0]["boardID"].ToString());
        thread_Name = DT.Rows[0]["thread_Name"].ToString();
        thread_Description = DT.Rows[0]["thread_Description"].ToString();
        createdTimestamp = DateTime.Parse(DT.Rows[0]["createdTimestamp"].ToString());
        createdBy = Int32.Parse(DT.Rows[0]["createdBy"].ToString());

        getPosts();
	}


    // private methods below
    private void getPosts()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Posts] WHERE [threadID] = @threadID ORDER BY [createdTimestamp] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@threadID", threadID);

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            Post newPost = new Post(Int32.Parse(DR["postID"].ToString()));
            PostList.Add(newPost);
        }
    }



    // Public Methods below.
    public int count()
    {
        return PostList.Count;
    }

    public int get_threadID()
    {
        return threadID;
    }

    public int get_boardID()
    {
        return boardID;
    }

    public string get_thread_Name()
    {
        return thread_Name;
    }

    public string get_thread_Description()
    {
        return thread_Description;
    }

    public DateTime get_createdTimestamp()
    {
        return createdTimestamp;
    }

    public int get_createdBy()
    {
        return createdBy;
    }




    public static int addNewThread(int boardID, string threadName, string threadDescription, int userID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Board_Threads] VALUES(@boardID, @threadName, @threadDescription, CURRENT_TIMESTAMP, @user)";
        cmd.Parameters.AddWithValue("@boardID", boardID);
        cmd.Parameters.AddWithValue("@threadName", threadName);
        cmd.Parameters.AddWithValue("@threadDescription", threadDescription);
        cmd.Parameters.AddWithValue("@user", userID);
        DataTable DT = TTDB.TTQuery(cmd);

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