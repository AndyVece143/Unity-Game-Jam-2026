using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public List<QuizQuestion> allQuestions;
    public List<QuizQuestion> selectedQuestions;
    public Button number1Button;
    public TMP_InputField playerAnswer;
    public QuizPaper paper;
    public int questionCount;
    private bool buffering;
    public int currentQuestion = 0;
    public int correctAnswers = 0;
    public Portrait playerPortrait;

    public AnswerBlock[] answers;
    public TMP_Text screenText;

    public GameObject directions;

    public AudioClip click;
    public AudioClip winSound;
    public AudioClip lose;

    //public AnswerBlock answer1;
    //public AnswerBlock answer2;
    //public AnswerBlock answer3;
    //public AnswerBlock answer4;
    //public AnswerBlock answer5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedQuestions = GenerateList(allQuestions, questionCount);
        StartCoroutine(Beginning());
        //StartCoroutine(SpawnPaper());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<QuizQuestion> GenerateList(List<QuizQuestion> everything, int count)
    {
        List<QuizQuestion> results = new List<QuizQuestion>();
        List<QuizQuestion> availableQuestions = new List<QuizQuestion>(everything);

        int itemsToPick = Mathf.Min(count, availableQuestions.Count);

        for (int i = 0; i < itemsToPick; i++)
        {
            int randomIndex = Random.Range(0, availableQuestions.Count);
            results.Add(availableQuestions[randomIndex]);
            availableQuestions.RemoveAt(randomIndex);
        }

        return results;
    }

    public void InputNumber(int i)
    {
        if (playerAnswer.text.Length < 1)
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

        StartCoroutine(SpawnPaper());
    }

    public IEnumerator SpawnPaper()
    {
        QuizPaper newPaper = Instantiate(paper, gameObject.transform);
        newPaper.data = selectedQuestions[currentQuestion];
        newPaper.labelText.text = "Question " + (currentQuestion + 1);
        currentQuestion++;

        Vector3 centerPosition = newPaper.transform.position;
        newPaper.transform.position = new Vector3(centerPosition.x, centerPosition.y - 12f, centerPosition.z);
        Vector3 offScreenPosition = newPaper.transform.position;

        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            newPaper.transform.position = Vector3.Lerp(offScreenPosition, centerPosition, t);
            yield return null;
        }
        number1Button.Select();
        playerAnswer.text = string.Empty;
        buffering = false;
        playerPortrait.ChangeEmotionMinigame(2);
    }

    public IEnumerator DespawnPaper()
    {
        QuizPaper newPaper = QuizPaper.FindAnyObjectByType<QuizPaper>();
        Vector3 centerPosition = newPaper.transform.position;
        Vector3 offScreenPosition = new Vector3(centerPosition.x, centerPosition.y - 12f, centerPosition.z);

        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            newPaper.transform.position = Vector3.Lerp(centerPosition, offScreenPosition, t);
            yield return null;
        }

        newPaper.KillMyself();

        yield return new WaitForSeconds(0.5f);

        if (currentQuestion < 5)
        {
            StartCoroutine(SpawnPaper());
        }
        else
        {
            StartCoroutine(RevealAnswers());
        }
    }

    public void GiveAnAnswer()
    {
        if (buffering == false && playerAnswer.text.Length == 1)
        {
            buffering = true;
            playerPortrait.ChangeEmotionMinigame(9);
            SoundManager.instance.PlaySound(click);
            int answerInt = int.Parse(playerAnswer.text);
            QuizPaper newPaper = QuizPaper.FindAnyObjectByType<QuizPaper>();

            if (answerInt == newPaper.data.answer)
            {
                correctAnswers++;
                answers[currentQuestion- 1].UpdateText(currentQuestion, true);
                Debug.Log("CORRECT");
            }
            else
            {
                answers[currentQuestion- 1].UpdateText(currentQuestion, false);
                Debug.Log("WRONG");
            }

            StartCoroutine(DespawnPaper());
        }
    }

    IEnumerator RevealAnswers()
    {
        playerPortrait.ChangeEmotionMinigame(6);
        screenText.text = "GRADING...";
        Debug.Log("YAHAH YOU FOUND ME");
        for (int i = 0; i < answers.Length; i++)
        {
            StartCoroutine(answers[i].ScrollUp());
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1.5f);

        if (correctAnswers >= 3)
        {
            playerPortrait.ChangeEmotionMinigame(1);
            screenText.text = "YOU PASSED!";
            SoundManager.instance.PlaySound(winSound);
            screenText.color = Color.green;

            yield return new WaitForSeconds(3);
            StaticData.quizDone = true;
            StaticData.playerSpawnPoint = 2;
            LevelLoader loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
            loader.LoadNextLevel("Town");
        }
        else
        {
            playerPortrait.ChangeEmotionMinigame(5);
            SoundManager.instance.PlaySound(lose);
            screenText.text = "YOU FAILED!";
            screenText.color = Color.red;

            yield return new WaitForSeconds(3);
            LevelLoader loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
            loader.LoadNextLevel("Quiz");
        }
        //StartCoroutine(answers[0].ScrollUp());
        //yield return new WaitForSeconds(0.5f);
        //StartCoroutine(answers[1].ScrollUp());
        //yield return new WaitForSeconds(0.5f);
        //StartCoroutine(answers[2].ScrollUp());
        //yield return new WaitForSeconds(0.5f);
        //StartCoroutine(answers[3].ScrollUp());
        //yield return new WaitForSeconds(0.5f);
        //StartCoroutine(answers[4].ScrollUp());
    }

    //public IEnumerator SubmitAnswer()
    //{
    //    if (playerAnswer.text.Length == 1)
    //    {

    //    }
    //}
}
