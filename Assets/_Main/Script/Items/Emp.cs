using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emp : MonoBehaviour
{
    public float projectileSpeed;

    public Transform shootLoc;
    public GameObject projectile;


    public Camera cam;



    public void ShootEmp()  // emp 발사체 발사 버튼을 눌렀을 경우 전방에 emp 발사
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
