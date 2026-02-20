using System.Diagnostics;
using UnityEngine;

public class SoldierPickup : MonoBehaviour
{
    private bool picked = false;

    private SpriteRenderer sr;
    private Collider2D col;
    public bool IsPicked() => picked;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return;
        if (!other.CompareTag("Helicopter")) return;
        print("已经被拾取");
        HelicopterCargo cargo = other.GetComponent<HelicopterCargo>();
        if (cargo == null) return;

        // ✅ 先判断是否能拾取
        bool success = cargo.TryPickupOneSoldier();
        if (!success)
        {
            // 满载：不拾取，不隐藏士兵
            return;
        }

        // ✅ 成功拾取：标记 + 隐藏
        picked = true;
        HideSoldier();
        if (SoldierManager.Instance != null)
            SoldierManager.Instance.CheckWin();

    }

    private void HideSoldier()
    {
        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void ResetSoldier()
    {
        picked = false;
        if (sr != null) sr.enabled = true;
        if (col != null) col.enabled = true;

        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
}
