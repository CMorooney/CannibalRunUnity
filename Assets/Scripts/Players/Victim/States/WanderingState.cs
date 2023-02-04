using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : BaseVictimState
{
    public WanderingState(VictimScript victim) : base(victim) {}

    public override void OnEnter()
    {
        Victim.GetComponent<SpriteRenderer>().color = Color.magenta;
    }
}
