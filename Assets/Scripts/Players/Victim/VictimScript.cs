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
    private readonly List<BodyPart> _bodyParts = BodyParts.All();
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
                                                   t => Vector2.Distance(_navMeshAgent.destination, transform.position) >= 1));

        _stateMachine.AddTransition(new Transition(nameof(WanderingState),
                                                   nameof(WaitingState),
                                                   t => 
                                                        Vector2.Distance(_navMeshAgent.destination, transform.position) <= 1));
    }

    private void Start() => _stateMachine.Init();

    private void Update() => _stateMachine.OnLogic();

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
        int tries = 0;
        int maxTries = 30;

        while (tries < maxTries)
        {
            var randomSpot = transform.position + Random.insideUnitSphere * Random.Range(8, 31);

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
            // TODO: i don't think this is working
            Wait();
        }
    }

    public void Wait() => StartCoroutine(WanderAfter(Random.Range(5, 15)));

    private IEnumerator WanderAfter(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Wander();
    }
}

