using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Shop Manager")]
    [SerializeField] private ShopManager shopManager;

    [Header("탭 전환 버튼")]
    [SerializeField] private Button customTabBtn;
    [SerializeField] private Button vehicleTabBtn;
    [SerializeField] private Button customCloseBtn;
    [SerializeField] private Button vehicleCloseBtn;

    [Header("탭 Panel")]
    [SerializeField] private GameObject customTabPanel;
    [SerializeField] private GameObject vehicleTabPanel;

    [Header("상품 리스트")]
    [SerializeField] private Transform customProductList;
    [SerializeField] private Transform vehicleProductList;
    public GameObject productBtnPrefab;

    [Header("소지금")]
    public Text goldTxt;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        customTabBtn?.onClick.AddListener(ShowCustomTab);
        vehicleTabBtn?.onClick.AddListener(ShowVehicleTab);

        customCloseBtn?.onClick.AddListener(CloseSkinTab);
        vehicleCloseBtn?.onClick.AddListener(CloseVehicleTab);
    }

    private void UnsubscribeEvents()
    {
        customTabBtn?.onClick.RemoveListener(ShowCustomTab);
        vehicleTabBtn?.onClick.RemoveListener(ShowVehicleTab);

        customCloseBtn?.onClick.RemoveListener(CloseSkinTab);
        vehicleCloseBtn?.onClick.RemoveListener(CloseVehicleTab);
    }

    #region 탭전환
    private void ShowCustomTab()
    {
        customTabPanel?.SetActive(true);
        CreateProductList(shopManager.characterSkin, customProductList, ProductType.CharacterSkin);
    }

    private void ShowVehicleTab()
    {
        vehicleTabPanel?.SetActive(true);
        CreateProductList(shopManager.vehicle, vehicleProductList, ProductType.Vehicle);
    }

    private void CloseSkinTab()
    {
        customTabPanel?.SetActive(false);
    }

    private void CloseVehicleTab()
    {
        vehicleTabPanel?.SetActive(false);
    }
    #endregion

    #region 상점 물품 등록
    private void CreateProductList(List<Product> products, Transform parent, ProductType type)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (Product product in products)
        {
            bool isPurchased = shopManager.IsPurchased(product.productID);

            if (isPurchased || product.productID == 0)
                continue;

            GameObject btnObj = Instantiate(productBtnPrefab, parent);
            Image productImg = btnObj.transform.Find("Product_Img")?.GetComponent<Image>();
            Text nameText = btnObj.transform.Find("ProductName_Txt")?.GetComponent<Text>();
            Text priceText = btnObj.transform.Find("Price_Txt")?.GetComponent<Text>();
            Button buyBtn = btnObj.transform.Find("Buy_Btn")?.GetComponent<Button>();

            if (productImg != null) productImg.sprite = product.productIcon;
            if (nameText != null) nameText.text = product.productName;
            if (priceText != null) priceText.text = $"{product.price}G";

            if (buyBtn != null)
            {
                buyBtn.interactable = GameManager.Instance.Gold >= product.price;

                int productID = product.productID;
                buyBtn.onClick.AddListener(() =>
                {
                    if (shopManager.PurchaseProduct(productID))
                    {

                        RefreshCurrentTab(type);
                    }
                });
            }
        }
    }
    #endregion

    #region 상점 새로고침
    private void RefreshCurrentTab(ProductType type)
    {
        if (type == ProductType.CharacterSkin)
        {
            ShowCustomTab();
        }
        else
        {
            ShowVehicleTab();
        }
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        if (goldTxt != null && GameManager.Instance != null)
        {
            goldTxt.text = $"Gold: {GameManager.Instance.Gold}";
        }
    }
    #endregion
}
