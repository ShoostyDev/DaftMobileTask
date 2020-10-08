using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Infrastructure;

public class GameOverTapButton : TapButton
{
    protected override void OnButtonTap()
    {
        GameManager.GameEventBus.Trigger<GameOverEvent>(new GameOverEvent());
    }
}
