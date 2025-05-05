using UnityEngine;
using UnityEngine.SceneManagement;

public class _sceneLoader : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;
    
    public void LoadScene()
    {
        Debug.Log("Scene Loaded");
        SceneManager.LoadScene(sceneIndex);
    }
}
