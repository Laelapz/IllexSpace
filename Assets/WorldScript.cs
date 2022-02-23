using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldScript : MonoBehaviour
{
    public void Play () {
        SceneManager.LoadScene("Assets/Scenes/World.unity");
    }

    public void Quit () {
        Application.Quit();
    }
}
