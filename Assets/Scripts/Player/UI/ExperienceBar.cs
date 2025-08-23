using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        PlayerController.Instance.playerLevel.OnExperienceChanged += new OnExperienceChangedEventHandler(UpdateExperienceVariables);
    }


    private void UpdateExperienceVariables(float levelUpExperience, float currentExperience, int level)
    {
        // Update Slider
        slider.maxValue = levelUpExperience;
        slider.value = currentExperience;

        // Update Texts
        experienceText.text = currentExperience + " / " + levelUpExperience;
        levelText.text = level.ToString();
    }
}
