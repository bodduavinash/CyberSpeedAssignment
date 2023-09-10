using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : SingletonWithMonobehaviour<UIManager>
{
    [SerializeField] private TextMeshProUGUI noOfTurnsValueText;
    [SerializeField] private TextMeshProUGUI noOfMatchedValueText;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button saveButton;

    private GameSceneManager _gameSceneManager;
    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);

        homeButton.onClick.AddListener(OnHomeButtonClicked);
        saveButton.onClick.AddListener(onSaveButtonClicked);
    }

    private void OnHomeButtonClicked()
    {
        _gameSceneManager.LoadScene(GameScenesEnum.LayoutMenuScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void onSaveButtonClicked()
    {
        _dataPersistenceManager.SaveGame();
    }

    public void UpdateTurnsValueText(int score)
    {
        noOfTurnsValueText.text = score.ToString();
    }

    public void UpdateMatchedValueText(int matched)
    {
        noOfMatchedValueText.text = matched.ToString();
    }
}
