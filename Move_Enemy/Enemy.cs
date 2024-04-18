using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    //이동할 방향
    [SerializeField]
    private int nextMoveX;
    [SerializeField]
    private int nextMoveY;

    //적 이동 속도
    private float moveSpeed = 1.0f;
    private float followSpeed = 3.0f;
    private float currentSpeed;

    //추격 모드
    public bool isFollow;

    //적의 공격 여부
    private bool isAttack;

    //적의 공격 쿨 타임
    private float attackCoolTime = 3.5f;

    //라이트
    public GameObject enemy_Light;

    //타겟(=플레이어)
    public Transform target;

    [SerializeField]
    private float followRange = 5.0f; //추격 시작 범위
    private float attackRange = 1.0f; //공격 시작 범위

    //코루틴
    IEnumerator coroutineDirection;
    IEnumerator coroutineAttack;

    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = moveSpeed;
        StartSetDirection();
        StartCoroutine(GameOver());
    }

    private void FixedUpdate()
    {
        if (GameManager.isDie == true)
            return;

        Move();
    }

    void Move()
    {
        Vector3 movePosition = Vector3.zero;

        if (nextMoveX == 1)
        {
            movePosition = Vector3.right;
            //이미지 플립
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }
        else if (nextMoveX == -1)
        {
            movePosition = Vector3.left;
            //이미지 플립
            transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
        }

        if (nextMoveY == 1)
        {
            movePosition = Vector3.up;
        }
        else if (nextMoveY == -1)
        {
            movePosition = Vector3.down;
        }

        //추격 모드
        float distance = Vector3.Distance(transform.position, target.position);

        //플레이어가 가까워졌을 때
        if (distance <= followRange)
        {
            //추격 모드 시작
            isFollow = true;
            currentSpeed = followSpeed;

            if (distance <= attackRange && isAttack == false)
            {
                StartAttack();
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

            //타겟이 오른쪽에 있을 경우(=오른쪽으로 이동)
            if (transform.position.x < target.position.x)
            {
                //이미지 플립
                transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
            }
            //타겟이 왼쪽에 있을 경우(=왼쪽으로 이동)
            else if (transform.position.x >= target.position.x)
            {
                //이미지 플립
                transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
            }
        }
        //멀어지면 추격 모드 종료
        else
        {
            isFollow = false;
            currentSpeed = moveSpeed;

            transform.position += movePosition * currentSpeed * Time.deltaTime;
        }
    }

    //이동 방향
    IEnumerator SetDirection()
    {
        //추격 모드가 아닐 경우에만 방향을 설정
        if (isFollow == false)
        {
            nextMoveX = Random.Range(-1, 2); //-1이면 왼쪽, 0이면 멈추기, 1이면 오른쪽으로 이동 
            nextMoveY = Random.Range(-1, 2); //-1이면 아래쪽, 0이면 멈추기, 1이면 위쪽으로 이동

            yield return new WaitForSeconds(3.0f);

            StartSetDirection();
        }
    }

    void StartSetDirection()
    {
        coroutineDirection = SetDirection();
        StartCoroutine(SetDirection());
    }

    void StopSetDirection()
    {
        if (coroutineDirection != null)
        {
            StopCoroutine(coroutineDirection);
        }
    }

    //공격
    IEnumerator Attack()
    {
        if (!isFollow)
            yield return null;

        isAttack = true;
        animator.SetTrigger("AttackTrigger");

        yield return new WaitForSeconds(attackCoolTime);
        isAttack = false;

        yield return null;
    }

    void StartAttack()
    {
        coroutineAttack = Attack();
        StartCoroutine(Attack());
    }

    void StopAttack()
    {
        if (coroutineAttack != null)
        {
            StopCoroutine(coroutineAttack);
        }
    }

    IEnumerator GameOver()
    {
        while (true)
        {
            if (GameManager.isDie == true)
            {
                //모든 움직임 멈춤
                StopSetDirection();
                StopAttack();

                yield return null;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //벽에 부딪히면 이동 방향 다시 설정
            Debug.Log("충돌");

            StopSetDirection();
            StartSetDirection();
        }
        //플레이어와 부딪혔을 때 밀림 방지
        else if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector3.zero;
        }
    }
}
