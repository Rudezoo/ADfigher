using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { coin, health, emp };
public class Item : MonoBehaviour //아이템 정보를 관리하기위한 class
{
    public ItemType itemtype;
    public GameObject touchImpact;


    public int value;

    private void Start()
    {
       
    }

    public void TouchItem() //아이템 개체와 닿았을경우
    {

        if (itemtype == ItemType.coin) //코인인경우
        {
            
            PlayerInfo.instance.coin += value;
            InGameManager.instance.Particle(touchImpact,transform);
            Destroy(gameObject);
        }
        if (itemtype == ItemType.health) //healthpack인경우
        {

            PlayerInfo.instance.healthpack += value;
            InGameManager.instance.Particle(touchImpact, transform);
            Destroy(gameObject);


        }
        if (itemtype == ItemType.emp) //emp인경우
        {
            PlayerInfo.instance.emp += value;
            InGameManager.instance.Particle(touchImpact, transform);
            Destroy(gameObject);
        }
        SoundManager.instance.playItemSound();
        InGameManager.instance.SetInfoMenuTxt();
    }




}
