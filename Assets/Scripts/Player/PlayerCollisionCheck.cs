using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionCheck : MonoBehaviour
{
    [SerializeField] private IMiniGameManager miniGameManager;

    private void Start()
    {
        miniGameManager = MiniGameManagerRegistry.GetCurrent();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    #region 충돌제어
    private void HandleCollision(GameObject target)
    {
        if (miniGameManager == null) 
            return;

        if (miniGameManager.IsGameOver) 
            return;

        if (target.CompareTag("Obstacle"))
        {
            miniGameManager.GameOver();
        }

        if (target.CompareTag("Ground"))
        {
            miniGameManager.GameOver();
        }

        if(target.CompareTag("ScoreTag"))
        {
            miniGameManager.AddScore(1);
        }
    }
    #endregion
}
