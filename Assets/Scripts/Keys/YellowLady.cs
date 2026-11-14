using UnityEngine;

public class YellowLady : MonoBehaviour
{
    public Player player;
    public BigDialogue dialogue;
    public InspectBox inspectBox;
    public DialogueData data1;
    public DialogueData data2;
    public string[] text;
    public string[] text2;
    public BoxCollider2D boxCollider;
    public bool talkedOnce = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (StaticData.talkedToYellow == true)
        {
            talkedOnce = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TalkToYellow()
    {
        if (talkedOnce == false && StaticData.keysDone == false)
        {
            StaticData.talkedToYellow = true;
            talkedOnce = true;
            BigDialogue newDialogue = Instantiate(dialogue);
            newDialogue.data = data1;
        }
        else if (talkedOnce == true && StaticData.keysDone == false)
        {
            if (StaticData.key1 == true && StaticData.key2 == true && StaticData.key3 == true)
            {
                BigDialogue newDialogue = Instantiate(dialogue);
                newDialogue.data = data2;
            }
            else
            {
                InspectBox newInspectBox = Instantiate(inspectBox);
                newInspectBox.lines = text;
            }
        }
        else
        {
            InspectBox newInspectBox = Instantiate(inspectBox);
            newInspectBox.lines = text2;
        }
    }
}
