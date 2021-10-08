using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomArea : MonoBehaviour //emp가 터졌을때 범위 피해를 주기위한 충돌체
{
    public int damage;
    private void OnTriggerEnter(Collider other) 
    {

        if (other.gameObject.tag == "Enemy") //적과 겹친다면 데미지를 준다
        {
            Enemy core = other.gameObject.GetComponent<Enemy>();
            core.TakeDamage(damage);
        } 
        if (other.gameObject.tag == "Base") //Base와 겹친다면 데미지를 준다.
        {
            Base core = other.gameObject.GetComponent<Base>();
            core.TakeDamage(damage);
        }
    }

}
