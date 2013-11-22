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
/// Summary description for Feature
/// </summary>
public class Feature
{
    private int ID;
    private int projectID;
    private int parentID;
    private string featureName;
    private string featureDescription;
    private int weight;
    private int boardID;
    private int percentComplete;
    private bool flagCompleted;
    private DateTime createdTimestamp;
    private DateTime completedTimestamp;
    private DateTime updatedTimestamp;

    private bool isMilestone;
    private bool isFeature;
    public bool hasChildren;
    public List<Feature> myChildren;

	public Feature(int ID)
	{
        myChildren = new List<Feature>();
        hasChildren = false;
        
        DataTable DT = getCurrent(ID);
        if (DT.Rows.Count == 1)
        {

            // SET CURRENT FEATURE DATA
            DataRow DR = DT.Rows[0];
            setBoardID(int.Parse(DR["boardID"].ToString()));
            if (DR["completedTimestamp"].ToString() != "")
                setCompletedTimestamp(DateTime.Parse(DR["completedTimestamp"].ToString()));
            else
                setCompletedTimestamp(DateTime.MinValue);
            if (DR["createdTimestamp"].ToString() != "")
                setCreatedTimestamp(DateTime.Parse(DR["createdTimestamp"].ToString()));
            else
                setCreatedTimestamp(DateTime.MinValue);
            setFeatureDescription(DR["featureDescription"].ToString());
            setFeatureName(DR["featureName"].ToString());
            if (DR["flagCompleted"].ToString() == "1")
                setFlagCompleted(true);
            else
                setFlagCompleted(false);
            setID(int.Parse(DR["ID"].ToString()));
            if (DR["parentID"].ToString() == "")
                setIsMilestone(true);
            else
                setIsMilestone(false);
            if (DR["parentID"].ToString() != "")
                setParentID(int.Parse(DR["parentID"].ToString()));
            else
                setParentID(-1);
            setPercentComplete(int.Parse(DR["percentComplete"].ToString()));
            setProjectID(int.Parse(DR["projectID"].ToString()));
            if (DR["updatedTimestamp"].ToString() != "")
                setUpdatedTimestamp(DateTime.Parse(DR["updatedTimestamp"].ToString()));
            else
                setUpdatedTimestamp(DateTime.MinValue);
            setWeight(int.Parse(DR["weight"].ToString()));

            // GATHER CHILDREN NOW
            DT = getFeatures(getID());
            foreach (DataRow child in DT.Rows)
            {
                myChildren.Add(new Feature(int.Parse(child["ID"].ToString())));
            }
            if (myChildren.Count > 0) hasChildren = true;
        }
        else
        {
            // WHAT THE HELL@?!?!  GO HOME, YOUR DRUNK.
        }
	}


    //Feature Gets
    public int getID()
    {
        return ID;
    }

    public int getProjectID()
    {
        return projectID;
    }

    public int getParentID()
    {
        return parentID;
    }

    public string getFeatureName()
    {
        return featureName;
    }
   
    public string getFeatureDescription()
    {
        return featureDescription;
    }

    public int getWeight()
    {
        return weight;
    }

    public int getBoardID()
    {
        return boardID;
    }

    public int getPercentComplete()
    {
        return percentComplete;
    }

    public bool getFlagCompleted()
    {
        return flagCompleted;
    }

    public DateTime getCreatedTimestamp()
    {
        return createdTimestamp;
    }

    public DateTime getCompletedTimestamp()
    {
        return completedTimestamp;
    }

    public DateTime getUpdatedTimestamp()
    {
        return updatedTimestamp;
    }

    public bool getIsMilestone()
    {
        return isMilestone;
    }

    public bool getIsFeature()
    {
        return isFeature;
    }

    //Feature Sts
    public void setID(int i)
    {
        ID = i;
    }

    public void setProjectID(int i)
    {
        projectID = i;
    }

    public void setParentID(int i)
    {
        parentID = i;
    }

    public void setFeatureName(string s)
    {
        featureName = s;
    }

    public void setFeatureDescription(string s)
    {
        featureDescription = s;
    }

    public void setWeight(int i)
    {
        weight = i;
    }

    public void setBoardID(int i)
    {
        boardID = i;
    }

    public void setPercentComplete(int i)
    {
        percentComplete = i;
    }
    
    public void setFlagCompleted(bool b)
    {
        flagCompleted = b;
    }

    public void setCreatedTimestamp(DateTime d)
    {
        createdTimestamp = d;

    }

    public void setCompletedTimestamp(DateTime d)
    {
        completedTimestamp = d;
    }

    public void setUpdatedTimestamp(DateTime d)
    {
        updatedTimestamp = d;
    }

