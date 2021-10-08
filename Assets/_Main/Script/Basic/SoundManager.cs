using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour //효과음 관리를 위한 clss
{

    public AudioSource audioSource;
    public AudioSource mainMusic;
    public AudioSource BattleMusic;
    public AudioSource WinMusic;
    public AudioSource LoseMusic;
    public AudioClip menuSound;
    public AudioClip DiffChooseSound;
    public AudioClip purchaseSound;
    public AudioClip nopurchaseSound;
    public AudioClip countSound;
    public AudioClip StartSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;
    public AudioClip LaserSound;
    public AudioClip EnemyLaserSound;
    public AudioClip BoomSound;
    public AudioClip BoomSound2;

    public AudioClip EmpSound;
    public AudioClip EmpBoomSound;

    public AudioClip BossEmpSound;
    public AudioClip BossBoomSound;

    public AudioClip HealSound;
    public AudioClip ItemSound;

    public AudioClip HitSound;

    public AudioClip SpeedupSound;

    public float volume;

    // Start is called before the first frame update

    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainMusic.Play();
    }



    public void playmenuSound() //메뉴
    {
        audioSource.PlayOneShot(menuSound, volume);
    }

    public void playDiffChooseSound() //난이도 메뉴
    {
        audioSource.PlayOneShot(DiffChooseSound, volume/2);
    }

    public void playPurchaseSound() //구매
    {
        audioSource.PlayOneShot(purchaseSound, volume);
    }
    public void playnoPurchaseSound() //구매불가
    {
        audioSource.PlayOneShot(nopurchaseSound, volume);
    }
    public void playcountSound() //숫자 카운트, 
    {
        audioSource.PlayOneShot(countSound, volume);
    }
    public void playStartSound() //시작
    {
        audioSource.PlayOneShot(StartSound, volume);
    }
    public void playWinSound()//승리
    {
        audioSource.PlayOneShot(WinSound, volume);
    }
    public void playLoseSound()//패배
    {
        audioSource.PlayOneShot(LoseSound, volume);
    }

    public void playLaserSound()//플레이어 투사체
    {
        audioSource.PlayOneShot(LaserSound, volume);
    }
    public void playEnemyLaserSound()//적 투사체
    {
        audioSource.PlayOneShot(EnemyLaserSound, volume);
    }


    public void playBoomSound() //폭발
    {
        audioSource.PlayOneShot(BoomSound, volume);
    }

    public void playEnemyDieSound()//적 죽음
    {
        audioSource.PlayOneShot(BoomSound2, volume);
    }


    public void playEmpSound() //Emp
    {
        audioSource.PlayOneShot(EmpSound, volume*2);
    }

    public void playEmpBoomSound() //Emp터짐
    {
        audioSource.PlayOneShot(EmpBoomSound, volume*2);
    }

    public void playBossEmpSound() //보스 Emp
    {
        audioSource.PlayOneShot(BossEmpSound, volume*2);
    }

    public void playBossEmpBoomSound() //보스 Emp터짐
    {
        audioSource.PlayOneShot(BossBoomSound, volume*2);
    }

    public void playHealSound() //Base 체력회복 
    {
        audioSource.PlayOneShot(HealSound, volume);
    }

    public void playItemSound() //아이템 습득
    {
        audioSource.PlayOneShot(ItemSound, volume);
    }

    public void playHitSound() //타격
    {
        audioSource.PlayOneShot(HitSound, volume/1.5f);
    }


    public void playSpeedupSound() //가속
    {
        audioSource.PlayOneShot(SpeedupSound, volume);
    }

}
