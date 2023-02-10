using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BasePlayerState
{
    public DeadState(CannibalScript cannibal) : base(cannibal) {}

    public override void OnEnter() => Cannibal.SetAnimation("CannibalDieFront", false);
}
