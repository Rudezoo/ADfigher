using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpSwitch : MonoBehaviour //아군 Emp가 터졌을경우
{
    public GameObject boomArea;

    private void Start() //3초 지나서 알아서 터진다
    {
        Invoke("EmpTimedown",3f);
    }
    private void OnTriggerEnter(Collider other) //boss나 enemy와 겹쳤을경우 터진다
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
        {
            EmpTimedown();
        }
    }

    private void EmpTimedown()  //emp가 터지는 로직
    {
        SoundManager.instance.playEmpBoomSound();
        Handheld.Vibrate();
        Destroy(gameObject);
        GameObject boom = Instantiate(boomArea, transform.position, Quaternion.identity);  //터진경우 해당 장소에 BoomArea를 소환
        Destroy(boom, 0.3f);
    }
}
