using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindZone : MonoBehaviour
{
    public float windForce = 5f; // Lực gió
    public float windDuration = 3f; // Thời gian gió thổi liên tục
    public float windPause = 5f; // Thời gian giữa các lần chuyển đổi gió
    public int waveCount = 5; // Số lượng đường gợn sóng
    public float waveLength = 2f; // Độ dài của các đường gợn sóng
    public float waveSpeed = 1f; // Tốc độ di chuyển của các đường gợn sóng
    public float waveHeight = 0.5f; // Chiều cao của gợn sóng
    public Vector3 waveOffset = new Vector3(0, 0.5f, 0); // Vị trí lệch của gợn sóng từ vị trí nhân vật

    private bool isWindActive = false; // Trạng thái của gió
    private float currentWindForce = 0f; // Lực gió hiện tại
    private List<LineRenderer> waves = new List<LineRenderer>(); // Danh sách các đường gợn sóng
    private List<Rigidbody2D> affectedObjects = new List<Rigidbody2D>(); // Danh sách các đối tượng bị ảnh hưởng
    private Transform playerTransform; // Lưu trữ transform của nhân vật

    private void Start()
    {
        StartCoroutine(WindCycle());
    }

    private void Update()
    {
        if (isWindActive && playerTransform != null)
        {
            // Lấy vị trí của nhân vật khi có gió
            Vector3 playerPosition = playerTransform.position;

            // Di chuyển các đường gợn sóng xung quanh nhân vật
            MoveWaves(playerPosition);

            // Áp dụng lực gió lên các đối tượng trong vùng gió
            ApplyWindForce();
        }
        else
        {
            foreach (var wave in waves)
            {
                wave.enabled = false; // Tắt các đường gợn sóng khi không có gió
            }
        }
    }

    private void MoveWaves(Vector3 playerPosition)
    {
        float direction = Mathf.Sign(currentWindForce);

        for (int i = 0; i < waves.Count; i++)
        {
            LineRenderer lr = waves[i];
            lr.enabled = true;

            int segmentCount = lr.positionCount;

            for (int j = 0; j < segmentCount; j++)
            {
                Vector3 pos = lr.GetPosition(j);

                // Di chuyển theo gió
                pos.x += direction * waveSpeed * Time.deltaTime;

                // **Giảm độ dao động theo thời gian**
                pos.y += Mathf.Sin(Time.time * waveSpeed + j * 0.5f) * 0.002f; // **Giảm biên độ dao động**

                lr.SetPosition(j, pos);
            }
        }
    }

    private void ApplyWindForce()
    {
        foreach (var rb in affectedObjects)
        {
            if (rb != null)
            {
                // Áp dụng lực gió lên đối tượng (nhân vật) trong vùng gió
                rb.AddForce(new Vector2(currentWindForce, 0), ForceMode2D.Force);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && other.CompareTag("Player"))
        {
            if (!affectedObjects.Contains(rb))
            {
                affectedObjects.Add(rb);
                playerTransform = other.transform;
                CreateWaves(playerTransform.position); // Tạo gợn sóng khi nhân vật vào vùng gió
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && affectedObjects.Contains(rb))
        {
            affectedObjects.Remove(rb);
            playerTransform = null; // Dừng việc tạo gợn sóng khi nhân vật ra khỏi vùng gió
        }
    }

    private void CreateWaves(Vector3 playerPosition)
    {
        // Xóa các gợn sóng cũ trước khi tạo mới
        foreach (var wave in waves)
        {
            Destroy(wave.gameObject);
        }
        waves.Clear();

        // Xác định hướng xuất hiện của gợn sóng
        float direction = Mathf.Sign(currentWindForce);

        for (int i = 0; i < waveCount; i++)
        {
            GameObject waveObj = new GameObject($"Wave_{i}");
            waveObj.transform.parent = transform;
            LineRenderer lr = waveObj.AddComponent<LineRenderer>();

            int segmentCount = 20; // Số điểm trên sóng sin
            lr.positionCount = segmentCount;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = new Color(1, 1, 1, 0.6f);
            lr.endColor = new Color(1, 1, 1, 0.3f);

            // **Luôn đặt sóng gần nhân vật, không dựa vào sóng trước**
            Vector3 startPosition = playerPosition + waveOffset + new Vector3(direction * Random.Range(0.1f, 0.3f), 0, 0);

            for (int j = 0; j < segmentCount; j++)
            {
                float x = startPosition.x + (j / (float)(segmentCount - 1)) * waveLength;
                float y = startPosition.y + Mathf.Sin((j / (float)(segmentCount - 1)) * Mathf.PI * 2) * (waveHeight * 0.5f);
                lr.SetPosition(j, new Vector3(x, y, 0));
            }

            waves.Add(lr);
        }
    }

    IEnumerator WindCycle()
    {
        while (true)
        {
            // Gió thổi ngẫu nhiên
            isWindActive = true;
            currentWindForce = Random.Range(0, 2) == 0 ? -windForce : windForce; // Gió thổi trái hoặc phải

            // **Đặt sóng lại gần nhân vật khi gió đổi hướng**
            if (playerTransform != null)
            {
                CreateWaves(playerTransform.position);
            }

            yield return new WaitForSeconds(windDuration);

            // Dừng gió
            isWindActive = false;
            currentWindForce = 0f;
            yield return new WaitForSeconds(windPause);
        }
    }
}
