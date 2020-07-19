using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CauseDamageRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.CauseDamage;
        base.Awake();
    }

    public void SendRequest(int damage)
    {
        base.SendRequest(damage.ToString());

    }
}
