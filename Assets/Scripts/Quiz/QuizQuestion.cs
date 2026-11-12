using UnityEngine;

[CreateAssetMenu(menuName = "QuizGame/QuizQuestion")]
public class QuizQuestion : ScriptableObject
{
    [TextArea(3, 10)]
    public string question;
    public int answer;
}
