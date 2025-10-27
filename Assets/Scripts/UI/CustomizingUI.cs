using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingUI : MonoBehaviour
{
    [Header("직접 참조")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private PlayerCustomizing playerCustomizing;
    [SerializeField] private VehicleManager vehicleManager;

    [Header("탭 전환 버튼")]
    [SerializeField] private Button skinTabBtn;
    [SerializeField] private Button vehicleTabBtn;
    [SerializeField] private Button skinTabCloseBtn;
    [SerializeField] private Button vehicleTabCloseBtn;

    [Header("탭 패널")]
    [SerializeField] private GameObject skinTabPanel;
    [SerializeField] private GameObject vehicleTabPanel;

    [Header("해금 리스트")]
    [SerializeField] private Transform skinProductList; 
    [SerializeField] private Transform vehicleProductList; 
    [SerializeField] private GameObject customizingButtonPrefab;

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
        skinTabBtn?.onClick.AddListener(ShowSkinTab);
        vehicleTabBtn?.onClick.AddListener(ShowVehicleTab);

        skinTabCloseBtn?.onClick.AddListener(CloseSkinTab);
        vehicleTabCloseBtn?.onClick.AddListener(CloseVehicleTab);
    }

    private void UnsubscribeEvents()
    {
        skinTabBtn?.onClick.RemoveListener(ShowSkinTab);
        vehicleTabBtn?.onClick.RemoveListener(ShowVehicleTab);

        skinTabCloseBtn?.onClick.RemoveListener(CloseSkinTab);
        vehicleTabCloseBtn?.onClick.RemoveListener(CloseVehicleTab);
    }

    #region 스킨/탈것 탭 열기/닫기
    private void ShowSkinTab()
    {
        skinTabPanel?.SetActive(true);

        CreateCustomizingList(shopManager.characterSkin, skinProductList, ProductType.CharacterSkin);
    }

    private void ShowVehicleTab()
    {
        vehicleTabPanel?.SetActive(true);

        CreateCustomizingList(shopManager.vehicle, vehicleProductList, ProductType.Vehicle);
    }

    private void CloseSkinTab()
    {
        skinTabPanel?.SetActive(false);
    }

    private void CloseVehicleTab()
    {
        vehicleTabPanel?.SetActive(false);
    }
    #endregion

    #region 커스터마이징 목록 생성/장착
    private void CreateCustomizingList(List<Product> products, Transform parent, ProductType type)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (Product product in products)
        {
            if (!shopManager.IsPurchased(product.productID)) //구매 안한 상품(IsPurchased 반환값이 false)는 Continue로 스킵
                continue;

            GameObject btnObj = Instantiate(customizingButtonPrefab, parent);

            Image productImg = btnObj.transform.Find("Product_Img")?.GetComponent<Image>();
            Text nameText = btnObj.transform.Find("ProductName_Txt")?.GetComponent<Text>();
            Button equipBtn = btnObj.transform.Find("Equip_Btn")?.GetComponent<Button>();

            if (productImg != null) productImg.sprite = product.productIcon;
            if (nameText != null) nameText.text = product.productName;

            bool isEquipped = false;

            if (type == ProductType.CharacterSkin)
            {
                isEquipped = (shopManager.customizingData.selectedSkinID == product.productID);
            }
            else if (type == ProductType.Vehicle)
            {
                isEquipped = (shopManager.customizingData.selectedVehicleID == product.productID);
            }

            if (equipBtn != null)
            {
                if (type == ProductType.CharacterSkin)
                {
                    equipBtn.interactable = !isEquipped;
                }
                else if (type == ProductType.Vehicle)
                {
                    equipBtn.interactable = true;
                }

                Text btnText = equipBtn.GetComponentInChildren<Text>();
                if (btnText != null)
                {
                    btnText.text = isEquipped ? "장착중" : "착용";
                }

                int productID = product.productID;
                equipBtn.onClick.AddListener(() =>
                {
                    EquipProduct(productID, type);
                });
            }
        }
    }

    private void EquipProduct(int productID, ProductType type)
    {
        Product product = shopManager.GetProductId(productID);

        if (product == null) return;

        if (type == ProductType.CharacterSkin)
        {
            shopManager.customizingData.selectedSkinID = productID;
            playerCustomizing.ApplyCharacterSkin(product.productSprite, product.animatorOverride);
        }
        else if (type == ProductType.Vehicle)
        {
            if (shopManager.customizingData.selectedVehicleID == productID)
            {
                shopManager.customizingData.selectedVehicleID = 0;
                playerCustomizing.UnequipVehicle();
            }
            else
            {
                shopManager.customizingData.selectedVehicleID = productID;
                playerCustomizing.ApplyVehicle(productID, vehicleManager);
            }
        }

        SaveManager.Instance.SaveCustomizationData(shopManager.customizingData);

        if (type == ProductType.CharacterSkin)
        {
            ShowSkinTab();
        }
        else
        {
            ShowVehicleTab();
        }
    }
    #endregion
}
