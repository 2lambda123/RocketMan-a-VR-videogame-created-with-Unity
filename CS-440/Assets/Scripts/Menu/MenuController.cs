using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public FadeScreen fadeScreen;

    public void PlayButton()
    {
        fadeScreen.fadeOut(changeScene, "Rocket");
        
    }

    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
