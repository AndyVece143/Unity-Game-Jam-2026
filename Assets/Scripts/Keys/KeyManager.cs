using System.Collections;
using TMPro;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public Portrait playerPortrait;
    public Portrait yellowPortrait;

    public ThrownKey[] keys;
    public GameObject directions;
    private bool keysThrown = false;
    private bool introDone = false;
    public TMP_Text screenText;
    public float waitTime;
    public AudioClip click;
    public AudioClip winSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Beginning());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && keysThrown == false && introDone == true)
        {
            keysThrown = true;
            playerPortrait.ChangeEmotionMinigame(9);
            SoundManager.instance.PlaySound(click);
            StartCoroutine(ThrowingKeys());
        }
    }

    IEnumerator ThrowingKeys()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            StartCoroutine(keys[i].GoRight());
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(waitTime);
        SoundManager.instance.PlaySound(winSound);
        screenText.text = "WINNER";
        screenText.color = Color.green;
        playerPortrait.ChangeEmotionMinigame(1);
        yellowPortrait.ChangeEmotionMinigame(3);
        yield return new WaitForSeconds(3);
        StaticData.keysDone = true;
        StaticData.playerSpawnPoint = 3;
        LevelLoader loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
        loader.LoadNextLevel("Town");
    }

    IEnumerator Beginning()
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

        introDone = true;
        screenText.text = "GIVE KEYS";
    }
}
