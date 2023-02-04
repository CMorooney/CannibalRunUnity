using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public abstract class CRStateBase : StateBase
{
    public CRStateBase(bool needsExitTime) : base(needsExitTime: false)
    {
    }
}

