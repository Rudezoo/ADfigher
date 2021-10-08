using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour //메인 메뉴상의 화면 컨트롤을 담당한다.
{
    // Start is called before the first frame update
    [Header("- GameScreen")]



    public GameObject StartMenu;
    public GameObject mainMenu;


    public GameObject FrontMenu;
    public GameObject DiffMenu;
    public GameObject ShopMenu;

    public GameObject LoadingScreen;

    public Text shopCoin;
    public Text shopHealth;
    public Text shopEmp;


    public GameObject crossHair;


    public static MenuManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartMenu.SetActive(true);
    }

    public void GameStart() //Game Start를 누르면, 메뉴 컨트롤
    {
        SoundManager.instance.playmenuSound();
        StartMenu.SetActive(false);
        mainMenu.SetActive(true);
        FrontMenu.SetActive(true);
    }

    public void GameEnd() //게임을 끈다
    {
        SoundManager.instance.playmenuSound();
        BaseGameManager.instance.SaveGame(); //종료전 게임을 저장한다

        Application.Quit();
    }

    public void ChooseDiff() //Battle을 누르면, 메뉴컨트롤
    {
        SoundManager.instance.playmenuSound();
        FrontMenu.SetActive(false);
        DiffMenu.SetActive(true);
    }

    public void ChooseShop() //Shop을 선택했을 떄의 화면
    {
        SoundManager.instance.playmenuSound();
        FrontMenu.SetActive(false);
        ShopMenu.SetActive(true);
        SetShopText();
    }

    public void GotoMainMenu() //메인메뉴로 돌아기기 선택시 화면
    {
        SoundManager.instance.playmenuSound();
        mainMenu.SetActive(true);
        FrontMenu.SetActive(true);

        //loseScreen.SetActive(false);
        DiffMenu.SetActive(false);
        ShopMenu.SetActive(false);
        crossHair.SetActive(false);
    }

    public void SetShopText() //구매가 이루어졌을때 상점의 정보텍스트를 갱신한다.
    {
        shopCoin.text = "x " + PlayerInfo.instance.coin;
        shopHealth.text = "x " + PlayerInfo.instance.healthpack;
        shopEmp.text = "x " + PlayerInfo.instance.emp;

    }

    public void BattleStart(int idx) //난이도 선택 버튼을 누르면 실행, 본격적인 ar 화면 표시
    {
        SoundManager.instance.playDiffChooseSound();
        mainMenu.SetActive(false);
        DiffMenu.SetActive(false);

        switch (idx)
        {
            case 0:
                BaseGameManager.instance.diff = Difficulty.easy;
                break;
            case 1:
                BaseGameManager.instance.diff = Difficulty.normal;
                break;
            case 2:
                BaseGameManager.instance.diff = Difficulty.hard;
                break;
        }

        StartCoroutine(Loading()); //로딩이있을수 있으므로 해당 로딩을 보여준다.


    }


    public IEnumerator Loading() //로딩화면 표시후 scene을 불러온다.
    {
        AsyncOperation loadingOperation=SceneManager.LoadSceneAsync("InGame",LoadSceneMode.Additive);
        while (!loadingOperation.isDone)
        {
            yield return null;
            Debug.Log(loadingOperation.progress);
            LoadingScreen.SetActive(true);
        }
        LoadingScreen.SetActive(false);

    }


}
