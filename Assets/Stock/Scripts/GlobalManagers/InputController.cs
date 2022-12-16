using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    private BindingProperties bindingProperties;
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

    public Vector2 LookValue()
    {
        float horizontalValue = Utility.ApplyDeadZone(Actions.lookAction.Value.x, minDeadzone, 1.0f) != 0 ? Mathf.Sign(Actions.lookAction.Value.x) : 0;
        float verticalValue = Utility.ApplyDeadZone(Actions.lookAction.Value.y, minDeadzone, 1.0f) != 0 ? Mathf.Sign(Actions.lookAction.Value.y) : 0;
        return new Vector2(horizontalValue, verticalValue);
    }

    public float ZoomValue()
    {
       return Actions.zoomAction.Value;
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
    public PlayerTwoAxisAction lookAction;
    public PlayerOneAxisAction zoomAction;



    public PlayerAction goLeftAction;
    public PlayerAction goRightAction;

    public PlayerAction goUpAction;
    public PlayerAction goDownAction;

    public PlayerAction lookLeftAction;
    public PlayerAction lookRightAction;

    public PlayerAction lookUpAction;
    public PlayerAction lookDownAction;

    public PlayerAction zoomInAction;
    public PlayerAction zoomOutAction;


    public PlayerAction jumpAction;
    public PlayerAction runAction;

    public PlayerAction mineAction;
    public InputActions()
    {
        goLeftAction = CreatePlayerAction("Go Left");
        goRightAction = CreatePlayerAction("Go Right");
        goUpAction = CreatePlayerAction("Go Up");
        goDownAction = CreatePlayerAction("Go Down");

        lookLeftAction = CreatePlayerAction("Look Left");
        lookRightAction = CreatePlayerAction("Look Right");
        lookUpAction = CreatePlayerAction("Look Up");
        lookDownAction = CreatePlayerAction("Look Down");
       
        zoomInAction = CreatePlayerAction("Zoom In");
        zoomOutAction = CreatePlayerAction("Zoom Out");

        jumpAction = CreatePlayerAction("Jump");
        runAction = CreatePlayerAction("Run");

        mineAction = CreatePlayerAction("Mine");
    }

    public static InputActions CreateWithDefaultBindings(float minDeadzone, float maxDeadzone)
    {
        var playerActions = new InputActions();
        BindingProperties bindingsScriptable  = ScriptableManager.Instance.bindingProperties;

        playerActions.goLeftAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Left").key);
        playerActions.goLeftAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Left").inputControlType);

        playerActions.goRightAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Right").key);
        playerActions.goRightAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Right").inputControlType);

        playerActions.goUpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Up").key);
        playerActions.goUpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Up").inputControlType);

        playerActions.goDownAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Down").key);
        playerActions.goDownAction.AddDefaultBinding(bindingsScriptable.GetBinding("Go Down").inputControlType);



        playerActions.lookLeftAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Left").key);
        playerActions.lookLeftAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Left").inputControlType);

        playerActions.lookRightAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Right").key);
        playerActions.lookRightAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Right").inputControlType);

        playerActions.lookUpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Up").key);
        playerActions.lookUpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Up").inputControlType);

        playerActions.lookDownAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Down").key);
        playerActions.lookDownAction.AddDefaultBinding(bindingsScriptable.GetBinding("Look Down").inputControlType);

        playerActions.zoomInAction.AddDefaultBinding(bindingsScriptable.GetBinding("Zoom In").mouse);
        playerActions.zoomInAction.AddDefaultBinding(bindingsScriptable.GetBinding("Zoom In").inputControlType);

        playerActions.zoomOutAction.AddDefaultBinding(bindingsScriptable.GetBinding("Zoom Out").mouse);
        playerActions.zoomOutAction.AddDefaultBinding(bindingsScriptable.GetBinding("Zoom Out").inputControlType);

        playerActions.moveAction = playerActions.CreateTwoAxisPlayerAction(playerActions.goLeftAction, playerActions.goRightAction, playerActions.goDownAction, playerActions.goUpAction);
        playerActions.lookAction = playerActions.CreateTwoAxisPlayerAction(playerActions.lookLeftAction, playerActions.lookRightAction, playerActions.lookDownAction, playerActions.lookUpAction);
        playerActions.zoomAction = playerActions.CreateOneAxisPlayerAction(playerActions.zoomInAction, playerActions.zoomOutAction);


        playerActions.jumpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Jump").key);
        playerActions.jumpAction.AddDefaultBinding(bindingsScriptable.GetBinding("Jump").inputControlType);

        playerActions.runAction.AddDefaultBinding(bindingsScriptable.GetBinding("Run").key);
        playerActions.runAction.AddDefaultBinding(bindingsScriptable.GetBinding("Run").inputControlType);

        playerActions.mineAction.AddDefaultBinding(bindingsScriptable.GetBinding("Mine").mouse);
        playerActions.mineAction.AddDefaultBinding(bindingsScriptable.GetBinding("Mine").inputControlType);

        return playerActions;
    }
}

