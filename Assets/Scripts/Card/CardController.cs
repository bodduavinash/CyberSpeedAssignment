using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class CardController : SingletonWithMonobehaviour<CardController>, IGameDataPersistence
{
    public GameObject CardPrefab;
    public Sprite questionCardSprite;
    public List<Sprite> fruitsSprites;
    public List<GameObject> GameLayouts;

    private const int NOT_MATCHED = 0;
    private const int MATCHED = 1;

    private List<GameObject> spawnedCardsList = new List<GameObject>();
    private List<Sprite> selectedCardSpritesList = new List<Sprite>();
    private List<int> matchedCardsList = new List<int>();
    private List<SelectedCardInfo> clickedCardsList = new List<SelectedCardInfo>(2);

    private bool isGameLoaded = false;

    private GameManager _gameManager;
    private GameSceneManager _gameSceneManager;
    private GameScoreManager _gameScoreManager;

    private DataPersistenceManager _dataPersistenceManager;

    private void Start()
    {
        _gameManager = (GameManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameManager);
        _gameSceneManager = (GameSceneManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameSceneManager);
        _dataPersistenceManager = (DataPersistenceManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.DataPersistenceManager);
        _gameScoreManager = GameScoreManager.instance;

        PopulateCardsLayout();
    }

    public void PopulateCardsLayout()
    {
        GameObject layoutGameObject = GameLayouts[(int)_gameManager.CurrentLayoutEnumSelected - 1];
        ILayoutProperties _layoutProperties = layoutGameObject.GetComponent<ILayoutProperties>();

        InstantiateCardSprites(_layoutProperties, layoutGameObject);

        if (isGameLoaded && _gameManager.CurrentLayoutEnumSelected != GameLayoutsEnum.None &&
            selectedCardSpritesList.Count != 0 &&
            matchedCardsList.Count != 0)
        {
            isGameLoaded = false;

            for(int i = 0; i < matchedCardsList.Count; i++)
            {
                if(matchedCardsList[i] == MATCHED)
                {
                    var imageComponents = spawnedCardsList[i].gameObject.GetComponentsInChildren<Image>().ToList();
                    imageComponents.ForEach(image => image.gameObject.SetActive(false));
                }
            }
        }
        else
        {
            _gameScoreManager.ResetScores();

            selectedCardSpritesList = GenerateCardSprites(_layoutProperties.GetNumberOfPairs());
        }
    }

    private void InstantiateCardSprites(ILayoutProperties properties, GameObject gameObject)
    {
        for (int i = 0; i < properties.GetNumberOfPairs() * 2; i++)
        {
            var spawnedCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity);
            spawnedCard.transform.SetParent(gameObject.gameObject.transform);
            spawnedCard.transform.localScale = Vector3.one * properties.GetSpriteScaleSize();
            spawnedCard.name = "Card_" + i;

            var cardProps = spawnedCard.GetComponent<CardProperties>();
            var cardImage = cardProps.GetCardImage();
            cardImage.sprite = questionCardSprite;

            cardProps.SetCardIndex(i);

            spawnedCardsList.Add(spawnedCard);

            if(isGameLoaded && selectedCardSpritesList.Count == 0)
            {
                matchedCardsList.Add(NOT_MATCHED);
            }
        }
    }

    private List<Sprite> GenerateCardSprites(int numberOfPairs)
    {
        List<Sprite> fruitSpritesClone = new List<Sprite>(fruitsSprites);
        List<Sprite> selectedSprites = new List<Sprite>();

        for (int i = 0; i < numberOfPairs; i++)
        {
            int randomElement = Random.Range(0, fruitSpritesClone.Count);

            //Adding sprites to list for twice to add the pairs
            selectedSprites.Add(fruitSpritesClone[randomElement]);
            selectedSprites.Add(fruitSpritesClone[randomElement]);

            fruitSpritesClone.Remove(fruitSpritesClone[randomElement]);
        }

        System.Random rnd = new System.Random();
        selectedSprites = selectedSprites.OrderBy(item => rnd.Next()).ToList();
        return selectedSprites;
    }

    public Sprite GetSelectedCardSpriteFromList(int index)
    {
        return selectedCardSpritesList[index];
    }

    public void CompareClickedCards(SelectedCardInfo cardInfo)
    {
        if (clickedCardsList.Count < 2)
        {
            clickedCardsList.Add(cardInfo);
        }

        if(clickedCardsList.Count >=2)
        {
            StartCoroutine(CompareCardsEnumerator());
        }
    }

    private IEnumerator CompareCardsEnumerator()
    {
        if(_gameScoreManager == null)
        {
            _gameScoreManager = GameScoreManager.instance;
            Debug.LogWarning("Creating the _gameScoreManager instance once again, need to check something was wrong!!");
        }

        if (clickedCardsList[0].sprite == clickedCardsList[1].sprite)
        {
            SoundManager.instance.PlaySound(GameAudioClipsEnum.CardMatched);

            _gameScoreManager.UpdateMatchedScore();
            _gameScoreManager.UpdateTurnsScore();

            yield return new WaitForSeconds(0.25f);

            clickedCardsList.ForEach(sprite => {
                var imageComponents = sprite.gameObject.GetComponentsInChildren<Image>().ToList();
                imageComponents.ForEach(image => image.gameObject.SetActive(false));

                int index = int.Parse(sprite.gameObject.name.Split('_')[1]);
                matchedCardsList[index] = MATCHED;
            });

            clickedCardsList.Clear();
        }
        else
        {
            SoundManager.instance.PlaySound(GameAudioClipsEnum.CardMatchError);

            _gameScoreManager.UpdateTurnsScore();

            yield return new WaitForSeconds(0.25f);

            clickedCardsList.ForEach(
                sprite => {
                    sprite.gameObject.GetComponent<CardProperties>().GetCardImage().sprite = questionCardSprite;
                    sprite.gameObject.GetComponent<Button>().enabled = true;
                });
            clickedCardsList.Clear();
        }

        if(matchedCardsList.Count != 0 &&
            matchedCardsList.TrueForAll(value => value == MATCHED))
        {
            _dataPersistenceManager.ResetGame();
            _gameSceneManager.LoadScene(GameScenesEnum.GameOverScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        yield return null;
    }

    public void LoadGame(GameData gameData)
    {
        if(_gameManager == null)
        {
            _gameManager = (GameManager)DontDestroyOnLoadObjects.instance.GetObjectFromDict(DontDestroyOnLoadEnums.GameManager);
        }

        if (gameData.currentLayout != GameLayoutsEnum.None)
        {
            _gameManager.CurrentLayoutEnumSelected = gameData.currentLayout;
        }

        selectedCardSpritesList = gameData.selectedCardSpritesList;
        matchedCardsList = gameData.matchedCardsList;

        isGameLoaded = true;
    }

    public void SaveGame(ref GameData gameData)
    {
        gameData.currentLayout = _gameManager.CurrentLayoutEnumSelected;
        gameData.selectedCardSpritesList = selectedCardSpritesList;
        gameData.matchedCardsList = matchedCardsList;
    }
}
