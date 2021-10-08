using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour //보스의 emp인 경우
{

    public GameObject boomArea;
    private void Start() //6초있다가 알아서 터진다
    {
        Invoke("BombTimedown", 6f);
    }
    private void OnTriggerEnter(Collider other) //만약 Base와 겹친다면
    {
        if (other.gameObject.tag == "Base")
        {
            BombTimedown();
        }
    }


    private void BombTimedown() //emp가 터지는 로직
    {
        SoundManager.instance.playBossEmpBoomSound();
        Handheld.Vibrate();
        Destroy(gameObject);
        GameObject boom = Instantiate(boomArea, transform.position, Quaternion.identity); //터진경우 해당 장소에 BoomArea를 소환
        Destroy(boom, 0.3f);
    }
}
