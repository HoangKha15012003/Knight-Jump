using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button continueButton; // Nút "Tiếp tục"

    void Start()
    {
        // Kiểm tra xem có dữ liệu lưu trước đó không
        if (PlayerPrefs.HasKey("HasSavedGame"))
        {
            continueButton.gameObject.SetActive(true); // Hiển thị nút "Tiếp tục"
        }
        else
        {
            continueButton.gameObject.SetActive(false); // Ẩn nếu chưa có game đã lưu
        }
    }

    public void OnNewGamePressed()
    {
        // Xóa dữ liệu đã lưu trước đó để bắt đầu game mới
        PlayerPrefs.DeleteKey("HasSavedGame");
        PlayerPrefs.DeleteKey("SavedScene");
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("PlayerZ");
        PlayerPrefs.Save(); // Lưu lại thay đổi

        SceneManager.LoadScene("Scene1"); // Chuyển đến màn chơi
    }

    public void OnContinuePressed()
    {
        string sceneToLoad = PlayerPrefs.GetString("SavedScene", "Level1");
        SceneManager.LoadScene(sceneToLoad);
    }
}