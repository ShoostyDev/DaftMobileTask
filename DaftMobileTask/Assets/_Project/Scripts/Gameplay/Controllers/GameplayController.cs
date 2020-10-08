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
        while (true)
        {
            if (Random.value > 0.1f)
            {
                pooledObject = objectPool.Fetch(PoolTypeEnum.ContinueButton, SpawnPoolableTapButton);
            }
            else
            {
                pooledObject = objectPool.Fetch(PoolTypeEnum.GameOverButton, SpawnPoolableTapButton);
            }

            pooledObject.Activate(RandomizeOnScreenPos());

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ScoreTimer()
    {
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

    private Vector2 RandomizeOnScreenPos(List<IPoolable> activeButtons = null)
    {
        Vector2 randomPos = new Vector2(Random.Range(0, (float)Screen.width), Random.Range(0, Screen.height));

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
                    return RandomizeOnScreenPos(activeButtons);
                }
            }

        return randomPos;
    }

    private void OnGameOverEvent(GameOverEvent arg)
    {
        StopCoroutine(gameplayLoopCoroutine);
        StopCoroutine(timerCoroutine);
        GameManager.GameState.BestTimeScore = scoreTimer;
    }

    private void OnDestroy()
    {
        GameManager.GameEventBus.Off<GameOverEvent>(OnGameOverEvent);
    }
}


