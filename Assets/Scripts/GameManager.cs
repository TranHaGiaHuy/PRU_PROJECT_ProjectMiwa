using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Game states of game
   public enum GameState
    {
        GamePlay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject levelUpScreen;


    [Header("Current Stats To Display")]
    public TMP_Text currentHealth;
    public TMP_Text currentRecovery;
    public TMP_Text currentMoveSpeed;
    public TMP_Text currentMight;
    public TMP_Text currentProjectileSpeed;
    public TMP_Text currentCollectRange;

    [Header("GameOverDisplay")]
    public Image chosenCharacterIcon;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReached;
    public TMP_Text timeSurvived;
    public List<Image> chosenWeaponsUI = new List<Image>();
    public List<Image> chosenPassiveItemsUI = new List<Image>();

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public TMP_Text stopwatchDisplay;

    //Check if game ended
    public bool isGameOver = false;
    public bool isPause = false;
    public bool isChoosingUpgrade = false;

    public GameObject playerObject;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra " + this + " Deleted!");
            Destroy(gameObject);
        }
        DisableScreens();
        currentState = GameState.GamePlay;
    }

    void Update()
    {
        // Each state behavior
        switch (currentState)
        {
            case GameState.GamePlay:
                isPause = false;
                //Code for gameplay state
                CheckForPauseAndResume();
                UpdateStopWatch();
                break;
            case GameState.Paused:
                isPause = true;
                //Code for paused state
                CheckForPauseAndResume();

                break;
            case GameState.GameOver:
                if (!isGameOver) // to call display 1 time only
                {
                    Time.timeScale = 0; /// stop game 
                    isGameOver = true;
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!isChoosingUpgrade)
                {
                    isChoosingUpgrade = true;
                    Time.timeScale = 0; /// stop game 
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("State does not exist!");
                break;
        }
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused) {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0; // stop game
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }

    }
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // game run
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");

        }
    }
    //Check when press ESC
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        levelUpScreen.SetActive(false);

    }
    public void GameOver()
    {
        timeSurvived.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver); 
    }
    void DisplayResults()
    {
        gameOverScreen.SetActive(true);
    }
    public void AssignChosenCharacrterUI(CharacterScriptableObject characterData)
    {
        chosenCharacterIcon.sprite = characterData.Icon;
        chosenCharacterName.text = characterData.Name;
    }
    public void AssignLevelReached(int currentLevelData)
    {
        levelReached.text = currentLevelData.ToString();
    }
    public void AssignChosenWeaponsAndItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count!=chosenWeaponsUI.Count||chosenPassiveItemsData.Count!=chosenPassiveItemsUI.Count)
        {
            Debug.LogWarning("Chosen weapons and passive items data list is not match!");
            return;
        }
        //add weapon to UI display
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponsData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }
        //add weapon to UI display
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopWatch()
    {
        stopwatchTime+=Time.deltaTime;
        UpdateStopwatchDisplay();
        if (stopwatchTime>=timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }
    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime /60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        isChoosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.GamePlay);
    }
}
