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
    public int nextDialId;  // 다음 대화 ID -1이면 대화 종료
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
