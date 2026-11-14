using UnityEngine;

public class TrueLady : MonoBehaviour
{
    public Player player;
    public BigDialogue dialogue;
    public InspectBox inspectBox;
    public DialogueData data;
    public string[] lines;

    public BoxCollider2D boxCollider;
    private int progress = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void TrueGirlTalk()
    {
        if (progress == 0)
        {
            BigDialogue newDialogue = Instantiate(dialogue);
            newDialogue.data = data;
            progress++;
        }
        else
        {
            InspectBox newBox = Instantiate(inspectBox);
            newBox.lines = lines;
        }
    }
}
