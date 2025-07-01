using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // This line brings in EditorApplication
#endif

public class QuitGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stops Play Mode in Editor
#else
        Application.Quit(); // Quits the built application
#endif
    }
}
