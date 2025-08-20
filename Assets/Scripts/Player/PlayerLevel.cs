using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private float initialLevelUpExperience;
    public float levelUpExperience { get; private set; }
    public float currentExperience { get; private set; }
    public int level { get; private set; } = 1;

    private void Awake()
    {
        levelUpExperience = initialLevelUpExperience;
    }

    private void Start()
    {
        float cumulative = 0f;
        for (int i = 1; i <= 10; i++)
        {
            cumulative += CalculateNextLevelExperienceAmount(i);
        }
    }

    // Gives player set experience, also handles leveling up
    public void GiveExperience(float experience)
    {
        currentExperience = currentExperience + experience;
        if (currentExperience >= levelUpExperience)
        {
            LevelUp();
        }
    }

    // Level up
    private void LevelUp()
    {
        // Increase level
        level++;

        // Store leftover experience
        currentExperience = currentExperience - levelUpExperience;

        // Calculate next levelUpExperience
        levelUpExperience = CalculateNextLevelExperienceAmount(level);

        // Open Item UI
        ItemUIManager.Instance.OpenUI();
    }

    // Calculate how much experience is needed for next level up
    private float CalculateNextLevelExperienceAmount(int level)
    {
        //return Mathf.Pow(((initialLevelUpExperience/16) * level), 1.5f) + initialLevelUpExperience;
        return Mathf.Round((initialLevelUpExperience * Mathf.Log10(level)) + initialLevelUpExperience);
    }
}
