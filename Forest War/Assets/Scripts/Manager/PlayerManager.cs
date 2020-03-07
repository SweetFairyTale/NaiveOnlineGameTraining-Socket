using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager {

    private UserData userData;


    public PlayerManager(GameFacade facade) : base(facade) { }

    public UserData UserData { get; set; }
}
