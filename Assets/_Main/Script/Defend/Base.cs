using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour //Base ��ü�� �����ϴ� class
{
    public int health;
    public int Maxhealth;
    public Text healthText;
    public GameObject healimpact;
    
    public Camera cam;


    public static Base instance;
    private MeshRenderer[] meshes;
    private void Start()
    {
        healthText.text = "HP: " + health; //Text ����
    }
    private void Awake()
    {
        instance = this;
        meshes = GetComponentsInChildren<MeshRenderer>();
    }

    private void Update()
    {
        healthText.transform.rotation = Quaternion.LookRotation(healthText.transform.position - cam.transform.position); //ü�� Text�� �׻� ȭ���� �ٶ󺸵����Ѵ�.

        if (InGameManager.instance.gameOver)
            return;

    }

    public void TakeDamage(int damage) //Base�� ���ݹ޾�����
    {
        Debug.Log("Base Attacked!");
        StartCoroutine(OnDamage()); 
        health -= damage;
        healthText.text = "HP: " + health; //Text ����

        if (health <= 0)
            InGameManager.instance.GameOver();
    }

    public void SetBaseTxt() 
    {
        healthText.text = "HP: " + health; //Text ����
    }

    public void HealEffect() //ü��ȸ���� ȿ��
    {
        InGameManager.instance.Particle(healimpact, transform);
        StartCoroutine(OnHeal());
    }
    IEnumerator OnHeal() //ȸ���� mesh �� ��ȭ => green
    {

        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.green;
        }
        yield return new WaitForSeconds(0.3f);
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }
    }

    IEnumerator OnDamage() //���ظ� �Ծ����� mesh �� ��ȭ =>red
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
