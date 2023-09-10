using UnityEngine;

public class GameManager : SingletonWithMonobehaviour<GameManager>
{
    private GameLayoutsEnum layoutSelected = GameLayoutsEnum.None;

    public GameLayoutsEnum CurrentLayoutEnumSelected { set => layoutSelected = value; get => layoutSelected; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}