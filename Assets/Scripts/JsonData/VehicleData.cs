using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VehicleData
{
    public int id;

    public float speed;
    public Vector2Data playerOffset;
    public Vector2Data colliderSize;
    public Vector2Data colliderOffset;
}

[Serializable]
public struct Vector2Data
{
    public float x;
    public float y;

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}

[Serializable]
public struct VehicleDataList
{
    public VehicleData[] vehicles;
}
