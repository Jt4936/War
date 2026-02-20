using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HelicopterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float acceleration = 30f;
    public float deceleration = 35f;

    [Header("Clamp To Screen")]
    public float padding = 0.25f;

    private Rigidbody2D rb;
    private Camera cam;
    private Vector2 input;

    private float halfW;
    private float halfH;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfW = sr.bounds.extents.x;
            halfH = sr.bounds.extents.y;
        }
        else
        {
            halfW = halfH = 0.25f;
        }
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate()
    {
        Vector2 targetVel = input * moveSpeed;

        float rate = (input.sqrMagnitude > 0.001f) ? acceleration : deceleration;

        // ✅ 用 rb.velocity（兼容更多 Unity 版本）
        rb.velocity = Vector2.MoveTowards(rb.velocity, targetVel, rate * Time.fixedDeltaTime);

        ClampToCamera();
    }

    private void ClampToCamera()
    {
        if (cam == null) return;

        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 pos = transform.position;

        float minX = min.x + halfW + padding;
        float maxX = max.x - halfW - padding;
        float minY = min.y + halfH + padding;
        float maxY = max.y - halfH - padding;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }

    public void SetMoveEnabled(bool enabled)
    {
        this.enabled = enabled;
        if (!enabled && rb != null) rb.velocity = Vector2.zero;
    }
}
