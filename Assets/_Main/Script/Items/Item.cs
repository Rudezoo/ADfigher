using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { coin, health, emp };
public class Item : MonoBehaviour //������ ������ �����ϱ����� class
{
    public ItemType itemtype;
    public GameObject touchImpact;


    public int value;

    private void Start()
    {
       
    }

    public void TouchItem() //������ ��ü�� ��������
    {

        if (itemtype == ItemType.coin) //�����ΰ��
        {
            
            PlayerInfo.instance.coin += value;
            InGameManager.instance.Particle(touchImpact,transform);
            Destroy(gameObject);
        }
        if (itemtype == ItemType.health) //healthpack�ΰ��
        {

            PlayerInfo.instance.healthpack += value;
            InGameManager.instance.Particle(touchImpact, transform);
            Destroy(gameObject);


        }
        if (itemtype == ItemType.emp) //emp�ΰ��
        {
            PlayerInfo.instance.emp += value;
            InGameManager.instance.Particle(touchImpact, transform);
            Destroy(gameObject);
        }
        SoundManager.instance.playItemSound();
        InGameManager.instance.SetInfoMenuTxt();
    }




}
