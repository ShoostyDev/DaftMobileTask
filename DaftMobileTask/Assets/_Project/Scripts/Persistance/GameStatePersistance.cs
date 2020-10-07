using Assets.Scripts.States;
using UnityEngine;

public static class GameStatePersistance
{
    public static void SaveGameStateData(GameState gameState)
    {
        PlayerPrefs.SetFloat("BestTimeScore", gameState.BestTimeScore);
    }

    public static GameState LoadGameStateData()
    {
        GameState gameState = new GameState();

        gameState.BestTimeScore = PlayerPrefs.GetFloat("BestTimeScore", 0);

        return gameState;
    }
}
