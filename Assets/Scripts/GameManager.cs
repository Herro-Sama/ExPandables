using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private float startPoolTime = 300f;

    [SerializeField]
    private float defaultLevelTime = 40f;

    [SerializeField, Tooltip("For UI not in-game.")]
    private float drainTime = 10f;

    [SerializeField]
    private float timePickupSpeed = 100f;

    private int currentLevel = 0;

    // Used for the UI timer bar.
    private float totalLevelTime = 0;
    public float TotalLevelTime
    {
        get { return totalLevelTime; }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
    }

    private float poolTime;
    public float PoolTime
    {
        get { return poolTime; }
    }

    private float levelTime;
    public float LevelTime
    {
        get { return levelTime; }
    }

    private IEnumerator addTimeToLevel;
    private IEnumerator addTimeToPool;
    private IEnumerator level;

    public float TotalTime
    {
        get { return poolTime + levelTime;}
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        addTimeToLevel = MoveTime(true);
        addTimeToPool = MoveTime(false);
        level = Level();

        StartNewGame();
    }

    // -- Game Setup ---

    public void StartNewGame()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }

        currentLevel = 0;
        poolTime = startPoolTime - defaultLevelTime;
        levelTime = defaultLevelTime;
        Debug.Log("Started new game.");
    }

    // --- Time Selection ---

    public void StartDrainingTime(bool addingTimeToLevel)
    {
        if(addingTimeToLevel)
            StartCoroutine(addTimeToLevel);
        else
            StartCoroutine(addTimeToPool);
    }

    public void StopDrainingTime()
    {
        StopCoroutine(addTimeToLevel);
        StopCoroutine(addTimeToPool);
    }

    private IEnumerator MoveTime(bool addingTimeToLevel)
    {
        while (true)
        {
            if (addingTimeToLevel)
            {
                if (poolTime > 0)
                {
                    levelTime += Time.deltaTime * drainTime;
                    poolTime -= Time.deltaTime * drainTime;
                }
            }
            else
            {
                if (levelTime > 0)
                {
                    levelTime -= Time.deltaTime * drainTime;
                    poolTime += Time.deltaTime * drainTime;
                }
            }

            // Clamp time.
            levelTime = Mathf.Clamp(levelTime, 0, TotalTime);
            poolTime = Mathf.Clamp(poolTime, 0, TotalTime);

            yield return null;
        }
    }

    // -- Gameplay --

    public void StartLevel()
    {
        if(levelTime <= 0)
        {
            return;
        }

        Debug.Log("Started new level.");
        LoadGameLevel(currentLevel);
        totalLevelTime = levelTime;
        level = Level();
        StartCoroutine(level);
    }

    private IEnumerator Level()
    {
        while(levelTime > 0)
        {
            levelTime -= Time.deltaTime;
            yield return null;
        }

        FinishLevel();
    }

    public void FinishLevel()
    {
        StopCoroutine(level);

        // Add powerups.
        if(levelTime > 0)
        {
            if(Inventory.instance != null)
            {
                Inventory.instance.TurnPowerupsIntoPool();
            }
        }


        if (levelTime <= 0 || poolTime <= 0)
        {
            Debug.Log("Ran out of time.");
            SceneManager.LoadScene(2);
            return;
        }
        
        totalLevelTime = 0;

        currentLevel++;
        levelTime = 0;

        if(poolTime >= defaultLevelTime)
        {
            poolTime -= defaultLevelTime;
            levelTime = defaultLevelTime;
        }
        else
        {
            levelTime = poolTime;
            poolTime = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void LoadGameLevel(int level)
    {
        SceneManager.LoadScene(1);
    }

    // -- Pickup --
    public void GiveTimeToPool(float time)
    {
        poolTime += time;
    }

    public void TakeTimeToLevel(float time)
    {
        StartCoroutine(AddTime(time));
    }

    private IEnumerator AddTime(float time)
    {
        while(time > 0)
        {
            time -= Time.deltaTime * timePickupSpeed;
            levelTime += Time.deltaTime * timePickupSpeed;

            if(levelTime > totalLevelTime)
            {
                totalLevelTime = levelTime;
            }

            yield return null;
        }
    }

    public void StopCounter()
    {
        //countTime = false;
    }

    public void RestartCounter()
    {
        //countTime = true;
    }
}