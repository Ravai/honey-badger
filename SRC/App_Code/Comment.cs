using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Comment
/// </summary>
public class Comment
{
    private int ID;
    private int taskID;
    private string comment;
    private DateTime createdTimestamp;
    private DateTime updatedTimestamp;

	public Comment()
	{
		
	}

    public int getID()
    {
        return ID;
    }

    public int getTaskID()
    {
        return taskID;
    }

    public string getComment()
    {
        return comment;
    }

    public DateTime getCreatedTimestamp()
    {
        return createdTimestamp;
    }

    public DateTime getUpdatedTimestamp()
    {
        return updatedTimestamp;
    }
}