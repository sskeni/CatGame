using UnityEngine;

public class PlayerLevel : MonoBehaviour
{

    // Variables
    [SerializeField] private float initialLevelUpExperience;
    public float levelUpExperience { get; private set; }
    public float currentExperience { get; private set; }
    public int level { get; private set; } = 1;


    private void Awake()
    {
        levelUpExperience = initialLevelUpExperience;
    }

    // Gives player set experience
    // Levels up if the player gains enough experience
    public void GiveExperience(float experience)
    {
        currentExperience = currentExperience + experience;
        if (currentExperience >= levelUpExperience)
        {
            LevelUp();
        }
    }

    // Level up
    // Context menu for debugging purposes
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
