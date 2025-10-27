using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerStrategy
{
    void Init(PlayerController player);

    void HandleMove(InputAction.CallbackContext context);
    void HandleJump(InputAction.CallbackContext context);
    void HandleAttack(InputAction.CallbackContext context);
    void HandleInteract(InputAction.CallbackContext context);

    void Update();
    void FixedUpdate();

    void Clean();
}
