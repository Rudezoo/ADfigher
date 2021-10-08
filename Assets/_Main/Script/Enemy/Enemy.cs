using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour //normal,speed,bomb, small(for Boss type2) Ÿ���� ���� �����ϱ����� class
{

    public enum Type { normal, speed, bomb, small };
    public Type type;

    public GameObject projectile;
    public Transform projectLoc;
    public float projectileSpeed;
    public float ShootRate;
    private float lastShootTIme;

    public float bulletDestroyTIme = 1.0f;

    public int health = 4;
    public int damage = 1;

    public Item[] dropItem;
    public float ItemspawnDistance;

    private MeshRenderer[] meshes;
    public GameObject Target;

    public float moveSpeed;
    public float attackRange;
    public float AccelSpeed = 2;

    bool moving;
    bool generateRandpos;
    bool coliiderSpawn;
    private Vector3 randpos;

    public GameObject boomEffect;
    public AudioSource enemySound;

    bool playsoundonce;

    private Collider selfCol;

    private void Awake()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();
        selfCol = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        if (BaseGameManager.instance)
        {
            if (BaseGameManager.instance.gameMode == GameMode.defense) //defense ����϶��� ��ǥ����
            {
                Target = GameObject.FindGameObjectWithTag("Base");
            }
            else //Attacker ����϶��� ��ǥ����
            {
                Target = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        enemySound.Play();


    }
    private void Update()
    {
        if (Target == null)
            return;



        float dist = Vector3.Distance(transform.position, Target.transform.position);  //Target�� enemy�� �Ÿ� ����

        if (dist > attackRange) //���� ���ݹ����ȿ� �������ʾҴٸ� Target�� ���� �̵�
        {
            if (!moving)
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {//���ݹ����ȿ� ���Դٸ�
            switch (type) //Ÿ�Կ� ���� �ٸ� ���� ����
            {
                case Type.normal: //normal Ÿ���� �ش� ��ġ�� �����ؼ� ����
                    if (Time.time - lastShootTIme >= ShootRate) //Shootrate�� ���� �����Ѵ�.
                        Shoot();
                    break;
                case Type.speed:
                    SpeedTypeLogic();
                    break;
                case Type.bomb:
                  
                    BombTypeLogic();
                    break;
                case Type.small:
                    SpeedTypeLogic();
                    break;

            }
        }

        if (generateRandpos && !moving) // ���� ������ Random�� ��ġ�� �����Ѵ�
        {
            Vector3 spawnCircle = Random.onUnitSphere; //Ư�� ������ �� ����
            spawnCircle.y = Mathf.Abs(spawnCircle.y); //y�� ���̳ʽ��� ���� ����
            randpos = Base.instance.transform.position + (spawnCircle * attackRange * 1.3f); //Random�� pos����
            coliiderSpawn = true; //�ش���ġ�� Enemy�� �����ߴ��� Ȯ���ϱ� ���� Trigger on
            generateRandpos = false;
            moving = true;
        }

        if (coliiderSpawn) //�ش���ġ�� Enemy�� �����ߴ��� Ȯ���Ѵ�.
        {
            Collider[] randcol = Physics.OverlapSphere(randpos, 0.2f); //������ randpos��ġ�� �������� coliider����
            foreach (Collider col in randcol)
            {
                if (col == selfCol) //���� Enemy�� collider�� ���� ������ collider�� �浿������
                {
                    Debug.Log("stop");

                    //randpos�� Enemy�� �����ߴٴ� ������ �Ǵ��ϰ� �ٸ� randpos�� �����ϱ� ���� �Ʒ� Trigger���� ���ش�

                    moving = false; //�������� �����
                    generateRandpos = false; //randpos�� ���������ʴ´�
                    coliiderSpawn = false; //collider�� ���������ʴ´�
                }
            }
        }

        if (moving) //���� randpos�� ���������Ѵٸ�
        {
            transform.position = Vector3.MoveTowards(transform.position, randpos, moveSpeed * Time.deltaTime);
        }


        transform.LookAt(Target.transform); //Target�� �ٶ󺻴�
    }


    void SpeedTypeLogic() //�� type�� speed�϶��� Logic�� ���
    {
        if (Time.time - lastShootTIme >= ShootRate) //Shootrate�� ���� �����Ѵ�.
        {
            Shoot();

            //����� Base ��ó ������ ��ġ�� �̵�(Defender����϶�)
            if (BaseGameManager.instance.gameMode == GameMode.defense)
            {
                generateRandpos = true; //���� ������ Random�� ��ġ ������ ���� TriggerOn

            }

        }

    }

    void BombTypeLogic() //�� type�� bomb�϶��� Logic�� ���
    {
        if (!playsoundonce) //�ѹ��� ���� �ӵ� ���
        {
            SoundManager.instance.playSpeedupSound();
            playsoundonce = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime * AccelSpeed); //��Ӽӵ��� �޾� ������ Target�� ���� ����
    }


    private void OnTriggerEnter(Collider other) //�����ÿ� Trigger�߻���
    {
        if (other.gameObject.tag == "Base") //Base�� ��ģ�ٸ�
        {
            if (type == Type.bomb) //bombŸ���϶� Base�� ���ظ� �ش�.
            {
                Base.instance.TakeDamage(damage);
                SoundManager.instance.playBoomSound();
                InGameManager.instance.Particle(boomEffect, transform);
                Destroy(gameObject);
                Handheld.Vibrate();
            }

        }
    }


    public void Shoot() //Normal�� Speed Ÿ�Կ��� ���Ǵ� ����ü �߻�
    {

        SoundManager.instance.playEnemyLaserSound();
        lastShootTIme = Time.time;
        GameObject proj = Instantiate(projectile, projectLoc.position, Quaternion.identity);
        projectLoc.LookAt(Target.transform);
        proj.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;

        Destroy(proj, bulletDestroyTIme);
    }



    public void TakeDamage(int damage) //���ظ� �������
    {
        health -= damage;
        StartCoroutine(OnDamage());

        if (health <= 0) //�׾�����
        {
            if (type != Type.small) //������ȯü�� count�����ʴ´�.
            {
                InGameManager.instance.killcount++;
                InGameManager.instance.waveEnemyCntTxt.text = "Enemy: " + (EnemySpawner.instance.total_enemyCount - InGameManager.instance.killcount);
                DropItem();
            }
            SoundManager.instance.playEnemyDieSound();
            InGameManager.instance.Particle(boomEffect, transform);
            Destroy(gameObject);
            Handheld.Vibrate();

        }

    }

    void DropItem() //������ ���
    {
        Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance�� �Ÿ��� ������ ��ġ�� �����ϰ�

        switch (type) //�� type�� ���� copper,silver,gold ���� ���
        {
            case Type.normal:
                Instantiate(dropItem[0], spawnPos, Quaternion.identity);
                break;
            case Type.speed:
                Instantiate(dropItem[1], spawnPos, Quaternion.identity);
                break;
            case Type.bomb:
                Instantiate(dropItem[2], spawnPos, Quaternion.identity);
                break;
        }
        spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance�� �Ÿ��� ������ ��ġ�� �����ϰ�
        int itemidx = Random.Range(3, 8); //emp�� healthpack�� 1/5Ȯ���� ���
        if (itemidx <= 4)
        {
            Instantiate(dropItem[itemidx], spawnPos, Quaternion.identity);
        }

    }

    IEnumerator OnDamage() //�����Ծ����� mesh�� red�� ��ȯ
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }
    }






}
