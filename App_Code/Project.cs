using System;
using System.Collections.Generic;
using System.Linq;
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

	public Project()
	{
		
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
}