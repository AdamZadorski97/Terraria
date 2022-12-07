using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    [SerializeField] public BindingsScriptable bindingsScriptable;
    [HideInInspector ] private InputActions actions;
    public float minDeadzone = .3f;
    public float maxDeadzone = 1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Vector2 MoveValue()
    {
        float horizontalValue = Utility.ApplyDeadZone(Actions.moveAction.Value.x, minDeadzone, 1.0f) != 0 ? Mathf.Sign(Actions.moveAction.Value.x) : 0;
        float verticalValue = Utility.ApplyDeadZone(Actions.moveAction.Value.y, minDeadzone, 1.0f) != 0 ? Mathf.Sign(Actions.moveAction.Value.y) : 0;
        return new Vector2(horizontalValue, verticalValue);
    }

    public InputActions Actions
    {
        get
        {
            if (actions == null)
            {
                actions = InputActions.CreateWithDefaultBindings(0, 0);
            }
            return actions;
        }
    }
}

public class InputActions : PlayerActionSet
{
    public PlayerTwoAxisAction moveAction;

    public PlayerAction goLeftAction;
    public PlayerAction goRightAction;

    public PlayerAction goUpAction;
    public PlayerAction goDownAction;
    public PlayerAction jumpAction;


    public InputActions()
    {
        goLeftAction = CreatePlayerAction("Go Left");
        goRightAction = CreatePlayerAction("Go Right");
        goUpAction = CreatePlayerAction("Go Up");
        goDownAction = CreatePlayerAction("Go Down");
        jumpAction = CreatePlayerAction("Jump");
    }

    public static InputActions CreateWithDefaultBindings(float minDeadzone, float maxDeadzone)
    {
        var playerActions = new InputActions();
        BindingsScriptable bindingsScriptable = InputController.Instance.bindingsScriptable;

        playerActions.goLeftAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Left").key);
        playerActions.goLeftAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Left").inputControlType);

        playerActions.goRightAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Right").key);
        playerActions.goRightAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Right").inputControlType);

        playerActions.goUpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Up").key);
        playerActions.goUpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Up").inputControlType);

        playerActions.goDownAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Down").key);
        playerActions.goDownAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Down").inputControlType);

        playerActions.moveAction = playerActions.CreateTwoAxisPlayerAction(playerActions.goLeftAction, playerActions.goRightAction, playerActions.goDownAction, playerActions.goUpAction);
       
        playerActions.jumpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Jump").key);
        playerActions.jumpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Jump").inputControlType);

        return playerActions;
    }
}


