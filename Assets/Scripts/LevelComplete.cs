/*using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public GameObject winPanel;  // Bảng thông báo thắng
    public TMP_Text winText;     // Hiển thị thời gian hoàn thành

    private float startTime;
    private bool levelCompleted = false;

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

            winText.text = $"Bạn đã hoàn thành màn chơi!\nThời gian: {minutes:D2}:{seconds:D2}";
            winPanel.SetActive(true);
            Time.timeScale = 0; // Dừng game

            UnlockNextLevel();
        }
    }

    void UnlockNextLevel()
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
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelection");
    }
}
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public static LevelComplete instance; // Singleton để dễ truy cập
    public GameObject winPanel;  // Bảng thông báo thắng
    public TMP_Text winText;     // Hiển thị thời gian hoàn thành

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

            winText.text = $"Bạn đã hoàn thành màn chơi!\nThời gian: {minutes:D2}:{seconds:D2}";
            winPanel.SetActive(true);
            Time.timeScale = 0; // Dừng game

            UnlockNextLevel();
        }
    }

    void UnlockNextLevel()
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
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelection");
    }

    public bool IsWinPanelActive()
    {
        return winPanel.activeSelf; // Kiểm tra trạng thái winPanel
    }
}
