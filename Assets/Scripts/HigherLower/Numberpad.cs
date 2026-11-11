using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Numberpad : MonoBehaviour
{
    public Button number1Button;
    public TMP_InputField playerAnswer;
    public TMP_Text hints;
    public HigherLower manager;
    private bool buffering;

    public Portrait playerPortrait;
    public Portrait ladyPortrait;
    public int guesses = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetNumberPad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetNumberPad()
    {
        number1Button.Select();
        playerAnswer.text = string.Empty;
        buffering = false;
        hints.text = "";
        playerPortrait.ChangeEmotionMinigame(2);
        ladyPortrait.ChangeEmotionMinigame(0);
    }

    public void InputNumber(int i)
    {
        if (playerAnswer.text.Length < 2)
        {
            playerAnswer.text += i.ToString();
        }
    }

    public void DeleteNumber()
    {
        if (playerAnswer.text.Length != 0)
        {
            playerAnswer.text = playerAnswer.text.Substring(0, playerAnswer.text.Length - 1);
        }
    }

    public void GiveAnAnswer()
    {
        if (buffering == false)
        {
            StartCoroutine(SubmitAnswer());
        }
    }

    public IEnumerator SubmitAnswer()
    {
        if (playerAnswer.text.Length == 2)
        {
            playerPortrait.ChangeEmotionMinigame(9);
            buffering = true;
            int answerInt = int.Parse(playerAnswer.text);

            yield return new WaitForSeconds(1);

            if (answerInt < manager.targetNumber)
            {
                ladyPortrait.ChangeEmotionMinigame(5);
                Debug.Log("Higher!");
                hints.text = "HIGHER";

                yield return new WaitForSeconds(3);

                CheckForGameOver();
            }
            else if (answerInt > manager.targetNumber)
            {
                ladyPortrait.ChangeEmotionMinigame(5);
                Debug.Log("Lower!");
                hints.text = "LOWER";

                yield return new WaitForSeconds(3);

                CheckForGameOver();
            }
            else
            {
                ladyPortrait.ChangeEmotionMinigame(3);
                playerPortrait.ChangeEmotionMinigame(1);
                Debug.Log("Correct!");
                hints.color = Color.green;
                hints.text = "WINNER";
            }

        }
    }

    private void CheckForGameOver()
    {
        guesses++;
        if (guesses == 10)
        {
            StartCoroutine(GameOverYeah());
        }
        else
        {
            ResetNumberPad();
        }
    }

    private IEnumerator GameOverYeah()
    {
        playerPortrait.ChangeEmotionMinigame(5);
        ladyPortrait.ChangeEmotionMinigame(2);
        hints.text = "LOSER!";
        hints.color = Color.red;
        yield return new WaitForSeconds(5);
    }
}
