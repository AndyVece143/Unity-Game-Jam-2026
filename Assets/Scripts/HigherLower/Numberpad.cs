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

    public GameObject directions;

    public AudioClip click;
    public AudioClip winSound;
    public AudioClip lose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Beginning());
        //ResetNumberPad();
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
            SoundManager.instance.PlaySound(click);
        }
    }

    public void DeleteNumber()
    {
        if (playerAnswer.text.Length != 0)
        {
            playerAnswer.text = playerAnswer.text.Substring(0, playerAnswer.text.Length - 1);
            SoundManager.instance.PlaySound(click);
        }
    }

    public void GiveAnAnswer()
    {
        if (buffering == false)
        {
            StartCoroutine(SubmitAnswer());
        }
    }

    public IEnumerator Beginning()
    {
        Vector3 ogPosition = directions.transform.position;
        directions.transform.position = new Vector3(ogPosition.x + 14f, ogPosition.y, ogPosition.z);
        Vector3 offScreen = directions.transform.position;

        yield return new WaitForSeconds(1);
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            directions.transform.position = Vector3.Lerp(offScreen, ogPosition, t);
            yield return null;
        }

        directions.transform.position = ogPosition;

        yield return new WaitForSeconds(3);

        time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            directions.transform.position = Vector3.Lerp(ogPosition, offScreen, t);
            yield return null;
        }

        ResetNumberPad();
    }

    public IEnumerator SubmitAnswer()
    {
        if (playerAnswer.text.Length == 2)
        {
            SoundManager.instance.PlaySound(click);
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
                SoundManager.instance.PlaySound(winSound);
                Debug.Log("Correct!");
                hints.color = Color.green;
                hints.text = "WINNER";

                yield return new WaitForSeconds(3);
                StaticData.highLowerDone = true;
                StaticData.playerSpawnPoint = 1;
                LevelLoader loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
                loader.LoadNextLevel("Town");
            }

        }
    }

    private void CheckForGameOver()
    {
        guesses++;
        if (guesses == 15)
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
        SoundManager.instance.PlaySound(lose);
        playerPortrait.ChangeEmotionMinigame(5);
        ladyPortrait.ChangeEmotionMinigame(2);
        hints.text = "LOSER!";
        hints.color = Color.red;
        yield return new WaitForSeconds(3);
        LevelLoader loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
        loader.LoadNextLevel("HigherLower");
    }
}
