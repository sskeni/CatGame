using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    private static LevelUIManager instance;
    public static LevelUIManager Instance { get { return instance; } }

    // UIs
    public EndScreenUI endScreen;
    public InventoryUI inventory;
    public ItemDeletionUI itemDeletion;
    public ItemGivenUI itemGiven;
    public ItemPickerUI itemPicker;

    private void Awake()
    {
        CheckSingleton();
    }

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
}
