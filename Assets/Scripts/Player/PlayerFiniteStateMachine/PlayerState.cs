using UnityEngine;

// base para todos os states do player
public class PlayerState {

    #region Variables
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;
    
    private string animBoolName;
    #endregion

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    // quando entra em um state
    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
    }

    // quando sai de um state
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
    }

    // chamado todo frame
    public virtual void LogicUpdate()
    {

    }

    // chamado a cada FixedUpdate
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    // chamamos do PhysicsUpdate e do Enter
    public virtual void DoChecks()
    {

    }
}
