using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Sukunimi : PlayerControllerInterface
{
    // T�M� TULEE TEHT�V�SS� T�YDENT��
    // K�yt� vain PlayerControllerInterfacessa olevia metodeja TIMiss� olevan ohjeistuksen mukaan
    public override void DecideNextMove()
    {
        // Tyhm� teko�ly, liikkuu eteenp�in jos edess� on tyhj� ruutu
        if (GetForwardTileStatus() == 0)
        {
            nextMove = MoveForward;
        }
        // Muuten ei tee mit��n
        else
        {
            nextMove = Pass;
        }
    }
}
