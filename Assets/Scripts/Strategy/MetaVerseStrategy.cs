using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MetaVerseStrategy : IPlayerStrategy
{
    PlayerController player;

    private Animator animator;
    private Rigidbody2D rigid2D;
    private SpriteRenderer spriteRenderer;
    private PlayerCustomizing playerCustomizing;

    [Header("플레이어 이동 및 상호작용")]
    private Vector2 input_Vec2;

    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float interactRange;

    [Header("카메라 및 경계 제한")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float padding;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    private readonly int isMove_Hash = Animator.StringToHash("isMove");

    public MetaVerseStrategy(float moveSpeed = 5f, float padding = 0.5f)
    {
        this.moveSpeed = moveSpeed;
        this.padding = padding;
    }

    public void FixedUpdate()
    {
        rigid2D.velocity = input_Vec2 * moveSpeed;
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

        this.playerCustomizing = Object.FindObjectOfType<PlayerCustomizing>();

        if (playerCustomizing != null)
        {
            playerCustomizing.vehicleEquipped += UpdateMoveSpeed;
        }

        baseMoveSpeed = 5f;

        interactRange = 2f;

        CalculateCameraLimit();

        animator?.SetBool(isMove_Hash, false);

        UpdateMoveSpeed();
    }

    private void UpdateMoveSpeed()
    {
        if (playerCustomizing != null && playerCustomizing.IsRidingVehicle())
        {
            float vehicleSpeed = playerCustomizing.GetCurrentSpeed();
            moveSpeed = baseMoveSpeed + vehicleSpeed;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
        }
    }

    #region 플레이어 제어
    public void HandleMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            input_Vec2 = context.ReadValue<Vector2>();
            if (animator != null && input_Vec2 != Vector2.zero)
            {
                animator.SetBool(isMove_Hash, true);

                if (input_Vec2.x > 0)
                {
                    SetFlip(false); 
                }
                else if (input_Vec2.x < 0)
                {
                    SetFlip(true); 
                }
            }
        }
        if (context.canceled)
        {
            input_Vec2 = Vector2.zero;
            animator?.SetBool(isMove_Hash, false);
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
        if (!context.performed) return;


        Collider2D[] npc = Physics2D.OverlapCircleAll(player.transform.position, interactRange, LayerMask.GetMask("NPC"));

        if (npc.Length > 0)
        {
            Collider2D closeNPC = null;
            float closeDistance = float.MaxValue;

            foreach (var other in npc)
            {
                float distance = Vector2.Distance(player.transform.position, other.transform.position);
                if (distance < closeDistance)
                {
                    closeDistance = distance;
                    closeNPC = other;
                }
            }

            if (closeNPC != null)
            {
                var targetNpc = closeNPC.GetComponent<NPCBase>();
                targetNpc?.Interact();
            }
        }
        else
        {
            Debug.Log("주변에 NPC가 없습니다.");
        }
    }

    private void ClampPosition() // 플레이어 이동 제한
    {
        Vector3 clampedPosition = player.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, min.x, max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, min.y, max.y);
        player.transform.position = clampedPosition;
    }

    private void CalculateCameraLimit() // 카메라 기반 경계 계산
    {
        if (mainCamera == null) return;

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 cameraPos = mainCamera.transform.position;

        min = new Vector2(cameraPos.x - cameraWidth + padding, cameraPos.y - cameraHeight + padding);
        max = new Vector2(cameraPos.x + cameraWidth - padding, cameraPos.y + cameraHeight - padding);
    }

    private void SetFlip(bool flip)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = flip;
        }

        if (playerCustomizing != null)
        {
            playerCustomizing.SetFlip(flip);
        }
    }
    #endregion

    #region 정리
    public void Clean()
    {
        input_Vec2 = Vector2.zero;
        rigid2D.velocity = Vector2.zero;

        if (playerCustomizing != null)
        {
            playerCustomizing.vehicleEquipped -= UpdateMoveSpeed;
        }
    }
    #endregion
}
