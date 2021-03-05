using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NiskakoskiScrapped : PlayerControllerInterface
{
    // Apumuuttujat
    private int nextTile;
    private int turns;              // Laskee k��nn�ksi�
    private bool firstIteration;    // Apumuuttuja vihollisten l�pik�ynniss�
    private int lastSpace;

    // Omat statusmuuttujat
    private int myHP;
    private Vector2 myPos;
    private Vector2 myRot;
    private float myRotX;
    private float myRotY;

    // Vihollisten statusmuuttujat
    private Vector2[] enemyPos;
    private Vector2 enemyRot;
    private float targetEnemyDistance;

    private Vector2 targetEnemy;

    private int direction;

    // K�yt� vain PlayerControllerInterfacessa olevia metodeja TIMiss� olevan ohjeistuksen mukaan
    public override void DecideNextMove()
    {
        // Selvitet��n oma status
        myHP = GetHP();
        myPos = GetPosition();

        direction = 0;

        // P�ivitet��n vihollisten sijainnit
        enemyPos = GetEnemyPositions();

        // Alustetaan lista-apumuuttuja
        firstIteration = true;

        // Haetaan l�hin vihollinen kenell� my�s v�hiten HP:ta j�ljell�
        foreach (Vector2 pos in enemyPos)
        {
            // Listan ensimm�inen alkio targetoidaan alustavasti
            if(firstIteration)
            {
                targetEnemy = pos;
                targetEnemyDistance = Vector2.Distance(myPos, pos);
                firstIteration = false;
            }
            // T�m�n j�lkeen verrataan seuraavia alkioita. Targetoidaan l�hin vihollinen.
            else if (targetEnemyDistance > Vector2.Distance(myPos, pos))
            {
                targetEnemy = pos;
                targetEnemyDistance = Vector2.Distance(myPos, pos);
            }
        }

        // Selvitet��n mihin suuntaan minun tulee liikkua saavuttaakseen kohteen
        // lopputulosta k�ytet��n switch-casessa.
        // Vihollinen on...
        //
        // Alavasemmalla,       direction == -3
        // Yl�vasemmalla,       direction == -1
        // Suoraan vasemmalla,  direction == -6
        //
        // Alaoikealla,         direction == 1
        // Yl�oikealla,         direction == 3
        // Suoraan oikealla,    direction == -2
        // 
        // Suoraan alapuolella, direction == 4
        // Suoraan yl�puolella, direction == 6
        //
        // Jos kohde on x-akselilla vasemmalla
        if (targetEnemy.x < myPos.x) direction -= 2;
        // Jos kohde on x-akselilla oikealla
        else if (targetEnemy.x > myPos.x) direction += 2;
        // Jos kohde on samalla x-akselilla
        else if (targetEnemy.x == myPos.x) direction += 5;

        // Jos kohde on y-akselilla alapuolella
        if (targetEnemy.y < myPos.y) direction -= 1;
        // Jos kohde on y-akselilla yl�puolella
        else if (targetEnemy.y > myPos.y) direction += 1;
        // Jos kohde on samalla y-akselilla
        else if (targetEnemy.y == myPos.y) direction -= 4;

        Debug.Log("My position: " + myPos);
        Debug.Log("Target distance: " + targetEnemyDistance);
        Debug.Log("Target enemy position: " + targetEnemy);
        Debug.Log("Direction to move: " + direction);

        // Selvitet��n edess� olevan ruudun tila
        // Ensin tallennetaan edellinen ruutu
        // 0 = tyhj�
        // 1 = sein�
        // 2 = pelaaja
        lastSpace = nextTile;
        nextTile = GetForwardTileStatus();

        // L�hdet��n liikkeelle sen mukaan, mik� ruutu on edess�ni
        switch (nextTile)
        {
            //-------Jos edess� on tyhj��-------//
            case 0:
                // Huomioidaan orientaatio viholliseen
                switch (direction)
                {
                    // Vihollinen alavasemmalla (olkoot)
                    case -3:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = MoveForward;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = MoveForward;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen yl�vasemmalla (olkoot)
                    case -1:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = MoveForward;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = MoveForward;
                        break;

                    // Vihollinen suoraan vasemmalla
                    case -6:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = MoveForward;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen alaoikealla (olkoot)
                    case 1:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = MoveForward;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = MoveForward;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen yl�oikealla (olkoot)
                    case 3:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = MoveForward;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = MoveForward;
                        break;

                    // Vihollinen suoraan oikealla
                    case -2:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = MoveForward;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen suoraan yl�puolella
                    case 6:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = MoveForward;
                        break;

                    // Vihollinen suoraan alapuolella
                    case 4:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnLeft;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = MoveForward;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;
                }
                break;

            //-------Jos edess� on sein�-------//
            case 1:
                // Huomioidaan orientaatio viholliseen
                switch (direction)
                {
                    // Vihollinen alavasemmalla (olkoot)
                    case -3:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnLeft;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen yl�vasemmalla (olkoot)
                    case -1:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen suoraan vasemmalla
                    case -6:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen alaoikealla (olkoot)
                    case 1:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnLeft;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen yl�oikealla (olkoot)
                    case 3:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen suoraan oikealla
                    case -2:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen suoraan yl�puolella
                    case 6:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen suoraan alapuolella
                    case 4:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnRight;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnLeft;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on yl�s
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;
                }
                break;

            //-------Jos edess� on vihollinen-------//
            case 2:
                nextMove = Hit;
                break;
        }

        myRot = GetRotation();
        myRotX = Mathf.Round(myRot.x);
        myRotY = Mathf.Round(myRot.y);

        /* Original 2p AI, jos uudemmat versiot on huonompia
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
        */
    }
}
