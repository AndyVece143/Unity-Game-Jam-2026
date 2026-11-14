using UnityEngine;

public class Disclaimer : MonoBehaviour
{
    public LevelLoader loader;
    private bool transition = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && transition == false)
        {
            transition = true;
            loader.LoadNextLevel("Title");
        }
    }
}
