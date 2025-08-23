using UnityEngine;

public delegate void OnExperienceChangedEventHandler(float levelUpExperience, float currentExperience, int level);

public class PlayerLevel : MonoBehaviour
{

    // Variables
    [SerializeField] private float initialLevelUpExperience;
    private float levelUpExperience;
    private float currentExperience;
    private int level = 1;

    // Events
    public event OnExperienceChangedEventHandler OnExperienceChanged;

    private void Awake()
    {
        levelUpExperience = initialLevelUpExperience;
    }

    private void Start()
    {
        OnExperienceChanged?.Invoke(levelUpExperience, currentExperience, level);
    }

    // Gives player set experience, also handles leveling up
    public void GiveExperience(float experience)
    {
        currentExperience = currentExperience + experience;
        if (currentExperience >= levelUpExperience)
        {
            LevelUp();
        }
        OnExperienceChanged?.Invoke(levelUpExperience, currentExperience, level);
    }

    // Level up
    [ContextMenu("Level Up")]
    private void LevelUp()
    {
        // Increase level
        level++;

        // Store leftover experience
        if (currentExperience >= levelUpExperience) currentExperience -= levelUpExperience;

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
