using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCustomizing : MonoBehaviour
{
    [Header("스킨")]
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Collider2D playerCollider;

    [Header("탈것")]
    [SerializeField] private GameObject vehicleSlot;
    [SerializeField] private SpriteRenderer vehicleRenderer;
    [SerializeField] private Animator vehicleAnimator;
    [SerializeField] private Collider2D vehicleCollider;

    [Header("직접 참조")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private VehicleManager vehicleManager;

    private Vector2 originalPlayerPosition;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    private RuntimeAnimatorController currentSkinAnimator;

    public event Action vehicleEquipped;   

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (vehicleCollider is BoxCollider2D boxCollider)
        {
            originalColliderSize = boxCollider.size;
            originalColliderOffset = boxCollider.offset;
        }

        if (playerAnimator != null)
        {
            currentSkinAnimator = playerAnimator.runtimeAnimatorController;
        }
    }

    public void ApplyCharacterSkin(Sprite skinSprite, RuntimeAnimatorController animatorOverride)
    {
        if (playerSpriteRenderer != null&& playerAnimator != null)
        {
            playerSpriteRenderer.sprite = skinSprite;
        }

        if (animatorOverride != null)
        {
            currentSkinAnimator = animatorOverride;

            if (playerAnimator != null && playerAnimator.enabled)
            {
                playerAnimator.runtimeAnimatorController = animatorOverride;
            }
        }
    }

    public void ApplyVehicle(int vehicleID, VehicleManager vehicleManager)
    {
        if (vehicleID == 0)
        {
            UnequipVehicle();
            return;
        }

        if (vehicleManager == null)
        {
            return;
        }

        Product vehicle = vehicleManager.GetVehicle(vehicleID);
        VehicleData? vehicleData = vehicleManager.GetVehicleData(vehicleID);

        VehicleData data = vehicleData.Value;

        if (vehicleRenderer != null && vehicle.productSprite != null)
        {
            vehicleRenderer.sprite = vehicle.productSprite;
            vehicleSlot.SetActive(true);
        }

        if (vehicleAnimator != null && vehicle.animatorOverride != null)
        {
            vehicleAnimator.runtimeAnimatorController = vehicle.animatorOverride;
            vehicleAnimator.enabled = true;
        }

        if (playerAnimator != null)
        {
            playerAnimator.speed = 0f;
        }

        if (player != null)
        {
            Vector2 offset = data.playerOffset.ToVector2();
        }

        if (vehicleCollider is BoxCollider2D boxCollider)
        {
            boxCollider.size = data.colliderSize.ToVector2();
            boxCollider.offset = data.colliderOffset.ToVector2();
            boxCollider.enabled = true;
        }

        if (playerCollider != null)
            playerCollider.enabled = false;

        vehicleEquipped?.Invoke();
    }

    public void UnequipVehicle()
    {
        if (vehicleSlot != null)
            vehicleSlot.SetActive(false);

        if (vehicleAnimator != null)
        {
            vehicleAnimator.enabled = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.speed = 1f; 
        }

        if (vehicleCollider is BoxCollider2D boxCollider)
        {
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;
            boxCollider.enabled = false;
        }

        if (playerCollider != null)
            playerCollider.enabled = true;

        vehicleEquipped?.Invoke();
    }

    public void ApplyCurrentCustomization(ShopManager shopManager, VehicleManager vehicleManager)
    {
        Product skin = shopManager.GetProductId(shopManager.customizingData.selectedSkinID);
        if (skin != null && skin.type == ProductType.CharacterSkin)
        {
            ApplyCharacterSkin(skin.productSprite, skin.animatorOverride);
        }

        if (shopManager.customizingData.selectedVehicleID != 0)
        {
            ApplyVehicle(shopManager.customizingData.selectedVehicleID, vehicleManager);
        }
        else
        {
            UnequipVehicle();
        }
    }

    public float GetCurrentSpeed()
    {
        if (shopManager.customizingData.selectedVehicleID == 0)
            return 0f;

        return vehicleManager.GetVehicleSpeed(shopManager.customizingData.selectedVehicleID);
    }

    public void SetFlip(bool flip)
    {
        if (vehicleRenderer != null && vehicleSlot.activeSelf)
        {
            vehicleRenderer.flipX = flip;
        }
    }

    public bool IsRidingVehicle()
    {
        return vehicleSlot != null && vehicleSlot.activeSelf;
    }
}
