using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    float offsetX;

    void Start()
    {
        if (target == null)
            return;

        offsetX = transform.position.x - target.position.x; //카메라와 플레이어 사이의 거리 저장
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos.x = target.position.x + offsetX;
        transform.position = pos;
    }
}
