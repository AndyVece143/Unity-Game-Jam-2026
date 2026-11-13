using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    public enum State
    {
        FollowPlayer,
        StayStill,
    }
    public State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.FollowPlayer:
                FollowPlayer();
                break;
            case State.StayStill:
                break;
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = player.transform.position;
        //targetPosition.y = 0;
        targetPosition.z = -10;

        if (targetPosition.x < 0f)
        {
            targetPosition.x = 0f;
        }

        transform.position = targetPosition;
    }

    public void BeginFollowingPlayer()
    {
        state = State.FollowPlayer;
    }

    public void StopFollowingPlayer()
    {
        state = State.StayStill;
    }
}
