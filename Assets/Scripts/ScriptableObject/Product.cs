using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductType
{
    CharacterSkin,
    Vehicle
}

[CreateAssetMenu(fileName = "New Product", menuName = "Shop/Product")]
public class Product : ScriptableObject
{
    public int productID;
    public string productName;
    public Sprite productIcon; //UI에 표시할 스프라이트
    public Sprite productSprite; //실제 적용될 스프라이트
    public RuntimeAnimatorController animatorOverride;
    public int price;
    public ProductType type;
    public string description;
}
