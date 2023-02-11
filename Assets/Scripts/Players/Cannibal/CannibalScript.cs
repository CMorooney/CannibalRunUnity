using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FSM;

public delegate void HealthChanged(float newHealth);
public delegate void BiteTaken(float remainingFoodPercent);
public delegate void InventoryChanged(BodyPart bodyPart);

public class CannibalScript : MonoBehaviour
{
    public float Speed = 10;
    public float Acceleration = 300;
    public float Deceleration = 1000;
    public float CollisionDistanceThreshold = 0.1f;
    public float HealthPerTurn = -.0001f;
    public float BiteAmount = 0.02f;

    public event HealthChanged HealthChanged;
    public event BiteTaken BiteTaken;
    public event InventoryChanged InventoryChanged;

    private float _health = 1f;// let's say 0 - 1 now
    private VictimScript _currentVictim;
    private BodyPart _currentBodyPartEating;
    private ActionMenuScript _actionMenuScript;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
 
    private CRStateMachine _stateMachine;

    private readonly List<BodyPart> _bodyParts = BodyParts.All();

    public void Awake()
    {
        _stateMachine = new CRStateMachine();
        _stateMachine.AddState(nameof(ProwlingState), new ProwlingState(this));
        _stateMachine.AddState(nameof(PickFromVictimState), new PickFromVictimState(this));
        _stateMachine.AddState(nameof(EatingState), new EatingState(this));
        _stateMachine.AddState(nameof(DeadState), new DeadState(this));

        _stateMachine.AddTransition(new Transition(nameof(ProwlingState),
                                                         nameof(PickFromVictimState),
                                                         t => _currentVictim != null));

        _stateMachine.AddTransition(new Transition(nameof(PickFromVictimState),
                                                   nameof(EatingState),
                                                   t => _currentBodyPartEating != null));

        _stateMachine.AddTransition(new Transition(nameof(EatingState),
                                                   nameof(ProwlingState),
                                                   t => _currentBodyPartEating == null));

        _stateMachine.AddTransitionFromAny(new Transition(
                                                    string.Empty,    // 'From' can be left empty, as it has no meaning in this context
                                                    nameof(DeadState),
                                                    t => _health <= 0));

        _actionMenuScript = GetComponentInChildren<ActionMenuScript>();
        _actionMenuScript.ItemSelected += MenuItemSelected;

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start() => _stateMachine.Init();

    public void OnDestroy() => _actionMenuScript.ItemSelected -= MenuItemSelected;

    public void Update() => _stateMachine.OnLogic();

    public void SetVictim(VictimScript victim)
    {
        _currentBodyPartEating = null;
        _currentVictim = victim;
    }

    public void ShowMenuForCurrentVictim() => _actionMenuScript.Show(_currentVictim.GetAvailableBodyParts());

    public void MenuItemSelected(BodyPart bodyPart)
    {
        _currentVictim.TakeBodyPart(bodyPart.Name);
        SetBodyPart(bodyPart);
    }

    public void SetBodyPart(BodyPart bodyPart)
    {
        _currentVictim = null;
        _currentBodyPartEating = bodyPart;
        InventoryChanged?.Invoke(bodyPart);
    }

    public void HideMenu() => _actionMenuScript.Hide();

    public void TakeBite(bool gobble)
    {
        if (_currentBodyPartEating == null) return;

        var amount = gobble ? BiteAmount / 2 : BiteAmount;

        _currentBodyPartEating.Health = Math.Clamp(
                                            _currentBodyPartEating.Health - amount,
                                            0,
                                            _currentBodyPartEating.MaxHealth);
        AddHealth(amount);

        BiteTaken?.Invoke(_currentBodyPartEating.Health / _currentBodyPartEating.MaxHealth);

        // just a little threshold to stop it from being a teeny teeny number the UI wont be able to display as "organ health"// just a little threshold to stop it from being a teeny teeny number the UI wont be able to display as "organ health"
        if(_currentBodyPartEating.Health <= 0.00001)
        {
            _currentBodyPartEating = null;
            InventoryChanged?.Invoke(null);
        }
    }

    public void AddHealth(float amount)
    {
        _health = Math.Clamp(
                    _health + amount,
                    Constants.Player.MinHealth,
                    Constants.Player.MaxHealth);

        HealthChanged?.Invoke(_health);
    }

    public void SetAnimation(string name, bool flipX)
    {
        _spriteRenderer.flipX = flipX;
        _animator.Play(name);
    }
}

