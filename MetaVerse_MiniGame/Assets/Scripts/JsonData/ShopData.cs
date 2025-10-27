using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ShopData
{
    public List<int> purchasedProducts; // ������ ��ǰ ID��

    public ShopData(bool init)
    {
        purchasedProducts = new List<int>();
    }
}
