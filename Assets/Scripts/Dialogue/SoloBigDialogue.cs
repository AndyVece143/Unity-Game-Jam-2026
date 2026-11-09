using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
public class SoloBigDialogue : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI nameText;
    public DialogueData data;
    public float textSpeed;
    private int index;
    public Canvas canvas;

    public Portrait character1;
    public float moveDuration;

    private Vector3 character1Position;
    public GameObject textBox;
    private Vector3 textBoxPosition;
    private Vector3 character1EndPosition;
    private Vector3 textBoxEndPosition;

    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;
    private bool ending = false;
    public float dampSpeed;
    public Image background;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainText.text = string.Empty;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        mainText.text = string.Empty;
        nameText.text = data.Sentences[index].name;

        character1.GetComponent<Image>().enabled = false;
        background.enabled = false;
        textBox.SetActive(false);
        StartCoroutine(TransitionBeginning());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (ready == true)
            {
                NextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && ending == false)
        {
            StopAllCoroutines();
            StartCoroutine(MoveSpritesEnd());
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        ready = false;
        if (index < data.Sentences.Count - 1)
        {
            index++;
            ChangeEmotion();
            mainText.text = string.Empty;
            nameText.text = data.Sentences[index].name;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(MoveSpritesEnd());
        }
    }

    void BeginningSprite()
    {
        //ChangeEmotion();
        //character1.ChangeEmotion(data.Sentences[index].emotion);
        //character1.anim.SetBool("done", true);
        character1.SetEmotion(data.Sentences[index].emotion);
    }

    void ChangeEmotion()
    {
        character1.ChangeEmotion(data.Sentences[index].emotion);
        //character1.anim.SetBool("done", true);
    }

    void SetPositions()
    {
        character1Position = character1.transform.position;
        textBoxPosition = textBox.transform.position;

        character1.transform.position = new Vector3(character1Position.x - 12f, character1Position.y, character1Position.z);
        textBox.transform.position = new Vector3(textBoxPosition.x, textBoxPosition.y - 5, textBoxPosition.z);

        character1EndPosition = character1.transform.position;
        textBoxEndPosition = textBox.transform.position;

        StartCoroutine(MoveSpritesBeginning());
    }

    IEnumerator TransitionBeginning()
    {
        //StartCoroutine(displayController.FadeToWhite(1));
        //yield return new WaitForSeconds(1.1f);
        Vector3 bgPosition = background.transform.position;
        background.transform.position = new Vector3(bgPosition.x + 20f, bgPosition.y, bgPosition.z);
        Vector3 bgPositionEnd = background.transform.position;
        background.enabled = true;

        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;
            background.transform.position = Vector3.Lerp(bgPositionEnd, bgPosition, t);
            yield return null;
        }

        //StartCoroutine(displayController.FadeFromWhite(1));
        //yield return new WaitForSeconds(1);
        character1.GetComponent<Image>().enabled = true;

        textBox.SetActive(true);
        BeginningSprite();
        SetPositions();
    }

    IEnumerator TransitionEnd()
    {
        Vector3 bgPosition = background.transform.position;
        Vector3 bgPositionEnd = new Vector3(bgPosition.x - 20f, bgPosition.y, bgPosition.z);
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;
            background.transform.position = Vector3.Lerp(bgPosition, bgPositionEnd, t);
            yield return null;
        }

        //if (data.canPlayerMove == true)
        //{
        //    player.StartMoving();
        //}


        Destroy(gameObject);
    }

    IEnumerator TypeLine()
    {
        character1.anim.SetBool("done", false);
        int i = 2;
        string originalText = data.Sentences[index].Text;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in data.Sentences[index].Text.ToCharArray())
        {
            alphaIndex++;
            mainText.text = originalText;
            displayedText = mainText.text.Insert(alphaIndex, HTML_ALPHA);
            mainText.text = displayedText;

            i++;
            if (i == 5)
            {
                SoundManager.instance.PlaySound(data.Sentences[index].sound);
                i = 0;
            }

            yield return new WaitForSeconds(textSpeed);
        }
        ready = true;
        Debug.Log("done");
        character1.DoneTalking();
    }

    IEnumerator MoveSpritesBeginning()
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = 1.0f - Mathf.Exp(-dampSpeed * Time.deltaTime);
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1Position, t);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxPosition, t);
            yield return null;
        }
        StartDialogue();
    }

    IEnumerator MoveSpritesEnd()
    {
        ending = true;
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = 1.0f - Mathf.Exp(-dampSpeed * Time.deltaTime);
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1EndPosition, t);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxEndPosition, t);
            yield return null;
        }
        //if (sceneTransition)
        //{
        //    loader.LoadNextLevel(sceneName);
        //}
        //if (canPlayerMove)
        //{
        //    player.StartMoving();
        //    mainCamera.state = mainCamera.initialState;
        //}

        //mainCamera.state = mainCamera.initialState;
        StartCoroutine(TransitionEnd());
        //Destroy(gameObject);

        //player.state = Player.State.Standard;
        //mainCamera.state = CameraController.State.FollowPlayer;
        //mainCamera.anim.enabled = false;
    }
}
