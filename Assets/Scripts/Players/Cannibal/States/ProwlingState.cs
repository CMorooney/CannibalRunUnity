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
            Cannibal.SetVictim(cast.collider.gameObject.GetComponent<VictimScript>());
        }
        else
        {
            base.Move(input, cast);
        }
    }
}

