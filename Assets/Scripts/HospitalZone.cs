using UnityEngine;

public class HospitalZone : MonoBehaviour
{
    [Header("Debug")]
    public bool printDebug = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ✅ 兼容直升机碰到的是子物体 Collider 的情况
        HelicopterCargo cargo = other.GetComponentInParent<HelicopterCargo>();
        if (cargo == null) return;

        int unloaded = cargo.UnloadToHospital();
        if (printDebug && unloaded > 0)
        {
            Debug.Log($"Unloaded {unloaded} soldier(s) to hospital. Total rescued: {cargo.soldiersRescued}");
        }
    }
}
