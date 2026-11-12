using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuizPaper : MonoBehaviour
{
    public QuizQuestion data;
    public TMP_Text questionText;
    public TMP_Text labelText;
    //public Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //canvas = GetComponent<Canvas>();
        //canvas.worldCamera = Camera.main;
        questionText.text = data.question;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillMyself()
    {
        Destroy(gameObject);
    }
}
