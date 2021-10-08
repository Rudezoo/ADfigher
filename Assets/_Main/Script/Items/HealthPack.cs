using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour 
{
    // Start is called before the first frame update
    public void useHealthPack()
    {
        SoundManager.instance.playmenuSound();
        if (BaseGameManager.instance.gameMode == GameMode.attack) //attack 모드일떄
        {
            if (PlayerInfo.instance.healthpack > 0) //Player의 체력회복
            {
                PlayerInfo.instance.health += 10;
                PlayerInfo.instance.healthpack--;
                InGameManager.instance.SetInfoMenuTxt();
            }

        }
        else //defender 모드일때
        {
            if (PlayerInfo.instance.healthpack > 0 && Base.instance.health < Base.instance.Maxhealth) //Base의 체력회복
            {
                if (Base.instance.health + 10 >= Base.instance.Maxhealth) //만약 회복시에 maxheatlh보다 큰 경우 값을 maxhealth로 대체
                {
                    Base.instance.health = Base.instance.Maxhealth;
                }
                else
                {
                    Base.instance.health += 10; //아닌경우 10의 체력화복
                }
                SoundManager.instance.playHealSound();

                Base.instance.SetBaseTxt();
                Base.instance.HealEffect();
                PlayerInfo.instance.healthpack--; //healthpack하나 제거

                InGameManager.instance.SetInfoMenuTxt();
            }

        }
    }
}
