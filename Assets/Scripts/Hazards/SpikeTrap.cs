using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem.Instance?.TakeDamage(damage);

            // Bounce player away from spikes
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 bounceDir = (other.transform.position - transform.position).normalized;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(bounceDir.x * 4f, 8f), ForceMode2D.Impulse);
            }
        }
    }
}
