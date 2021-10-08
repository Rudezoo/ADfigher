using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour 
{
    // Start is called before the first frame update
    public void useHealthPack()
    {
        SoundManager.instance.playmenuSound();
        if (BaseGameManager.instance.gameMode == GameMode.attack) //attack ����ϋ�
        {
            if (PlayerInfo.instance.healthpack > 0) //Player�� ü��ȸ��
            {
                PlayerInfo.instance.health += 10;
                PlayerInfo.instance.healthpack--;
                InGameManager.instance.SetInfoMenuTxt();
            }

        }
        else //defender ����϶�
        {
            if (PlayerInfo.instance.healthpack > 0 && Base.instance.health < Base.instance.Maxhealth) //Base�� ü��ȸ��
            {
                if (Base.instance.health + 10 >= Base.instance.Maxhealth) //���� ȸ���ÿ� maxheatlh���� ū ��� ���� maxhealth�� ��ü
                {
                    Base.instance.health = Base.instance.Maxhealth;
                }
                else
                {
                    Base.instance.health += 10; //�ƴѰ�� 10�� ü��ȭ��
                }
                SoundManager.instance.playHealSound();

                Base.instance.SetBaseTxt();
                Base.instance.HealEffect();
                PlayerInfo.instance.healthpack--; //healthpack�ϳ� ����

                InGameManager.instance.SetInfoMenuTxt();
            }

        }
    }
}
