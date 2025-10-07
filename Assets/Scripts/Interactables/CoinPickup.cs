using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinPickup : MonoBehaviour
{
    // Public References
    public int coinAmount;
    public float initialXForce;
    public float initialYForce;
    public float initialYForceVariance;

    public Rigidbody2D rb;
    public GameObject parentObject;
    public Animator anim;

    public AudioClip pickUpSoundClip;
    public float pickUpVolume;
    public float pickUpPitchRange;

    // Private References
    private bool pickupable;

    private void Start()
    {
        pickupable = false;

        StartCoroutine(DelayPickUpable());

        float xForce = Random.Range(-initialXForce, initialXForce);
        float yForce = Random.Range(initialYForce, initialYForce + initialYForceVariance);
        Vector2 initialForce = new Vector2(xForce, yForce); // Random initial movement
        rb.AddForce(initialForce);

        anim.Play("Bob", -1, Random.Range(0f, 1f)); // Set the animation to a random interval
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && pickupable)
        {
            SoundFXHandler.Instance.PlaySoundFXClip(pickUpSoundClip, transform, pickUpVolume, pickUpPitchRange, pickUpPitchRange);
            PlayerCoins.Instance.AddCoins(coinAmount);
            Destroy(parentObject);
        }
    }

    // Delay so it doesn't immediately get picked up
    private IEnumerator DelayPickUpable()
    {
        yield return new WaitForSeconds(1);
        pickupable = true;
    }
}
