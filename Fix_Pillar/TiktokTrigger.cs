using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;

public class TiktokTrigger : MonoBehaviour
{
    //��ų üũ
    public GameObject skillCheckObj;
    public SkillCheck skillCheck;

    //��ų üũ ��ġ ����
    public bool isMatch = false;

    //�߰��Ǵ� �ð�
    public float addTime = 0f;
    //���ҵǴ� �ð�
    public float removeTime = 0f;

    //�ٲ� ���� �ð�
    public float changedCompleteTime;

    public Pillar pillar;

    //Ȱ��ȭ�� ��
    private void OnEnable()
    {
        pillar = skillCheck.pillar;
        StartCoroutine(OnAndOffSkillCheck());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Range")
        {
            isMatch = true;
            Debug.Log("��ų üũ ����");
        }
    }

    IEnumerator OnAndOffSkillCheck()
    {
        while (true)
        {
            //Ŭ���ϸ� �ݶ��̴� Ȱ��ȭ
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                pillar.isChecked = true;

                StartCoroutine(GoSkillCheck());
            }
            yield return null;
        }
    }

    IEnumerator GoSkillCheck()
    {
        yield return new WaitForSeconds(0.1f);

        //��ġ ����
        if (isMatch)
        {
            pillar.isMatched = true;
            removeTime = Random.Range(pillar.fixTime, pillar.leftTime);

            //changedCompleteTime = pillar.completeTime - removeTime;
            pillar.completeTime -= removeTime;
            Debug.Log($"��ġ ���� complete time {removeTime}�� ����");
        }
        //��ġ ����
        else
        {
            pillar.isMatched = false;
            addTime = Random.Range(pillar.fixTime, pillar.leftTime);


            //changedCompleteTime = pillar.completeTime + addTime;
            pillar.completeTime += addTime;
            Debug.Log($"��ġ ���� complete time {addTime}�� ����");
        }

        yield return new WaitForSeconds(0.5f);

        skillCheckObj.SetActive(false);
    }

    //��Ȱ��ȭ�� ��
    private void OnDisable()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        pillar = null;
    }

}
