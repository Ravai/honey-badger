using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for User
/// </summary>
public class User
{
    private int ID;
    private string ownerAlias;

	public User(int ID)
	{
		
	}

    public int getID()
    {
        return ID;
    }
    public string getAlias()
    {
        return ownerAlias;
    }
}