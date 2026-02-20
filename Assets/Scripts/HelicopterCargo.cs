using UnityEngine;

public class HelicopterCargo : MonoBehaviour
{
    [Header("Capacity")]
    public int maxCapacity = 3;

    [Header("Runtime")]
    public int soldiersInHelicopter = 0;

    [Header("Score")]
    public int soldiersRescued = 0;

    [Header("Audio")]
    public AudioSource audioSource;      // 拖 Helicopter 上的 AudioSource
    public AudioClip pickupClip;         // 拖你的拾取音效
    [Range(0f, 1f)] public float pickupVolume = 1f;

    private void Awake()
    {
        // 如果忘了拖，自动找一下（更稳）
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public bool IsFull() => soldiersInHelicopter >= maxCapacity;

    public bool TryPickupOneSoldier()
    {
        if (IsFull())
        {
            Debug.Log("Cargo FULL! Can't pick up more.");
            return false;
        }

        soldiersInHelicopter++;
        Debug.Log($"Picked up soldier. In helicopter: {soldiersInHelicopter}/{maxCapacity}");

        // ✅ 拾取成功：播放音效
        PlayPickupSfx();

        return true;
    }

    private void PlayPickupSfx()
    {
        Debug.Log("播放音效");
        if (audioSource == null || pickupClip == null) return;
        audioSource.PlayOneShot(pickupClip, pickupVolume);
    }

    public int UnloadToHospital()
    {
        if (soldiersInHelicopter <= 0) return 0;

        int unloaded = soldiersInHelicopter;
        soldiersRescued += unloaded;
        soldiersInHelicopter = 0;
        return unloaded;
    }
}