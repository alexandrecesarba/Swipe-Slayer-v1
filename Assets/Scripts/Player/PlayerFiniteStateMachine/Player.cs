using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine {get; private set;}

    public PlayerIdleState IdleState {get; private set;}
    public PlayerMoveState MoveState {get; private set;}
    public MovingObject movingScript;

    public Animator Anim {get; private set;}
    public PlayerInputHandler InputHandler {get; private set;}

    [SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        movingScript = GetComponent<MovingObject>();
        movingScript.OnMove += () => StateMachine.ChangeState(MoveState);
        movingScript.OnMoveEnd += () => StateMachine.ChangeState(IdleState);

        //inicia a StateMachina com o state idle
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}
