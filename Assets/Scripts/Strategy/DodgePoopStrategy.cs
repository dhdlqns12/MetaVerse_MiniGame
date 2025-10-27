using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DodgePoopStrategy : IPlayerStrategy
{
    private PlayerController player;

    private Animator animator;
    private Rigidbody2D rigid2D;
    private SpriteRenderer spriteRenderer;

    [Header("이동 설정")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 input_Vec2;

    [Header("카메라 및 경계 제한")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float padding;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    private readonly int isMove_Hash = Animator.StringToHash("isMove");

    public DodgePoopStrategy(float speed = 5f, float padding = 0.5f)
    {
        this.moveSpeed = speed;
        this.padding = padding;
    }

    public void FixedUpdate()
    {
        rigid2D.velocity = new Vector2(input_Vec2.x * moveSpeed, 0);
        ClampPosition();
    }

    public void Update()
    {
    }

    public void Init(PlayerController player)
    {
        this.player = player;
        this.animator = player._animator;
        this.rigid2D = player._rigid2D;
        this.spriteRenderer = player.spriteRenderer;
        this.mainCamera = Camera.main;

        CalculateCameraLimit();
        animator?.SetBool(isMove_Hash, false);
    }

    #region 플레이어 제어
    public void HandleMove(InputAction.CallbackContext context)
    {
        Vector2 xInput = context.ReadValue<Vector2>();

        float normalizedX = 0f;

        if (xInput.x > 0)            // w랑 d를 동시에 누르면 이동속도가 살짝 느려지는 문제 발생->x값 정규화
        {
            normalizedX = 1f;
        }
        else if (xInput.x < 0)
        {
            normalizedX = -1f;
        }

        input_Vec2 = new Vector2(normalizedX, 0);

        if (animator != null && input_Vec2 != Vector2.zero)
        {
            animator.SetBool(isMove_Hash, true);

            if (spriteRenderer != null)
            {
                if (input_Vec2.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (input_Vec2.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
    }

    public void HandleAttack(InputAction.CallbackContext context)
    {
    }

    public void HandleInteract(InputAction.CallbackContext context)
    {
    }

    public void Clean()
    {
        input_Vec2 = Vector2.zero;
        rigid2D.velocity = Vector2.zero;
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = player.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, min.x, max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, min.y, max.y);
        player.transform.position = clampedPosition;
    }

    private void CalculateCameraLimit()
    {
        if (mainCamera == null) return;

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 cameraPos = mainCamera.transform.position;

        min = new Vector2(cameraPos.x - cameraWidth + padding, cameraPos.y - cameraHeight + padding);
        max = new Vector2(cameraPos.x + cameraWidth - padding, cameraPos.y + cameraHeight - padding);
    }
    #endregion
}


