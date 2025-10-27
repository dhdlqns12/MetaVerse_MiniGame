using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiniGameManagerRegistry
{
    private static IMiniGameManager curManager_Mini;

    public static void Register_MiniGame(IMiniGameManager manager)
    {
        curManager_Mini = manager;
        Debug.Log($"�Ŵ��� ���: {manager.GetType().Name}");
    }

    public static void Unregister_MIniGame(IMiniGameManager manager)
    {
        if (curManager_Mini == manager)
        {
            curManager_Mini = null;
            Debug.Log("�Ŵ��� ����");
        }
    }

    public static IMiniGameManager GetCurrent()
    {
        return curManager_Mini;
    }
}
