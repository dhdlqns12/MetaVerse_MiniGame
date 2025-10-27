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
    public Sprite productIcon; //UI�� ǥ���� ��������Ʈ
    public Sprite productSprite; //���� ����� ��������Ʈ
    public RuntimeAnimatorController animatorOverride;
    public int price;
    public ProductType type;
    public string description;
}
