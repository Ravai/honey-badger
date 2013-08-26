using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Milestone
/// </summary>
public class Milestone
{
    private int ID;
    private int projectID;
    private string milestoneName;
    private string milestoneDescription;
    private int weight;
    private int boardID;
    private DateTime createdTimestamp;
    private DateTime updatedTimestamp;
    private int completedPercent;
    private DateTime completedTimestamp;

	public Milestone()
	{
		
	}

    public int getID()
    {
        return ID;
    }

    public int getProjectID()
    {
        return projectID;
    }

    public string getMilestoneName()
    {
        return milestoneName;
    }

    public string getMilestoneDescription()
    {
        return milestoneDescription;
    }

    public int getWeight()
    {
        return weight;
    }

    public int getBoardID()
    {
        return boardID;
    }

    public DateTime getCreatedTimestamp()
    {
        return createdTimestamp;
    }

    public DateTime getUpdatedTimestamp()
    {
        return updatedTimestamp;
    }

    public int getCompletedPercent()
    {
        return completedPercent;
    }

    public DateTime getCompletedTimestamp()
    {
        return completedTimestamp;
    }
}