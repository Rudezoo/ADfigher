using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour //Boss Ÿ���� ���� �����ϱ� ���� class
{
    public enum Type { Boss1, Boos2 };
    public Type type;

    public GameObject projectile;
    public GameObject Bombprojectile;
    public GameObject smallEnemy;

    public Transform bombLoc;
    public Transform[] projectLocs;
    public Transform[] spawnLocs;

    public float projectileSpeed; //�߻�ü �ӵ�
    public float BombSpeed; //emp �ӵ�

    public float ShootRate; //�߻� rate
    public float BombShootRate; //emp �߻� rate

    private float lastShootTIme;
    private float BomblastShootTIme;

    public float bulletDestroyTIme = 1.0f;

    public int health = 50;
    public int damage = 3;

    public Item[] dropItem; //drop�ϴ� item ������Ʈ��
    public float ItemspawnDistance; //drop�� �Ÿ�

    private MeshRenderer[] meshes;
    public GameObject Target;

    public float moveSpeed;
    public float attackRange;

    bool moving; //������ ��ġ�� �����̱� �����Ҷ� trigger
    bool generateRandpos; //������ ��ġ ������ ���� trigger
    bool coliiderSpawn; //������ ��ġ�� �浹ü ���� trigger
    bool shootBomb; //emp�� �߻����� �����  trigger

    private Vector3 randpos; //��������� ���� ��ġ
    private Collider selfCol; //�������� �浹ü

    public GameObject boomEffect;
    public AudioSource bossSound;

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
        bossSound.Play(); 
    }
    private void Update()
    {
        if (Target == null)
            return;

        transform.LookAt(Target.transform); //Target�� �ٶ󺻴�


        float dist = Vector3.Distance(transform.position, Target.transform.position); //Target�� boss�� �Ÿ� ����

        if (dist > attackRange) //���� ���ݹ����ȿ� �������ʾҴٸ� Target�� ���� �̵�
        {
            if (!moving)
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        { //���ݹ����ȿ� ���Դٸ�
            switch (type) //Ÿ�Կ� ���� �ٸ� ���� ����
            {
                case Type.Boss1:
                    Boss1Logic();
                    break;
                case Type.Boos2:
                    Boss2Logic();
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
            Collider[] randcol = Physics.OverlapSphere(randpos, 1f); //������ randpos��ġ�� �������� coliider����
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



    }

    void Boss1Logic() //Ÿ��1 Boss�� ���� ����
    {
       
        if (!shootBomb) //emp�� �߻��ϴ� ��찡 �ƴ϶��
        {
            int attackmode = Random.Range(1, 3); //���� ���� 2������ �ϳ��� �����ϰ� ����
            if (attackmode == 1)
            {
                if (Time.time - lastShootTIme >= ShootRate) //Shootrate�� ���� �����Ѵ�.
                    Shoot();
            }
            else
            {
                if (Time.time - BomblastShootTIme >= BombShootRate)
                    StartCoroutine(ShootBomb()); // emp�� ���

            }
        }

    }



    IEnumerator ShootBomb() //emp�� �߻��Ѵ�.
    {
        shootBomb = true;
        generateRandpos = false;
        moving = false;
        BomblastShootTIme = Time.time;

        SoundManager.instance.playBossEmpSound();
        GameObject bomb = Instantiate(Bombprojectile, bombLoc.position, Quaternion.identity); //������Ʈ ������ emp�� Ŀ���� ��ٸ���,

        yield return new WaitForSeconds(1.5f); //1.5�� ����

        SoundManager.instance.playEmpSound();
        
        bombLoc.LookAt(Target.transform); //emp�� ���ư��� ��ġ����
        bomb.GetComponent<Rigidbody>().velocity = bombLoc.forward * BombSpeed; //�߻�

        generateRandpos = true;
        shootBomb = false;

    }

    void Boss2Logic() //Ÿ�� 2 ������ ���� ����
    {
  ;
        if (Time.time - lastShootTIme >= ShootRate) //���� �ð��� �Ǹ� ,ShootRate��ŭ�� ������
        {
            lastShootTIme = Time.time;
            foreach (Transform spawnLoc in spawnLocs) //�ΰ��� ��ȯ ��ġ����
            {
                for (int i = 0; i < 3; i++) // �� ��ġ�� 3���� ���� �� ��ȯ
                {
                    Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(spawnLoc.position, 1f, false); //1f�� �Ÿ��� ������ ��ġ�� �����ϰ�
                    Instantiate(smallEnemy, spawnPos, Quaternion.identity); //�ش� ��ġ�� ���� �� ��ȯ
                }

            }
        }
        generateRandpos = true;

    }


    public void Shoot() //����ü �߻�
    {
        lastShootTIme = Time.time;
        if (type == Type.Boss1) //Type 1�� �������
        {
            foreach (Transform projectLoc in projectLocs) //4������ ����ü �߻�
            {
                GameObject proj = Instantiate(projectile, projectLoc.position, Quaternion.identity);
                projectLoc.LookAt(Target.transform);
                proj.GetComponent<Rigidbody>().velocity = projectLoc.forward * projectileSpeed;

                Destroy(proj, bulletDestroyTIme);
            }
        }

    }



    public void TakeDamage(int damage) //���ظ� �Ծ��ٸ�
    {
        health -= damage;
        StartCoroutine(OnDamage());

        if (health <= 0) //���� �����
        {
            InGameManager.instance.killcount++;
            InGameManager.instance.waveEnemyCntTxt.text = "Enemy: " + (EnemySpawner.instance.total_enemyCount - InGameManager.instance.killcount);
            DropItem();
            InGameManager.instance.Particle(boomEffect, transform);
            Destroy(gameObject);
            Handheld.Vibrate();

        }

    }

    void DropItem()
    {
        Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance�� �Ÿ��� ������ ��ġ�� �����ϰ�
        for (int i = 0; i < 3; i++) //ù��° ������ ��, ��� ������ 3�� ���
        {
            Instantiate(dropItem[0], spawnPos, Quaternion.identity);
        }


        spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance�� �Ÿ��� ������ ��ġ�� �����ϰ�

        int itemidx = Random.Range(1, 9); //emp�� healthpack�� 1/8Ȯ���� ���
        if (itemidx <= 2)
        {
            Instantiate(dropItem[itemidx], spawnPos, Quaternion.identity);
        }

    }

    IEnumerator OnDamage() //mesh �� red�� ��ȭ
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
