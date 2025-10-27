using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour // 전략 패턴으로 한 스크립트로 모든 미니게임 밑 메타버스 상황 제어
{
    [Header("전략")]
    [SerializeField] private StrategyType initStrategy;
    [SerializeField] private IPlayerStrategy curStrategy;

    [Header("속성")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigid2D;
    public SpriteRenderer spriteRenderer;

    [Header("이동")]
    [SerializeField] Vector2 input_Vec2;

    public Animator _animator => animator;
    public Rigidbody2D _rigid2D => rigid2D;
    private Player_Input player_Input;

    public enum StrategyType
    {
        MainGame,
        FlappyBird,
        DodgePoop
    }

    private void Awake()
    {
        player_Input = new Player_Input();
        player_Input.Enable();
    }

    private void Start()
    {
        Init();
        if (GameManager.Instance != null)
        {
            SetStrategy(GameManager.Instance.curStrategy);
            Debug.Log($"전략 적용: {GameManager.Instance.curStrategy}");
        }
        else
        {
            SetStrategy(initStrategy);
        }
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void FixedUpdate()
    {
        curStrategy?.FixedUpdate();
    }

    private void Update()
    {
        curStrategy?.Update();
    }

    private void Init()
    {
        initStrategy = StrategyType.MainGame;
    }

    public void SetStrategy(StrategyType strategyType) // 전략 패턴을 플레이어만 사용하는게 아니라 추후 다른 곳에서도 사용한다면 StartegyManager를 두고 이벤트 구독/해제(옵저버 패턴)로 사용하는게 좋다고 하심
    {
        curStrategy?.Clean();

        Debug.Log($"전략 변경 시도: {strategyType}");

        switch (strategyType)
        {
            case StrategyType.MainGame:
                curStrategy = new MetaVerseStrategy(5f);
                break;

            case StrategyType.FlappyBird:
                curStrategy = new FlappyBirdStrategy();
                break;

            case StrategyType.DodgePoop:
                curStrategy = new DodgePoopStrategy();
                break;
        }

        curStrategy?.Init(this);
        initStrategy = strategyType;
        Debug.Log($"전략 변경 완료: {curStrategy?.GetType().Name}");
    }

    void OnDestroy()
    {
        curStrategy?.Clean();
    }

    #region 이벤트 구독/해제
    private void SubscribeEvents()
    {
        // 움직임 이벤트 구독
        player_Input.PlayerInput.Move.performed += OnMove;
        player_Input.PlayerInput.Move.canceled += OnMove;
        // 상호작용 이벤트 구독
        player_Input.PlayerInput.Interact.performed += OnInteract;
        player_Input.PlayerInput.Interact.canceled += OnInteract;
        // 점프 이벤트 구독
        player_Input.FlappyBirdInput.Jump.performed += OnJump;
        player_Input.FlappyBirdInput.Jump.canceled += OnJump;
    }

    private void UnsubscribeEvents()
    {
        player_Input.PlayerInput.Move.performed -= OnMove;
        player_Input.PlayerInput.Move.canceled -= OnMove;

        player_Input.PlayerInput.Interact.performed -= OnInteract;
        player_Input.PlayerInput.Interact.canceled -= OnInteract;

        player_Input.FlappyBirdInput.Jump.performed -= OnJump;
        player_Input.FlappyBirdInput.Jump.canceled -= OnJump;

        player_Input.Disable();
    }
    #endregion

    #region 입력 callback
    public void OnMove(InputAction.CallbackContext context)
    {
        curStrategy?.HandleMove(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        curStrategy?.HandleJump(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        curStrategy?.HandleAttack(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        curStrategy?.HandleInteract(context);
    }
    #endregion
}
