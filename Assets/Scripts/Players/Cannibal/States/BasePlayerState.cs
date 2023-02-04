using FSM;
using UnityEngine;

public class BasePlayerState : CRStateBase
{
    protected readonly CannibalScript Cannibal;

    public BasePlayerState(CannibalScript cannibal) : base(needsExitTime: false)
    {
        Cannibal = cannibal;
    }

    public override void OnLogic()
    {
        Cannibal.AddHealth(Cannibal.HealthPerTurn);
    }
}

