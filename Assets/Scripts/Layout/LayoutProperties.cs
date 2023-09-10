using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutProperties : MonoBehaviour, ILayoutProperties
{
    [SerializeField] private int numberOfPairs = 2;
    [SerializeField] private float spriteScaleSize = 1f;

    public int GetNumberOfPairs()
    {
        return numberOfPairs;
    }

    public float GetSpriteScaleSize()
    {
        return spriteScaleSize;
    }
}
