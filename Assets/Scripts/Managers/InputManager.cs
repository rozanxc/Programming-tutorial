using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class InputManager
{

    private static GameControl _gameControl;

    public static void Init(Player myPlayer)
    {
        _gameControl = new GameControl();

        _gameControl.Permanent.Enable();

        _gameControl.InGame.Movement.performed += xer =>
        {
            myPlayer.SetMovementDirection(xer.ReadValue<Vector3>());
            
        };
        _gameControl.InGame.Jump.performed += xer => //NEW - ADDED A JUMP FUNCTION
        {
            myPlayer.Jump();
        };

        _gameControl.InGame.Look.performed += ctx =>
        {
            myPlayer.SetLookRotation(ctx.ReadValue<Vector2>());
        };

        _gameControl.InGame.Shoot.started += ctx =>
        {
            myPlayer.Shoot();
        };
        
        //RELOAD FUNCTION
        _gameControl.InGame.Reload.performed += ctx =>
        {
            myPlayer.Reload();
        };
    }

    public static void SetGameControl()
    {
        _gameControl.InGame.Enable();
        _gameControl.UI.Disable();
    }

    public static void SetUIControl()
    {
        _gameControl.UI.Enable();
        _gameControl.InGame.Disable();
    }


}


