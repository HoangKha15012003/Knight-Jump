﻿/*using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public GameObject settingsPanel;
    private bool isPaused = false;
    public Transform player;
    public PlayerMovement playerMovement;
    public AudioManager audioManager;
 

    void Start()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Mở bằng phím ESC
        {
            ToggleSettings();
        }
    }

    // Phương thức này dùng chung cho cả ESC và Button
    public void ToggleSettings()
    {
        

        if (isPaused)
            CloseSettings();
        else
            OpenSettings();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0;
        playerMovement.isPaused = true;
        isPaused = true;
        playerMovement.canMove = false;
        playerMovement.canJump = false;
        audioManager.isPaused = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        playerMovement.isPaused = false;
        playerMovement.canMove = true;
        playerMovement.canJump = true;
        audioManager.isPaused = false;
    }

    public void SaveAndExit()
    {
        SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    void SaveGame()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.Save();
    }
}
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public GameObject settingsPanel;
    private bool isPaused = false;
    public Transform player;
    public PlayerMovement playerMovement;
    public AudioManager audioManager;

    void Start()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Mở bằng phím ESC
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        // Kiểm tra nếu winPanel đang bật thì không mở settings
        if (LevelComplete.instance != null && LevelComplete.instance.IsWinPanelActive())
        {
            Debug.Log("⚠️ Không thể mở cài đặt khi đã thắng!");
            return;
        }

        if (isPaused)
            CloseSettings();
        else
            OpenSettings();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0;
        playerMovement.isPaused = true;
        isPaused = true;
        playerMovement.canMove = false;
        playerMovement.canJump = false;
        audioManager.isPaused = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        playerMovement.isPaused = false;
        playerMovement.canMove = true;
        playerMovement.canJump = true;
        audioManager.isPaused = false;
    }

    public void SaveAndExit()
    {
        SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    void SaveGame()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.Save();
    }
}

