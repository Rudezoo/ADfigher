using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpSwitch : MonoBehaviour //�Ʊ� Emp�� ���������
{
    public GameObject boomArea;

    private void Start() //3�� ������ �˾Ƽ� ������
    {
        Invoke("EmpTimedown",3f);
    }
    private void OnTriggerEnter(Collider other) //boss�� enemy�� ��������� ������
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
        {
            EmpTimedown();
        }
    }

    private void EmpTimedown()  //emp�� ������ ����
    {
        SoundManager.instance.playEmpBoomSound();
        Handheld.Vibrate();
        Destroy(gameObject);
        GameObject boom = Instantiate(boomArea, transform.position, Quaternion.identity);  //������� �ش� ��ҿ� BoomArea�� ��ȯ
        Destroy(boom, 0.3f);
    }
}
