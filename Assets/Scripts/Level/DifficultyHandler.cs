using UnityEngine;

public class DifficultyHandler : MonoBehaviour
{
    private static DifficultyHandler instance;
    public static DifficultyHandler Instance { get { return instance; } }

    public float difficulty = 1;

    private void Awake()
    {
        CheckSingleton();
    }

    // Set up Singleton and don't destroy on load
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
}
