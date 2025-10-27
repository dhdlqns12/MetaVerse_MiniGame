using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    public static event Action<NPCBase> OnNPCInteract;

    public string npcName;

    [SerializeField] private string dialogueFileName;

    private DialogueDataList dialogueData;
    private DialogueUI dialogueUI;
    private bool dialogueLoaded = false;

    protected virtual void Start()
    {
        if (!string.IsNullOrEmpty(dialogueFileName))
        {
            LoadDialogueData();
        }
    }

    private void LoadDialogueData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Data/Dialogue/{dialogueFileName}");
        dialogueData = JsonUtility.FromJson<DialogueDataList>(jsonFile.text);

        if (dialogueData != null && dialogueData.dial != null)
        {
            dialogueLoaded = true;
        }
    }

    public virtual void Interact()
    {
        OnNPCInteract?.Invoke(this);
    }

    public DialogueDataList GetDialogueData()
    {
        return dialogueLoaded ? dialogueData : null;
    }

    public string GetDialogueFileName()
    {
        return dialogueFileName;
    }
}

