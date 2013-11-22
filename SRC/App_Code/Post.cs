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
/// Summary description for Post
/// </summary>
public class Post
{
    private int postID;
    private int threadID;
    private string post_Full;
    private int postBy;
    private DateTime createdTimestamp;
    private int updatedBy;
    private DateTime updatedTimestamp;
    private string Display_Image;
    private string Display_Name;

	public Post(int ID)
	{
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [viewTrackingTool_Boards_Posts] WHERE [postID] = @postID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@postID", ID);

        DataTable DT = TTDB.TTQuery(cmd);

        if (DT.Rows.Count == 1)
        {
            postID = Int32.Parse(DT.Rows[0]["postID"].ToString());
            threadID = Int32.Parse(DT.Rows[0]["threadID"].ToString());
            post_Full = DT.Rows[0]["post_Full"].ToString();
            postBy = Int32.Parse(DT.Rows[0]["postBy"].ToString());
            createdTimestamp = DateTime.Parse(DT.Rows[0]["createdTimestamp"].ToString());
            Display_Image = DT.Rows[0]["Display_Image"].ToString();
            Display_Name = DT.Rows[0]["Display_Name"].ToString();
            if (DT.Rows[0]["updatedBy"].ToString() != "")
            {
                updatedBy = Int32.Parse(DT.Rows[0]["updatedBy"].ToString());
                updatedTimestamp = DateTime.Parse(DT.Rows[0]["updatedTimestamp"].ToString());
            }
        }
	}

    public int get_postID()
    {
        return postID;
    }

    public int get_threadID()
    {
        return threadID;
    }

    public string get_post_Full()
    {
        return post_Full;
    }

    public int get_postBy()
    {
        return postBy;
    }

    public DateTime get_createdTimestamp()
    {
        return createdTimestamp;
    }

    public int get_updatedBy()
    {
        return updatedBy;
    }

    public DateTime get_updatedTimestamp()
    {
        return updatedTimestamp;
    }

    public string get_DisplayName()
    {
        return Display_Name;
    }

    public string get_DisplayImage()
    {
        return Display_Image;
    }

    public void update_post_Full(string s, int user)
    {
        post_Full = s;
        updatedBy = user;
        updatedTimestamp = DateTime.Now;

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Board_Posts] SET [post_Full] = @post_Full, [updatedby] = @updatedBy, [updatedTimestamp] = @updatedTimestamp WHERE [postID] = @postID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@post_Full", post_Full);
        cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
        cmd.Parameters.AddWithValue("@updatedTimestamp", updatedTimestamp);

        DataTable DT = TTDB.TTQuery(cmd);
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



}