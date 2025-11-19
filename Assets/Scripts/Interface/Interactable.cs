using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void Start()
    {
        PlayerController.Instance.controls.Player.Interact.performed += ctx => Interact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            EnterTrigger(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            ExitTrigger(collision);
    }

    protected abstract void EnterTrigger(Collider2D collision);

    protected abstract void ExitTrigger(Collider2D collision);

    public abstract void Interact();
}
