using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    private Damageable playerDamageable;

    private void Start()
    {
        playerDamageable = FindObjectOfType<Damageable>();

        playerDamageable.damageableDeath.AddListener(ShowGameOver);
    }

    private void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
