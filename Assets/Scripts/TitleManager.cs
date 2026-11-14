using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour
{
    public Button playButton;
    public Button backButton;
    public GameObject mainTitle;
    public GameObject credits;
    public float buttonTimer;
    public float duration;
    public float moving;
    public LevelLoader loader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.Select();
        ResetVariables();
        Debug.Log(mainTitle.transform.position);
        Debug.Log(credits.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        buttonTimer -= Time.deltaTime;
    }

    public void CreditsScreen()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(false));
            buttonTimer = duration + 0.1f;
            backButton.Select();
        }
    }

    public void CreditsToMain()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(true));
            buttonTimer = duration + 0.1f;
            playButton.Select();
        }
    }

    public void BeginGame()
    {
        loader.LoadNextLevel("Town");
    }
    //public void SlideLeft()
    //{
    //    if (buttonTimer <= 0)
    //    {
    //        StartCoroutine(MoveElements(false));
    //        buttonTimer = duration;
    //    }
    //}

    //public void SlideRight()
    //{
    //    if (buttonTimer <= 0)
    //    {
    //        StartCoroutine(MoveElements(true));
    //        buttonTimer = duration;
    //    }
    //}

    IEnumerator MoveElements(bool right)
    {
        float time = 0;
        float moveAmount = moving;

        if (!right)
        {
            moveAmount = -moving;
        }

        Vector2 titleVector = new Vector2(mainTitle.transform.position.x + moveAmount, 0);
        Vector2 creditsVector = new Vector2(credits.transform.position.x + moveAmount, 0);

        Vector2 mainOG = mainTitle.transform.position;
        Vector2 creditsOG = credits.transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            mainTitle.gameObject.transform.position = Vector2.Lerp(mainOG, titleVector, t);
            credits.gameObject.transform.position = Vector2.Lerp(creditsOG, creditsVector, t);

            yield return null;
        }
    }

    private void ResetVariables()
    {
        StaticData.highLowerDone = false;
        StaticData.quizDone = false;
        StaticData.keysDone = false;
        StaticData.playerSpawnPoint = 0;
        StaticData.coolLuckyNumber = 0;
        StaticData.key1 = false;
        StaticData.key2 = false;
        StaticData.key3 = false;
        StaticData.talkedToYellow = false;
    }
}
