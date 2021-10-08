using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour //���� �޴����� ȭ�� ��Ʈ���� ����Ѵ�.
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

    public void GameStart() //Game Start�� ������, �޴� ��Ʈ��
    {
        SoundManager.instance.playmenuSound();
        StartMenu.SetActive(false);
        mainMenu.SetActive(true);
        FrontMenu.SetActive(true);
    }

    public void GameEnd() //������ ����
    {
        SoundManager.instance.playmenuSound();
        BaseGameManager.instance.SaveGame(); //������ ������ �����Ѵ�

        Application.Quit();
    }

    public void ChooseDiff() //Battle�� ������, �޴���Ʈ��
    {
        SoundManager.instance.playmenuSound();
        FrontMenu.SetActive(false);
        DiffMenu.SetActive(true);
    }

    public void ChooseShop() //Shop�� �������� ���� ȭ��
    {
        SoundManager.instance.playmenuSound();
        FrontMenu.SetActive(false);
        ShopMenu.SetActive(true);
        SetShopText();
    }

    public void GotoMainMenu() //���θ޴��� ���Ʊ�� ���ý� ȭ��
    {
        SoundManager.instance.playmenuSound();
        mainMenu.SetActive(true);
        FrontMenu.SetActive(true);

        //loseScreen.SetActive(false);
        DiffMenu.SetActive(false);
        ShopMenu.SetActive(false);
        crossHair.SetActive(false);
    }

    public void SetShopText() //���Ű� �̷�������� ������ �����ؽ�Ʈ�� �����Ѵ�.
    {
        shopCoin.text = "x " + PlayerInfo.instance.coin;
        shopHealth.text = "x " + PlayerInfo.instance.healthpack;
        shopEmp.text = "x " + PlayerInfo.instance.emp;

    }

    public void BattleStart(int idx) //���̵� ���� ��ư�� ������ ����, �������� ar ȭ�� ǥ��
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

        StartCoroutine(Loading()); //�ε��������� �����Ƿ� �ش� �ε��� �����ش�.


    }


    public IEnumerator Loading() //�ε�ȭ�� ǥ���� scene�� �ҷ��´�.
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
