using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        SyncRoom,
        QuitRoom,
        StartGame,
        Countdown,
        GoPlay,
        SyncMove,
        SyncArrow,
        CauseDamage,
        GameOver,
        UpdateHistory,
        AbortGame
    }
}
