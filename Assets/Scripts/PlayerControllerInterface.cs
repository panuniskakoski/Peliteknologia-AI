using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerInterface : MonoBehaviour
{
    private PlayerController playerController;

    // nextMoveen tallennetaan metodi joka halutaan suorittaa seuraavalla vuorolla
    public delegate void NextMove();
    public NextMove nextMove;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // T�m� metodi ylikirjoitetaan omassa skriptiss�
    public virtual void DecideNextMove()
    {

    }

    // --- N�IT� METODEJA SAA K�YTT�� OMASSA SKRIPTISS� ---
    // Liikkuu yhden ruudun eteenp�in
    public void MoveForward()
    {
        playerController.MoveForward();
    }

    // K��ntyy 90 astetta vastap�iv��n
    public void TurnLeft()
    {
        playerController.TurnLeft();
    }

    // K��ntyy 90 astetta my�t�p�iv��n
    public void TurnRight()
    {
        playerController.TurnRight();
    }

    // Ly� edess� olevaa kohdetta
    public void Hit()
    {
        playerController.Hit();
    }

    // Skippaa vuoron eli ei tee mit��n
    public void Pass()
    {
        playerController.Pass();
    }

    // Antaa tiedon onko pelaajan edess� olevassa ruudussa tyhj�, sein� vai pelaaja
    // 0 = tyhj�, 1 = sein�, 2 = pelaaja
    public int GetForwardTileStatus()
    {
        return playerController.GetForwardTileStatus();
    }

    // Palauttaa oman sijainnin pelimaailmassa 2D-vektorina
    public Vector2 GetPosition()
    {
        return playerController.GetPosition();
    }

    // Palauttaa oman rotaation vektorina
    // (1,0) = oikealle | (-1,0) = vasemmalle | (0,1) = yl�s | (0,-1) = alas
    public Vector2 GetRotation()
    {
        return playerController.GetRotation();
    }

    // Palauttaa oman HP-m��r�n
    public int GetHP()
    {
        return playerController.GetHP();
    }

    // Palauttaa listan, jossa on kaikkien vihollisten sijainti pelimaailmassa
    public Vector2[] GetEnemyPositions()
    {
        return playerController.GetEnemyPositions();
    }

    // Palauttaa annetussa sijainnissa olevan vihollisen rotaation vektorina
    // (1,0) = oikealle | (-1,0) = vasemmalle | (0,1) = yl�s | (0,-1) = alas
    // Palauttaa (0, 0) jos sijainnissa ei ole vihollista
    public Vector2 GetEnemyRotation(Vector2 enemyPos)
    {
        return playerController.GetEnemyRotation(enemyPos);
    }

    // Palauttaa annetussa sijainnissa olevan vihollisen HP-m��r�n
    // Palauttaa -1 jos sijainnissa ei ole vihollista
    public int GetEnemyHP(Vector2 pos)
    {
        return playerController.GetEnemyHP(pos);
    }
}
