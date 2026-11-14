using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberGuesser : MonoBehaviour
{
    public Player player;
    public TMP_InputField playerAnswer;
    public GameObject UI;
    public Button number1Button;
    public bool buffering = false;
    public bool win = false;
    public Canvas canvas;
    public AudioClip click;
    public AudioClip winSound;
    public AudioClip lose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        StartCoroutine(SlideIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SlideIn()
    {
        Vector2 ogPosition = UI.transform.position;
        UI.transform.position = new Vector2(ogPosition.x + 15f, ogPosition.y);
        Vector2 offScreen = UI.transform.position;

        //yield return new WaitForSeconds(1);
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            UI.transform.position = Vector3.Lerp(offScreen, ogPosition, t);
            yield return null;
        }
        UI.transform.position = ogPosition;
        number1Button.Select();
    }

    IEnumerator SlideOff()
    {
        Vector2 ogPosition = UI.transform.position;
        Vector2 offScreen = new Vector2(ogPosition.x + 15f, ogPosition.y);

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            UI.transform.position = Vector3.Lerp(ogPosition, offScreen, t);
            yield return null;
        }

        if (win == true)
        {
            GuesserManager manager = GuesserManager.FindAnyObjectByType<GuesserManager>();
            manager.SpawnDialogue();
        }
        else
        {
            player.StartMoving();
        }
        Destroy(gameObject);
    }

    public void InputNumber(int i)
    {
        if (playerAnswer.text.Length < 6)
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

    public void GiveAnswer()
    {
        if (buffering == false)
        {
            buffering = true;

            int answerInt = int.Parse(playerAnswer.text);

            if (answerInt == StaticData.coolLuckyNumber)
            {
                Debug.Log("you got it");
                win = true;
                SoundManager.instance.PlaySound(winSound);
            }
            else
            {
                SoundManager.instance.PlaySound(lose);
            }

            StartCoroutine(SlideOff());
        }
    }
}
