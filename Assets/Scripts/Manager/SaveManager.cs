using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string savePath;

    void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    protected override void Init()
    {
        savePath = Path.Combine(Application.persistentDataPath, "SaveData");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    void OnApplicationQuit()
    {
        AutoSave();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"{scene.name}");
        AutoSave();
    }

    public void AutoSave()
    {
        if (GameManager.Instance != null)
        {
            SaveAll(GameManager.Instance.playerData, GameManager.Instance.gameData, GameManager.Instance.leaderboardData);
        }
    }

    #region �÷��̾� ������ ����/�ε�
    public void SavePlayerData(PlayerData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            string filePath = Path.Combine(savePath, "PlayerData.json");
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"PlayerData ���� ����: {e.Message}");
        }
    }

    public PlayerData LoadPlayerData()
    {
        try
        {
            string filePath = Path.Combine(savePath, "PlayerData.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                return data;
            }
            else
            {
                return new PlayerData(); // ���� ������ ���� �� ���� ���۽�  null�����ϱ� ���� ����  ����
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"PlayerData �ҷ����� ����: {e.Message}");
            return new PlayerData();
        }
    }
    #endregion

    #region ���� ������ ����/�ε�
    public void SaveGameData(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(savePath, "GameData.json");
        File.WriteAllText(filePath, json);
    }

    public GameData LoadGameData()
    {
        try
        {
            string filePath = Path.Combine(savePath, "GameData.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                GameData data = JsonUtility.FromJson<GameData>(json);
                return data;
            }
            else
            {
                return new GameData(); // ���� ������ ���� �� ���� ���۽� null�����ϱ� ���� ����  ����
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"GameData �ҷ����� ����: {e.Message}");
            return new GameData();
        }
    }
    #endregion

    #region ���� ������ ����/�ε�
    public void SaveShopData(ShopData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(savePath, "ShopData.json");
        File.WriteAllText(filePath, json);
    }

    public ShopData LoadShopData()
    {
        try
        {
            string filePath = Path.Combine(savePath, "ShopData.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                ShopData data = JsonUtility.FromJson<ShopData>(json);
                return data;
            }
            else
            {
                ShopData data = new ShopData(true);
                data.purchasedProducts.Add(0); // �⺻ ��Ų
                return data;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"ShopData �ҷ����� ����: {e.Message}");
            ShopData data = new ShopData(true);
            data.purchasedProducts.Add(0);
            return data;
        }
    }
    #endregion

    #region �÷��̾� Ŀ���͸���¡ ������ ����/�ε�
    public void SaveCustomizationData(CustomizingData data)
    {

        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(savePath, "CustomizationData.json");
        File.WriteAllText(filePath, json);
    }

    public CustomizingData LoadCustomizationData()
    {

        string filePath = Path.Combine(savePath, "CustomizationData.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CustomizingData data = JsonUtility.FromJson<CustomizingData>(json);
            return data;
        }
        else
        {
            return new CustomizingData { selectedSkinID = 0, selectedVehicleID = 0 };
        }
    }

    #endregion

    #region �������� ������ ����/�ε�
    public void SaveLeaderboardData(LeaderboardData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            string filePath = Path.Combine(savePath, "LeaderboardData.json");
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"LeaderboardData ���� ����: {e.Message}");
        }
    }

    public LeaderboardData LoadLeaderboardData()
    {
        try
        {
            string filePath = Path.Combine(savePath, "LeaderboardData.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
                return data;
            }
            else
            {
                Debug.LogWarning("LeaderboardData ������ ����_���� ����");
                return new LeaderboardData(true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"LeaderboardData �ҷ����� ����: {e.Message}");
            return new LeaderboardData(true);
        }
    }
    #endregion

    #region ��ü ����/����
    public void SaveAll(PlayerData playerData, GameData gameData, LeaderboardData leaderboardData)
    {
        SavePlayerData(playerData);
        SaveGameData(gameData);
        SaveLeaderboardData(leaderboardData);

        ShopManager shopManager = FindObjectOfType<ShopManager>();
        if (shopManager != null)
        {
            SaveShopData(shopManager.shopData);
            SaveCustomizationData(shopManager.customizingData);
        }
    }

    public void DeleteAllSaveData()
    {
        if (Directory.Exists(savePath))
        {
            Directory.Delete(savePath, true);
            Directory.CreateDirectory(savePath);
        }
    }
    #endregion
}
