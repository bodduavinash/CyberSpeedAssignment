using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : SingletonWithMonobehaviour<DataPersistenceManager>
{
    private FileDataHandler _dataHandler;

    private GameData _gameData;

    private List<IGameDataPersistence> _dataPersistenceObjectsList;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        _dataHandler = new FileDataHandler(Application.persistentDataPath, "CyberSpeedGameData.json");
        _dataPersistenceObjectsList = FindAllDataPersistenceObjects();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        if(_gameData == null)
        {
            Debug.Log("No save data was found. Initializing new game data!!!");
            NewGame();
        }

        if(_dataPersistenceObjectsList == null || _dataPersistenceObjectsList.Count == 0)
        {
            _dataPersistenceObjectsList = FindAllDataPersistenceObjects();
        }

        foreach(IGameDataPersistence dataPersistenceObj in _dataPersistenceObjectsList)
        {
            dataPersistenceObj.LoadGame(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IGameDataPersistence dataPersistenceObj in _dataPersistenceObjectsList)
        {
            dataPersistenceObj.SaveGame(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    public void ResetGame()
    {
        NewGame();
        _dataHandler.Save(_gameData);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _dataPersistenceObjectsList = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IGameDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IGameDataPersistence> dataPersistenceObjs = FindObjectsOfType<MonoBehaviour>().OfType<IGameDataPersistence>();

        return new List<IGameDataPersistence>(dataPersistenceObjs);
        //return FindObjectsOfType<MonoBehaviour>().OfType<IGameDataPersistence>().ToList<IGameDataPersistence>();
    }
}
