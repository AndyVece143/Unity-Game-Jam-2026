using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerBlock : MonoBehaviour
{
    public TMP_Text text;
    public Vector3 originalPosition;
    public Vector3 offscreenPosition;
    public float duration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        transform.position = new Vector3(originalPosition.x, originalPosition.y - 10f, originalPosition.z);
        offscreenPosition = transform.position;
    }

    public void UpdateText(int i, bool correct)
    {
        if (correct == true)
        {
            text.color = Color.forestGreen;
            text.text = "Question " + i + ": Correct";
        }
        else
        {
            text.color = Color.red;
            text.text = "Question " + i + ": Wrong";
        }
    }

    public IEnumerator ScrollUp()
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / 1f;
            transform.position = Vector3.Lerp(offscreenPosition, originalPosition, t);
            yield return null;
        }
    }
}
