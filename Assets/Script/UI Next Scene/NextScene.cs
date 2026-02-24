using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
   public void GotoInc()
    {
        SceneManager.LoadScene("Inc");
        
    }
    public void GotoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
