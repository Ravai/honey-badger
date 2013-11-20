using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Roles
/// </summary>
public class Roles
{
    public enum Roles_t { ProjectManager, Supervisor, TeamMember, Client, Public };
    enum Read_t { False, True };
    enum Write_t { False, True };

    /// <summary>
    /// WIll return a string representation of the role passed in.
    /// </summary>
    /// <param name="role">The role to display</param>
    /// <returns>String of the permissions in form [Project- x/x], [Boards- x/x]</returns>
    public static String retrievePermissions(int role){
        string stringBuilder = "";

        switch(role){
            case (int)Roles_t.ProjectManager:
                stringBuilder = ProjectManager.toString();
                break;
            case (int)Roles_t.Supervisor:
                stringBuilder = Supervisor.toString();
                break;
            case (int)Roles_t.TeamMember:
                stringBuilder = TeamMember.toString();
                break;
            case (int)Roles_t.Client:
                stringBuilder = Client.toString();
                break;
            case (int)Roles_t.Public:
                stringBuilder = Public.toString();
                break;
            default:
                stringBuilder = "Select a role to view permission levels";
                break;
        }
        return stringBuilder;
    }

    /// <summary>
    /// WIll return an array representation of permissions.
    /// </summary>
    /// <param name="role">The role to get the permissions forl</param>
    /// <returns>4 index array of the permissions in the form:
    ///          index0: read property of project.
    ///          index1: write property of project.
    ///          index2: read property of board.
    ///          index3: write property of board.
    ///          </returns>
    public static int[] getPermissions(int role)
    {
        int[] permissions = new int[4];
        switch (role)
        {
            case (int)Roles_t.ProjectManager:
                permissions = ProjectManager.getPermissions();
                break;
            case (int)Roles_t.Supervisor:
                permissions = Supervisor.getPermissions();
                break;
            case (int)Roles_t.TeamMember:
                permissions = TeamMember.getPermissions();
                break;
            case (int)Roles_t.Client:
                permissions = Client.getPermissions();
                break;
            case (int)Roles_t.Public:
                permissions = Public.getPermissions();
                break;
        }
        return permissions;
    }
    private class ProjectManager
    {
        public static int readProject = (int)Read_t.True;
        public static int writeProject = (int)Write_t.True;
        public static int readBoard = (int)Read_t.True;
        public static int writeBoard = (int)Write_t.True;

        public static int[] getPermissions()
        {
            return new int[] { readProject, writeProject, readBoard, writeBoard };
        }

        // Will return a representation of the permissions
        public static string toString()
        {
            return "[Project - Read/Write] [Boards - Read/Write]";
        }
    }

    private class Supervisor
    {
        public static int readProject = (int)Read_t.True;
        public static int writeProject = (int)Write_t.True;
        public static int readBoard = (int)Read_t.True;
        public static int writeBoard = (int)Write_t.True;

        public static int[] getPermissions()
        {
            return new int[] { readProject, writeProject, readBoard, writeBoard };
        }

        public static string toString()
        {
            return "[Project - Read/Write] [Boards - Read/Write]";
        }
    }

    private class TeamMember
    {
        public static int readProject = (int)Read_t.True;
        public static int writeProject = (int)Write_t.True;
        public static int readBoard = (int)Read_t.True;
        public static int writeBoard = (int)Write_t.True;

        public static int[] getPermissions()
        {
            return new int[] { readProject, writeProject, readBoard, writeBoard };
        }

        public static string toString()
        {
            return "[Project - Read/Write] [Boards - Read/Write]";
        }
    }

    private class Client
    {
        public static int readProject = (int)Read_t.False;
        public static int writeProject = (int)Write_t.False;
        public static int readBoard = (int)Read_t.True;
        public static int writeBoard = (int)Write_t.True;

        public static int[] getPermissions()
        {
            return new int[] { readProject, writeProject, readBoard, writeBoard };
        }

        public static string toString()
        {
            return "[Project - None] [Boards - Read/Write]";
        }
    }

    private class Public
    {
        public static int readProject = (int)Read_t.False;
        public static int writeProject = (int)Write_t.False;
        public static int readBoard = (int)Read_t.True;
        public static int writeBoard = (int)Write_t.False;

        public static int[] getPermissions()
        {
            return new int[] { readProject, writeProject, readBoard, writeBoard };
        }

        public static string toString()
        {
            return "[Project - None] [Boards - Read]";
        }
    }
}