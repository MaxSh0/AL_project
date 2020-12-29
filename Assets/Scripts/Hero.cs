using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Hero : MonoBehaviour
{
    public int HP = 10;
    public int Score = 0;
    public TMP_Text textName;
    public TMP_Text textHP;
    public TMP_Text textScore;
    public TMP_Text textLOG;
    public GameObject Lboard;
    private Leaderboard ScriptLeader; 


    void Start() 
    {
        ScriptLeader = Lboard.GetComponent<Leaderboard>();
    }
    void Update()
    {
        if (HP<=0) 
        {
            textLOG.text += "\n" + textName.text + " погиб со счетом:" + Score.ToString();
            Lboard.GetComponent<Leaderboard>().textLeaderboard.text +=ScriptLeader.NotNullObject(Lboard.GetComponent<Leaderboard>().Heroes).ToString() + ". " + textName.text + " Монет:"+ textScore.text + "\n";
            Destroy(gameObject);
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            int Value = Random.Range(1, 4);
            Destroy(other.gameObject);
            Score += Value;
            textScore.text = Score.ToString();
        }
        else if (other.gameObject.CompareTag("Heal")) 
        {
            Destroy(other.gameObject);
            if (HP + 5 <= 10)
            {
                HP += 5;
            }
            else
            {
                HP = 10;
            }
            textHP.text = HP.ToString();
        }
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
