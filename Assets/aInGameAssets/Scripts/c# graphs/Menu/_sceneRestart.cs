using UnityEngine;
using UnityEngine.SceneManagement;

public class _sceneRestart : MonoBehaviour
{
    public KeyCode restartKey = KeyCode.R;
   
    void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            RestartScene();
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

