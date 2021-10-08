using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour //normal,speed,bomb, small(for Boss type2) 타입의 적을 관리하기위한 class
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
            if (BaseGameManager.instance.gameMode == GameMode.defense) //defense 모드일때의 목표설정
            {
                Target = GameObject.FindGameObjectWithTag("Base");
            }
            else //Attacker 모드일때의 목표설정
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



        float dist = Vector3.Distance(transform.position, Target.transform.position);  //Target과 enemy의 거리 측정

        if (dist > attackRange) //만약 공격범위안에 들어오지않았다면 Target을 향해 이동
        {
            if (!moving)
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {//공격범위안에 들어왔다면
            switch (type) //타입에 따라 다른 패턴 구사
            {
                case Type.normal: //normal 타입은 해당 위치에 정지해서 공격
                    if (Time.time - lastShootTIme >= ShootRate) //Shootrate에 맞춰 공격한다.
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
            Collider[] randcol = Physics.OverlapSphere(randpos, 0.2f); //생성된 randpos위치에 구형태의 coliider생성
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


        transform.LookAt(Target.transform); //Target을 바라본다
    }


    void SpeedTypeLogic() //적 type이 speed일때의 Logic을 담당
    {
        if (Time.time - lastShootTIme >= ShootRate) //Shootrate에 맞춰 공격한다.
        {
            Shoot();

            //쏘고나서 Base 근처 랜덤한 위치로 이동(Defender모드일때)
            if (BaseGameManager.instance.gameMode == GameMode.defense)
            {
                generateRandpos = true; //일정 범위안 Random한 위치 생성을 위해 TriggerOn

            }

        }

    }

    void BombTypeLogic() //적 type이 bomb일때의 Logic을 담당
    {
        if (!playsoundonce) //한번만 가속 속도 재생
        {
            SoundManager.instance.playSpeedupSound();
            playsoundonce = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime * AccelSpeed); //기속속도를 받아 빠르게 Target을 향해 돌진
    }


    private void OnTriggerEnter(Collider other) //돌진시에 Trigger발생시
    {
        if (other.gameObject.tag == "Base") //Base와 겹친다면
        {
            if (type == Type.bomb) //bomb타입일때 Base에 피해를 준다.
            {
                Base.instance.TakeDamage(damage);
                SoundManager.instance.playBoomSound();
                InGameManager.instance.Particle(boomEffect, transform);
                Destroy(gameObject);
                Handheld.Vibrate();
            }

        }
    }


    public void Shoot() //Normal과 Speed 타입에서 사용되는 투사체 발사
    {

        SoundManager.instance.playEnemyLaserSound();
        lastShootTIme = Time.time;
        GameObject proj = Instantiate(projectile, projectLoc.position, Quaternion.identity);
        projectLoc.LookAt(Target.transform);
        proj.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;

        Destroy(proj, bulletDestroyTIme);
    }



    public void TakeDamage(int damage) //피해를 받은경우
    {
        health -= damage;
        StartCoroutine(OnDamage());

        if (health <= 0) //죽었을때
        {
            if (type != Type.small) //보스소환체는 count하지않는다.
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

    void DropItem() //아이템 드랍
    {
        Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance의 거리의 랜덤한 위치를 생성하고

        switch (type) //각 type에 맞춰 copper,silver,gold 코인 드랍
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
        spawnPos = BaseGameManager.instance.GetRandomPos(transform.position, ItemspawnDistance, false); //ItemspawnDistance의 거리의 랜덤한 위치를 생성하고
        int itemidx = Random.Range(3, 8); //emp와 healthpack을 1/5확률로 드랍
        if (itemidx <= 4)
        {
            Instantiate(dropItem[itemidx], spawnPos, Quaternion.identity);
        }

    }

    IEnumerator OnDamage() //피해입었을시 mesh를 red로 변환
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
