using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public HelicopterMovement helicopterMovement;
    public Rigidbody2D helicopterRb;           // ✅ 新增：用于清速度
    public Transform helicopterTransform;       // ✅ 新增：用于重置位置
    public HelicopterCargo cargo;              // ✅ 新增：用于清空计数（Rescued/InHeli）

    [Header("End UI")]
    public GameObject endPanel;
    public TextMeshProUGUI endMessage;

    [Header("Restart")]
    public KeyCode restartKey = KeyCode.R;
    [Header("End SFX")]
    public AudioSource sfxSource;     // 拖 GameManager 上的 AudioSource（可不拖，会自动 GetComponent）
    public AudioClip winClip;         // 拖胜利音效
    public AudioClip loseClip;        // 拖失败音效
    [Range(0f, 1f)] public float endSfxVolume = 1f;

    private bool endSfxPlayed = false; // 防止重复播放
    public bool IsGameOver { get; private set; }
    public bool IsWin { get; private set; }

    private Vector3 heliStartPos;              // ✅ 记录开局位置

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        if (endPanel != null) endPanel.SetActive(false);
        IsGameOver = false;
        IsWin = false;
        endSfxPlayed = false;

        if (helicopterTransform != null)
            heliStartPos = helicopterTransform.position;
    }
    private void Update()
    {
        // ✅ 只有在胜利/失败状态才允许按 R 重开（更符合街机）
        if ((IsGameOver || IsWin) && Input.GetKeyDown(restartKey))
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        if (IsWin || IsGameOver) return;

        IsGameOver = true;
        Debug.Log("GAME OVER!");

        if (helicopterMovement != null)
            helicopterMovement.SetMoveEnabled(false);
        PlayEndSfx(loseClip);
        ShowEndUI("GAME OVER\nPress R to Restart");

    }

    public void Win()
    {
        if (IsGameOver || IsWin) return;

        IsWin = true;
        Debug.Log("YOU WIN!");

        if (helicopterMovement != null)
            helicopterMovement.SetMoveEnabled(false);
        PlayEndSfx(winClip);
        ShowEndUI("YOU WIN!\nPress R to Restart");
    }

    private void ShowEndUI(string msg)
    {
        if (endPanel != null) endPanel.SetActive(true);
        if (endMessage != null) endMessage.text = msg;
    }

    // ✅ 第12条：重置逻辑
    public void RestartGame()
    {
        endSfxPlayed = false;
        Debug.Log("Restarting...");

        // 1) 状态重置
        IsGameOver = false;
        IsWin = false;

        // 2) 隐藏结束UI
        if (endPanel != null) endPanel.SetActive(false);

        // 3) 恢复直升机移动
        if (helicopterMovement != null)
            helicopterMovement.SetMoveEnabled(true);

        // 4) 重置直升机位置 + 速度
        if (helicopterTransform != null)
            helicopterTransform.position = heliStartPos;

        if (helicopterRb != null)
            helicopterRb.velocity = Vector2.zero;

        // 5) 清空机舱与救援计数（重开归零）
        if (cargo != null)
        {
            cargo.soldiersInHelicopter = 0;
            cargo.soldiersRescued = 0;
        }

        // 6) 重置所有士兵（重新显示 + 可拾取）
        if (SoldierManager.Instance != null)
            SoldierManager.Instance.ResetAllSoldiers();
    }
    private void PlayEndSfx(AudioClip clip)
    {
        if (endSfxPlayed) return;
        if (sfxSource == null || clip == null) return;

        endSfxPlayed = true;
        sfxSource.PlayOneShot(clip, endSfxVolume);
    }
}
