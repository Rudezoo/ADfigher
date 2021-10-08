using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour //Player�� �߻縦 �����ϴ� class
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
     
        if (Input.touchCount > 0 && InGameManager.instance.canAttack) //ȭ���� touch�ϰ� �����Ҽ��ִٸ�
        {
            if (Time.time - lastShootTIme >= ShootRate) //ShootRate�� ���� �߻�ü �߻�
                Shoot();
        }
    }

    void Shoot() //�ΰ����� �߻�ü�� �߻��Ѵ�.
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
