using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emp : MonoBehaviour
{
    public float projectileSpeed;

    public Transform shootLoc;
    public GameObject projectile;


    public Camera cam;



    public void ShootEmp()  // emp �߻�ü �߻� ��ư�� ������ ��� ���濡 emp �߻�
    {
        if (PlayerInfo.instance.emp > 0)
        {
            SoundManager.instance.playmenuSound();

            PlayerInfo.instance.emp--;

            InGameManager.instance.SetInfoMenuTxt();

            GameObject proj = Instantiate(projectile, shootLoc.position, Quaternion.identity);
            proj.GetComponent<Rigidbody>().velocity = cam.transform.forward * projectileSpeed;

            SoundManager.instance.playEmpSound();
        }
    }



}
