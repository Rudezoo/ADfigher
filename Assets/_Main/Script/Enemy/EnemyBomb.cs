using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour //������ emp�� ���
{

    public GameObject boomArea;
    private void Start() //6���ִٰ� �˾Ƽ� ������
    {
        Invoke("BombTimedown", 6f);
    }
    private void OnTriggerEnter(Collider other) //���� Base�� ��ģ�ٸ�
    {
        if (other.gameObject.tag == "Base")
        {
            BombTimedown();
        }
    }


    private void BombTimedown() //emp�� ������ ����
    {
        SoundManager.instance.playBossEmpBoomSound();
        Handheld.Vibrate();
        Destroy(gameObject);
        GameObject boom = Instantiate(boomArea, transform.position, Quaternion.identity); //������� �ش� ��ҿ� BoomArea�� ��ȯ
        Destroy(boom, 0.3f);
    }
}
