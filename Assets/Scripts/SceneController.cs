using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


    public void SceneChange(string name)
    {
        AudioManager.Instance.PlayerSFX("GameStart");
        if (name=="Menu")
        {
            AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.PlayerMusic("Menu");

        }
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }



    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
