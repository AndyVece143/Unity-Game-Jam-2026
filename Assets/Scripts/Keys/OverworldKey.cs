using UnityEngine;

public class OverworldKey : MonoBehaviour
{
    public Player player;
    public string[] lines;
    public InspectBox inspectBox;
    public BoxCollider2D boxCollider;
    public int keyNumber;

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

    public void UnaliveMyself()
    {
        Destroy(gameObject);
    }

    public void PickUpKey()
    {
        InspectBox newInspectBox = Instantiate(inspectBox);
        newInspectBox.lines = lines;

        switch (keyNumber)
        {
            case 1:
                StaticData.key1 = true;
                break;
            case 2:
                StaticData.key2 = true;
                break;
            case 3:
                StaticData.key3 = true;
                break;
        }

        Destroy(gameObject);
    }
}
