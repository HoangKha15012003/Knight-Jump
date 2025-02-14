using UnityEngine;
//using Cinemachine;
public class CameraLockX : MonoBehaviour
{
    public Transform player; // Nhân vật cần theo dõi
    private float minY; // Lưu vị trí thấp nhất của camera
    private float fixedX; // Lưu vị trí X cố định

    private void Start()
    {
        minY = transform.position.y; // Khởi tạo vị trí Y ban đầu của camera
        fixedX = transform.position.x; // Khởi tạo vị trí X cố định
    }

    private void LateUpdate()
    {
        if (player.position.y > minY) // Camera chỉ di chuyển lên
        {
            transform.position = new Vector3(fixedX, player.position.y, transform.position.z);
            minY = transform.position.y; // Ngăn camera di chuyển xuống
        }
    }
}
