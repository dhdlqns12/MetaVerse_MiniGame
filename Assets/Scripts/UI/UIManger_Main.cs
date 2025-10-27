using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger_Main : MonoBehaviour
{
    [Header("��ư")]
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button[] closeBtn;
    [SerializeField] private Button customizingOpenBtn;

    [Header("�ؽ�Ʈ")]
    [SerializeField] private Text bestScore_Txt;
    [SerializeField] private Text gold_Txt;

    [Header("�г�")]
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
        UpdateGold(GameManager.Instance.Gold); // ȣ�� ���� ������ SubScribeEnvents�� ���� ��� GameManager���� Awake���ִ� LoadGame���� ���� UIManager_Main�� OnEnable�� ȣ���... ����Ƽ �����Ϳ��� ������ ȣ��� ��ũ��Ʈ�� ������ �� �� ������ Start�� �Űܼ� �ذ�                                               // �Ǳ� ������ STart�� �Űܼ� �ذ�...
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

        if (GameManager.Instance != null) // ���ص� �Ǳ� �ϴµ� UnityEditor���� ���� �÷��� ����� Error�ߴ°� ���� �Ⱦ ����
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

    #region ��ȭ
    private void ShowDialogue(NPCBase npc)
    {
        DialogueDataList dialogueData = npc.GetDialogueData();

        if (dialogueData != null && dialogueData.dial != null && dialogueData.dial.Count > 0)
        {

            dialogueUI.StartDialogue(dialogueData);
        }
    }
    #endregion

    #region �г� ����
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

        cur_GameNPC = null; // npc�� �ٷ� ��ȣ�ۿ������� â�� �ٷ� ������ �����°� ���� ����
        cur_ShopNPC = null;
        cur_Leaderboard_NPC = null;
    }
    #endregion

    #region ��ư �̺�Ʈ
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

    #region ����Ʈ ���� ǥ��
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
            bestScore_Txt.text = $"{npc.gameSceneName} �ְ�����: {bestScore}";
        }
    }
    #endregion

    #region ��� ǥ��
    private void UpdateGold(int gold)
    {
        Debug.Log($"UpdateGold ȣ�� gold: {gold}");
        if (gold_Txt != null)
        {
            gold_Txt.text = $"Gold: {gold}";
            Debug.Log($"gold_Txt ������Ʈ �Ϸ�: {gold_Txt.text}");
        }
    }
    #endregion
}
