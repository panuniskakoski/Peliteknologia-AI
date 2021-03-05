using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_NiskakoskiScrapped : PlayerControllerInterface
{
    // Apumuuttujat
    private int nextTile;
    private int turns;              // Laskee käännöksiä
    private bool firstIteration;    // Apumuuttuja vihollisten läpikäynnissä
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

    // Käytä vain PlayerControllerInterfacessa olevia metodeja TIMissä olevan ohjeistuksen mukaan
    public override void DecideNextMove()
    {
        // Selvitetään oma status
        myHP = GetHP();
        myPos = GetPosition();

        direction = 0;

        // Päivitetään vihollisten sijainnit
        enemyPos = GetEnemyPositions();

        // Alustetaan lista-apumuuttuja
        firstIteration = true;

        // Haetaan lähin vihollinen kenellä myös vähiten HP:ta jäljellä
        foreach (Vector2 pos in enemyPos)
        {
            // Listan ensimmäinen alkio targetoidaan alustavasti
            if(firstIteration)
            {
                targetEnemy = pos;
                targetEnemyDistance = Vector2.Distance(myPos, pos);
                firstIteration = false;
            }
            // Tämän jälkeen verrataan seuraavia alkioita. Targetoidaan lähin vihollinen.
            else if (targetEnemyDistance > Vector2.Distance(myPos, pos))
            {
                targetEnemy = pos;
                targetEnemyDistance = Vector2.Distance(myPos, pos);
            }
        }

        // Selvitetään mihin suuntaan minun tulee liikkua saavuttaakseen kohteen
        // lopputulosta käytetään switch-casessa.
        // Vihollinen on...
        //
        // Alavasemmalla,       direction == -3
        // Ylävasemmalla,       direction == -1
        // Suoraan vasemmalla,  direction == -6
        //
        // Alaoikealla,         direction == 1
        // Yläoikealla,         direction == 3
        // Suoraan oikealla,    direction == -2
        // 
        // Suoraan alapuolella, direction == 4
        // Suoraan yläpuolella, direction == 6
        //
        // Jos kohde on x-akselilla vasemmalla
        if (targetEnemy.x < myPos.x) direction -= 2;
        // Jos kohde on x-akselilla oikealla
        else if (targetEnemy.x > myPos.x) direction += 2;
        // Jos kohde on samalla x-akselilla
        else if (targetEnemy.x == myPos.x) direction += 5;

        // Jos kohde on y-akselilla alapuolella
        if (targetEnemy.y < myPos.y) direction -= 1;
        // Jos kohde on y-akselilla yläpuolella
        else if (targetEnemy.y > myPos.y) direction += 1;
        // Jos kohde on samalla y-akselilla
        else if (targetEnemy.y == myPos.y) direction -= 4;

        Debug.Log("My position: " + myPos);
        Debug.Log("Target distance: " + targetEnemyDistance);
        Debug.Log("Target enemy position: " + targetEnemy);
        Debug.Log("Direction to move: " + direction);

        // Selvitetään edessä olevan ruudun tila
        // Ensin tallennetaan edellinen ruutu
        // 0 = tyhjä
        // 1 = seinä
        // 2 = pelaaja
        lastSpace = nextTile;
        nextTile = GetForwardTileStatus();

        // Lähdetään liikkeelle sen mukaan, mikä ruutu on edessäni
        switch (nextTile)
        {
            //-------Jos edessä on tyhjää-------//
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen ylävasemmalla (olkoot)
                    case -1:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = MoveForward;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on ylös
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
                        // Jos katse on ylös
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen yläoikealla (olkoot)
                    case 3:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = MoveForward;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on ylös
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen suoraan yläpuolella
                    case 6:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on ylös
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;
                }
                break;

            //-------Jos edessä on seinä-------//
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnLeft;
                        break;

                    // Vihollinen ylävasemmalla (olkoot)
                    case -1:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on ylös
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
                        // Jos katse on ylös
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen yläoikealla (olkoot)
                    case 3:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnLeft;
                        // Jos katse on ylös
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;

                    // Vihollinen suoraan yläpuolella
                    case 6:
                        // Jos katse on oikealle
                        if (myRotX == 1.0) nextMove = TurnLeft;
                        // Jos katse on vasemmalle
                        else if (myRotX == -1.0) nextMove = TurnRight;
                        // Jos katse on alas
                        else if (myRotY == -1.0) nextMove = TurnRight;
                        // Jos katse on ylös
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
                        // Jos katse on ylös
                        else if (myRotY == 1.0) nextMove = TurnRight;
                        break;
                }
                break;

            //-------Jos edessä on vihollinen-------//
            case 2:
                nextMove = Hit;
                break;
        }

        myRot = GetRotation();
        myRotX = Mathf.Round(myRot.x);
        myRotY = Mathf.Round(myRot.y);

        /* Original 2p AI, jos uudemmat versiot on huonompia
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
        */
    }
}
