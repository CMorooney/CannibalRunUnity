using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class CRStateMachine : StateMachine
{
    public void AddState<T>() where T : CRStateBase, new () => AddState(typeof(T).Name, new T());
}

