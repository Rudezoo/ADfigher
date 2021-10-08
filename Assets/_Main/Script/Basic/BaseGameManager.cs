using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { attack, defense }; //���� ��� ����, ����� defense�� ���
public enum Difficulty { easy, normal, hard }; //���� ���̵� ����
public class BaseGameManager : MonoBehaviour //�⺻���� ���� ������ �����Ѵ�. 
{
    [Header("- GameControl")]
    public GameMode gameMode; //���� ��带 �����Ѵ�.
    public Difficulty diff; //���� ���̵��� �����Ѵ�.
   

    public static BaseGameManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update

    private void Start()
    {
        LoadGame(); //����� ���� �����Ͱ��ִٸ� �ҷ��´�
    }

    public void Purchase(int idx) // ���� ���ű��
    {
        if (idx == 0) //healthpack ���Խ�
        {
            if (PlayerInfo.instance.coin >= 150) //150��
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
            if (PlayerInfo.instance.coin >= 300) //300��
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

    public Vector3 GetRandomPos(Vector3 center, float spawnDistance,bool abs) //spawnDistance��ŭ�� �������� ���� �������� ������ ��ġ�� ��´�.
    {
        Vector3 spawnCircle = Random.onUnitSphere; //�������·� ������ ��ġ�� ��´� �̶� �������� 1

        if (abs)
            spawnCircle.y = Mathf.Abs(spawnCircle.y); //������ ��ġ�� y���� ���� ����� ��ȯ

        Vector3 spawnPos = center + (spawnCircle * spawnDistance); //center��ġ�� �������� spawnDistance��ŭ�� ������ ��ġ�� ��´�.

        return spawnPos;
    }

    public void SaveGame() //���� ������ ����
    {
        PlayerPrefs.SetInt("Coin", PlayerInfo.instance.coin);
        PlayerPrefs.SetInt("Emp", PlayerInfo.instance.emp);
        PlayerPrefs.SetInt("HealthPack", PlayerInfo.instance.healthpack);
    }

    public void LoadGame() //���� ������ �ҷ�����
    {
        if (PlayerPrefs.HasKey("Coin"))
            PlayerInfo.instance.coin = PlayerPrefs.GetInt("Coin");

        if (PlayerPrefs.HasKey("Emp"))
            PlayerInfo.instance.emp = PlayerPrefs.GetInt("Emp");

        if (PlayerPrefs.HasKey("HealthPack"))
            PlayerInfo.instance.healthpack = PlayerPrefs.GetInt("HealthPack");
    }

}
