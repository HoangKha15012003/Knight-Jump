using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons;     // Các nút chọn màn
    public Image[] levelImages;       // Các hình ảnh của màn chơi
    public Sprite[] unlockedSprites;  // Ảnh màu khi màn chơi mở khóa
    public Sprite[] lockedSprites;    // Ảnh trắng đen khi màn chơi bị khóa

    void Start()
    {
        LoadLevelProgress();
    }

    void LoadLevelProgress()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"🔹 Màn đã mở khóa: {unlockedLevel}");

        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool isUnlocked = (i + 1) <= unlockedLevel;

            if (levelButtons[i] != null)
                levelButtons[i].interactable = isUnlocked;

            if (levelImages[i] != null)
            {
                // Thay đổi hình ảnh dựa vào trạng thái mở khóa
                levelImages[i].sprite = isUnlocked ? unlockedSprites[i] : lockedSprites[i];
            }
        }
    }

    public void SelectLevel(int levelIndex)
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (levelIndex <= unlockedLevel)
        {
            string sceneName = "Scene" + levelIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log($"⚠️ Màn {levelIndex} chưa được mở khóa!");
        }
    }
}
