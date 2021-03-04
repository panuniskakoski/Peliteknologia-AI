using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public TurnManager turnManager;
    public PlayerControllerInterface playerControllerInterface;

    public int hp = 3;
    public int killCount = 0;
    public int deathCount = 0;

    void Start()
    {
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        playerControllerInterface = GetComponent<PlayerControllerInterface>();

        // P��tet��n seuraava siirto aina kun TurnManager ilmoittaa vuoron p��ttyneen
        TurnManager.turnEndDelegate += playerControllerInterface.DecideNextMove;
        // P��tet��n ensimm�inen siirto
        playerControllerInterface.DecideNextMove();
    }

    // --- TEHT�V�SS� K�YTETT�VIEN FUNKTIOIDEN TOTEUTUS ---
    // Liikkuu yhden ruudun eteenp�in
    public void MoveForward()
    {
        if (GetForwardTileStatus() != 0) 
        {
            Debug.Log("EI VOI LIIKKUA KOSKA RUUTU T�YNN�");
        }
        else
        {
            Vector2 oldPos = transform.position;
            transform.position += transform.right;
            UpdatePlayerPosition(oldPos);
        }
    }

    // K��ntyy 90 astetta vastap�iv��n
    public void TurnLeft()
    {
        transform.Rotate(0, 0, 90, Space.World);
    }

    // K��ntyy 90 astetta my�t�p�iv��n
    public void TurnRight()
    {
        transform.Rotate(0, 0, -90, Space.World);
    }

    // Ly� edess� olevaa kohdetta
    public void Hit()
    {
        if (GetForwardTileStatus() == 2)
        {
            // K�yd��n l�pi kaikki pelaajat ja katsotaan kuka on edess�
            foreach (GameObject enemy in turnManager.players)
            {
                if (enemy.transform.position == transform.position + transform.right)
                {
                    PlayerController enemyController = enemy.GetComponent<PlayerController>();

                    // Jos ly� naamaa p�in niin 1 damage
                    if (enemyController.GetRotation() + GetRotation() == new Vector2(0,0))
                    {
                        enemyController.TakeDamage(gameObject, 1);
                    }
                    // Jos ly� kylkeen tai selk��n niin 3 damage
                    else
                    {
                        enemyController.TakeDamage(gameObject, 3);
                    }
                }
            }
        }
    }

    // �l� tee mit��n
    public void Pass()
    {
        Debug.Log("Skipattiin vuoro");
    }

    // Antaa maailmasijainnista tiedon onko pelaajan edess� olevassa ruudussa tyhj�, sein� vai pelaaja
    public int GetForwardTileStatus()
    {
        return GetPositionStatus(transform.position + transform.right);
    }

    // Palauttaa oman sijainnin gridiss�
    public Vector2 GetPosition()
    {
        return transform.position;
    }

    // Palauttaa oman rotaation
    // 0 astetta on oikealle p�in
    public Vector2 GetRotation()
    {
        return transform.right;
    }

    // Palauttaa oman HP-m��r�n
    public int GetHP()
    {
        return hp;
    }

    // Palauttaa listan, jossa on kaikkien vihollisten sijainti pelimaailmassa
    public Vector2[] GetEnemyPositions()
    {
        List<Vector2> enemyPositions = new List<Vector2>();
        foreach (GameObject enemy in turnManager.players)
        {
            // Ei lis�� pelaajaa itse��n listaan
            if ((Vector2)enemy.transform.position != GetPosition())
            {
                enemyPositions.Add(enemy.transform.position);
            }
        }
        return enemyPositions.ToArray();
    }

    // Palauttaa annetussa sijainnissa olevan vihollisen rotaation vektorina
    // (1,0) = oikealle | (-1,0) = vasemmalle | (0,1) = yl�s | (0,-1) = alas
    public Vector2 GetEnemyRotation(Vector2 pos)
    {
        Vector2 enemyRotation = new Vector2(0, 0);
        foreach (GameObject enemy in turnManager.players)
        {
            if (enemy.transform.position == new Vector3(pos.x, pos.y, 0))
            {
                enemyRotation = enemy.GetComponent<PlayerController>().GetRotation();
            }
        }
        return enemyRotation;
    }

    // Palauttaa annetussa sijainnissa olevan pelaajan HP-m��r�n
    public int GetEnemyHP(Vector2 pos)
    {
        int enemyHP = -1;
        foreach (GameObject enemy in turnManager.players)
        {
            if (enemy.transform.position == new Vector3(pos.x, pos.y, 0))
            {
                enemyHP = enemy.GetComponent<PlayerController>().GetHP();
            }
        }
        return enemyHP;
    }

    // ------ MUUT FUNKTIOT ------ //
    // N�� varmaan pit�s olla lopullisessa sit jossain muualla

    // V�hent�� pelaajan HPta kun sit� ly�d��n. 
    // hitter: vihollinen joka l�i pelaajaa
    public void TakeDamage(GameObject hitter, int amt)
    {
        hp -= amt;
        if (hp < 1)
        {
            Die(hitter);
        }
    }

    //Tappaa pelaajan
    private void Die(GameObject killer)
    {
        Vector2 oldPos = transform.position;
        deathCount++;
        gridManager.SpawnPlayer(gameObject.transform);
        UpdatePlayerPosition(oldPos);
        hp = 3;
        killer.GetComponent<PlayerController>().killCount++;
    }

    //Antaa maailmasijainnin gridisijaintina
    private int[] WorldPosToGridPos(Vector2 worldPos)
    {
        int[] gridPos = { 0, 0 };
        gridPos[0] = Convert.ToInt32(gridManager.size - 0.5 + worldPos.x);
        gridPos[1] = Convert.ToInt32(gridManager.size - 0.5 + worldPos.y);
        return gridPos;
    }

    // Antaa maailmasijainnista tiedon onko ruudussa tyhj�, sein� vai pelaaja
    private int GetPositionStatus(Vector2 worldPos)
    {
        int[] gridPos = WorldPosToGridPos(worldPos);
        int positionStatus = gridManager.grid[gridPos[0], gridPos[1]];
        return positionStatus;
    }

    //P�ivitt�� pelaajan sijainnin gridiin, parametri on pelaajan sijainti aiemmin
    private void UpdatePlayerPosition(Vector2 oldPosition)
    {
        // Tyjennet��n vanhan sijainnin ruutu
        int[] oldGridPos = WorldPosToGridPos(oldPosition);
        gridManager.grid[oldGridPos[0], oldGridPos[1]] = 0;

        // T�ytet��n uuden sijainnin ruutu
        int[] newGridPos = WorldPosToGridPos(transform.position);
        gridManager.grid[newGridPos[0], newGridPos[1]] = 2;
    }
}
