using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour //Base 자체를 관리하느 class
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
        healthText.text = "HP: " + health; //Text 설정
    }
    private void Awake()
    {
        instance = this;
        meshes = GetComponentsInChildren<MeshRenderer>();
    }

    private void Update()
    {
        healthText.transform.rotation = Quaternion.LookRotation(healthText.transform.position - cam.transform.position); //체력 Text가 항상 화면을 바라보도록한다.

        if (InGameManager.instance.gameOver)
            return;

    }

    public void TakeDamage(int damage) //Base가 공격받았을때
    {
        Debug.Log("Base Attacked!");
        StartCoroutine(OnDamage()); 
        health -= damage;
        healthText.text = "HP: " + health; //Text 설정

        if (health <= 0)
            InGameManager.instance.GameOver();
    }

    public void SetBaseTxt() 
    {
        healthText.text = "HP: " + health; //Text 설정
    }

    public void HealEffect() //체력회복시 효과
    {
        InGameManager.instance.Particle(healimpact, transform);
        StartCoroutine(OnHeal());
    }
    IEnumerator OnHeal() //회복시 mesh 색 변화 => green
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

    IEnumerator OnDamage() //피해를 입었을때 mesh 색 변화 =>red
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
