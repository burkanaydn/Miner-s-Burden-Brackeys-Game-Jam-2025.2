using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target; // takip edilecek obje (player)
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f); // kamera konum offset
    [SerializeField] private float followSpeed = 5f; // takip hýzý, ne hýzlý ne yavaþ

    [Header("Optional Limits")]
    [SerializeField] private bool useLimits = false;
    [SerializeField] private Vector2 minLimits; // kamera minimum x,y
    [SerializeField] private Vector2 maxLimits; // kamera maksimum x,y

    private void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyonu + offset
        Vector3 desiredPosition = target.position + offset;

        // Limitler uygulanacaksa
        if (useLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minLimits.x, maxLimits.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minLimits.y, maxLimits.y);
        }

        // Smooth hareket
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
