using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    public HelicopterCargo cargo;

    [Header("TMP Text")]
    public TextMeshProUGUI inHelicopterText;
    public TextMeshProUGUI rescuedText;

    void Update()
    {
        if (cargo == null) return;

        // In helicopter: 2/3
        if (inHelicopterText != null)
            inHelicopterText.text = $"In Helicopter: {cargo.soldiersInHelicopter}/{cargo.maxCapacity}";

        // Rescued: 5
        if (rescuedText != null)
            rescuedText.text = $"Rescued: {cargo.soldiersRescued}";
    }
}
