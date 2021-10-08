using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour //Player의 발사를 관리하는 class
{
    
    public GameObject projectile;
    public float projectileSpeed;
    public float ShootRate;
    private float lastShootTIme;

    public Transform shootLoc1;
    public Transform shootLoc2;

    public Camera cam;

    public static Shooting instance;
    private void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
     
        if (Input.touchCount > 0 && InGameManager.instance.canAttack) //화면을 touch하고 공격할수있다면
        {
            if (Time.time - lastShootTIme >= ShootRate) //ShootRate에 맞춰 발사체 발사
                Shoot();
        }
    }

    void Shoot() //두곳에서 발사체를 발사한다.
    {
        lastShootTIme = Time.time;

        SoundManager.instance.playLaserSound();


        GameObject proj1 = Instantiate(projectile, shootLoc1.position, Quaternion.identity);
        GameObject proj2 = Instantiate(projectile, shootLoc2.position, Quaternion.identity);

        proj1.GetComponent<Rigidbody>().velocity = cam.transform.forward * projectileSpeed;
        proj2.GetComponent<Rigidbody>().velocity = cam.transform.forward * projectileSpeed;

        Destroy(proj1, 2.0f);
        Destroy(proj2, 2.0f);

    }
}
