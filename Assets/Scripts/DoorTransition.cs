using System.Collections;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    public Vector3 playerTeleportPoint;
    public Vector3 cameraTeleportPoint;
    public Player player;
    public CameraController mainCamera;
    public bool changeMusic;
    public AudioClip newSong;
    public ScreenTransition screenTrans;
    public bool goingIndoors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        screenTrans = ScreenTransition.FindAnyObjectByType<ScreenTransition>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.state != Player.State.NoMove)
        {
            player.StopMoving();
            StartCoroutine(TeleportTime());
        }
    }

    IEnumerator TeleportTime()
    {
        if (goingIndoors)
        {
            screenTrans.BecomeTransBlack();

        }
        else
        {
            screenTrans.BecomeTransWhite();

        }
        yield return new WaitForSeconds(1.1f);

        if (goingIndoors)
        {
            mainCamera.StopFollowingPlayer();
        }
        else
        {
            mainCamera.BeginFollowingPlayer();
        }
        player.transform.position = playerTeleportPoint;
        mainCamera.transform.position = cameraTeleportPoint;
        yield return new WaitForSeconds(1.4f);
        player.StartMoving();
    }
}
