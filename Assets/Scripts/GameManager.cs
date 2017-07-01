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

    [SerializeField]
    private int levels = 4;
    public int TotalLevels
    {
        get { return levels; }
    }

    [SerializeField]
    private GameObject panCamera;

    [SerializeField]
    private GameObject gameCamera;

    [SerializeField]
    private GameObject player;


    private int currentLevel = 0;
    private bool countTime = true;
    private bool inLevel = false;

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
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }

        SceneManager.LoadScene(GetLevelIndex(currentLevel), LoadSceneMode.Additive);

        inLevel = false;
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

        inLevel = true;
        Debug.Log("Started new level.");
        SceneManager.LoadScene(GetLevelIndex(currentLevel));
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        totalLevelTime = levelTime;
        level = Level();
        StartCoroutine(level);
    }

    private IEnumerator Level()
    {
        while (levelTime > 0)
        {
            if (countTime)
                levelTime -= Time.deltaTime;
            yield return null;
        }

        FinishLevel();
    }

    public void FinishLevel()
    {
        StopCoroutine(level);
        inLevel = false;

        // Add powerups.
        if (levelTime > 0)
        {
            if(Inventory.instance != null)
            {
                Inventory.instance.TurnPowerupsIntoPool();
            }
        }

        if (levelTime <= 0 || poolTime <= 0)
        {
            Debug.Log("Ran out of time.");
            SceneManager.LoadScene(3);
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

        if(currentLevel == 4)
        {
            SceneManager.LoadScene(4);
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }

        SceneManager.LoadScene(GetLevelIndex(currentLevel), LoadSceneMode.Additive);
    }

    private int GetLevelIndex(int level)
    {
        switch(currentLevel)
        {
            default:
            case 0:
                return 5;
            case 1:
                return 6;
            case 2:
                return 7;
            case 3:
                return 8;
        }
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
        countTime = false;
    }

    public void RestartCounter()
    {
        countTime = true;
    }

    // -- Spawning
    public void LevelLoaded()
    {
        if(inLevel)
        {
            SpawnCharacter();
            SpawnGameCamera();
        }
        else
        {
            SpawnPanCamera();
        }
    }

    private void SpawnPanCamera()
    {
        Transform spawn = GameObject.FindGameObjectWithTag("PanCameraSpawn").transform;
        Instantiate(panCamera, spawn.position, Quaternion.identity);
    }

    private void SpawnGameCamera()
    {
        Transform spawn = GameObject.FindGameObjectWithTag("GameCameraSpawn").transform;
        Instantiate(gameCamera, spawn.position, Quaternion.identity);
    }

    private void SpawnCharacter()
    {
        Transform spawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        Instantiate(player, spawn.position, Quaternion.identity);
    }
}