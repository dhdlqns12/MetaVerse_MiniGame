using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [SerializeField] private List<Product> vehicleProducts = new List<Product>();

    private Dictionary<int, Product> vehicleList;
    private Dictionary<int, VehicleData> vehicleDataList;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        LoadVehicleData();
    }

    #region 탈 것 정보 불러오기
    private void LoadVehicleData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/VehicleData");

        if (jsonFile == null)
        {
            return;
        }

        VehicleDataList jsonData = JsonUtility.FromJson<VehicleDataList>(jsonFile.text);

        vehicleDataList = new Dictionary<int, VehicleData>();

        foreach (var vehicle in jsonData.vehicles)
        {
            vehicleDataList[vehicle.id] = vehicle;
        }

        vehicleList = new Dictionary<int, Product>();

        foreach (var product in vehicleProducts)
        {
            if (product.type == ProductType.Vehicle)
            {
                vehicleList[product.productID] = product;
            }
        }     
    }

    public Product GetVehicle(int vehicleId)
    {
        if (vehicleList != null && vehicleList.TryGetValue(vehicleId, out var vehicle))
        {
            return vehicle;
        }

        return null;
    }

    public VehicleData? GetVehicleData(int vehicleId)
    {
        if (vehicleDataList != null && vehicleDataList.TryGetValue(vehicleId, out var data))
        {
            return data;
        }

        return null;
    }

    public float GetVehicleSpeed(int vehicleId)
    {
        VehicleData? data = GetVehicleData(vehicleId);
        return data?.speed ?? 5f;
    }

    public List<Product> GetAllVehicles()
    {
        return vehicleProducts;
    }
    #endregion
}
