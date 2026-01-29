using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton Instance
    public static LevelManager Instance { get; private set; }

    // Dýþarýdan okunabilir ve deðiþtirilebilir CurrentLevel
    public int CurrentLevel { get; set; } = 1;

    private void Awake()
    {
        // Singleton kontrolü
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
