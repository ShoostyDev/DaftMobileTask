using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class GameplayController : MonoBehaviour
{
    private ObjectPool objectPool;
    private float scoreTimer = 0;
    private Coroutine timerCoroutine;
    private Coroutine gameplayLoopCoroutine;

    [SerializeField]
    private GameObject continueButtonPrefab;
    [SerializeField]
    private GameObject gameOverButtonPrefab;
    [SerializeField]
    private Transform buttonsParent;

    private float startSpawnDelay = 3f;

    void Awake()
    {
        objectPool = new ObjectPool();
        objectPool.Add(PoolTypeEnum.ContinueButton, SpawnPoolableTapButton(PoolTypeEnum.ContinueButton));
        objectPool.Add(PoolTypeEnum.GameOverButton, SpawnPoolableTapButton(PoolTypeEnum.GameOverButton));

        GameManager.GameEventBus.On<GameOverEvent>(OnGameOverEvent);
    }

    void Start()
    {
        timerCoroutine = StartCoroutine(ScoreTimer());
        gameplayLoopCoroutine = StartCoroutine(GameplayLoop());
    }

    private IEnumerator GameplayLoop()
    {
        IPoolable pooledObject;
        float buttonLifeTime;
        float loopDelay;
        float difficultyAccelerator;

        while (true)
        {
            difficultyAccelerator = scoreTimer * 0.1f;
            // if(dif

            if (Random.value > 0.1f)
            {
                pooledObject = objectPool.Fetch(PoolTypeEnum.ContinueButton, SpawnPoolableTapButton);
                buttonLifeTime = Random.Range(2f, 4f) - difficultyAccelerator * 0.1f;
            }
            else
            {
                pooledObject = objectPool.Fetch(PoolTypeEnum.GameOverButton, SpawnPoolableTapButton);
                buttonLifeTime = 3f;
            }

            pooledObject.Activate(RandomizeOnScreenPos(), buttonLifeTime);

            loopDelay = Random.Range(startSpawnDelay - (startSpawnDelay * 0.2f), startSpawnDelay);
            loopDelay -= difficultyAccelerator;

            Debug.Log("dd " + difficultyAccelerator.ToString("n2") + " lf " + buttonLifeTime.ToString("n2") + " dl " + loopDelay.ToString("n2"));
            yield return new WaitForSeconds(loopDelay);
        }
    }

    private IEnumerator ScoreTimer()
    {
        scoreTimer = 0;

        while (true)
        {
            scoreTimer += Time.deltaTime;
            GameManager.GameState.TimeScore = scoreTimer;
            yield return null;
        }
    }

    private IPoolable SpawnPoolableTapButton(PoolTypeEnum typeToSpawn)
    {
        GameObject prefab = typeToSpawn == PoolTypeEnum.ContinueButton ? continueButtonPrefab : gameOverButtonPrefab;
        return Instantiate(prefab, Vector2.zero, Quaternion.identity, buttonsParent).GetComponent<IPoolable>();
    }

    private Vector2 RandomizeOnScreenPos(List<IPoolable> activeButtons = null, int TryCount = 0)
    {
        Vector2 randomPos = new Vector2(Random.Range((float)Screen.width * 0.1f, (float)Screen.width * 0.9f), Random.Range(Screen.height * 0.1f, Screen.height * 0.9f));

        if (TryCount > 1000) return Vector3.zero;//stackOverflow security

        if (activeButtons == null)
        {
            activeButtons = new List<IPoolable>();
            objectPool.GetAllActive(PoolTypeEnum.ContinueButton).AddTo(activeButtons);
            objectPool.GetAllActive(PoolTypeEnum.GameOverButton).AddTo(activeButtons);
        }

        if (activeButtons != null)
            foreach (var active in activeButtons)
            {
                if ((active.GetPosition() - randomPos).sqrMagnitude < 125 * 125)
                {
                    return RandomizeOnScreenPos(activeButtons, ++TryCount);
                }
            }

        return randomPos;
    }

    private void OnGameOverEvent(GameOverEvent arg)
    {
        StopCoroutine(gameplayLoopCoroutine);
        StopCoroutine(timerCoroutine);

        if (GameManager.GameState.BestTimeScore < (int)scoreTimer)
        {
            GameManager.GameState.BestTimeScore = (int)scoreTimer;
            GameManager.GameEventBus.Trigger<NewHighScoreEvent>(new NewHighScoreEvent());
        }
    }

    private void OnDestroy()
    {
        GameManager.GameEventBus.Off<GameOverEvent>(OnGameOverEvent);
    }
}


//wybuch
//skalowanie buttona gdy active deactive
//poczatkowo 2-4 potem krocej zyja
//coraz szybszy spawn - poziom trudnosci
//skalowanie wzgledem ekranu