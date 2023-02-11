using UnityEngine;

public class BasePlayerMovementState : BasePlayerState
{
    private Vector2 _velocity = new Vector2(0, 0);

    private Vector2 _facing = Vector2.down;

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
        SetAnimation(input);
    }

    private bool IsBlocked(RaycastHit2D cast) => cast.collider != null &&
                                                 cast.collider.CompareTagsOR("block player",
                                                                             "victim");

    protected virtual void Move(Vector2 input, RaycastHit2D cast)
    {
        if(input.x != 0 || input.y != 0)
        {
            _facing = input switch
            {
                { x: > 0 } => Vector2.right,
                { x: < 0 } => Vector2.left,
                { y: > 0 } => Vector2.up,
                { y: < 0} => Vector2.down,
                _ => Vector2.down
            };
        }

        if (!IsBlocked(cast))
        {
            var adjustedSpeed = Cannibal.Speed * SpeedModifier;

            _velocity.x = input.x != 0 ?
                            Mathf.MoveTowards(_velocity.x,
                                              input.x * adjustedSpeed,
                                              Cannibal.Acceleration * Time.deltaTime)
                            : 0;

            _velocity.y = input.y != 0 ?
                            Mathf.MoveTowards(_velocity.y,
                                              input.y * adjustedSpeed,
                                              Cannibal.Acceleration * Time.deltaTime)
                            : 0;

            Cannibal.transform.Translate(_velocity * Time.deltaTime);
        }
    }

    private void SetAnimation(Vector2 input)
    {
        var idle = input.x == 0 && input.y == 0;
        var r = idle ? "Idle" : "Run";

        var animName = _facing switch
        {
            Vector2 v when v == Vector2.up => $"Cannibal{r}Back",
            Vector2 v when v == Vector2.down => $"Cannibal{r}Front",
            _ => $"Cannibal{r}Side"
        };

        Cannibal.SetAnimation(animName, _facing == Vector2.left);
    }
}

