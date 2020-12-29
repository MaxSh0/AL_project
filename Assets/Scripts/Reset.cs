using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Reset : MonoBehaviour
{

    public GameObject ResetButton;
    public GameObject Camera;
    public GameObject WinnerText;
    public GameObject DrawText;
    void Update()
    {
        if (NotNullObject(Camera.GetComponent<Spawn>().Heroes) == 1) 
        {
            ResetButton.SetActive(true);
            WinnerText.SetActive(true);
        }
        else if(NotNullObject(Camera.GetComponent<Spawn>().Heroes) == 0) 
        {
            ResetButton.SetActive(true);
            DrawText.SetActive(true);
        }
    }


    //Метод перезапуска сцены
    public void OnResetButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    int NotNullObject(GameObject[] objects)
    {
        int num = 0;
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                num++;
            }
        }
        return num;
    }
}
