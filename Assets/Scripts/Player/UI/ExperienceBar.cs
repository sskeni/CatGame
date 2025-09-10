using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    // UI References
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (PlayerController.Instance != null) UpdateExperienceVariables(PlayerController.Instance.level);
    }

    // Updates the UI slider and text fields
    private void UpdateExperienceVariables(PlayerLevel playerLevel)
    {
        // Update Slider
        slider.maxValue = playerLevel.levelUpExperience;
        slider.value = playerLevel.currentExperience;

        // Update Texts
        experienceText.text = playerLevel.currentExperience + " / " + playerLevel.levelUpExperience;
        levelText.text = playerLevel.level.ToString();
    }
}
