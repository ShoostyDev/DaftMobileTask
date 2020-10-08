using System;
using System.Collections;
using Assets.Scripts.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private HUDView hUDView;
    private Coroutine timerCoroutine;

    private void Awake()
    {
        GameManager.GameEventBus.On<GameOverEvent>(OnGameOverEvent);
    }

    private void Start()
    {
        timerCoroutine = StartCoroutine(SetTimer());
    }

    IEnumerator SetTimer()
    {
        while (true)
        {
            float minutes = Mathf.FloorToInt(GameManager.GameState.TimeScore / 60);
            float seconds = Mathf.FloorToInt(GameManager.GameState.TimeScore % 60);
            hUDView.TimeScoreTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
    }

    private void OnGameOverEvent(GameOverEvent arg)
    {
        StopCoroutine(timerCoroutine);
        UITweener.FadeImage(hUDView.BlackScreen, 0, 1, 1f);
    }

    private void OnDestroy()
    {
        GameManager.GameEventBus.Off<GameOverEvent>(OnGameOverEvent);
    }
}

[Serializable]
public class HUDView
{
    public TextMeshProUGUI TimeScoreTxt;
    public Image BlackScreen;
}