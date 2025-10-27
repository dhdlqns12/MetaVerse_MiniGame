using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text speakerText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Transform choiceButtonContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    private DialogueDataList currentDialogue;
    private int currentDialId;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }

        dialoguePanel?.SetActive(false);
    }

    #region 대화 설정
    public void StartDialogue(DialogueDataList dialogueData)
    {
        currentDialogue = dialogueData;
        currentDialId = 0;

        dialoguePanel?.SetActive(true);
        ShowDialogue(currentDialId);
    }

    private void ShowDialogue(int dialId)
    {
        Dialogue dialogue = GetDialogueById(dialId);

        if (dialogue == null)
        {
            EndDialogue();
            return;
        }

        if (speakerText != null)
        {
            speakerText.text = dialogue.speaker;
        }

        if (dialogueText != null)
        {
            dialogueText.text = dialogue.text;
        }

        if (dialogue.choices != null && dialogue.choices.Count > 0)
        {
            ShowChoices(dialogue.choices);

            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(false);
            }
        }
        else
        {
            ClearChoices();

            if (nextButton != null)
            {
                if (dialogue.nextDialId == -1)
                {
                    nextButton.gameObject.SetActive(true);
                    Text btnText = nextButton.GetComponentInChildren<Text>();
                    if (btnText != null) btnText.text = "닫기";
                }
                else
                {
                    nextButton.gameObject.SetActive(true);
                    Text btnText = nextButton.GetComponentInChildren<Text>();
                    if (btnText != null) btnText.text = "다음";
                }
            }
        }

        currentDialId = dialId;
    }

    private void ShowChoices(List<DialogueChoice> choices)
    {
        ClearChoices();

        foreach (DialogueChoice choice in choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            Button btn = btnObj.GetComponent<Button>();
            Text btnText = btnObj.GetComponentInChildren<Text>();

            if (btnText != null)
            {
                btnText.text = choice.text;
            }

            if (btn != null)
            {
                int nextDialId = choice.nextDialId;
                btn.onClick.AddListener(() => OnChoiceSelected(nextDialId));
            }
        }
    }

    private void ClearChoices()
    {
        if (choiceButtonContainer == null) return;

        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnChoiceSelected(int nextDialId)
    {
        if (nextDialId == -1)
        {
            EndDialogue();
        }
        else
        {
            ShowDialogue(nextDialId);
        }
    }

    private void OnNextButtonClick()
    {
        Dialogue dialogue = GetDialogueById(currentDialId);

        if (dialogue == null)
        {
            EndDialogue();
            return;
        }

        if (dialogue.nextDialId == -1)
        {
            EndDialogue();
        }
        else
        {
            ShowDialogue(dialogue.nextDialId);
        }
    }

    private Dialogue GetDialogueById(int id)
    {
        if (currentDialogue == null || currentDialogue.dial == null)
            return null;

        return currentDialogue.dial.Find(dialogue => dialogue.id == id);
    }

    private void EndDialogue()
    {
        dialoguePanel?.SetActive(false);
        ClearChoices();
    }
    #endregion
}

