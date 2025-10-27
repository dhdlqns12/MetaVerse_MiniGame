using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dialogue
{
    public int id;
    public string speaker; 
    public string text;
    public List<DialogueChoice> choices; 
    public int nextDialId;  // ���� ��ȭ ID -1�̸� ��ȭ ����
}

[Serializable]
public class DialogueChoice
{
    public string text; 
    public int nextDialId; 
}

[Serializable]
public class DialogueDataList
{
    public List<Dialogue> dial;
}
