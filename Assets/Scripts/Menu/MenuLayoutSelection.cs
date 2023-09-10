using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLayoutSelection : MonoBehaviour
{
    [SerializeField] private GameLayoutsEnum selectedGameLayout;

    private GameSceneManager _gameSceneManager;
    private GameManager _gameManager;

    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnLayoutButtonClicked());

        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _gameManager = (GameManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);
    }

    private void OnLayoutButtonClicked()
    {
        _dataPersistenceManager.ResetGame();

        _gameManager.CurrentLayoutEnumSelected = selectedGameLayout;
        _gameSceneManager.LoadScene();
    }
}
