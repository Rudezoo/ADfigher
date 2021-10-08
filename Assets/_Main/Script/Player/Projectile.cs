using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour //발사체를 관리하기 위한 class
{

    public enum Type { player,enemy};
    public Type type;
    public GameObject boomArea;
    public GameObject hitEffect;


    public int damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (type == Type.enemy) //적 기체의 총알일때
        {
            if (other.gameObject.tag == "Base") //Base에 닿았을때 피해를준다
            {
                Base core = other.gameObject.GetComponent<Base>();
                core.TakeDamage(damage);
                Destroy(gameObject);
                InGameManager.instance.Particle(hitEffect, transform);
                SoundManager.instance.playHitSound();
            }
        }
        else if(type == Type.player)//플레이어의 총알일때
        {
            if (other.gameObject.tag == "Enemy")  //Enemy에 닿았을때 피해를준다
            {
                Enemy core = other.gameObject.GetComponent<Enemy>();
                core.TakeDamage(damage);
                Destroy(gameObject);
                InGameManager.instance.Particle(hitEffect, transform);
                SoundManager.instance.playHitSound();
            
            }
            if (other.gameObject.tag == "Boss")  //Boss에 닿았을때 피해를준다
            {
                Boss core = other.gameObject.GetComponent<Boss>();
                core.TakeDamage(damage);
                Destroy(gameObject);
                InGameManager.instance.Particle(hitEffect, transform);
                SoundManager.instance.playHitSound();
              
            }
            else if(other.gameObject.tag == "Item")  //Item에 닿는다면 아이템 획득
            {
                Item item = other.gameObject.GetComponent<Item>();
                item.TouchItem();
                Destroy(gameObject);
          
            }
            else if (other.gameObject.tag == "Plane") //바닥면에 닿았을경우 제거, 이는 현재 사용되지는 않지만 추후 추가할 예정
            {
                Destroy(gameObject);
            }
        }
    }



}
