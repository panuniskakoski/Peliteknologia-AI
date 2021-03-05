using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Niskakoski : PlayerControllerInterface
{
    private int nextTile;
    private int passedTurns;
    private int turns;
    private int myHP;

    // TÄMÄ TULEE TEHTÄVÄSSÄ TÄYDENTÄÄ
    // Käytä vain PlayerControllerInterfacessa olevia metodeja TIMissä olevan ohjeistuksen mukaan
    public override void DecideNextMove()
    {
        // Selvitetään oma hp tilanne
        myHP = GetHP();

        // Selvitetään edessä olevan ruudun tila
        // 0 = tyhjä
        // 1 = seinä
        // 2 = pelaaja
        nextTile = GetForwardTileStatus();

        switch (nextTile)
        {
            case 0: // Jos edessä on tyhjä ruutu
                nextMove = MoveForward;
                break;

            case 1: // Jos edessä on seinä
                // Robotti laskee käännöksiä, jotta ei jäädä jumiin
                if (turns < 3)
                {
                    nextMove = TurnLeft;
                    turns++;
                }
                // Kun on käännytty 3 kertaa samaan suuntaan, vaihdetaan kääntymissuuntaa
                else
                {
                    nextMove = TurnRight;
                    turns++;
                    if (turns == 5) turns = 0;
                }
                break;

            case 2: // Jos edessä on vihollinen
                nextMove = Hit;
                break;
        }

        // Debug.Log(nextMove);
    }
}
