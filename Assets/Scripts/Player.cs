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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inspect")
        {
            inspectIcon.enabled = false;
            closestObject = null;
        }
    }
}
