using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ShopData
{
    public List<int> purchasedProducts; // 구매한 상품 ID만

    public ShopData(bool init)
    {
        purchasedProducts = new List<int>();
    }
}
