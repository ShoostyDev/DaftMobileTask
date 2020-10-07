using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.States;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameState gameState;
    public static GameState GameState { get => gameState; private set => gameState = value; }
    private static GameEventBus gameEventBus;
    public static GameEventBus GameEventBus { get => gameEventBus; private set => gameEventBus = value; }

    void Awake()
    {
        GameState = GameStatePersistance.LoadGameStateData();
        GameEventBus = new GameEventBus();
        
        GameEventBus.On<GameStartedEvent>(OnGameStartedEvent);
    }

    void Start()
    {
    }

    private void SetScore(float score)
    {
        GameState.TimeScore = score;
    }

    private void OnGameStartedEvent(GameStartedEvent arg)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync("GameplayScene", LoadSceneMode.Additive);
    }

    void OnDestroy()
    {
        GameEventBus.Off<GameStartedEvent>(OnGameStartedEvent);
    }
}
