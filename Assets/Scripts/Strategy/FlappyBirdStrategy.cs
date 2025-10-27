using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlappyBirdStrategy : IPlayerStrategy
{
    private PlayerController player;
    private Animator animator;
    private Rigidbody2D rigid2D;

    [Header("이동 설정")]
    [SerializeField] private float forwardSpeed;      // 속도
    [SerializeField] private float jumpForce;         // 점프
    [SerializeField] private float gravity;  // 중력

    public FlappyBirdStrategy(float forwardSpeed = 3f, float jumpForce = 8f)
    {
        this.forwardSpeed = forwardSpeed;
        this.jumpForce = jumpForce;
    }

    public void FixedUpdate()
    {
        rigid2D.velocity = new Vector2(forwardSpeed, rigid2D.velocity.y);
    }

    public void Update()
    {
    }

    public void Init(PlayerController player)
    {
        gravity = 2f;

        this.player = player;
        this.animator = player._animator;
        this.rigid2D = player._rigid2D;

        rigid2D.gravityScale = gravity;
    }

    #region 플레이어 제어
    public void HandleMove(InputAction.CallbackContext context)
    {
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Jump();
    }

    public void HandleAttack(InputAction.CallbackContext context)
    {
    }

    public void HandleInteract(InputAction.CallbackContext context)
    {
    }

    private void Jump()
    {
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
    }

    public void Clean()
    {
        rigid2D.gravityScale = 0f;
        rigid2D.velocity = Vector2.zero;
    }
    #endregion
}
