using UnityEngine;

public class GuesserManager : MonoBehaviour
{
    public NumberGuesser guesser;
    public BoxCollider2D boxCollider;
    public Player player;
    public BigDialogue dialogue;
    public DialogueData data;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void AttemptGuess()
    {
        NumberGuesser newGuess = Instantiate(guesser);
    }

    public void SpawnDialogue()
    {
        BigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data;
    }

    public void OffMyself()
    {
        Destroy(gameObject);
    }
}
