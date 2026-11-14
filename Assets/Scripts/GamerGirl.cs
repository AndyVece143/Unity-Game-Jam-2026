using UnityEngine;

public class GamerGirl : MonoBehaviour
{
    public Player player;
    public BigDialogue dialogue;
    public InspectBox inspectBox;
    public DialogueData data1;
    public string[] text;
    public BoxCollider2D boxCollider;
    public int minigame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TalkToGirl()
    {
        

        switch (minigame)
        {
            //Higher Lower
            case 0:
                if (StaticData.highLowerDone == false)
                {
                    BigDialogue newDialogue = Instantiate(dialogue);
                    newDialogue.data = data1;
                }
                else
                {
                    InspectBox newInspectBox = Instantiate(inspectBox);
                    newInspectBox.lines = text;
                }
                break;
            //Quiz
            case 1:
                if (StaticData.quizDone == false)
                {
                    BigDialogue newDialogue = Instantiate(dialogue);
                    newDialogue.data = data1;
                }
                else
                {
                    InspectBox newInspectBox = Instantiate(inspectBox);
                    newInspectBox.lines = text;
                }
                break;
            //Keys
            case 2:
                if (StaticData.keysDone == false)
                {
                    BigDialogue newDialogue = Instantiate(dialogue);
                    newDialogue.data = data1;
                }
                else
                {
                    InspectBox newInspectBox = Instantiate(inspectBox);
                    newInspectBox.lines = text;
                }
                break;
        }
    }
}
