using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager Instance { get; private set; }

    private SoldierPickup[] soldiers;
    public void ResetAllSoldiers()
    {
        if (soldiers == null)
            soldiers = FindObjectsOfType<SoldierPickup>(true);

        foreach (var s in soldiers)
        {
            if (s != null)
                s.ResetSoldier();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 自动找到场景中所有士兵
        soldiers = FindObjectsOfType<SoldierPickup>(true);
    }

    /// <summary>
    /// 返回还没被拾取/还可见的士兵数量
    /// </summary>
    public int GetRemainingSoldiers()
    {
        if (soldiers == null) return 0;

        int count = 0;
        foreach (var s in soldiers)
        {
            if (s != null && !s.IsPicked())
                count++;
        }
        return count;
    }

    /// <summary>
    /// 每次拾取成功后调用，检查是否胜利
    /// </summary>
    public void CheckWin()
    {
        int remaining = GetRemainingSoldiers();
        // Debug.Log("Remaining soldiers: " + remaining);

        if (remaining <= 0)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.Win();
        }
    }
}
