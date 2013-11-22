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
/// Summary description for Board
/// </summary>
public class Board
{
    // private variables declaration
    private int boardID;
    private string board_CategoryName;
    private int weight;
    private DateTime createdTimestamp;
    private int createdBy;

    private bool Active;

    private ArrayList ThreadList = new ArrayList();

    // Constructors
	public Board(int ID)
	{
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Board_Main] WHERE [boardID] = @boardID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", ID);

        DataTable DT = TTDB.TTQuery(cmd);

        if (DT.Rows.Count == 1)
        {
            boardID = Int32.Parse(DT.Rows[0]["boardID"].ToString());
            board_CategoryName = DT.Rows[0]["board_CategoryName"].ToString();
            weight = Int32.Parse(DT.Rows[0]["weight"].ToString());
            createdTimestamp = DateTime.Parse(DT.Rows[0]["createdTimestamp"].ToString());
            createdBy = Int32.Parse(DT.Rows[0]["createdBy"].ToString());

            getThreads();
            Active = true;
        }
	}

    // private methods
    private void getThreads()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Threads] WHERE [boardID] = @boardID ORDER BY [createdTimestamp] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@boardID", boardID);

        DataTable DT = TTDB.TTQuery(cmd);

        foreach (DataRow DR in DT.Rows)
        {
            ThreadList.Add(new Thread(Int32.Parse(DR["threadID"].ToString())));
        }
    }

    // public methods
    public bool isActive()
    {
        return Active;
    }

    public int PostCount()
    {
        int total = 0;

        for (int i = 0; i < ThreadList.Count; i++)
        {
            total += ((Thread)ThreadList[i]).count();
        }

        return total;
    }

    public int ThreadCount()
    {
        return ThreadList.Count;
    }

    public int get_BoardID()
    {
        return boardID;
    }

    public string get_board_CategoryName()
    {
        return board_CategoryName;
    }

    public int get_weight()
    {
        return weight;
    }

    public DateTime get_createdTimestamp()
    {
        return createdTimestamp;
    }

    public int get_createdBy()
    {
        return createdBy;
    }



    public static int addNewBoard(string boardName, int userID, int importance)
    {
        // importance is 1 if its a milestone, 0 if its a general board.
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Board_Main] VALUES(@boardName, @importance, CURRENT_TIMESTAMP, @user)";
        cmd.Parameters.AddWithValue("@boardName", boardName);
        cmd.Parameters.AddWithValue("@importance", importance);
        cmd.Parameters.AddWithValue("@user", userID);
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT [boardID] FROM [TrackingTool_Board_Main] WHERE [board_CategoryName] = @boardName AND [createdBy] = @user ORDER BY [createdTimestamp] DESC";
        cmd.Parameters.AddWithValue("@boardName", boardName);
        cmd.Parameters.AddWithValue("@user", userID);
        DataTable DT = TTDB.TTQuery(cmd);

        int boardID = -1;
        if (DT.Rows.Count > 0)
        {
            boardID = Int32.Parse(DT.Rows[0]["boardID"].ToString());
        }

        Thread.addNewThread(boardID, "General Discussion", "Generic Thread for the Board", userID);

        return boardID;
    }
	




}