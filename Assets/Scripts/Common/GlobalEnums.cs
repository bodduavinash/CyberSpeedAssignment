using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnums : MonoBehaviour
{
    
}

public enum GameLayoutsEnum
{
    None,
    Layout2x2,
    Layout2x3,
    Layout4x4,
    Layout5x6
}

public enum GameScenesEnum
{
    MainMenuScene,
    LayoutMenuScene,
    GameScene,
    GameOverScene
}

public enum GameAudioClipsEnum
{
    None,
    ButtonClick,
    CardMatchError,
    CardMatched,
    GameOver
}

public enum DontDestroyOnLoadEnums
{
    None,
    GameSceneManager,
    GameManager,
    DataPersistenceManager
}