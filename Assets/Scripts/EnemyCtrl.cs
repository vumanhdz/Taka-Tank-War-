using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed = 3f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float detectRadius = 10f;
    [SerializeField] float attackRadius = 3f;
    [SerializeField] Slider slider;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject gun, model, smock, expl, die,sHP;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float gravityForce = 30f;
    PlayerCtrl playerCtrl;
    Audio_Ctrl audio_Ctrl;
    private Transform targetPlayer;
    private float nextFireTime;
    private float Hp;
    private bool isFiring;
    private Vector3 moveDir;
    private bool isMoving = true;
    PoolingObj pooling;
    private Coroutine randomMoveCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        randomMoveCoroutine = StartCoroutine(RandomMove());
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        pooling = FindObjectOfType<PoolingObj>();
        audio_Ctrl = FindObjectOfType<Audio_Ctrl>();
        Hp = slider.value;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Vector3 velocity = rb.velocity;

        velocity.y += -gravityForce * Time.fixedDeltaTime;

        rb.velocity = velocity;
    }

    void Update()
    {
        if (isMoving)
        {
            DetectPlayer();
            HandleMovement();
            RotateGunToTarget();
            TryShoot();
        }
    }

    void DetectPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
        if (players.Length > 0)
        {
            targetPlayer = players[0].transform;

            if (randomMoveCoroutine != null)
            {
                StopCoroutine(randomMoveCoroutine);
                randomMoveCoroutine = null;
            }
        }
        else
        {
            targetPlayer = null;

            if (randomMoveCoroutine == null)
                randomMoveCoroutine = StartCoroutine(RandomMove());
        }
    }

    void HandleMovement()
    {
        if (targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, targetPlayer.position);

            if (distance > attackRadius)
            {
                Vector3 dir = (targetPlayer.position - transform.position).normalized;
                dir.y = 0;
                rb.velocity = dir * speed;

                if (dir.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * 10f);
                }

                isFiring = false;
            }
            else
            {
                rb.velocity = Vector3.zero;
                isFiring = true;
            }
        }
        else
        {
            rb.velocity = moveDir * speed;

            if (moveDir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(moveDir);
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, rot, Time.deltaTime * 10f);
            }

            isFiring = false;
        }
    }


    IEnumerator RandomMove()
    {
        while (true)
        {
            float delay = Random.Range(1f, 2.5f);
            yield return new WaitForSeconds(delay);

            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        }
    }

    void RotateGunToTarget()
    {
        if (targetPlayer != null)
        {
            Vector3 dir = targetPlayer.position - gun.transform.position;
            dir.y = 0;
            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, lookRot, Time.deltaTime * 10f);
            }
        }
        else
        {
            gun.transform.localRotation = Quaternion.Slerp(gun.transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }
    }

    void TryShoot()
    {
        if (isFiring && Time.time >= nextFireTime)
        {
            GameObject Bullet = pooling.GetObjBulletE();
            Bullet.transform.position = shootPoint.position;
            Rigidbody rb = Bullet.GetComponent<Rigidbody>();
            rb.velocity = gun.transform.forward * 10;
            Quaternion lookRotation = Quaternion.LookRotation(rb.velocity);
            Bullet.transform.rotation = lookRotation * Quaternion.Euler(90, 0, 0);
            nextFireTime = Time.time + fireRate;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;
            moveDir = Vector3.Reflect(moveDir, normal).normalized;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            slider.value -= playerCtrl.Dame;
            if(slider.value <= 0)
            {
                audio_Ctrl.ExploiSound();
                playerCtrl.slider.value += 5f; ;
                playerCtrl.score++;
                isMoving = false;
                expl.SetActive(true);
                die.SetActive(true);
                smock.SetActive(true);
                model.SetActive(false);
                rb.isKinematic = true;
                gameObject.layer = 0;
                slider.value = Hp;
                sHP.SetActive(false);
                StartCoroutine(Repool());
            }
        }
    }
    IEnumerator Repool()
    {
        yield return new WaitForSeconds(10f);
        isMoving = true;
        expl.SetActive(false);
        die.SetActive(false);
        smock.SetActive(false);
        model.SetActive(true);
        rb.isKinematic = false;
        gameObject.layer = 7;
        slider.value = Hp;
        sHP.SetActive(true);
        pooling.ReturnObjEnemy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