    public void setIsMilestone(bool b)
    {
        isMilestone = b;
        isFeature = !b;
    }



//Feature Functions
    public DataTable getFeatures(int parentID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE [parentID] = @parentID ORDER BY [createdTimestamp] ASC";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@parentID", parentID);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable getCurrent(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            return new DataTable();
        }
        else
        {
            return DT;
        }
    }

    public void deleteFeature()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM [TrackingTool_Features] WHERE [ID] = @featureID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featureID", getID());

        TTDB.TTQuery(cmd);

        Feature parentFeature = new Feature(getParentID());
        if (parentFeature.hasChildren)
        {
            Feature child = parentFeature.myChildren[0];
            child.updateAllPercentCompletes();
        }
        else
        {
            parentFeature.updatePercentComplete(0);
        }
    }

    static public int addNewMilestone(int projectID, string Name, string Description, int Weight, int boardID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Features] VALUES(@projectID, NULL, @featureName, @featureDescription, @weight, @boardID, 0, 0, CURRENT_TIMESTAMP, NULL, NULL)";
        cmd.Parameters.AddWithValue("@projectID", projectID);
        cmd.Parameters.AddWithValue("@featureName", Name);
        cmd.Parameters.AddWithValue("@featureDescription", Description);
        cmd.Parameters.AddWithValue("@weight", Weight);
        cmd.Parameters.AddWithValue("@boardID", boardID);
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE projectID = @projectID AND parentID IS NULL AND featureName = @featureName and featureDescription = @featureDescription and weight = @weight";
        cmd.Parameters.AddWithValue("@projectID", projectID);
        cmd.Parameters.AddWithValue("@featureName", Name);
        cmd.Parameters.AddWithValue("@featureDescription", Description);
        cmd.Parameters.AddWithValue("@weight", Weight);
        DataTable DT = TTDB.TTQuery(cmd);

        Feature ftr = new Feature(int.Parse(DT.Rows[0]["ID"].ToString()));
        ftr.updateAllPercentCompletes();

        if (DT.Rows.Count > 0)
        {
            return Int32.Parse(DT.Rows[0]["ID"].ToString());
        }
        else
        {
            return -1;
        }
    }

    static public int addNewFeature(int projectID, int parentID, string Name, string Description, int Weight, int boardID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Features] VALUES(@projectID, @parentID, @featureName, @featureDescription, @weight, @boardID, 0, 0, CURRENT_TIMESTAMP, NULL, NULL)";
        cmd.Parameters.AddWithValue("@projectID", projectID);
        cmd.Parameters.AddWithValue("@parentID", parentID);
        cmd.Parameters.AddWithValue("@featureName", Name);
        cmd.Parameters.AddWithValue("@featureDescription", Description);
        cmd.Parameters.AddWithValue("@weight", Weight);
        cmd.Parameters.AddWithValue("@boardID", boardID);
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Features] WHERE projectID = @projectID AND parentID = @parentID AND featureName = @featureName and featureDescription = @featureDescription and weight = @weight";
        cmd.Parameters.AddWithValue("@projectID", projectID);
        cmd.Parameters.AddWithValue("@parentID", parentID);
        cmd.Parameters.AddWithValue("@featureName", Name);
        cmd.Parameters.AddWithValue("@featureDescription", Description);
        cmd.Parameters.AddWithValue("@weight", Weight);
        DataTable DT = TTDB.TTQuery(cmd);

        Feature ftr = new Feature(int.Parse(DT.Rows[0]["ID"].ToString()));
        ftr.updateAllPercentCompletes();

        if (DT.Rows.Count > 0)
        {
            return Int32.Parse(DT.Rows[0]["ID"].ToString());
        }
        else
        {
            return -1;
        }
    }

    public DataTable updatePercentComplete(int perComplete)
    {
        setPercentComplete(perComplete);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [percentComplete] = @perComplete, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@perComplete", perComplete);
        cmd.Parameters.AddWithValue("@ID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        updateAllPercentCompletes();
        return DT;
    }

    public void updateAllPercentCompletes()
    {
        if (getParentID() != -1)
        {
            Feature parent = new Feature(getParentID());
            decimal total = 0;
            decimal counter = 0;
            
            if (parent.myChildren.Count > 0)
            {
                foreach (Feature child in parent.myChildren)
                {
                    counter++;
                    total += child.getPercentComplete();
                }
                parent.updatePercentComplete((int)(total / counter));
            }
            parent.updateAllPercentCompletes();
        }
    }

    public DataTable updateFeatureDetails(string name, string description, int weight, int percentComplete)
    {
        setFeatureName(name);
        setFeatureDescription(description);
        setWeight(weight);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [featureName] = @featName, [featureDescription] = @featDesc, [weight] = @weight, [updatedTimestamp] = CURRENT_TIMESTAMP, percentComplete = @percentComplete WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featName", getFeatureName());
        cmd.Parameters.AddWithValue("@featDesc", getFeatureDescription());
        cmd.Parameters.AddWithValue("@weight", getWeight());
        cmd.Parameters.AddWithValue("@ID", getID());
        cmd.Parameters.AddWithValue("@percentComplete", percentComplete);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        updateAllPercentCompletes();
        return DT;
    }

    public DataTable updateFeatureDetails(string name, string description, int weight)
    {
        setFeatureName(name);
        setFeatureDescription(description);
        setWeight(weight);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [featureName] = @featName, [featureDescription] = @featDesc, [weight] = @weight, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featName", getFeatureName());
        cmd.Parameters.AddWithValue("@featDesc", getFeatureDescription());
        cmd.Parameters.AddWithValue("@weight", getWeight());
        cmd.Parameters.AddWithValue("@ID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable updateFeatureName(string name)
    {
        setFeatureName(name);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [featureName] = @featName, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featName", getFeatureName());
        cmd.Parameters.AddWithValue("@ID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable updateFeatureDescription(string desc)
    {
        setFeatureDescription(desc);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [featureDescription] = @featDesc, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@featDesc", getFeatureDescription());
        cmd.Parameters.AddWithValue("@ID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable updateWeight(int weight)
    {
        setWeight(weight);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Features] SET [weight] = @weight, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@weight", getWeight());
        cmd.Parameters.AddWithValue("@ID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    public DataTable updateFlagCompleted(bool completed)
    {
        setFlagCompleted(completed);

        SqlCommand cmd = new SqlCommand();
        if (completed)
        {
            cmd.CommandText = "UPDATE [TrackingTool_Features] SET [flagCompleted] = @flagCompleted, [updatedTimestamp] = CURRENT_TIMESTAMP, [completedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        }
        else
        {
            cmd.CommandText = "UPDATE [TrackingTool_Features] SET [flagCompleted] = @flagCompleted, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        }
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@flagCompleted", getFlagCompleted());
        cmd.Parameters.AddWithValue("@ID", getID());

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }
   

}