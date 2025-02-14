using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public Transform player; // Player 

    void Start()
    {
        if (PlayerPrefs.HasKey("HasSavedGame"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            player.position = new Vector3(x, y, z);
        }
    }
}
