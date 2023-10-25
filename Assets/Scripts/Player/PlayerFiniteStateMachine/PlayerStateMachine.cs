using UnityEngine;

public class PlayerStateMachine {

    #region Variables
    public PlayerState CurrentState {get; private set;}
    #endregion

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

}
