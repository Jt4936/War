using UnityEngine;

public class HelicopterCrash : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Åöµ½ Tree ¾Í GameOver
        if (collision.collider.CompareTag("Tree"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }
}
