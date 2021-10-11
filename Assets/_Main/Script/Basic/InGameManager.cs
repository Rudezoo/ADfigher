using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour //InGame������ ��� ��Ʈ���� �����Ѵ�
{
    [Header("- GameControl")] //Ingame�� game Contol ���� ������
    public bool canAttack;
    public int killcount = 0;

    private MenuManager menu;
    public bool gameOver;

    public Difficulty diff;
    public GameObject placeBtn;


    [Header("- WaveControl")]  //Ingame�� Wave Contol ���� ������
    private float waveCountDown = 4;
    public bool canStartWave;
    public int totalWave;
    public int wave;

    [Header("- WaveScreen")] //Ingame�� Wave ���� Ui��

    public Text waveTxt;
    public Text waveCountTxt;
    public Text waveEnemyCntTxt;
    public GameObject waveBtn;

    public GameObject InfoMenu;
    public Text healthCntTxt;
    public Text empCntTxt;
    public Text coinCntTxt;
    public GameObject backButton;

    public Text DiffTxt;


    public GameObject winScreen; //�¸� ȭ��
    public GameObject loseScreen; //�й� ȭ��

    public GameObject moveScreen;


    public static InGameManager instance;

    private void Awake()
    {
        instance = this;
        menu = MenuManager.instance;
    }
    // Start is called before the first frame update
    void Start()
    {

        //placeBtn.SetActive(true);

        ClearObjects(); //ȭ�� ����

        if (BaseGameManager.instance) //BaseGameManager�� �ν��Ѵٸ�
        {
            diff = BaseGameManager.instance.diff;
        }

        DiffTxt.text = diff.ToString(); //���̵� ǥ�� UI ����
        DiffTxt.gameObject.SetActive(true);
        backButton.SetActive(true);

        moveScreen.SetActive(true);
    }

    // Update is called once per frame
    public void Ready() //Wave ������ ���� ��, wave ����
    {
        switch (diff) //���̵��� ��ü wave ����
        {
            case Difficulty.easy:
                totalWave = 3;
                break;
            case Difficulty.normal:
                totalWave = 6;
                break;
            case Difficulty.hard:
                totalWave = 9;
                break;
        }

        canStartWave = true; //wave ���� �غ�ȯ��
        waveTxt.gameObject.SetActive(true);
        placeBtn.SetActive(false);
    }

    void Update()
    {
        if (canStartWave) //Wave ���� �غ�
        {
            if (waveCountDown <= 0) //ī��Ʈ�ٿ��� ������
            {
                backButton.SetActive(true);
                waveTxt.text = "Wave Start";
                SoundManager.instance.playStartSound();
                Invoke("WaveStart", 1f); //1�� ���� wave ����
                canStartWave = false;
                waveCountDown = 4;
                SoundManager.instance.mainMusic.Stop();
                SoundManager.instance.BattleMusic.Play();
            }
            else //ī��Ʈ�ٿ���
            {

                backButton.SetActive(false);
                waveTxt.text = ((int)waveCountDown).ToString();
                if (waveCountDown%1000 == 0)
                {
                    SoundManager.instance.playcountSound();
                }
                waveCountDown -= Time.deltaTime;
            }


        }

    }

    public void WaveStart() //Wave ����
    {
        menu.crossHair.SetActive(true);
        InfoMenu.SetActive(true);
        waveTxt.gameObject.SetActive(false);
        waveCountTxt.gameObject.SetActive(true);

        canAttack = true; //Player�� ���� �� �� �ִ�
        
        EnemySpawner.instance.SetEnemyCount(); // spawn�� ��ü Enemy�� ����
        SetInfoMenuTxt();

        waveEnemyCntTxt.text = "Enemy: " + (EnemySpawner.instance.total_enemyCount - killcount);
        waveEnemyCntTxt.gameObject.SetActive(true);

        EnemySpawner.instance.canSpawnEnemies = true; // ���� spawn�ϱ� �����Ѵ�
    }

    public void WaveEnd() //Wave ����
    {
        menu.crossHair.SetActive(false);
        InfoMenu.SetActive(false);
        waveEnemyCntTxt.gameObject.SetActive(false);

        canAttack = false; //Player�� �����Ҽ� ����.
        EnemySpawner.instance.canSpawnEnemies = false; //Enemy�� Spawn���� �ʴ´�.

        if (wave == totalWave) //���� ��� Wave�� �����ٸ�
        {
            Win(); //�¸�
        }
        else
        {
            waveTxt.gameObject.SetActive(true);
            waveTxt.text = "Wave End";
            waveBtn.SetActive(true);
        }

        ClearObjects(); //ȭ�鰻��

        killcount = 0;
    }
    public void NextWave() //���� wave ����
    {
        SoundManager.instance.playmenuSound();

        wave++;
        waveCountTxt.text = "Wave :" + wave;

        waveBtn.SetActive(false);

        canStartWave = true; //wave ���� �غ�

    }
    public void RestartGame() //���� �����, scene�� ������Ѵ�.
    {
        SoundManager.instance.playmenuSound();
        SceneManager.UnloadSceneAsync("InGame");
        StartCoroutine(MenuManager.instance.Loading());
    }
    public void GameOver() //�й����� ���
    {

        SoundManager.instance.BattleMusic.Stop();
        SoundManager.instance.LoseMusic.Play();


        backButton.SetActive(false);
        menu.crossHair.SetActive(false);
        InfoMenu.SetActive(false);
        waveEnemyCntTxt.gameObject.SetActive(false);

        canAttack = false; 
        EnemySpawner.instance.canSpawnEnemies = false;

        //���� ���� �ʱ�ȭ
        killcount = 0;
        wave = 0;

        gameOver = true;

        loseScreen.SetActive(true);

        ClearObjects(); //ȭ�� ����

        GameObject Base = GameObject.Find("Base"); //Base�� ����� �����.
        Base.SetActive(false);
    }



    public void Win() //Game���� �¸�, �� ��� wave�� clear ������
    {
        EnemySpawner.instance.canSpawnEnemies = false;
        SoundManager.instance.BattleMusic.Stop();
        SoundManager.instance.WinMusic.Play();

        backButton.SetActive(false);
        winScreen.SetActive(true);
        wave = 1; //wave �� �ʱ�ȭ
    }

    public void SetInfoMenuTxt() //���� ����� �� �������� ���� �����ִ� ȭ���� �����Ѵ�.
    {
        healthCntTxt.text = "x " + PlayerInfo.instance.healthpack;
        empCntTxt.text = "x " + PlayerInfo.instance.emp;
        coinCntTxt.text = "x " + PlayerInfo.instance.coin;
    }

    void ClearObjects() //������ �������� ȭ���� �����ϴ� �Լ�
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); //���� object�� ã�� �ı�

        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item"); //�������� ��� �ı��� ���ÿ� ȹ�� �ȴ�.

        foreach (GameObject item in items)
        {
            Item tempitem=item.GetComponent<Item>();
            switch (tempitem.itemtype)
            {
                case ItemType.coin:
                    PlayerInfo.instance.coin+=tempitem.value;
                    break;
                case ItemType.emp:
                    PlayerInfo.instance.emp+=1;
                    break;
                case ItemType.health:
                    PlayerInfo.instance.healthpack += 1;
                    break;

            }
            
            Destroy(item);
            SetInfoMenuTxt();
        }

        GameObject[] playerBullet = GameObject.FindGameObjectsWithTag("PlayerBullet"); //ȭ�鿡 ���� �Ʊ��� �߻�ü�� ����������� �����.
        GameObject[] EnemyBullet = GameObject.FindGameObjectsWithTag("EnemyBullet");
        
        foreach (GameObject bullet in playerBullet)
        {
            Destroy(bullet);
        }
        foreach (GameObject bullet in EnemyBullet)
        {
            Destroy(bullet);
        }

        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss"); //Boss ��ü�� �������� ��� �����.
        foreach (GameObject b in boss)
        {
            Destroy(b);
        }
    }
    public void Endscene() //Scene�� ������ ���
    {

        ClearObjects();
        SoundManager.instance.playmenuSound();

        SoundManager.instance.BattleMusic.Stop();
        SoundManager.instance.mainMusic.Play();

        SceneManager.UnloadSceneAsync("InGame"); //InGame Scene�� unload��Ų��.

       MenuManager.instance.mainMenu.SetActive(true);
        MenuManager.instance.DiffMenu.SetActive(true);


    }


    public void Particle(GameObject effect, Transform pos)  //pos��ġ�� effect ��ƼŬ�� ��ȯ�Ѵ�.
    {
        GameObject particle = Instantiate(effect, pos.position, pos.rotation);
        Destroy(particle, 1f);
    }
}
