using System;
using UnityEngine;

public class EatingState : BasePlayerMovementState
{
    protected override float SpeedModifier => IsGobbling() ? 1 : 0.5f;

    private long _lastInputTime = 0;

    private bool IsGobbling() => CurrentMilliseconds() - _lastInputTime <= 150;
    private long CurrentMilliseconds() => DateTimeOffset.Now.ToUnixTimeMilliseconds();

    public EatingState(CannibalScript cannibal) : base(cannibal) {}

    public override void OnLogic()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Cannibal.TakeBite(IsGobbling());
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            _lastInputTime = CurrentMilliseconds();
        }

        base.OnLogic();
    }
}

