using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiniGameManagerRegistry
{
    private static IMiniGameManager curManager_Mini;

    public static void Register_MiniGame(IMiniGameManager manager)
    {
        curManager_Mini = manager;
        Debug.Log($"매니저 등록: {manager.GetType().Name}");
    }

    public static void Unregister_MIniGame(IMiniGameManager manager)
    {
        if (curManager_Mini == manager)
        {
            curManager_Mini = null;
            Debug.Log("매니저 해제");
        }
    }

    public static IMiniGameManager GetCurrent()
    {
        return curManager_Mini;
    }
}
