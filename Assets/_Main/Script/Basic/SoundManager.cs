using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour //ȿ���� ������ ���� clss
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



    public void playmenuSound() //�޴�
    {
        audioSource.PlayOneShot(menuSound, volume);
    }

    public void playDiffChooseSound() //���̵� �޴�
    {
        audioSource.PlayOneShot(DiffChooseSound, volume/2);
    }

    public void playPurchaseSound() //����
    {
        audioSource.PlayOneShot(purchaseSound, volume);
    }
    public void playnoPurchaseSound() //���źҰ�
    {
        audioSource.PlayOneShot(nopurchaseSound, volume);
    }
    public void playcountSound() //���� ī��Ʈ, 
    {
        audioSource.PlayOneShot(countSound, volume);
    }
    public void playStartSound() //����
    {
        audioSource.PlayOneShot(StartSound, volume);
    }
    public void playWinSound()//�¸�
    {
        audioSource.PlayOneShot(WinSound, volume);
    }
    public void playLoseSound()//�й�
    {
        audioSource.PlayOneShot(LoseSound, volume);
    }

    public void playLaserSound()//�÷��̾� ����ü
    {
        audioSource.PlayOneShot(LaserSound, volume);
    }
    public void playEnemyLaserSound()//�� ����ü
    {
        audioSource.PlayOneShot(EnemyLaserSound, volume);
    }


    public void playBoomSound() //����
    {
        audioSource.PlayOneShot(BoomSound, volume);
    }

    public void playEnemyDieSound()//�� ����
    {
        audioSource.PlayOneShot(BoomSound2, volume);
    }


    public void playEmpSound() //Emp
    {
        audioSource.PlayOneShot(EmpSound, volume*2);
    }

    public void playEmpBoomSound() //Emp����
    {
        audioSource.PlayOneShot(EmpBoomSound, volume*2);
    }

    public void playBossEmpSound() //���� Emp
    {
        audioSource.PlayOneShot(BossEmpSound, volume*2);
    }

    public void playBossEmpBoomSound() //���� Emp����
    {
        audioSource.PlayOneShot(BossBoomSound, volume*2);
    }

    public void playHealSound() //Base ü��ȸ�� 
    {
        audioSource.PlayOneShot(HealSound, volume);
    }

    public void playItemSound() //������ ����
    {
        audioSource.PlayOneShot(ItemSound, volume);
    }

    public void playHitSound() //Ÿ��
    {
        audioSource.PlayOneShot(HitSound, volume/1.5f);
    }


    public void playSpeedupSound() //����
    {
        audioSource.PlayOneShot(SpeedupSound, volume);
    }

}
