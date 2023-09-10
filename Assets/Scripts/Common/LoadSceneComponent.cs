using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneComponent : MonoBehaviour
{
    [SerializeField] private GameScenesEnum scenesEnum;
    [SerializeField] private LoadSceneMode sceneMode;

    private GameSceneManager _gameSceneManager;

    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
    }

    public void LoadScene(LoadSceneComponent sceneComponent)
    {
        _gameSceneManager.LoadScene(sceneComponent.scenesEnum, sceneComponent.sceneMode);
    }
}
