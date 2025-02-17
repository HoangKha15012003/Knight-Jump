using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public static LevelComplete instance; 
    public GameObject winPanel;  // Bảng thông báo thắng
    public TMP_Text winText;     // Hiển thị thời gian hoàn thành
    public GameObject nextPanel; 
    private float startTime;
    private bool levelCompleted = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startTime = Time.time;
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gate") && !levelCompleted)
        {
            levelCompleted = true;
            float elapsedTime = Time.time - startTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            if (winText != null)
                winText.text = $"Cảm ơn bạn đã trải nghiệm trò chơi!\nThời gian hoàn thành: {minutes:D2}:{seconds:D2}";

            if (winPanel != null)
                winPanel.SetActive(true);

            Time.timeScale = 0; // Dừng game

            /*UnlockNextLevel();*/

          
        }
        
        
    }
    
    // UPDATE SAU
    /*void UnlockNextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        Debug.Log($"📌 Hoàn thành màn {currentLevel}, màn đã mở khóa: {unlockedLevel}");

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log($"✅ Đã mở khóa màn {currentLevel + 1}");
        }
    }*/

    public void NextPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false); // Đóng panel hiện tại
        }

        if (nextPanel != null)
        {
            nextPanel.SetActive(true); // Hiển thị panel tiếp theo
        }
        else
        {
            // Nếu không có panel tiếp theo, có thể load màn mới hoặc quay về menu
            Time.timeScale = 1; // Đặt lại tốc độ game
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Chuyển sang màn tiếp theo "up date sau"
        }
    }

    public bool IsWinPanelActive()
    {
        return winPanel.activeSelf; // Kiểm tra trạng thái winPanel
    }
}
