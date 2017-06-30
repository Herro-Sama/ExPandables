using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private float startPoolTime = 300f;

    [SerializeField]
    private float defaultLevelTime = 40f;

    [SerializeField, Tooltip("For UI not in-game.")]
    private float drainTime = 10f;

    private float poolTime;
    private float levelTime;

    private float TotalTime
    {
        get
        {
            return poolTime + levelTime;
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        poolTime = startPoolTime;
        levelTime = defaultLevelTime;
    }

    // --- Pre-level.
    //public 
}