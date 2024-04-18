using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    //�̵��� ����
    [SerializeField]
    private int nextMoveX;
    [SerializeField]
    private int nextMoveY;

    //�� �̵� �ӵ�
    private float moveSpeed = 1.0f;
    private float followSpeed = 3.0f;
    private float currentSpeed;

    //�߰� ���
    public bool isFollow;

    //���� ���� ����
    private bool isAttack;

    //���� ���� �� Ÿ��
    private float attackCoolTime = 3.5f;

    //����Ʈ
    public GameObject enemy_Light;

    //Ÿ��(=�÷��̾�)
    public Transform target;

    [SerializeField]
    private float followRange = 5.0f; //�߰� ���� ����
    private float attackRange = 1.0f; //���� ���� ����

    //�ڷ�ƾ
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
            //�̹��� �ø�
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }
        else if (nextMoveX == -1)
        {
            movePosition = Vector3.left;
            //�̹��� �ø�
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

        //�߰� ���
        float distance = Vector3.Distance(transform.position, target.position);

        //�÷��̾ ��������� ��
        if (distance <= followRange)
        {
            //�߰� ��� ����
            isFollow = true;
            currentSpeed = followSpeed;

            if (distance <= attackRange && isAttack == false)
            {
                StartAttack();
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

            //Ÿ���� �����ʿ� ���� ���(=���������� �̵�)
            if (transform.position.x < target.position.x)
            {
                //�̹��� �ø�
                transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
            }
            //Ÿ���� ���ʿ� ���� ���(=�������� �̵�)
            else if (transform.position.x >= target.position.x)
            {
                //�̹��� �ø�
                transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
            }
        }
        //�־����� �߰� ��� ����
        else
        {
            isFollow = false;
            currentSpeed = moveSpeed;

            transform.position += movePosition * currentSpeed * Time.deltaTime;
        }
    }

    //�̵� ����
    IEnumerator SetDirection()
    {
        //�߰� ��尡 �ƴ� ��쿡�� ������ ����
        if (isFollow == false)
        {
            nextMoveX = Random.Range(-1, 2); //-1�̸� ����, 0�̸� ���߱�, 1�̸� ���������� �̵� 
            nextMoveY = Random.Range(-1, 2); //-1�̸� �Ʒ���, 0�̸� ���߱�, 1�̸� �������� �̵�

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

    //����
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
                //��� ������ ����
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
            //���� �ε����� �̵� ���� �ٽ� ����
            Debug.Log("�浹");

            StopSetDirection();
            StartSetDirection();
        }
        //�÷��̾�� �ε����� �� �и� ����
        else if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector3.zero;
        }
    }
}
