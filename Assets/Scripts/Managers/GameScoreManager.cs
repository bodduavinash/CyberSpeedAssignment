
using UnityEngine;

public class GameScoreManager : SingletonWithMonobehaviour<GameScoreManager>, IGameDataPersistence
{
    private int noOfTurns = 0;
    private int noOfMatched = 0;

    public void UpdateTurnsScore()
    {
        noOfTurns ++;
        UIManager.instance.UpdateTurnsValueText(noOfTurns);
    }

    public void UpdateMatchedScore()
    {
        noOfMatched ++;
        UIManager.instance.UpdateMatchedValueText(noOfMatched);
    }

    public int GetNoOfTurnsScore()
    {
        return noOfTurns;
    }

    public int GetNoOfMatchedScore()
    {
        return noOfMatched;
    }

    public void ResetScores()
    {
        noOfTurns = 0;
        noOfMatched = 0;
    }

    public void LoadGame(GameData gameData)
    {
        noOfTurns = gameData.noOfTurns;
        noOfMatched = gameData.noOfMatched;

        UIManager.instance.UpdateTurnsValueText(noOfTurns);
        UIManager.instance.UpdateMatchedValueText(noOfMatched);
    }

    public void SaveGame(ref GameData gameData)
    {
        gameData.noOfTurns = noOfTurns;
        gameData.noOfMatched = noOfMatched;
    }
}
