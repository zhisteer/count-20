using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // -----------------------------------------------------------
    //  GameState
    // -----------------------------------------------------------

    public enum GameState
    {
        Menu,
        Playing,
        Waiting,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    // -----------------------------------------------------------
    //  References
    // -----------------------------------------------------------

    [Header("Spawners / Managers")]
    public vegetableSpawner spawner;
    public AnswerSpawner answerSpawner;

    [Header("UI Elements")]
    public GameObject startMenuUI;
    public GameObject inGameUI;
    public GameObject waitingUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI friendText;

    [Header("Answered Popup UI")]
    public GameObject answeredButton;
    public TextMeshProUGUI answeredText;
    public Image answeredButtonBackground;
    public UnityEvent onAnswerCorrect;

    public GameData gameData = new ();

    // -----------------------------------------------------------
    //  Game State & Turn Logic
    // -----------------------------------------------------------

    public GameState currentState = GameState.Menu;
    public int currentTurn = 0;   
    public int currentAnswer;
    private int pastMistakes = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()

    {
        gameData.maxTurn = 5;
        currentTurn = 0;
        gameData.correctAnswers = 0;
        gameData.playTime = 0f;

        SetGameState(GameState.Menu);
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            gameData.playTime += Time.deltaTime;
        }
    }

    // -----------------------------------------------------------
    //  Public Methods (Game Flow)
    // -----------------------------------------------------------

    public void SendToBackEnd()
    {

    }
    public void OnStartButtonClicked()
    {
        StartGame();
    }

    public void StartGame()
    {
        currentTurn = 1;
        gameData.correctAnswers = 0;
        gameData.playTime = 0f;

        // Spawn the initial set of vegetables
        spawner.SpawnVegetables();

        turnText.text = $"{currentTurn} / {gameData.maxTurn}";

        SetGameState(GameState.Playing);

    }

    public void EndTurn(bool wasCorrect, int answer)
    {
        currentAnswer = answer;
        answeredText.text = answer.ToString();

        if (wasCorrect)
        {
            friendText.text = "Зөв хариуллаа!";
            gameData.correctAnswers++;
            onAnswerCorrect.Invoke();
        }
        else
        {
            friendText.text = "Буруу!";
        }

        currentTurn++;
        

        if (currentTurn > gameData.maxTurn)
        {
             EndGame();
        }
        else
        {
            WaitForNextRound(wasCorrect);
        }
    }

    public void EndGame()
    {
        // Hide turnText
        turnText.text = "";
        spawner.ClearSpawnedVegetables();
        answerSpawner.ClearAnswers();
        int mistakes = gameData.maxTurn - gameData.correctAnswers;
        mistakes = mistakes - pastMistakes;
        string text;
        gameData.wrongAnswers = gameData.maxTurn - gameData.correctAnswers;

        string summary =
            $"Тоглосон тоо: {gameData.maxTurn} удаа\r\n" +
            $"Зөв хариулсан: {gameData.correctAnswers}\r\n" +
            $"Буруу хариулсан: {gameData.wrongAnswers}\r\n" +
            $"Зарцуулсан хугацаа:\n {gameData.playTime:F2} секунд";

        scoreText.text = summary;
        SetGameState(GameState.GameOver);

        if (mistakes == 0)
        {
            friendText.text = "Амжилттай дуусгалаа. " +
                "Манай найз тоогоо маш сайн сурсан байна. \r\n" +
                "Баяр хүргэе.\r\n";
            GameOver();
        }
        else if (mistakes ==1)
        {
            text = "Найзаа чи 1 алдсан учир дахин тоглож тоогоо тоолж сураарай.";
            ExtendTurn(mistakes, text);
        }
        else if (mistakes == 2)
        {
            text = "Энэ удаа 2 алдсан учир дахин тоглож тоогоо тоолж сураарай.";
            ExtendTurn(mistakes, text);
        }
        else
        {
            text = $"Хөөх чи {mistakes} алдсан байна. " +
                $"Тоогоо сайн сурахын тулд дахин тоглоорой. ";
            ExtendTurn(mistakes, text);
        }

          
    }

    private void ExtendTurn(int mistakes, string text)
    {
        if (mistakes <=2)
        {
            gameData.maxTurn = gameData.maxTurn + 2;
        }
        else
        {
            gameData.maxTurn = gameData.maxTurn + 4;
        }
        friendText.text = text;
        waitingUI.SetActive(true);
        pastMistakes += mistakes;


    }

    public void GameOver()
    {
        //TODO: Send gameData to backend
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnNextButtonClick()
    {

        answeredButton.SetActive(false);

        turnText.text = $"{currentTurn} / {gameData.maxTurn}";

        SetGameState(GameState.Playing);

        SpawnNewRound();
    }

    private void SpawnNewRound()
    {
        answeredButton.SetActive(false);

        spawner.ClearSpawnedVegetables();
        spawner.SpawnVegetables();
    }

    private void WaitForNextRound(bool wasCorrect)
    {
        answerSpawner.ClearAnswers();

        answeredButton.SetActive(true);
        answeredButtonBackground.color = wasCorrect ? Color.green : Color.red;

        SetGameState(GameState.Waiting);
    }

    private void SetGameState(GameState newState)
    {
        currentState = newState;

        if (startMenuUI) startMenuUI.SetActive(false);
        if (inGameUI) inGameUI.SetActive(false);
        if (waitingUI) waitingUI.SetActive(false);
        if (gameOverUI) gameOverUI.SetActive(false);

        switch (currentState)
        {
            case GameState.Menu:
                if (startMenuUI) startMenuUI.SetActive(true);
                break;

            case GameState.Playing:
                if (inGameUI) inGameUI.SetActive(true);
                break;

            case GameState.Waiting:
                if (waitingUI) waitingUI.SetActive(true);
                break;

            case GameState.GameOver:
                if (gameOverUI) gameOverUI.SetActive(true);
                break;
        }
    }
}
