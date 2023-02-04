using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using FSM;
using Random = UnityEngine.Random;
using static UnityEngine.AI.NavMesh;

public delegate void RequestNewWanderingLocation();

public class VictimScript : MonoBehaviour
{
    public RequestNewWanderingLocation RequestNewWanderingLocation;

    private NavMeshAgent _navMeshAgent;
    private Transform _alertSource;
    private readonly List<BodyPart> _bodyParts = BodyParts.All();
    private const float _walkingSpeed = 3.5f;
    private const float _runningSpeed = 7f;
    private float _health = 1f;// let's say 0 - 1

    private readonly CRStateMachine _stateMachine = new CRStateMachine();

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _stateMachine.AddState(nameof(WanderingState), new WanderingState(this));
        _stateMachine.AddState(nameof(WaitingState), new WaitingState(this));
        _stateMachine.AddState(nameof(AlertedState), new AlertedState(this));

        _stateMachine.AddTransition(new Transition(nameof(WaitingState),
                                                   nameof(WanderingState),
                                                   t => !IsAtNavDestination()));

        _stateMachine.AddTransition(new Transition(nameof(WanderingState),
                                                   nameof(WaitingState),
                                                   t => IsAtNavDestination()));

        _stateMachine.AddTransitionFromAny(new Transition("",
                                                          nameof(AlertedState),
                                                          t => _alertSource != null));

        _stateMachine.AddTransition(new Transition(nameof(AlertedState),
                                                   nameof(WanderingState),
                                                   t => _alertSource == null));
    }

    private void Start() => _stateMachine.Init();

    private void Update() => _stateMachine.OnLogic();

    private void SetRandomDestination(int rangeMin, int rangeMax, bool allowWait)
    {
        int tries = 0;
        int maxTries = allowWait ? 30 : 1000000000;// don't want to lock up unity so just making this really large...

        while (tries < maxTries)
        {
            var randomSpot = transform.position + Random.insideUnitSphere * Random.Range(rangeMin, rangeMax);

            //TODO: AllAreas seems wrong but I get no hits otherwise (should be bitmask for "Walkable"?)
            if (SamplePosition(randomSpot, out var hit, 1, AllAreas))
            {
                _navMeshAgent.destination = hit.position;
                break;
            }
            tries++;
        }

        if(tries > maxTries -1)
        {
            Wait();
        }
    }

    private IEnumerator WanderAfter(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Wander();
    }

    public List<BodyPart> GetAvailableBodyParts() => _bodyParts;

    public void TakeBodyPart(string name)
    {
        var toRemove = _bodyParts.FirstOrDefault(b => b.Name == name);
        if(toRemove != null)
        {
            _health = Math.Clamp(_health - toRemove.MaxHealth, 0, 1);
            _bodyParts.Remove(toRemove);
        }
    }

    public void Wander()
    {
        _navMeshAgent.speed = _walkingSpeed;
        SetRandomDestination(8, 31, true);
    }

    public void Run()
    {
        _navMeshAgent.speed = _runningSpeed;
        SetRandomDestination(4, 10, false);
    }

    public void Wait() => StartCoroutine(WanderAfter(Random.Range(5, 15)));

    public void Alert(Transform source) => _alertSource = source;

    public float DistanceFromAlert => Vector2.Distance(_alertSource.position, transform.position);

    public bool IsAtNavDestination() => Vector2.Distance(_navMeshAgent.destination, transform.position) <= 1;

    public void Calm() => _alertSource = null;
}

