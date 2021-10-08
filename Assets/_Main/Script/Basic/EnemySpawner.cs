using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour //적 스폰을 관리하는 class
{
    [Header("- EnemyCommon")]
    public GameObject[] enemys; //적 오브젝트 저장
    public GameObject[] Boss; //보스 오브젝트 저장
    public int total_enemyCount; //wave 전체 적 개수
    public int cur_enemyCount; //현재 wave에 남아있는 적 수
    public float spawnDistance; //적 spawn 거리
    public bool canSpawnEnemies; // 적을 spawn한다는 trigger


    [Header("- EnemySpawnTimeSet")]
    public float startSpawnRate; //시작 spawn 속도
    public float minSpawnRate; //최소 spawn 속도, 점점 속도를 낮출때 사용
    public float timeToMinSpawnRate; //시작에서 최소 spawn 속도까지 가는데 걸리는 시간
    private float spawnRateMod; //spawn rate 계산을 위한 값

    private float lastSpawnTime; //마지막 Spawn했던 시간
    private float spawnRate; //현재시간에서 Spawn까지의 시간





    //instance

    public static EnemySpawner instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnRate = startSpawnRate; 
        spawnRateMod = (minSpawnRate - startSpawnRate) / timeToMinSpawnRate; //Spawn 속도 비율
    }

    private void Update()
    {
        if (!canSpawnEnemies)
            return;

        if (BaseGameManager.instance.gameMode == GameMode.attack) //Attacker 모드일때
        {
            /*           if (Time.time - lastSpawnTime >= spawnRate)
                           //SpawnEnemy();

                       if (spawnRate > minSpawnRate)
                       {
                           spawnRate -= spawnRateMod * Time.deltaTime;
                       }*/

        }
        else //Defender 모드일때
        {
            if (total_enemyCount > cur_enemyCount) //spawn된 적이 전체 spawn되야하는 적의 수보다 작을때
            {
                spawnRate -= 1 * Time.deltaTime;
                if (spawnRate <= 0) //SpawnRate에 맞춰 
                {
                    SpawnEnemy();  //Enemy를 Spawn한다
                    spawnRate = startSpawnRate;  //spawnRate 조정
                }
            }

            
            if (InGameManager.instance.killcount >= total_enemyCount) //전체 spawn되어야하는 적을 모두 죽였을경우
            {
                canSpawnEnemies = false; //Spawn을 종료
                total_enemyCount = 0;
                cur_enemyCount = 0;
                InGameManager.instance.WaveEnd(); //Wave를 종료시킨다.
            }


        }


    }

    public void SetEnemyCount() // total_enemyCount값을 Set하기 위한 함수
    {
        total_enemyCount = 5 * InGameManager.instance.wave;
    }



    void SpawnEnemy() //적을 Spawn하는 함수
    {
        lastSpawnTime = Time.time; 

        Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(Base.instance.transform.position, spawnDistance,true); //Base를 기준으로 spawnDistance만큼 거리를 얻는다. 이때 y값은 양수

        if (InGameManager.instance.wave == InGameManager.instance.totalWave) //last wave일때 Boss를 Spawn한다
        {
            total_enemyCount = 1;
            InGameManager.instance.waveEnemyCntTxt.text = "Enemy: " + (EnemySpawner.instance.total_enemyCount - InGameManager.instance.killcount);

            ChooseBoss(spawnPos);

        }
        else
        {
            switch (BaseGameManager.instance.diff) //각 난이도별로 적을 소환
            {
                case Difficulty.easy:  //easy에서는 normal
                    ChooseEnemy(1, spawnPos);
                    break;
                case Difficulty.normal: //normal에서는 normal,speed
                    ChooseEnemy(2, spawnPos);
                    break;
                case Difficulty.hard:
                    ChooseEnemy(3, spawnPos); //hard에서는 normal,speed,bomb 타입이 나온다.
                    break;
            }
        }



    }

    void ChooseEnemy(int diff, Vector3 spawnPos) //난이도별로 적을 소환한다.
    {
        int ranidx = Random.Range(0, diff);
        Instantiate(enemys[ranidx], spawnPos, Quaternion.identity);
        cur_enemyCount++;
    }

    void ChooseBoss(Vector3 spawnPos) //두종류의 boss중 하나를 랜덤으로 소환
    {
        int ranidx = Random.Range(0, 2);
        Instantiate(Boss[ranidx], spawnPos, Quaternion.identity);
        cur_enemyCount++;
    }
}
