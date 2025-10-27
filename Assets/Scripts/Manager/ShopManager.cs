using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("직접 참조")]
    [SerializeField] private PlayerCustomizing playerCustomizing;
    [SerializeField] private VehicleManager vehicleManager;

    [Header("상품 리스트")]
    public List<Product> characterSkin;
    public List<Product> vehicle;

    public ShopData shopData;
    public CustomizingData customizingData;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        LoadData();
        playerCustomizing.ApplyCurrentCustomization(this,vehicleManager);
    }

    #region 구매 관련
    public bool PurchaseProduct(int productId)
    {
        if (shopData.purchasedProducts.Contains(productId))
        {
            return false;
        }
        Product product = GetProductId(productId);

        if (product == null)
            return false;

        if (GameManager.Instance.Gold >= product.price)
        {
            GameManager.Instance.Gold -= product.price;
            shopData.purchasedProducts.Add(productId);

            SaveManager.Instance.SaveShopData(shopData);
            return true;
        }
        return false;
    }

    public bool IsPurchased(int productID)
    {
        return shopData.purchasedProducts.Contains(productID) || productID == 0;
    }
    #endregion

    #region 아이템 선택
    public void SelectProduct(int productId)
    {
        if (!shopData.purchasedProducts.Contains(productId) && productId != 0)
        {
            return;
        }

        Product product = GetProductId(productId);

        if (product == null) return;

        if (product.type == ProductType.CharacterSkin)
        {
            customizingData.selectedSkinID = productId;
            playerCustomizing.ApplyCharacterSkin(product.productSprite, product.animatorOverride);
        }
        else if (product.type == ProductType.Vehicle)
        {
            customizingData.selectedVehicleID = productId;
            playerCustomizing.ApplyVehicle(productId, vehicleManager);
        }

        SaveManager.Instance.SaveCustomizationData(customizingData);
    }

    public Product GetProductId(int productId)
    {
        List<Product> allProducts = new List<Product>();
        allProducts.AddRange(characterSkin);
        allProducts.AddRange(vehicle);

        return allProducts.Find(p => p.productID == productId);
    }
    #endregion

    #region 데이터 불러오기
    private void LoadData()
    {
        shopData = SaveManager.Instance.LoadShopData();
        customizingData = SaveManager.Instance.LoadCustomizationData();
        vehicle = vehicleManager.GetAllVehicles();
    }
    #endregion
}

