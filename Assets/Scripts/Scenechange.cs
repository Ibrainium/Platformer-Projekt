using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    public void LoadWinScene()
    {
        // Enable cursor before switching screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Load scene
        SceneManager.LoadScene("Win");
    }

    public void LoadMainLevel()
    {
        // No cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Load scene
        SceneManager.LoadScene("prototype");
    }
    public void LoadGameOverScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      
        SceneManager.LoadScene("Gameover");
    }
    public void LoadMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SceneManager.LoadScene("Main menu");
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pirate_man"))
        {
            Debug.Log("Trigger entered by pirate_man");
            LoadWinScene();

        
        }
    }
}
