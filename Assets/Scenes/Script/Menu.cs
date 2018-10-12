using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void LoadLevel1()
    {
        Debug.Log("To");
        SceneManager.LoadScene(0);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(1);
    }
}
