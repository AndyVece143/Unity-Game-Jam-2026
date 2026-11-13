using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public Animator anim;

    public void BecomeTransBlack()
    {
        anim.Play("blacktrans");
    }
    public void BecomeTransWhite()
    {
        anim.Play("whitetrans");
    }
}
