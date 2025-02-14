using UnityEngine;

public class CameraFollowY : MonoBehaviour
{
    public Transform player;  // Gán nhân vật vào đây
    public float offsetY = 2.0f; // Khoảng cách giữa nhân vật và đáy màn hình
    public float smoothSpeed = 5.0f; // Tốc độ di chuyển mượt

    private float fixedX; // Giữ nguyên trục X của camera

    private void Start()
    {
        fixedX = transform.position.x; // Lưu vị trí X ban đầu của camera
    }

    private void LateUpdate()
    {
        // Tính toán vị trí mới của camera
        float targetY = player.position.y + offsetY;
        Vector3 targetPosition = new Vector3(fixedX, targetY, transform.position.z);

        // Di chuyển camera một cách mượt mà
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
