using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int damage = 10;
    public float attackDuration = 0.2f;

    private Collider swordCollider;

    void Start()
    {
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false; // Off by default
    }

    void Update()
    {
        // Attack input (Left Click)
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(EnableHitbox());
        }
    }

    System.Collections.IEnumerator EnableHitbox()
    {
        swordCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        swordCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy for " + damage + " damage!");
        }
    }
}
