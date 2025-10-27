using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [Header("속도")]
    [SerializeField] private float fallSpeed = 5f;


    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void Init()
    {
        fallSpeed = 5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DontCollisionGround"))
        {
            Destroy(gameObject);
        }
    }
}
