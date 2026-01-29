using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);
    [SerializeField] private float followSpeed = 5f;

    [Header("Optional Limits")]
    [SerializeField] private bool useLimits = false;
    [SerializeField] private Vector2 minLimits; 
    [SerializeField] private Vector2 maxLimits; 

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (useLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minLimits.x, maxLimits.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minLimits.y, maxLimits.y);
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
