using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class BaseVictimState : StateBase
{
    protected readonly VictimScript Victim;

    public BaseVictimState(VictimScript victim) : base(needsExitTime: false)
    {
        Victim = victim;
    }
}
