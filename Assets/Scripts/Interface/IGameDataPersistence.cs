using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataPersistence
{
    void LoadGame(GameData gameData);
    void SaveGame(ref GameData gameData);
}
