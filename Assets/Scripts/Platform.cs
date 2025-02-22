using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < -6f) // Khi chạm đáy màn hình, hủy đối tượng
        {
            Destroy(gameObject);
        }
    }
}
