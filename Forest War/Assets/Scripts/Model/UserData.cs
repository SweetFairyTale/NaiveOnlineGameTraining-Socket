using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UserData
{
    public int ID { get; private set; }
    public string Username { get; private set; }
    public int TotalCount { get; set; }
    public int WinCount { get; set; }

    public UserData(string username, int totalCount, int winCount)
    {
        ID = -1;
        Username = username;
        TotalCount = totalCount;
        WinCount = winCount;
    }

    public UserData(int id, string username, int totalCount, int winCount)
    {
        ID = id;
        Username = username;
        TotalCount = totalCount;
        WinCount = winCount;
    }

    public UserData(string formatUserData)
    {
        string[] strs = formatUserData.Split(',');
        ID = int.Parse(strs[0]);
        Username = strs[1];
        TotalCount = int.Parse(strs[2]);
        WinCount = int.Parse(strs[3]);
    }
}

