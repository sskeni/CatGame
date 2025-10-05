using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    // Singleton References
    private static PlayerCoins instance;
    public static PlayerCoins Instance { get { return instance; } }

    // Private References
    [SerializeField] private int coins;

    private void Awake()
    {
        CheckSingleton();
    }

    // Set up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Adds coins
    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
        RunStatisticsHandler.Instance.totalCoinsCollected += coinsToAdd;
    }

    // Removes coins
    public void RemoveCoins(int coinsToRemove)
    {
        coins -= coinsToRemove;
    }

    // Returns current coin count
    public int CoinCount()
    {
        return coins;
    }
}
