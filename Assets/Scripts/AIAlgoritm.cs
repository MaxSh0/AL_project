using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { Attack, Healing, LootCoin, Perseption,  Won };
public class AIAlgoritm : MonoBehaviour
{
    public GameObject[,] Area = new GameObject[8, 8];
    public GameObject[] AreaElements;
    public GameObject[] Heals;
    public GameObject[] Moneys;
    public GameObject[] Heroes;
    public int column, row = 0;

    private int EnemyColumn, EnemyRow = 0;


    private GameObject NearestHeal;
    private GameObject NearestMoney;
    private GameObject NearestEnemy;

    public BattleState _state;
    
    void Start()
    {
        //Инициализирует арену для персонажа
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Area[i, j] = AreaElements[i * 8 + j];
            }
        }
        

    }


    //Вызывает следующую итерацию мира
    public void OnButtonNextStep()
    {
        _state = StateController();

        if (_state == BattleState.Won) 
        {
            gameObject.transform.position = new Vector3(110.4f, 102.7f,0f);
        }
        else if (_state == BattleState.Healing) 
        {
            MoveFastest(NearestHeal);
        }
        else if (_state == BattleState.LootCoin) 
        {
            MoveFastest(NearestMoney);
        }
        else if (_state == BattleState.Perseption) 
        {
            Persecution();
        }
        else if (_state == BattleState.Attack) 
        {
            AttackEnemy();
        }
    }


    //ПОЛНОЦЕННЫЕ СОСТОЯНИЯ


    //Преследует противника
    void Persecution() 
    {
        MoveFastest(NearestEnemy);
    }

    //Атакует противника по 1 хп
    void AttackEnemy() 
    {
        if(EnemiesAffectedArea()) 
        {
            NearestEnemy.GetComponent<Hero>().textHP.text = (NearestEnemy.GetComponent<Hero>().HP -= 1).ToString();  
        }
    }

    //Пытается выйти из зоны поражения ближайшего противника
    /*
    void Escape() 
    {

        if (EnemyRow < row)
        {
            if (EnemyColumn < column)
            {
                if (column + 1 < 7 && row + 1 < 7)
                {
                    VerticalDown();
                    HorizontalRight();
                }
                else if (row < 7 && column == 7)
                {
                    VerticalDown();
                }
                else if (row == 7 && column < 7)
                {
                    HorizontalRight();
                }
                else
                {
                    VerticalUp();
                }
            }
            else
            {
                if (column - 1 > 0 && row + 1 < 7)
                {
                    VerticalDown();
                    HorizontalLeft();
                }
                else if (row < 7 && column == 0)
                {
                    VerticalDown();
                }
                else if (row == 7 && column > 0)
                {
                    HorizontalLeft();
                }
                else
                {
                    VerticalUp();
                }
            }
        }
        else if (EnemyRow > row)
        {
            if (EnemyColumn < column)
            {
                if (column + 1 < 7 && row - 1 > 0)
                {
                    VerticalUp();
                    HorizontalRight();
                }
                else if (row > 0 && column == 7)
                {
                    VerticalDown();
                }
                else if (row == 0 && column < 7)
                {
                    HorizontalRight();
                }
                else
                {
                    VerticalDown();
                }
            }
            else
            {
                if (column - 1 > 0 && row - 1 > 0)
                {
                    VerticalUp();
                    HorizontalLeft();
                }
                else if (row > 0 && column == 0)
                {
                    VerticalUp();
                }
                else if (row == 0 && column > 0)
                {
                    HorizontalLeft();
                }
                else
                {
                    VerticalDown();
                }
            }
        }
        else if (EnemyRow == row)
        {
            if (column < EnemyColumn)
            {
                if (column > 0)
                {
                    HorizontalLeft();
                }
                else if (row < 7)
                {
                    VerticalDown();
                }
                else
                {
                    VerticalUp();
                }
            }
            else
            {
                if (column < 7)
                {
                    HorizontalRight();
                }
                else if (row < 7)
                {
                    VerticalDown();
                }
                else
                {
                    VerticalUp();
                }
            }
        }
        else if (EnemyColumn == column)
        {
            if (row < EnemyRow)
            {

            }
        }
    }
    */

    //Функция перехода между состояниями персонажа
    BattleState StateController() 
    {
        //Обновляем информацию о близжайших объектах
        locate();
        if (ThereObject(Heroes)) 
        {
            locateNearestEnemy();
        }
        if (ThereObject(Heals)) 
        {
            HealSearch();
        }
        if (ThereObject(Moneys)) 
        {
            CoinSearch();
        }
        
        
        BattleState state = new BattleState();
        if (!ThereObject(Heroes)) 
        {
            state = BattleState.Won;
        }
        else if (gameObject.GetComponent<Hero>().HP <= 5 && ThereObject(Heals))
        {
            state = BattleState.Healing;
        }
        else if (EnemiesAffectedArea()) 
        {
            state = BattleState.Attack;
        }
        else if (!PlayerInAffectedArea()&& gameObject.GetComponent<Hero>().HP > 5 && ThereObject(Moneys)) 
        {
            state = BattleState.LootCoin;
        }
        else  
        {
            state = BattleState.Perseption;
        }


        return state;
    }


    //ЗОНЫ ПОРАЖЕНИЯ

    //Сообщает есть ли враг в зоне поражения персонажа с координатами
    bool EnemiesAffectedArea()
    {
        if(Mathf.Abs(row - EnemyRow)<=2 && Mathf.Abs(column - EnemyColumn) <= 2) 
        {
            return true;
        }
        return false;
    }

    //Сообщает находимся ли мы в зоне поражения хоть одного противника
    bool PlayerInAffectedArea() 
    {
        foreach(GameObject Hero in Heroes) 
        {
            if (Hero != null)
            {
                if (Mathf.Abs(row - Hero.GetComponent<AIAlgoritm>().row) <= 2 && Mathf.Abs(column - Hero.GetComponent<AIAlgoritm>().column) <= 2) 
                {
                    return true;
                }
            }
        }
        return false;
    }


    //ПОИСК ПРЕДМЕТОВ И ВРАГОВ

    //Находит ближайшую к персонажу аптечку 
    void HealSearch()
    {
        //Находим близжайшую аптечку
        for (int i = 0; i < Heals.Length; i++)
        {
            if (Heals[i] != null)
            {
                NearestHeal = Heals[i];
                i = Heals.Length;
            }
        }

        var range = NearestHeal.transform.position - gameObject.transform.position;
        for (int i = 1; i < Heals.Length; i++)
        {
            if (Heals[i] != null)
            {
                if (range.sqrMagnitude > (Heals[i].transform.position - gameObject.transform.position).sqrMagnitude)
                {
                    range = Heals[i].transform.position - gameObject.transform.position;
                    NearestHeal = Heals[i];
                }
            }
        }
    }

    //Находит ближайшую к персонажу монету
    void CoinSearch()
    {
        //Находим ближайшую монету
        for (int i = 0; i < Moneys.Length; i++)
        {
            if (Moneys[i] != null)
            {
                NearestMoney = Moneys[i];
                i = Moneys.Length;
            }
        }

        var range = NearestMoney.transform.position - gameObject.transform.position;
        for (int i = 1; i < Moneys.Length; i++)
        {
            if (Moneys[i] != null)
            {
                if (range.sqrMagnitude > (Moneys[i].transform.position - gameObject.transform.position).sqrMagnitude)
                {
                    range = Moneys[i].transform.position - gameObject.transform.position;
                    NearestMoney = Moneys[i];
                }
            }
        }
    }

    //Находит персонажа на доске
    void locate() 
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (gameObject.transform.position.x == Area[i, j].transform.position.x && gameObject.transform.position.z == Area[i, j].transform.position.z)
                {
                    column = j;
                    row = i;
                }
            }
        }
    }

    //Находит ближайшего врага на доске
    void locateNearestEnemy() 
    {
        for (int i = 0; i < Heroes.Length; i++)
        {
            if (Heroes[i] != null)
            {
                NearestEnemy = Heroes[i];
                i = Heroes.Length;
            }
        }

        var range = NearestEnemy.transform.position - gameObject.transform.position;
        for (int i = 1; i < Heroes.Length; i++)
        {
            if (Heroes[i] != null)
            {
                if (range.sqrMagnitude > (Heroes[i].transform.position - gameObject.transform.position).sqrMagnitude)
                {
                    range = Heroes[i].transform.position - gameObject.transform.position;
                    NearestEnemy = Heroes[i];
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (NearestEnemy.transform.position.x == Area[i, j].transform.position.x && NearestEnemy.transform.position.z == Area[i, j].transform.position.z)
                {
                    EnemyColumn = j;
                    EnemyRow = i;
                }
            }
        }
    }


    //ДВИЖЕНИЕ

    //Движения по всем направлениям
    void HorizontalLeft() 
    {
        column -= 1;
        gameObject.transform.position = new Vector3(Area[row, column].transform.position.x, 37.5f, Area[row, column].transform.position.z);
    }
    void HorizontalRight()
    {
        column += 1;
        gameObject.transform.position = new Vector3(Area[row, column].transform.position.x, 37.5f, Area[row, column].transform.position.z);
    }
    void VerticalUp() 
    {
        row -= 1;
        gameObject.transform.position = new Vector3(Area[row, column].transform.position.x, 37.5f, Area[row, column].transform.position.z);
    }
    void VerticalDown()
    {
        row += 1;
        gameObject.transform.position = new Vector3(Area[row, column].transform.position.x, 37.5f, Area[row, column].transform.position.z);
    }

    //Движение к объекту с минимальным кол-вом шагов, обходя других врагов
    void MoveFastest(GameObject target) 
    {
        //Ровно снизу
        if (target.transform.position.x > gameObject.transform.position.x && target.transform.position.z == gameObject.transform.position.z)
        {
            if (ObstacleInWay(row + 1, column))
            {
                if (column - 1 >= 0)
                {
                    HorizontalLeft();
                    VerticalDown();
                }
                else
                {
                    HorizontalRight();
                    VerticalDown();
                }
            }
            else
            {
                VerticalDown();

            }
            

        }
        //Ровно сверху
        else if (target.transform.position.x < gameObject.transform.position.x && target.transform.position.z == gameObject.transform.position.z)
        {
            if (ObstacleInWay(row - 1, column))
            {
                if (column - 1 >= 0)
                {
                    HorizontalLeft();
                    VerticalUp();
                }
                else
                {
                    HorizontalRight();
                    VerticalUp();
                }
            }
            else
            {
                VerticalUp();
            }
        }
        //Ровно слева
        else if (target.transform.position.x == gameObject.transform.position.x && target.transform.position.z < gameObject.transform.position.z)
        {
            if (ObstacleInWay(row, column - 1))
            {
                if (row - 1 >= 0)
                {
                    HorizontalLeft();
                    VerticalUp();
                }
                else
                {
                    HorizontalLeft();
                    VerticalDown();
                }
            }
            else
            {
                HorizontalLeft();
            }
        }
        //Ровно справа
        else if (target.transform.position.x == gameObject.transform.position.x && target.transform.position.z > gameObject.transform.position.z)
        {
            if (ObstacleInWay(row, column + 1))
            {
                if (row - 1 >= 0)
                {
                    HorizontalRight();
                    VerticalUp();
                }
                else
                {
                    HorizontalRight();
                    VerticalDown();
                }
            }
            else
            {
                HorizontalRight();
            }
        }
        //По диагонали слева снизу
        else if (target.transform.position.x > gameObject.transform.position.x && target.transform.position.z < gameObject.transform.position.z)
        {
            if (ObstacleInWay(row + 1, column - 1))
            {
                if (target.transform.position.x > gameObject.transform.position.x + 10)
                {
                    VerticalDown();
                }
                else
                {
                    HorizontalLeft();
                }
            }
            else
            {
                VerticalDown();
                HorizontalLeft();
            }
        }
        //По диагонали справа снизу
        else if (target.transform.position.x > gameObject.transform.position.x && target.transform.position.z > gameObject.transform.position.z)
        {
            if (ObstacleInWay(row + 1, column + 1))
            {
                if (target.transform.position.x > gameObject.transform.position.x + 10)
                {
                    VerticalDown();
                }
                else
                {
                    HorizontalRight();
                }
            }
            else
            {
                VerticalDown();
                HorizontalRight(); 
            }
        }
        //По диагонали слева сверху
        else if (target.transform.position.x < gameObject.transform.position.x && target.transform.position.z < gameObject.transform.position.z)
        {
            if (ObstacleInWay(row - 1, column - 1))
            {
                if (target.transform.position.x < gameObject.transform.position.x - 10)
                {
                    VerticalUp();
                }
                else
                {
                    HorizontalLeft();
                }
            }
            else
            {
                VerticalUp();
                HorizontalLeft();
            }
        }
        //По диагонали справа сверху
        else if (target.transform.position.x < gameObject.transform.position.x && target.transform.position.z > gameObject.transform.position.z)
        {
            if (ObstacleInWay(row - 1, column + 1))
            {
                if (target.transform.position.x < gameObject.transform.position.x - 10)
                {
                    VerticalUp();
                }
                else
                {
                    HorizontalRight();
                }
            }
            else
            {
                VerticalUp();
                HorizontalRight();
            }
        }
    }

    //Говорит есть ли препятствие на пути следования, которое надо обойти
    bool ObstacleInWay(int row_enemy, int column_enemy) 
    {
        foreach(GameObject Hero in Heroes) 
        {
            if (Hero != null)
            {
                if (row_enemy == Hero.GetComponent<AIAlgoritm>().row && column_enemy == Hero.GetComponent<AIAlgoritm>().column)
                {
                    return true;
                }
            }
        }
        return false;
    }





    //ПРОВЕРКИ МАССИВОВ


    //Проверяет есть ли хоть один объект в массиве
    bool ThereObject(GameObject[] objects) 
    {
        foreach(GameObject obj in objects)
        {
            if(obj != null) 
            {
                return true;
            }
        }
        return false;
    }
}
