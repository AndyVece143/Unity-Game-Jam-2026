using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    private float regularSpeed;
    public Rigidbody2D body;
    public BoxCollider2D boxCollider;
    public Animator anim;
    private Vector2 movement;
    public enum State
    {
        Standard,
        NoMove,
    }
    public State state;
    public SpriteRenderer inspectIcon;
    public InteractableObject closestObject;
    public GamerGirl closestGirl;
    public YellowLady closestYellow;
    public OverworldKey closestKey;
    public GuesserManager closestGuess;
    public TrueLady closestTrue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        regularSpeed = speed;
        inspectIcon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Standard:
                Movement();
                break;
            case State.NoMove:
                StayStill();
                break;
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = regularSpeed * 2;
            anim.speed = 2;
        }
        else
        {
            speed = regularSpeed;
            anim.speed = 1;
        }
        movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        body.linearVelocity = movement * speed;

        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);

        if (movement != Vector2.zero)
        {
            anim.SetFloat("lasthorizontal", movement.x);
            anim.SetFloat("lastvertical", movement.y);
        }
    }

    public void StayStill()
    {
        movement.Set(0, 0);
        body.linearVelocity = Vector2.zero;
        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);
        if (movement != Vector2.zero)
        {
            anim.SetFloat("lasthorizontal", movement.x);
            anim.SetFloat("lastvertical", movement.y);
        }
    }

    public void StartMoving()
    {
        state = State.Standard;
        anim.Play("Idle");
    }

    public void StopMoving()
    {
        movement.Set(0, 0);
        body.linearVelocity = Vector2.zero;

        state = State.NoMove;
    }

    public void Talk(InputAction.CallbackContext context)
    {
        if (context.started && closestObject != null && state == State.Standard)
        {
            closestObject.GenerateTextBox();
            StopMoving();
            inspectIcon.enabled = false;
        }

        if (context.started && closestGirl != null && state == State.Standard)
        {
            closestGirl.TalkToGirl();
            StopMoving();
            inspectIcon.enabled = false;
        }

        if (context.started && closestYellow != null && state == State.Standard)
        {
            closestYellow.TalkToYellow();
            StopMoving();
            inspectIcon.enabled = false;
        }

        if (context.started && closestKey != null && state == State.Standard)
        {
            closestKey.PickUpKey();
            StopMoving();
            inspectIcon.enabled = false;
        }

        if (context.started && closestGuess != null && state == State.Standard)
        {
            closestGuess.AttemptGuess();
            StopMoving();
            inspectIcon.enabled = false;
        }

        if (context.started && closestTrue != null && state == State.Standard)
        {
            closestTrue.TrueGirlTalk();
            StopMoving();
            inspectIcon.enabled = false;
        }
    }

    public IEnumerator GoToPlace(Vector2 location, float duration, Vector2 direction)
    {
        StartMoving();
        state = State.NoMove;
        float time = 0;
        Vector2 startingPos = transform.position;

        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
        movement.Set(direction.x, direction.y);

        while (time < duration)
        {
            anim.SetFloat("horizontal", direction.x);
            anim.SetFloat("vertical", direction.y);
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector2.Lerp(startingPos, location, t);
            yield return null;
        }

        transform.position = location;

        switch (direction)
        {
            case Vector2 v when v.x == 1 && v.y == 0:
                anim.Play("idleright");
                break;

            case Vector2 v when v.x == 0 && v.y == 1:
                anim.Play("idleup");
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inspect" && state != State.NoMove)
        {
            inspectIcon.enabled = true;

            if (collision.gameObject.GetComponent("InteractableObject") as InteractableObject != null)
            {
                closestObject = collision.gameObject.GetComponent<InteractableObject>();
                if (collision.gameObject.GetComponent<InteractableObject>().checker == false)
                {
                    inspectIcon.color = Color.white;
                }
                else
                {
                    inspectIcon.color = Color.gray;
                }
            }

            if (collision.gameObject.GetComponent("GamerGirl") as GamerGirl != null)
            {
                closestGirl = collision.gameObject.GetComponent<GamerGirl>();
                inspectIcon.color = Color.white;
            }

            if (collision.gameObject.GetComponent("YellowLady") as YellowLady != null)
            {
                closestYellow = collision.gameObject.GetComponent<YellowLady>();
                inspectIcon.color = Color.white;
            }

            if (collision.gameObject.GetComponent("OverworldKey") as OverworldKey != null)
            {
                closestKey = collision.gameObject.GetComponent<OverworldKey>();
                inspectIcon.color = Color.white;
            }

            if (collision.gameObject.GetComponent("GuesserManager") as GuesserManager != null)
            {
                closestGuess = collision.gameObject.GetComponent<GuesserManager>();
                inspectIcon.color = Color.white;
            }

            if (collision.gameObject.GetComponent("TrueLady") as TrueLady != null)
            {
                closestTrue = collision.gameObject.GetComponent<TrueLady>();
                inspectIcon.color = Color.white;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inspect")
        {
            inspectIcon.enabled = false;
            closestObject = null;
            closestGirl = null;
            closestYellow = null;
            closestKey = null;
            closestGuess = null;
            closestTrue = null;
        }
    }
}
