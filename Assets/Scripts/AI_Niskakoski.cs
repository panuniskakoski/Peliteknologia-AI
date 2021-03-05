using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Niskakoski : PlayerControllerInterface
{
    private int nextTile;
    private int passedTurns;
    private int turns;
    private int myHP;

    // T�M� TULEE TEHT�V�SS� T�YDENT��
    // K�yt� vain PlayerControllerInterfacessa olevia metodeja TIMiss� olevan ohjeistuksen mukaan
    public override void DecideNextMove()
    {
        // Selvitet��n oma hp tilanne
        myHP = GetHP();

        // Selvitet��n edess� olevan ruudun tila
        // 0 = tyhj�
        // 1 = sein�
        // 2 = pelaaja
        nextTile = GetForwardTileStatus();

        switch (nextTile)
        {
            case 0: // Jos edess� on tyhj� ruutu
                nextMove = MoveForward;
                break;

            case 1: // Jos edess� on sein�
                // Robotti laskee k��nn�ksi�, jotta ei j��d� jumiin
                if (turns < 3)
                {
                    nextMove = TurnLeft;
                    turns++;
                }
                // Kun on k��nnytty 3 kertaa samaan suuntaan, vaihdetaan k��ntymissuuntaa
                else
                {
                    nextMove = TurnRight;
                    turns++;
                    if (turns == 5) turns = 0;
                }
                break;

            case 2: // Jos edess� on vihollinen
                nextMove = Hit;
                break;
        }

        // Debug.Log(nextMove);
    }
}
