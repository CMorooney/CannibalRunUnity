using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : BaseVictimState
{
    public WaitingState(VictimScript victim) : base(victim) { }

    public override void OnEnter()
    {
        Victim.GetComponent<SpriteRenderer>().color = Color.yellow;
        Victim.Wait();
    }
}
