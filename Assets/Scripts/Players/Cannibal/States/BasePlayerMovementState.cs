using UnityEngine;

public class BasePlayerMovementState : BasePlayerState
{
    private Vector2 _velocity = new Vector2(0, 0);

    protected virtual float SpeedModifier => 1.0f;

    public BasePlayerMovementState(CannibalScript cannibal) : base(cannibal) { }

    public override void OnLogic()
    {
        base.OnLogic();

        var input = InputManager.CurrentDirectionalInput();

        var cast = Physics2D.BoxCast(origin: Cannibal.transform.position,
                                 size: new Vector2(0.8f, 0.8f),
                                 angle: 0,
                                 direction: input,
                                 distance: Cannibal.CollisionDistanceThreshold,
                                 layerMask: LayerMask.GetMask("Obstacle"));

        Move(input, cast);
    }

    protected virtual void Move(Vector2 input, RaycastHit2D cast)
    {
        if (cast.collider != null &&
            cast.collider.CompareTagsOR("block player", "victim"))
        {
            return;
        }

        var adjustedSpeed = Cannibal.Speed * SpeedModifier;

        _velocity.x = input.x != 0 ?
                        Mathf.MoveTowards(_velocity.x,
                                          input.x * adjustedSpeed,
                                          Cannibal.Acceleration * Time.deltaTime)
                        :
                        Mathf.MoveTowards(_velocity.x,
                                          0,
                                          Cannibal.Deceleration * Time.deltaTime);

        _velocity.y = input.y != 0 ?
                        Mathf.MoveTowards(_velocity.y,
                                          input.y * adjustedSpeed,
                                          Cannibal.Acceleration * Time.deltaTime)
                        :
                        Mathf.MoveTowards(_velocity.y,
                                          0,
                                          Cannibal.Deceleration * Time.deltaTime);

        Cannibal.transform.Translate(_velocity * Time.deltaTime);
    }
}

