using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger_Main : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button[] closeBtn;
    [SerializeField] private Button customizingOpenBtn;

    [Header("텍스트")]
    [SerializeField] private Text bestScore_Txt;
    [SerializeField] private Text gold_Txt;

    [Header("패널")]
    [SerializeField] private GameObject gameGuidePanel;
    [SerializeField] private Text gameNameText;
    [SerializeField] private Text gameDescriptionText;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject customizingPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [SerializeField] private DialogueUI dialogueUI;

    private GameNPC cur_GameNPC;
    private ShopNPC cur_ShopNPC;
    private LeaderboardNPC cur_Leaderboard_NPC;

    private void OnEnable()
    {
        NPCBase.OnNPCInteract += HandleNPCInteract;
        SubscribeEvents();
    }

    private void Start()
    {
        UpdateGold(GameManager.Instance.Gold); // 호출 순서 문제로 SubScribeEnvents에 넣을 경우 GameManager에서 Awake에있는 LoadGame보다 먼저 UIManager_Main의 OnEnable이 호출됨... 유니티 에디터에서 강제로 호출될 스크립트르 지정해 줄 순 있지만 Start로 옮겨서 해결                                               // 되기 때문에 STart로 옮겨서 해결...
    }

    private void OnDisable()
    {
        NPCBase.OnNPCInteract -= HandleNPCInteract;
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        startGameBtn?.onClick.AddListener(OnStartGameClicked);
        customizingOpenBtn?.onClick.AddListener(ShowCustomizingPanel);

        GameManager.Instance.OnGoldChanged += UpdateGold;

        if (closeBtn != null)
        {
            foreach (var btn in closeBtn)
            {
                btn?.onClick.AddListener(OnCloseClicked);
            }
        }
    }

    private void UnsubscribeEvents()
    {
        startGameBtn?.onClick.RemoveListener(OnStartGameClicked);
        customizingOpenBtn?.onClick.RemoveListener(ShowCustomizingPanel);

        if (GameManager.Instance != null) // 안해도 되긴 하는데 UnityEditor에서 게임 플레이 종료시 Error뜨는게 보기 싫어서 삽입
        {
            GameManager.Instance.OnGoldChanged -= UpdateGold;
        }

        if (closeBtn != null)
        {
            foreach (var btn in closeBtn)
            {
                btn?.onClick.RemoveListener(OnCloseClicked);
            }
        }

    }

    private void HandleNPCInteract(NPCBase npc)
    {
        CloseAllPanels();
        if (npc is GameNPC _gameNPC)
        {
            cur_GameNPC = _gameNPC;
            ShowGameGuidePanel(cur_GameNPC);
        }
        else if (npc is ShopNPC _shopNPC)
        {
            cur_ShopNPC = _shopNPC;
            ShowShopPanel(cur_ShopNPC);
        }
        else if (npc is LeaderboardNPC _leaderboardNPC)
        {
            cur_Leaderboard_NPC = _leaderboardNPC;
            ShowLeaderboardPanel();
        }
        ShowDialogue(npc);
    }

    #region 대화
    private void ShowDialogue(NPCBase npc)
    {
        DialogueDataList dialogueData = npc.GetDialogueData();

        if (dialogueData != null && dialogueData.dial != null && dialogueData.dial.Count > 0)
        {

            dialogueUI.StartDialogue(dialogueData);
        }
    }
    #endregion

    #region 패널 제어
    private void ShowGameGuidePanel(GameNPC npc)
    {
        if (gameGuidePanel == null) return;

        gameGuidePanel.SetActive(true);
        if (gameNameText != null)
        {
            gameNameText.text = npc.npcName;
        }

        if (gameDescriptionText != null)
        {
            gameDescriptionText.text = npc.gameDescription;
        }
        UpdateCurrentGameBestScore(npc);
    }

    private void ShowShopPanel(ShopNPC npc)
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
    }

    private void ShowCustomizingPanel()
    {
        customizingPanel.SetActive(true);
    }

    private void ShowLeaderboardPanel()
    {
        leaderboardPanel.SetActive(true);
    }

    private void CloseAllPanels()
    {
        gameGuidePanel?.SetActive(false);
        shopPanel?.SetActive(false);
        customizingPanel?.SetActive(false);
        leaderboardPanel?.SetActive(false);

        cur_GameNPC = null; // npc와 바로 상호작용했을때 창이 바로 열리고 닫히는걸 막기 위해
        cur_ShopNPC = null;
        cur_Leaderboard_NPC = null;
    }
    #endregion

    #region 버튼 이벤트
    private void OnStartGameClicked()
    {
        if (cur_GameNPC != null)
        {

            cur_GameNPC.StartGame();
            CloseAllPanels();
        }
    }

    private void OnCloseClicked()
    {
        CloseAllPanels();
    }
    #endregion

    #region 베스트 점수 표시
    private void UpdateCurrentGameBestScore(GameNPC npc)
    {
        if (GameManager.Instance == null) return;

        int bestScore = 0;

        switch (npc.gameSceneName)
        {
            case "FlappyBird":
                bestScore = GameManager.Instance.gameData.bestScore_Flappy;
                break;
            case "DodgePoop":
                bestScore = GameManager.Instance.gameData.bestScore_Poop;
                break;
            default:
                break;
        }

        if (bestScore_Txt != null)
        {
            bestScore_Txt.text = $"{npc.gameSceneName} 최고점수: {bestScore}";
        }
    }
    #endregion

    #region 골드 표시
    private void UpdateGold(int gold)
    {
        Debug.Log($"UpdateGold 호출 gold: {gold}");
        if (gold_Txt != null)
        {
            gold_Txt.text = $"Gold: {gold}";
            Debug.Log($"gold_Txt 업데이트 완료: {gold_Txt.text}");
        }
    }
    #endregion
}
