using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : BaseVictimState
{
    public AlertedState(VictimScript victim) : base(victim) {}

    public override void OnEnter()
    {
        Victim.tag = "alertedvictim";
        Victim.GetComponent<SpriteRenderer>().color = Color.red;
        Victim.Run();
    }

    public override void OnLogic()
    {
        if (Victim.DistanceFromAlert > 5)
        {
            Victim.Calm();
        }
        else if(Victim.IsAtNavDestination())
        {
            Victim.Run();
        }
    }
}