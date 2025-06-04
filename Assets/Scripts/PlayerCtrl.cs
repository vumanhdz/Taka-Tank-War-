using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] TMP_Text scoreT,maxT;
    [SerializeField] Joystick joystick;
    [SerializeField] float speed , Bspeed, fireRate;
    [SerializeField] Transform Shoot;
    [SerializeField] GameObject Player, Gun,DiePn ;
    [SerializeField] LayerMask EnemyLayer;
    [SerializeField] public Slider slider;

    PoolingObj poolingObj;
    public float detectRadius, Dame,score;

    private Transform targetPlayer;
    private float MoveH, MoveV, nextFireTime;
    private bool isFire;
    int max;
    Audio_Ctrl audio_Ctrl;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        poolingObj = FindObjectOfType<PoolingObj>();
        audio_Ctrl = FindObjectOfType<Audio_Ctrl>();
    }

    void Update()
    {
        max = PlayerPrefs.GetInt("Max");
        if(max < score)
        {
            max = (int)score;
        }
        scoreT.text = score.ToString();
        maxT.text = max.ToString();
        PlayerPrefs.SetInt("Max", max);
        PlayerMove();
        PlayerRotationGun();
    }


    void PlayerMove()
    {
        MoveH = joystick.Horizontal;
        MoveV = joystick.Vertical;
        Vector3 moveDirection = new Vector3(MoveH, 0, MoveV);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        rb.velocity = moveDirection * speed;
    }


    void PlayerRotationGun()
    {
        Collider[] EnemysInRange = Physics.OverlapSphere(transform.position, detectRadius, EnemyLayer);
        if (EnemysInRange.Length > 0)
        {
            isFire = true;
            targetPlayer = EnemysInRange[0].transform;
            Vector3 dir = targetPlayer.position - transform.position;
            dir.y = 0;
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                Gun.transform.rotation = Quaternion.Slerp(Gun.transform.rotation, lookRot, Time.deltaTime * 10f);
            }
        }
        else
        {
/*            isFire = false;
*/            Gun.transform.rotation = Quaternion.Slerp(Gun.transform.rotation, Quaternion.identity, Time.deltaTime * 10f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            slider.value -= 5;
            if(slider.value <= 0)
            {
                DiePn.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void ButtonGunFire()
    {
        if (Time.time >= nextFireTime)
        {
            audio_Ctrl.ShootSound();
            GameObject Bullet = poolingObj.GetObjBullet();
            Bullet.transform.position = Shoot.position;
            Rigidbody rb = Bullet.GetComponent<Rigidbody>();
            rb.velocity = Gun.transform.forward * Bspeed;
            Quaternion lookRotation = Quaternion.LookRotation(rb.velocity);
            Bullet.transform.rotation = lookRotation * Quaternion.Euler(90, 0, 0);
            nextFireTime = Time.time + fireRate;
        }
    }
}
