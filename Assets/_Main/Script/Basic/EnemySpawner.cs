using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour //�� ������ �����ϴ� class
{
    [Header("- EnemyCommon")]
    public GameObject[] enemys; //�� ������Ʈ ����
    public GameObject[] Boss; //���� ������Ʈ ����
    public int total_enemyCount; //wave ��ü �� ����
    public int cur_enemyCount; //���� wave�� �����ִ� �� ��
    public float spawnDistance; //�� spawn �Ÿ�
    public bool canSpawnEnemies; // ���� spawn�Ѵٴ� trigger


    [Header("- EnemySpawnTimeSet")]
    public float startSpawnRate; //���� spawn �ӵ�
    public float minSpawnRate; //�ּ� spawn �ӵ�, ���� �ӵ��� ���⶧ ���
    public float timeToMinSpawnRate; //���ۿ��� �ּ� spawn �ӵ����� ���µ� �ɸ��� �ð�
    private float spawnRateMod; //spawn rate ����� ���� ��

    private float lastSpawnTime; //������ Spawn�ߴ� �ð�
    private float spawnRate; //����ð����� Spawn������ �ð�





    //instance

    public static EnemySpawner instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnRate = startSpawnRate; 
        spawnRateMod = (minSpawnRate - startSpawnRate) / timeToMinSpawnRate; //Spawn �ӵ� ����
    }

    private void Update()
    {
        if (!canSpawnEnemies)
            return;

        if (BaseGameManager.instance.gameMode == GameMode.attack) //Attacker ����϶�
        {
            /*           if (Time.time - lastSpawnTime >= spawnRate)
                           //SpawnEnemy();

                       if (spawnRate > minSpawnRate)
                       {
                           spawnRate -= spawnRateMod * Time.deltaTime;
                       }*/

        }
        else //Defender ����϶�
        {
            if (total_enemyCount > cur_enemyCount) //spawn�� ���� ��ü spawn�Ǿ��ϴ� ���� ������ ������
            {
                spawnRate -= 1 * Time.deltaTime;
                if (spawnRate <= 0) //SpawnRate�� ���� 
                {
                    SpawnEnemy();  //Enemy�� Spawn�Ѵ�
                    spawnRate = startSpawnRate;  //spawnRate ����
                }
            }

            
            if (InGameManager.instance.killcount >= total_enemyCount) //��ü spawn�Ǿ���ϴ� ���� ��� �׿������
            {
                canSpawnEnemies = false; //Spawn�� ����
                total_enemyCount = 0;
                cur_enemyCount = 0;
                InGameManager.instance.WaveEnd(); //Wave�� �����Ų��.
            }


        }


    }

    public void SetEnemyCount() // total_enemyCount���� Set�ϱ� ���� �Լ�
    {
        total_enemyCount = 5 * InGameManager.instance.wave;
    }



    void SpawnEnemy() //���� Spawn�ϴ� �Լ�
    {
        lastSpawnTime = Time.time; 

        Vector3 spawnPos = BaseGameManager.instance.GetRandomPos(Base.instance.transform.position, spawnDistance,true); //Base�� �������� spawnDistance��ŭ �Ÿ��� ��´�. �̶� y���� ���

        if (InGameManager.instance.wave == InGameManager.instance.totalWave) //last wave�϶� Boss�� Spawn�Ѵ�
        {
            total_enemyCount = 1;
            InGameManager.instance.waveEnemyCntTxt.text = "Enemy: " + (EnemySpawner.instance.total_enemyCount - InGameManager.instance.killcount);

            ChooseBoss(spawnPos);

        }
        else
        {
            switch (BaseGameManager.instance.diff) //�� ���̵����� ���� ��ȯ
            {
                case Difficulty.easy:  //easy������ normal
                    ChooseEnemy(1, spawnPos);
                    break;
                case Difficulty.normal: //normal������ normal,speed
                    ChooseEnemy(2, spawnPos);
                    break;
                case Difficulty.hard:
                    ChooseEnemy(3, spawnPos); //hard������ normal,speed,bomb Ÿ���� ���´�.
                    break;
            }
        }



    }

    void ChooseEnemy(int diff, Vector3 spawnPos) //���̵����� ���� ��ȯ�Ѵ�.
    {
        int ranidx = Random.Range(0, diff);
        Instantiate(enemys[ranidx], spawnPos, Quaternion.identity);
        cur_enemyCount++;
    }

    void ChooseBoss(Vector3 spawnPos) //�������� boss�� �ϳ��� �������� ��ȯ
    {
        int ranidx = Random.Range(0, 2);
        Instantiate(Boss[ranidx], spawnPos, Quaternion.identity);
        cur_enemyCount++;
    }
}
