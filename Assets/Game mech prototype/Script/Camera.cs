using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;     // The capsule (player)
    public float rotateSpeed = 60f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(target.position, Vector3.up, -rotateSpeed * Time.deltaTime);
        }
    }
}
