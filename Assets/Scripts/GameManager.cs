using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Xử lý khi bấm "Chơi Lại"
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Xử lý khi bấm "Quay Về Menu"
    public void BackToMenu()
    {
        Time.timeScale = 1;

        // xóa dữ liệu đã lưu 
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); 

        // quay về menu
        SceneManager.LoadScene("MainMenu");
    }
}
