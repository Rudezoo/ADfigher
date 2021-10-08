using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomArea : MonoBehaviour //emp�� �������� ���� ���ظ� �ֱ����� �浹ü
{
    public int damage;
    private void OnTriggerEnter(Collider other) 
    {

        if (other.gameObject.tag == "Enemy") //���� ��ģ�ٸ� �������� �ش�
        {
            Enemy core = other.gameObject.GetComponent<Enemy>();
            core.TakeDamage(damage);
        } 
        if (other.gameObject.tag == "Base") //Base�� ��ģ�ٸ� �������� �ش�.
        {
            Base core = other.gameObject.GetComponent<Base>();
            core.TakeDamage(damage);
        }
    }

}
