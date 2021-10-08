using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { attack, defense }; //게임 모드 설정, 현재는 defense만 사용
public enum Difficulty { easy, normal, hard }; //게임 난이도 설정
public class BaseGameManager : MonoBehaviour //기본적인 게임 정보를 포함한다. 
{
    [Header("- GameControl")]
    public GameMode gameMode; //게임 모드를 저장한다.
    public Difficulty diff; //게임 난이도를 저장한다.
   

    public static BaseGameManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update

    private void Start()
    {
        LoadGame(); //저장된 게임 데이터가있다면 불러온다
    }

    public void Purchase(int idx) // 상점 구매기능
    {
        if (idx == 0) //healthpack 구입시
        {
            if (PlayerInfo.instance.coin >= 150) //150원
            {
                SoundManager.instance.playPurchaseSound();
                PlayerInfo.instance.healthpack++;
                PlayerInfo.instance.coin -= 150;
                MenuManager.instance.SetShopText();
            }
            else
            {
                SoundManager.instance.playnoPurchaseSound();
            }
        }
        else
        {
            if (PlayerInfo.instance.coin >= 300) //300원
            {
                SoundManager.instance.playPurchaseSound();
                PlayerInfo.instance.emp++;
                PlayerInfo.instance.coin -= 300;
                MenuManager.instance.SetShopText();
            }
            else
            {
                SoundManager.instance.playnoPurchaseSound();
            }
        }
    }

    public Vector3 GetRandomPos(Vector3 center, float spawnDistance,bool abs) //spawnDistance만큼의 범위안의 구를 기준으로 랜덤한 위치를 얻는다.
    {
        Vector3 spawnCircle = Random.onUnitSphere; //구의형태로 랜덤한 위치를 얻는다 이때 반지름은 1

        if (abs)
            spawnCircle.y = Mathf.Abs(spawnCircle.y); //랜덤한 위치의 y값을 전부 양수로 변환

        Vector3 spawnPos = center + (spawnCircle * spawnDistance); //center위치를 기준으로 spawnDistance만큼의 랜덤한 위치를 얻는다.

        return spawnPos;
    }

    public void SaveGame() //게임 데이터 저장
    {
        PlayerPrefs.SetInt("Coin", PlayerInfo.instance.coin);
        PlayerPrefs.SetInt("Emp", PlayerInfo.instance.emp);
        PlayerPrefs.SetInt("HealthPack", PlayerInfo.instance.healthpack);
    }

    public void LoadGame() //게임 데이터 불러오기
    {
        if (PlayerPrefs.HasKey("Coin"))
            PlayerInfo.instance.coin = PlayerPrefs.GetInt("Coin");

        if (PlayerPrefs.HasKey("Emp"))
            PlayerInfo.instance.emp = PlayerPrefs.GetInt("Emp");

        if (PlayerPrefs.HasKey("HealthPack"))
            PlayerInfo.instance.healthpack = PlayerPrefs.GetInt("HealthPack");
    }

}
