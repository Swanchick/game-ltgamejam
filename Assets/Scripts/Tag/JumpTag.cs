using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTag : BaseTag
{
    public override void OnEnter(Player player)
    {
        player.Jump();
    }
}
