using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UserData
{
    public UserData(string username, int totalCount, int winCount)
    {
        Username = username;
        TotalCount = totalCount;
        WinCount = winCount;
    }

    public string Username { get; private set; }
    public int TotalCount { get; private set; }
    public int WinCount { get; private set; }
}

