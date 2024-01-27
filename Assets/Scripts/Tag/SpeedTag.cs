using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTag : BaseTag
{
    public override void OnEnter(Player player)
    {
        player.ChangeState(PlayerState.Run);
    }

    public override void OnExit(Player player)
    {
        player.ChangeState(PlayerState.Walk);
    }
}
