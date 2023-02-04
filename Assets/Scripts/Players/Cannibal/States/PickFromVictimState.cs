using System;

public class PickFromVictimState : BasePlayerState
{
    public PickFromVictimState(CannibalScript cannibal) : base(cannibal) {}

    public override void OnEnter() => Cannibal.ShowMenuForCurrentVictim();
    public override void OnExit() => Cannibal.HideMenu();
}

