using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour //Boss 타입의 적을 관리하기 위한 class
{
    public enum Type { Boss1, Boos2 };
    public Type type;

    public GameObject projectile;
    public GameObject Bombprojectile;
    public GameObject smallEnemy;

    public Transform bombLoc;
    public Transform[] projectLocs;
    public Transform[] spawnLocs;

    public float projectileSpeed; //발사체 속도
    public float BombSpeed; //emp 속도

    public float ShootRate; //발사 rate
    public float BombShootRate; //emp 발사 rate

    private float lastShootTIme;
    private float BomblastShootTIme;

    public float bulletDestroyTIme = 1.0f;

    public int health = 50;
    public int damage = 3;

    public Item[] dropItem; //drop하는 item 오브젝트들
    public float ItemspawnDistance; //drop시 거리

    private MeshRenderer[] meshes;
    public GameObject Target;

    public float moveSpeed;
    public float attackRange;

    bool moving; //랜덤한 위치로 움직이기 시작할때 trigger
    bool generateRandpos; //랜덤한 위치 생성을 위한 trigger
    bool coliiderSpawn; //랜덤한 위치에 충돌체 생성 trigger
    bool shootBomb; //emp를 발사했을 경우의  trigger

    private Vector3 randpos; //만들어지는 랜덤 위치
    private Collider selfCol; //스스로의 충돌체

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
            if (BaseGameManager.instance.gameMode == GameMode.defense) //defense 모드일때의 목표설정
            {
                Target = GameObject.FindGameObjectWithTag("Base");
            }
            else //Attacker 모드일때의 목표설정
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

        transform.LookAt(Target.transform); //Target을 바라본다


        float dist = Vector3.Distance(transform.position, Target.transform.position); //Target과 boss의 거리 측정

        if (dist > attackRange) //만약 공격범위안에 들어오지않았다면 Target을 향해 이동
        {
            if (!moving)
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        { //공격범위안에 들어왔다면
            switch (type) //타입에 따라 다른 패턴 구사
            {
                case Type.Boss1:
                    Boss1Logic();
                    break;
                case Type.Boos2:
                    Boss2Logic();
                    break;

            }
        }

        if (generateRandpos && !moving) // 일정 범위안 Random한 위치를 생성한다
        {
            Vector3 spawnCircle = Random.onUnitSphere; //특정 범위내 구 생성
            spawnCircle.y = Mathf.Abs(spawnCircle.y); //y가 마이너스인 구간 제외
            randpos = Base.instance.transform.position + (spawnCircle * attackRange * 1.3f); //Random한 pos생성
            coliiderSpawn = true; //해당위치에 Enemy가 도달했는지 확인하기 위해 Trigger on
            generateRandpos = false;
            moving = true;
        }

        if (coliiderSpawn) //해당위치에 Enemy가 도달했는지 확인한다.
        {
            Collider[] randcol = Physics.OverlapSphere(randpos, 1f); //생성된 randpos위치에 구형태의 coliider생성
            foreach (Collider col in randcol)
            {
                if (col == selfCol) //만약 Enemy의 collider가 위에 생성된 collider와 충동했을때
                {
                    Debug.Log("stop");

                    //randpos에 Enemy가 도달했다는 것으로 판단하고 다른 randpos를 생성하기 위해 아래 Trigger들을 꺼준다

                    moving = false; //움직임을 멈춘다
                    generateRandpos = false; //randpos를 생성하지않는다
                    coliiderSpawn = false; //collider를 생성하지않는다
                }
            }
        }

        if (moving) //만약 randpos로 움직여아한다면
        {
            transform.position = Vector3.MoveTowards(transform.position, randpos, moveSpeed * Time.deltaTime);
        }



    }

    void Boss1Logic() //타입1 Boss를 위한 로직
    {
       
        if (!shootBomb) //emp을 발사하는 경우가 아니라면
        {
            int attackmode = Random.Range(1, 3); //공격 패턴 2가지중 하나를 랜덤하게 선택
            if (attackmode == 1)
            {
                if (Time.time - lastShootTIme >= ShootRate) //Shootrate에 맞춰 공격한다.
                    Shoot();
            }
            else
            {
                if (Time.time - BomblastShootTIme >= BombShootRate)
                    StartCoroutine(ShootBomb()); // emp를 쏜다

            }
        }

    }



    IEnumerator ShootBomb() //emp를 발사한다.
    {
        shootBomb = true;
        generateRandpos = false;
        moving = false;
        BomblastShootTIme = Time.time;

        SoundManager.instance.playBossEmpSound();
        GameObject bomb = Instantiate(Bombprojectile, bombLoc.position, Quaternion.identity); //오브젝트 생성후 emp가 커지길 기다리고,

        yield return new WaitForSeconds(1.5f); //1.5초 이후

        SoundManager.instance.playEmpSound();
        
        bombLoc.LookAt(Target.transform); //emp가 날아가는 위치지정
        bomb.GetComponent<Rigidbody>().velocity = bombLoc.forward * BombSpeed; //발사

        generateRandpos = true;
        shootBomb = false;

    }

    void Boss2Logic() //타입 2 보스를 위한 로직
    {
  ;
        if (Time.time - lastShootTIme >= ShootRate) //일정 시간이 되면 ,ShootRate만큼이 지나면
        {
            lastShootTIme = Time.time;
            foreach (Transform spawnLoc in spawnLocs) //두개의 소환 위치에서
            {
                for (int i = 0; i < 3; i++) // 한 위치당 3개의 작은 적 소환
                {
                    Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(spawnLoc.position, 1f, false); //1f의 거리의 랜덤한 위치를 생성하고
                    Instantiate(smallEnemy, spawnPos, Quaternion.identity); //해당 위치에 작은 적 소환
                }

            }
        }
        generateRandpos = true;

    }


    public void Shoot() //투사체 발사
    {
        lastShootTIme = Time.time;
        if (type == Type.Boss1) //Type 1의 보스라면
        {
            foreach (Transform projectLoc in projectLocs) //4곳에서 투사체 발사
            {
                GameObject proj = Instantiate(projectile, projectLoc.position, Quaternion.identity);
                projectLoc.LookAt(Target.transform);
                proj.GetComponent<Rigidbody>().velocity = projectLoc.forward * projectileSpeed;

                Destroy(proj, bulletDestroyTIme);
            }
        }

    }



    public void TakeDamage(int damage) //피해를 입었다면
    {
        health -= damage;
        StartCoroutine(OnDamage());

        if (health <= 0) //보스 사망시
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
        Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance의 거리의 랜덤한 위치를 생성하고
        for (int i = 0; i < 3; i++) //첫번째 아이템 즉, 골드 코인을 3개 드랍
        {
            Instantiate(dropItem[0], spawnPos, Quaternion.identity);
        }


        spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance의 거리의 랜덤한 위치를 생성하고

        int itemidx = Random.Range(1, 9); //emp와 healthpack을 1/8확률로 드랍
        if (itemidx <= 2)
        {
            Instantiate(dropItem[itemidx], spawnPos, Quaternion.identity);
        }

    }

    IEnumerator OnDamage() //mesh 색 red로 변화
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
