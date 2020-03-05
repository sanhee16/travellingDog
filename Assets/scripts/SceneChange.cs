using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject cam;
    public void SceneChange0()
    {
        SceneManager.LoadScene(0);
    }
    public void SceneChange1()
    {
        SceneManager.LoadScene(1);
    }

}
