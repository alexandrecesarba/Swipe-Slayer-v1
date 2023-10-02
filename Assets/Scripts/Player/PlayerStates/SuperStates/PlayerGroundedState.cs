using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    #region Variables

    #endregion
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
}
