using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Leaderboard : MonoBehaviour
{


    public GameObject[] Heroes;
    public GameObject Lboard;
    public TMP_Text textLeaderboard;
    bool textfull =false ;

    public void OnGetMoney() 
    {
        if (NotNullObject(Heroes) == 1 && !textfull)
        {
            Lboard.SetActive(true);
            textLeaderboard.text += "1. " +Winner().GetComponent<Hero>().textName.text + " Монет:" + Winner().GetComponent<Hero>().Score+"\n";
            textfull = true;
        }

    }



    GameObject Winner() 
    {
        foreach (GameObject Hero in Heroes) 
        {
            if(Hero != null) 
            {
                return Hero;
            }
        }
        return null;
    }

    public int NotNullObject(GameObject[] objects)
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
