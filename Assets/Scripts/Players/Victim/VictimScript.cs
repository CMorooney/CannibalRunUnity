using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public delegate void RequestNewWanderingLocation();

public class VictimScript : MonoBehaviour
{
    public RequestNewWanderingLocation RequestNewWanderingLocation;

    private List<BodyPart> _bodyParts = BodyParts.All();
    private float _health = 1f;// let's say 0 - 1

    CRStateMachine _stateMachine = new CRStateMachine();

    private void Awake()
    {
        _stateMachine.AddState(nameof(WaitingState), new WaitingState(this));
        _stateMachine.AddState(nameof(WanderingState), new WanderingState(this));
        _stateMachine.AddState(nameof(AlertedState), new AlertedState(this));
    }

    private void Start()
    {
        _stateMachine.Init();
    }

    public List<BodyPart> GetAvailableBodyParts() => _bodyParts;
    public void TakeBodyPart(string name) => _bodyParts.RemoveAll(b => b.Name == name);
}

