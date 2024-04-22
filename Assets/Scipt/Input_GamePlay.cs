using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_GamePlay : MonoBehaviour
{
   public static Input_GamePlay Instance { get; private set; }
   public Action OnAction {  get; set; }
   public Action<Vector3> OnMovement {  get; set; }

   private InputReader input; 

    private void Start()
    {
        Instance = this;
        input = new();
        input.Enable();

        input.GamePlay.Jump.performed += (e) =>
        {
            OnAction?.Invoke();
        };
    }

    private void Update()
    {
        var axisX = input.GamePlay.Movement.ReadValue<Vector2>().x;
        var axisZ = input.GamePlay.Movement.ReadValue<Vector2>().y;
        OnMovement?.Invoke(new Vector3(axisX,0,axisZ));

        if (input.GamePlay.Accept.ReadValue<float>() > 0)
        {
            
        }
    }
}
