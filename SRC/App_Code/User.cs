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
/// Summary description for User
/// </summary>
public class User
{
    private int ID;
    private string ownerAlias;
    private string userPW;
    private string IPInitial;
    private string IPLast;
    private DateTime dateCreated;
    private DateTime dateLastLoggedIn;
    private DateTime dateLastLoggedOut;
    private int active;
    private int userLevel;
    private string firstName;
    private string middleName;
    private string lastName;
    private string email;
    private string phoneNumber;
    private string displayName;
    private string displayImage;
    private string userStatus;
    private string infoForumFooter;

    //Should we have time stamps with login and logout for dateLastLoggedIn and dateLastLoggedOut

    public User(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Users] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", );

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT.Rows.Count == 1)
        {
            //Set Current Data

            DataRow DR = DT.Rows[0];

            setOwnerAlias(DR["ownerAlias"].ToString());
            setUserPW(DR["user_PW"].ToString());
            setIPInitial(DR["IP_Initial"].ToString());
            setIPLast(DR["IP_Last"].ToString());
            setDateCreated(DateTime.Parse(DR["dateCreated"].ToString()));
            setDateLastLoggedIn(DateTime.Parse(DR["dateLastLoggedIn"].ToString()));
            setDateLastLoggedOut(DateTime.Parse(DR["dateLasLoggedOut"].ToString()));
            setActive(int.Parse(DR["Active"].ToString()));
            setUserLevel(int.Parse(DR["userLevel"].ToString()));
            setFirstName(DR["firstName"].ToString());
            setMiddleName(DR["middleName"].ToString());
            setLastName(DR["lastName"].ToString());
            setEmail(DR["eMail"].ToString());
            setPhoneNumber(DR["phoneNumber"].ToString());
            setDisplayName(DR["Display_Name"].ToString());
            setDisplayImage(DR["Display_Image"].ToString());
            setUserStatus(DR["User_Status"].ToString());
            setInfoForumFooter(DR["Info_ForumFooter"].ToString()); 
        }
        else
        {
            //Shouldnt happen
        }
    }

    public int getID()
    {
        return ID;
    }

    public string getOwnerAlias()
    {
        return ownerAlias;
    }

    public string getUserPW()
    {
        return userPW;
    }

    public string getIPInitial()
    {
        return IPInitial;
    }

    public string getIPLast()
    {
        return IPLast;
    }

    public DateTime getDateCreated()
    {
        return dateCreated;
    }

    public DateTime getDateLastLoggedIn()
    {
        return dateLastLoggedIn;
    }

    public DateTime getDateLastLoggedOut()
    {
        return dateLastLoggedOut;
    }

    public int getActive()
    {
        return active;
    }

    public int getUserLevel()
    {
        return userLevel;
    }

    public string getFirstName()
    {
        return firstName;
    }

    public string getMiddleName()
    {
        return middleName;
    }

    public string getLastName()
    {
        return lastName;
    }

    public string getEmail()
    {
        return email;
    }

    public string getPhoneNumber()
    {
        return phoneNumber;
    }

    public string getDisplayName()
    {
        return displayName;
    }

    public string getDisplayImage()
    {
        return displayImage;
    }

    public string getUserStatus()
    {
        return userStatus;
    }

    public string getInfoForumFooter()
    {
        return infoForumFooter;
    }


    private void setID(int i)
    {
        ID = i;
    }

    private void setOwnerAlias(string s)
    {
        ownerAlias = s;
    }

    private void setUserPW(string s)
    {
        userPW = s;
    }

    private void setIPInitial(string s)
    {
        IPInitial = s;
    }

    private void setIPLast(string s)
    {
        IPLast = s;
    }

    private void setDateCreated(DateTime d)
    {
        dateCreated = d;
    }

    private void setDateLastLoggedIn(DateTime d)
    {
        dateLastLoggedIn = d;
    }

    private void setDateLastLoggedOut(DateTime d)
    {
        dateLastLoggedOut = d;
    }

    private void setActive(int i)
    {
        active = i;
    }

    private void setUserLevel(int i)
    {
        userLevel = i;
    }

    private void setFirstName(string s)
    {
        firstName = s;
    }

    private void setMiddleName(string s)
    {
        middleName = s;
    }

    private void setLastName(string s)
    {
        lastName = s;
    }

    private void setEmail(string s)
    {
        email = s;
    }

    private void setPhoneNumber(string s)
    {
        phoneNumber = s;
    }

    private void setDisplayName(string s)
    {
        displayName = s;
    }

    private void setDisplayImage(string s)
    {
        displayImage = s;
    }

    private void setUserStatus(string s)
    {
        userStatus = s;
    }

    private void setInfoForumFooter(string s)
    {
        infoForumFooter = s;
    }




    //Old Functions from DataBase.cs need to be updated .

   public void updateUserInfo(int UserID, string FirstName, string MiddleName, string LastName, string DisplayName, string Email, string PhoneNumber, string DisplayImage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_Users] SET [firstName] = @FirstName, [middleName] = @MiddleName, [lastName] = @LastName, [Display_Name] = @DisplayName, [eMail] = @Email, [phoneNumber] = @PhoneNumber, [Display_Image] = @DisplayImage WHERE [ID] = @UserID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@FirstName", FirstName);
        cmd.Parameters.AddWithValue("@MiddleName", MiddleName);
        cmd.Parameters.AddWithValue("@LastName", LastName);
        cmd.Parameters.AddWithValue("@DisplayName", DisplayName);
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
        cmd.Parameters.AddWithValue("@DisplayImage", DisplayImage);
        cmd.Parameters.AddWithValue("@UserID", UserID);

        TTDB.TTQuery(cmd);
    } 
    
    public static DataTable searchUsersByName(string firstName, string middleName, string lastName)
    {
        if (firstName.Length == 0 && middleName.Length == 0 && lastName.Length == 0)
        {
            return new DataTable();
        }

        bool first = true;
        string whereClause = "WHERE ";

        if (firstName.Length > 0)
        {
            whereClause += "[firstName] LIKE @firstName ";
            first = false;
        }
        if (middleName.Length > 0)
        {
            if (!first)
                whereClause += "AND ";
            whereClause += "[middleName] LIKE @middleName ";
            first = false;
        }
        if (lastName.Length > 0)
        {
            if (!first)
                whereClause += "AND ";
            whereClause += "[lastName] LIKE @lastName ";
        }

        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT * FROM viewTrackingTool_Users " + whereClause;
        cmd.Parameters.AddWithValue("@firstName", "%" + firstName + "%");
        cmd.Parameters.AddWithValue("@middleName", "%" + middleName + "%");
        cmd.Parameters.AddWithValue("@lastName", "%" + lastName + "%");

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT != null)
            return DT;
        else
            return new DataTable();
    }

    public static DataTable searchUsersByUserName(string userName)
    {
        if (userName.Length == 0)
            return new DataTable();
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT * FROM viewTrackingTool_Users WHERE [ownerAlias] LIKE @userName";
        cmd.Parameters.AddWithValue("@userName", "%" + userName + "%");

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT != null)
            return DT;
        else
            return new DataTable();
    }

    public static DataTable getUserData(int userID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM viewTrackingTool_Users WHERE [ID] = @userID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userID", userID);

        return TTDB.TTQuery(cmd);
    }

    public string getActiveUserName(string IP)
    {
        DataTable DT = getActiveUserData(IP);
        if (DT.Rows.Count == 1)
            return DT.Rows[0]["ownerAlias"].ToString();
        else
            return "";
    }

    public DataTable getActiveUserData(string IP)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM viewTrackingTool_Active_Users WHERE [User_IP] = @IP";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@IP", IP);

        setIPLast(IP);

        DataTable DT = TTDB.TTQuery(cmd);

        if (DT != null)
        {
            if (DT.Rows.Count == 1)
            {
                user_UpdateActive();
                return DT;
            }
            else
                return new DataTable();
        }
        else
            return new DataTable();
    }

    private void user_UpdateActive()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE TrackingTool_Users_Active SET [Last_Active] = CURRENT_TIMESTAMP WHERE [User_IP] = @IP AND [uniqID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@IP", getIPLast());
        cmd.Parameters.AddWithValue("@ID", getID());

        TTDB.TTQuery(cmd);
    }

    public static DataTable getAllActiveUserData()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM view_Active_Users WHERE [Last_Active] > DateADD(mi, -15, Current_TimeStamp)";
        cmd.Parameters.Clear();

        return TTDB.TTQuery(cmd);
    }

    public void Logout_User(string IP)
    {
        DataTable DT = getActiveUserData(IP);

        if (DT.Rows.Count == 1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM TrackingTool_Users_Active WHERE [User_IP] = @IP";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@IP", DT.Rows[0]["User_IP"].ToString());
            TTDB.TTQuery(cmd);
        }
    }

    public bool Login_User(string UserName, string PW, string IP)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM TrackingTool_Users WHERE [ownerAlias] = @UserName AND [user_PW] = @PW";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@UserName", UserName);
        cmd.Parameters.AddWithValue("@PW", PW);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT.Rows.Count == 1)
        {
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM TrackingTool_Users_Active WHERE [uniqID] = @uniqID OR [User_IP] = @IP";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@uniqID", DT.Rows[0]["ID"].ToString());
            cmd.Parameters.AddWithValue("@IP", IP);

            TTDB.TTQuery(cmd);

            cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO TrackingTool_Users_Active VALUES(@uniqID, @userIP, CURRENT_TIMESTAMP)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@uniqID", Int32.Parse(DT.Rows[0]["ID"].ToString()));
            cmd.Parameters.AddWithValue("@userIP", IP);

            TTDB.TTQuery(cmd);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Register_User(string UserName, string firstName, string middleName, string lastName, string email, string phone, string DisplayName, string PW, string IP)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM TrackingTool_Users WHERE [ownerAlias] = @UserName";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@UserName", UserName);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT.Rows.Count == 0)
        {
            //try
            //{
            cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO TrackingTool_Users VALUES(@UserName, @user_PW, @IP, @IP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @Active, @userLevel, @firstName, @middleName, @lastName, @eMail, @phoneNumber, @DisplayName, @DisplayImage, @User_Status, @Footer)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@user_PW", PW);
            cmd.Parameters.AddWithValue("@IP", IP);
            cmd.Parameters.AddWithValue("@Active", 1);
            cmd.Parameters.AddWithValue("@userLevel", 0);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@middleName", middleName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@eMail", email);
            cmd.Parameters.AddWithValue("@phoneNumber", phone);
            cmd.Parameters.AddWithValue("@DisplayName", DisplayName);
            cmd.Parameters.AddWithValue("@DisplayImage", "");
            cmd.Parameters.AddWithValue("@User_Status", "");
            cmd.Parameters.AddWithValue("@Footer", "");

            TTDB.TTQuery(cmd);
            return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }
        else
        {
            return false;
        }
    }
  
    public static int addNewUser(string userName)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO [TrackingTool_Users] VALUES(@userName)";
        cmd.Parameters.AddWithValue("@userName", userName);
        TTDB.TTQuery(cmd);

        cmd = new SqlCommand();
        cmd.CommandText = "SELECT [ID] FROM [TrackingTool_Users] WHERE [ownerAlias] = @userName";
        cmd.Parameters.AddWithValue("@userName", userName);
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

    public static int getUserID(string userName)
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
    }

    public static string getUserAlias(int ID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_Users] WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", ID);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null || DT.Rows.Count == 0)
        {
            return "";
        }
        else
        {
            return DT.Rows[0]["ownerAlias"].ToString();
        }
    }

    public DataTable getUserProjectPermissions(int projectID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @userID AND [projectID] = @projectID ORDER BY [updatedTimestamp]";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@userID", getID());
        cmd.Parameters.AddWithValue("@projectID", projectID);

        DataTable DT = TTDB.TTQuery(cmd);
        if (DT == null)
        {
            DT = new DataTable();
        }
        return DT;
    }

    // Changed getActiveUserData call
    public DataTable CheckNewAcknowledgements()
    {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [TrackingTool_ProjectPermissions] WHERE [user_GivenTo] = @ID AND [userAcknowledged] = 0";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", getID());

            DataTable DT = TTDB.TTQuery(cmd);
            if (DT == null)
            {
                DT = new DataTable();
            }
            return DT;
    }

    //used get call instead of param
    public void acknowledgeMessage(int projectID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE [TrackingTool_ProjectPermissions] SET [userAcknowledged] = 1, [updatedTimestamp] = CURRENT_TIMESTAMP WHERE [ID] = @ID";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@ID", projectID);

        TTDB.TTQuery(cmd);
    }



















}