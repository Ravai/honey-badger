using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Feature
/// </summary>
public class Feature
{
    private int ID;
    private int MilestoneID;
    private string FeatureName;
    private bool Completed;
    private DateTime dateCompleted;

	public Feature()
	{
		
	}

    public int getID()
    {
        return ID;
    }

    public int getMilestoneID()
    {
        return MilestoneID;
    }

    public string getFeature()
    {
        return FeatureName;
    }

    public bool getCompleted()
    {
        return Completed;
    }

    public DateTime getDateCompleted()
    {
        return dateCompleted;
    }
}