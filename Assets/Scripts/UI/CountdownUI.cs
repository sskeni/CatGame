using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    private static CountdownUI instance;
    public static CountdownUI Instance { get { return instance; } }

    public TextMeshProUGUI countdownText;
    public int countdownTime;

    private void Awake()
    {
        CheckSingleton();
        this.gameObject.SetActive(false);
    }

    // Sets up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Opens the countdown UI
    [ContextMenu("Start Countdown")]
    public void OpenUI()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Countdown());
    }

    // Counts down and then reloads the level
    private IEnumerator Countdown()
    {
        int timeLeft = countdownTime;
        while (timeLeft >= 0)
        {
            countdownText.text = string.Format("Continuing in: {0}", timeLeft);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        RoomHandler.Instance.ReloadRooms();
    }
}
