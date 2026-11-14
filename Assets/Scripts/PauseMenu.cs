using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Player player;
    public TMP_Text blueText;
    public TMP_Text redText;
    public TMP_Text yellowText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && player.state == Player.State.Standard)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void UpdateText()
    {
        string numberString = StaticData.coolLuckyNumber.ToString();

        if (StaticData.highLowerDone == true)
        {
            int firstDigit = numberString[0] - '0';
            int secondDigit = numberString[1] - '0';
            blueText.text = "" + firstDigit + "" + secondDigit;
        }

        if (StaticData.quizDone == true)
        {
            int firstDigit = numberString[2] - '0';
            int secondDigit = numberString[3] - '0';
            redText.text = "" + firstDigit + "" + secondDigit;
        }

        if (StaticData.keysDone == true)
        {
            int firstDigit = numberString[4] - '0';
            int secondDigit = numberString[5] - '0';
            yellowText.text = "" + firstDigit + "" + secondDigit;
        }
    }

    public void Resume()
    {
        UpdateText();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        UpdateText();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
