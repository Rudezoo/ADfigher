using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour //InGame에서의 모든 컨트롤을 관리한다
{
    [Header("- GameControl")] //Ingame상 game Contol 관련 변수들
    public bool canAttack;
    public int killcount = 0;

    private MenuManager menu;
    public bool gameOver;

    public Difficulty diff;
    public GameObject placeBtn;


    [Header("- WaveControl")]  //Ingame상 Wave Contol 관련 변수들
    private float waveCountDown = 4;
    public bool canStartWave;
    public int totalWave;
    public int wave;

    [Header("- WaveScreen")] //Ingame상 Wave 관련 Ui들

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


    public GameObject winScreen; //승리 화면
    public GameObject loseScreen; //패배 화면

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

        ClearObjects(); //화면 갱신

        if (BaseGameManager.instance) //BaseGameManager를 인식한다면
        {
            diff = BaseGameManager.instance.diff;
        }

        DiffTxt.text = diff.ToString(); //난이도 표시 UI 설정
        DiffTxt.gameObject.SetActive(true);
        backButton.SetActive(true);

        moveScreen.SetActive(true);
    }

    // Update is called once per frame
    public void Ready() //Wave 시작전 세팅 후, wave 시작
    {
        switch (diff) //난이도별 전체 wave 설정
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

        canStartWave = true; //wave 시작 준비환료
        waveTxt.gameObject.SetActive(true);
        placeBtn.SetActive(false);
    }

    void Update()
    {
        if (canStartWave) //Wave 시작 준비
        {
            if (waveCountDown <= 0) //카운트다운이 끝나면
            {
                backButton.SetActive(true);
                waveTxt.text = "Wave Start";
                SoundManager.instance.playStartSound();
                Invoke("WaveStart", 1f); //1초 이후 wave 시작
                canStartWave = false;
                waveCountDown = 4;
                SoundManager.instance.mainMusic.Stop();
                SoundManager.instance.BattleMusic.Play();
            }
            else //카운트다운중
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

    public void WaveStart() //Wave 시작
    {
        menu.crossHair.SetActive(true);
        InfoMenu.SetActive(true);
        waveTxt.gameObject.SetActive(false);
        waveCountTxt.gameObject.SetActive(true);

        canAttack = true; //Player가 공격 할 수 있다
        
        EnemySpawner.instance.SetEnemyCount(); // spawn할 전체 Enemy수 조정
        SetInfoMenuTxt();

        waveEnemyCntTxt.text = "Enemy: " + (EnemySpawner.instance.total_enemyCount - killcount);
        waveEnemyCntTxt.gameObject.SetActive(true);

        EnemySpawner.instance.canSpawnEnemies = true; // 적을 spawn하기 시작한다
    }

    public void WaveEnd() //Wave 끝남
    {
        menu.crossHair.SetActive(false);
        InfoMenu.SetActive(false);
        waveEnemyCntTxt.gameObject.SetActive(false);

        canAttack = false; //Player는 공격할수 없다.
        EnemySpawner.instance.canSpawnEnemies = false; //Enemy가 Spawn되지 않는다.

        if (wave == totalWave) //만약 모든 Wave가 끝났다면
        {
            Win(); //승리
        }
        else
        {
            waveTxt.gameObject.SetActive(true);
            waveTxt.text = "Wave End";
            waveBtn.SetActive(true);
        }

        ClearObjects(); //화면갱신

        killcount = 0;
    }
    public void NextWave() //다음 wave 시작
    {
        SoundManager.instance.playmenuSound();

        wave++;
        waveCountTxt.text = "Wave :" + wave;

        waveBtn.SetActive(false);

        canStartWave = true; //wave 시작 준비

    }
    public void RestartGame() //게임 재시작, scene을 재실행한다.
    {
        SoundManager.instance.playmenuSound();
        SceneManager.UnloadSceneAsync("InGame");
        StartCoroutine(MenuManager.instance.Loading());
    }
    public void GameOver() //패배했을 경우
    {

        SoundManager.instance.BattleMusic.Stop();
        SoundManager.instance.LoseMusic.Play();


        backButton.SetActive(false);
        menu.crossHair.SetActive(false);
        InfoMenu.SetActive(false);
        waveEnemyCntTxt.gameObject.SetActive(false);

        canAttack = false; 
        EnemySpawner.instance.canSpawnEnemies = false;

        //관련 변수 초기화
        killcount = 0;
        wave = 0;

        gameOver = true;

        loseScreen.SetActive(true);

        ClearObjects(); //화면 갱신

        GameObject Base = GameObject.Find("Base"); //Base의 모습도 감춘다.
        Base.SetActive(false);
    }



    public void Win() //Game에서 승리, 즉 모든 wave를 clear 했을때
    {
        EnemySpawner.instance.canSpawnEnemies = false;
        SoundManager.instance.BattleMusic.Stop();
        SoundManager.instance.WinMusic.Play();

        backButton.SetActive(false);
        winScreen.SetActive(true);
        wave = 1; //wave 수 초기화
    }

    public void SetInfoMenuTxt() //정보 변경시 각 아이템의 수를 보여주는 화면을 갱신한다.
    {
        healthCntTxt.text = "x " + PlayerInfo.instance.healthpack;
        empCntTxt.text = "x " + PlayerInfo.instance.emp;
        coinCntTxt.text = "x " + PlayerInfo.instance.coin;
    }

    void ClearObjects() //게임이 끝났을때 화면을 갱신하는 함수
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); //적인 object를 찾아 파괴

        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item"); //아이템의 경우 파괴와 동시에 획득 된다.

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

        GameObject[] playerBullet = GameObject.FindGameObjectsWithTag("PlayerBullet"); //화면에 적과 아군의 발사체가 남아있을경우 지운다.
        GameObject[] EnemyBullet = GameObject.FindGameObjectsWithTag("EnemyBullet");
        
        foreach (GameObject bullet in playerBullet)
        {
            Destroy(bullet);
        }
        foreach (GameObject bullet in EnemyBullet)
        {
            Destroy(bullet);
        }

        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss"); //Boss 개체가 남아있을 경우 지운다.
        foreach (GameObject b in boss)
        {
            Destroy(b);
        }
    }
    public void Endscene() //Scene을 끝내는 경우
    {

        ClearObjects();
        SoundManager.instance.playmenuSound();

        SoundManager.instance.BattleMusic.Stop();
        SoundManager.instance.mainMusic.Play();

        SceneManager.UnloadSceneAsync("InGame"); //InGame Scene을 unload시킨다.

       MenuManager.instance.mainMenu.SetActive(true);
        MenuManager.instance.DiffMenu.SetActive(true);


    }


    public void Particle(GameObject effect, Transform pos)  //pos위치에 effect 파티클을 소환한다.
    {
        GameObject particle = Instantiate(effect, pos.position, pos.rotation);
        Destroy(particle, 1f);
    }
}
