using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public GameLayoutsEnum currentLayout;
    public List<Sprite> selectedCardSpritesList;
    public List<int> matchedCardsList;

    public int noOfTurns;
    public int noOfMatched;
}
