using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TownManager : MonoBehaviour
{
    public Player player;
    public SoloBigDialogue soloDialogue;
    public BigDialogue bigDialogue;
    public BigDialogue blueDialogue;
    public DialogueData data1;
    public DialogueData data2;
    public DialogueData blueData;

    public BigDialogue redDialogue;
    public DialogueData redData;

    public BigDialogue yellowDialogue;
    public DialogueData yellowData;

    public DialogueData finalData;

    public GameObject entranceWall;

    public int progress = 0;
    public int gameProgress = 0;
    public Light2D townLight;
    public Color morningColor;
    public Color afternoonColor;
    public Color eveningColor;
    public Color nightColor;

    public Vector2 openingSpawn;
    public Vector2 higherLowerSpawn;
    public Vector2 quizSpawn;
    public Vector2 keysSpawn;

    public CameraController mainCamera;

    public Vector3 higherLowerCameraSpawn;
    public Vector3 quizCameraSpawn;

    public int luckyNumber;
    public AudioClip daytheme;
    public AudioClip nighttheme;

    public OverworldKey[] keys;
    public GameObject master;
    public AudioClip byebye;

    public GameObject trueLady;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckGameProgress();
        KillKeys();

        if (gameProgress == 0)
        {
            StartCutscene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckGameProgress()
    {
        if (StaticData.highLowerDone == true)
        {
            gameProgress++;
        }
        if (StaticData.quizDone == true)
        {
            gameProgress++;
        }
        if (StaticData.keysDone == true)
        {
            gameProgress++;
        }

        switch (gameProgress)
        {
            case 0:
                townLight.color = morningColor;
                townLight.intensity = 1;
                MusicPlayer.instance.ChangeSong(daytheme);
                break;
            case 1:
                townLight.color = afternoonColor;
                townLight.intensity = 0;
                MusicPlayer.instance.ChangeSong(daytheme);
                break;
            case 2:
                townLight.color = eveningColor;
                townLight.intensity = 1;
                MusicPlayer.instance.ChangeSong(nighttheme);
                break;
            case 3:
                townLight.color = nightColor;
                townLight.intensity = 1;
                MusicPlayer.instance.ChangeSong(nighttheme);
                trueLady.SetActive(true);
                break;
        }

        switch (StaticData.playerSpawnPoint)
        {
            case 0:
                player.transform.position = openingSpawn;

                break;
            case 1:
                player.transform.position = higherLowerSpawn;
                mainCamera.transform.position = higherLowerCameraSpawn;
                mainCamera.StopFollowingPlayer();
                StartCoroutine(BlueCutscene());
                break;
            case 2:
                player.transform.position = quizSpawn;
                mainCamera.transform.position = quizCameraSpawn;
                mainCamera.StopFollowingPlayer();
                StartCoroutine(RedCutscene());
                break;
            case 3:
                player.transform.position = keysSpawn;
                StartCoroutine(YellowCutscene());
                break;

        }
    }

    private void KillKeys()
    {
        if (StaticData.key1 == true)
        {
            keys[0].UnaliveMyself();
        }
        if (StaticData.key2 == true)
        {
            keys[1].UnaliveMyself();
        }
        if (StaticData.key3 == true)
        {
            keys[2].UnaliveMyself();
        }
    }

    public void StartCutscene()
    {
        switch (progress)
        {
            case 0:
                StartCoroutine(Cutscene());
                progress++;
                break;
            case 1:
                StartCoroutine(Cutscene2());
                progress++;
                break;
        }
    }

    public void FlyAway()
    {
        StartCoroutine(FlyingAway());
    }
    private IEnumerator Cutscene()
    {
        luckyNumber = Random.Range(100000, 999999);
        StaticData.coolLuckyNumber = luckyNumber;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(player.GoToPlace(new Vector2(-2, 0), 3, new Vector2(1, 0)));
        yield return new WaitForSeconds(3.5f);
        entranceWall.SetActive(true);
        SoloBigDialogue newDialogue = Instantiate(soloDialogue);
        newDialogue.data = data1;
    }

    private IEnumerator Cutscene2()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(player.GoToPlace(new Vector2(2.5f, 0), 2, new Vector2(1, 0)));
        yield return new WaitForSeconds(2);
        StartCoroutine(player.GoToPlace(new Vector2(2.5f, 0.1f), 0.1f, new Vector2(0, 1)));
        yield return new WaitForSeconds(1);
        BigDialogue newDialogue = Instantiate(bigDialogue);
        newDialogue.data = data2;
    }

    private IEnumerator BlueCutscene()
    {
        Debug.Log(StaticData.coolLuckyNumber);
        yield return new WaitForSeconds(1.5f);
        BigDialogue newDialogue = Instantiate(blueDialogue);
        newDialogue.data = blueData;
        int i = Mathf.Abs(StaticData.coolLuckyNumber);
        while (i >= 100)
        {
            i /= 10;
        }
        newDialogue.data.Sentences[2].Text = "The first two digits are " + i + ".";
    }

    private IEnumerator RedCutscene()
    {
        Debug.Log(StaticData.coolLuckyNumber);
        yield return new WaitForSeconds(1.5f);
        BigDialogue newDialogue = Instantiate(redDialogue);
        newDialogue.data = redData;

        string numberString = StaticData.coolLuckyNumber.ToString();

        //int i = int.Parse(numberString.Substring(2, 2));
        int thirdDigit = numberString[2] - '0';
        int fourthDigit = numberString[3] - '0';
        newDialogue.data.Sentences[2].Text = "The third and fourth digits are " + thirdDigit + "" + fourthDigit + ".";
    }

    private IEnumerator YellowCutscene()
    {
        yield return new WaitForSeconds(1.5f);
        BigDialogue newDialogue = Instantiate(yellowDialogue);
        newDialogue.data = yellowData;

        string numberString = StaticData.coolLuckyNumber.ToString();
        int fifthDigit = numberString[4] - '0';
        int sixthDigit = numberString[5] - '0';
        newDialogue.data.Sentences[2].Text = "The fifth and sixth digits are " + fifthDigit + "" + sixthDigit + ".";
    }

    private IEnumerator FlyingAway()
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 startingPos = master.transform.position;

        SoundManager.instance.PlaySound(byebye);
        float time = 0;
        while (time < 5)
        {
            time += Time.deltaTime;
            master.transform.Rotate(Vector3.forward * (360 * Time.deltaTime));
            float t = time / 5;

            master.transform.position = Vector2.Lerp(startingPos, new Vector2(startingPos.x, startingPos.y + 6), t);
            yield return null;
        }

        Destroy(master);

        player.StartMoving();
    }

    private IEnumerator FinalCutscene()
    {
        StartCoroutine(player.GoToPlace(new Vector2(200, -2), 2, new Vector2(0, 1)));
        yield return new WaitForSeconds(3);
        SoloBigDialogue newDialogue = Instantiate(soloDialogue);
        newDialogue.data = finalData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(FinalCutscene());
        }
    }
}
