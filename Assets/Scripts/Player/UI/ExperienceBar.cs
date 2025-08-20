using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    private PlayerLevel playerLevel;
    private Slider slider;

    void Start()
    {
        playerLevel = PlayerController.Instance.playerLevel;
        slider = GetComponent<Slider>();
        slider.maxValue = playerLevel.levelUpExperience;
        slider.value = playerLevel.currentExperience;
    }

    void FixedUpdate()
    {
        // Update Slider
        slider.maxValue = playerLevel.levelUpExperience;
        slider.value = playerLevel.currentExperience;

        // Update Texts
        experienceText.text = playerLevel.currentExperience + " / " + playerLevel.levelUpExperience;
        levelText.text = playerLevel.level.ToString();
    }
}
