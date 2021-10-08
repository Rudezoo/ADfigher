using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour //�߻�ü�� �����ϱ� ���� class
{

    public enum Type { player,enemy};
    public Type type;
    public GameObject boomArea;
    public GameObject hitEffect;


    public int damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (type == Type.enemy) //�� ��ü�� �Ѿ��϶�
        {
            if (other.gameObject.tag == "Base") //Base�� ������� ���ظ��ش�
            {
                Base core = other.gameObject.GetComponent<Base>();
                core.TakeDamage(damage);
                Destroy(gameObject);
                InGameManager.instance.Particle(hitEffect, transform);
                SoundManager.instance.playHitSound();
            }
        }
        else if(type == Type.player)//�÷��̾��� �Ѿ��϶�
        {
            if (other.gameObject.tag == "Enemy")  //Enemy�� ������� ���ظ��ش�
            {
                Enemy core = other.gameObject.GetComponent<Enemy>();
                core.TakeDamage(damage);
                Destroy(gameObject);
                InGameManager.instance.Particle(hitEffect, transform);
                SoundManager.instance.playHitSound();
            
            }
            if (other.gameObject.tag == "Boss")  //Boss�� ������� ���ظ��ش�
            {
                Boss core = other.gameObject.GetComponent<Boss>();
                core.TakeDamage(damage);
                Destroy(gameObject);
                InGameManager.instance.Particle(hitEffect, transform);
                SoundManager.instance.playHitSound();
              
            }
            else if(other.gameObject.tag == "Item")  //Item�� ��´ٸ� ������ ȹ��
            {
                Item item = other.gameObject.GetComponent<Item>();
                item.TouchItem();
                Destroy(gameObject);
          
            }
            else if (other.gameObject.tag == "Plane") //�ٴڸ鿡 �������� ����, �̴� ���� �������� ������ ���� �߰��� ����
            {
                Destroy(gameObject);
            }
        }
    }



}
