using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour //�÷��̾� ������ �����Ѵ�.
{

    public int health = 10;
    public int coin = 0;
    public int emp = 0;
    public int healthpack = 0;

    public static PlayerInfo instance;
    private void Awake()
    {
        instance = this;
    }



}
