using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(LoadNextScene), 5f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
