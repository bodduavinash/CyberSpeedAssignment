using UnityEngine;
using UnityEngine.UI;

public class MainMenuSelection : MonoBehaviour
{
    [SerializeField] private Button LoadButton;
    [SerializeField] private Button ExitButton;

    private GameSceneManager _gameSceneManager;
    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);

        LoadButton.onClick.AddListener(OnLoadButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnLoadButtonClicked()
    {
        _dataPersistenceManager.LoadGame();
        _gameSceneManager.LoadScene(GameScenesEnum.GameScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
