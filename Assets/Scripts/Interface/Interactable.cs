using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void Start()
    {
        PlayerController.Instance.controls.Player.Interact.performed += ctx => Interact();
    }

    public abstract void Interact();
}
