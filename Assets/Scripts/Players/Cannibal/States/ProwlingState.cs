using FSM;
using UnityEngine;

public class ProwlingState : BasePlayerMovementState
{
    public ProwlingState(CannibalScript cannibal) : base(cannibal) {}

    protected override void Move(Vector2 input, RaycastHit2D cast)
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            cast.collider != null &&
            cast.collider.CompareTag("victim"))
        {
            var victim = cast.collider.gameObject.GetComponent<VictimScript>();
            if (victim.IsAtNavDestination())
            {
                Cannibal.SetVictim(victim);
            }
            else
            {
                victim.Alert(Cannibal.transform);
            }
        }
        else
        {
            base.Move(input, cast);
        }
    }
}

