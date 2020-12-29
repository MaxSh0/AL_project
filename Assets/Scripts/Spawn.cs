using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] Heroes;
    public GameObject[] AreaElements;
    public GameObject[] Heals;
    public GameObject[,] Area = new GameObject[8,8];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<8; i++) 
        {
            for (int j = 0; j < 8; j++) 
            {
                Area[i,j] = AreaElements[i * 8 + j];
            }

        }
    
        foreach (GameObject Hero in Heroes) 
        {
            int row = 0;
            int column = 0;
            GameObject cell;
            do
            {
                row = Random.Range(0, 7);
                column = Random.Range(0, 7);
                cell = Area[row, column];


            } while (cell == null);

            Area[row, column] = null;

            Hero.transform.position = new Vector3(cell.transform.position.x, 37.5f, cell.transform.position.z);
        }
        foreach(GameObject Heal in Heals) 
        {
            int row =0;
            int column =0;
            GameObject cell;
            do
            {
                row = Random.Range(0, 7);
                column = Random.Range(0, 7);
                cell = Area[row, column];

                
            } while (cell == null);

            Area[row, column] = null;

            Heal.transform.position = new Vector3(cell.transform.position.x, 37.5f, cell.transform.position.z);
        }

    }
}
