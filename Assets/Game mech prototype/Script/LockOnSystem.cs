using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public float lockOnRange = 20f;
    public Transform currentTarget;
    public Camera mainCamera;

    public float playerTurnSpeed = 10f;

    private bool isLockedOn = false;
    private Quaternion originalCameraRotation;

    void Start()
    {
        if (mainCamera != null)
        {
            originalCameraRotation = mainCamera.transform.rotation;
        }
    }

    void Update()
    {
        // Lock ON / OFF (manual)
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isLockedOn)
            {
                Transform closestEnemy = FindClosestEnemy();
                if (closestEnemy != null)
                {
                    currentTarget = closestEnemy;
                    isLockedOn = true;

                    if (mainCamera != null)
                        originalCameraRotation = mainCamera.transform.rotation;

                    Debug.Log("Locked onto: " + currentTarget.name);
                }
            }
            else
            {
                ClearLockOn();
            }
        }

        // Switch target to closest enemy (manual)
        if (isLockedOn && Input.GetKeyDown(KeyCode.LeftControl))
        {
            Transform closestEnemy = FindClosestEnemy();
            if (closestEnemy != null && closestEnemy != currentTarget)
            {
                currentTarget = closestEnemy;
                Debug.Log("Switched target to closest: " + currentTarget.name);
            }
        }

        // Rotate player to face target
        if (isLockedOn && currentTarget != null)
        {
            Vector3 dir = currentTarget.position - transform.position;
            dir.y = 0f;

            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * playerTurnSpeed);
            }

            // Gently rotate camera toward target
            if (mainCamera != null)
            {
                Vector3 camDir = currentTarget.position - mainCamera.transform.position;
                Quaternion camRot = Quaternion.LookRotation(camDir);
                mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, camRot, Time.deltaTime * 2f);
            }
        }

        // If target dies or disappears
        if (isLockedOn && currentTarget == null)
        {
            ClearLockOn();
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= lockOnRange && distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    void ClearLockOn()
    {
        currentTarget = null;
        isLockedOn = false;

        // Reset camera to original facing
        if (mainCamera != null)
        {
            mainCamera.transform.rotation = originalCameraRotation;
        }

        Debug.Log("Lock-on cleared.");
    }
}