using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = "x " + PlayerCoins.Instance.CoinCount();
    }
}
