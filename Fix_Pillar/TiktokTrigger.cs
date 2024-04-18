using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;

public class TiktokTrigger : MonoBehaviour
{
    //스킬 체크
    public GameObject skillCheckObj;
    public SkillCheck skillCheck;

    //스킬 체크 매치 여부
    public bool isMatch = false;

    //추가되는 시간
    public float addTime = 0f;
    //감소되는 시간
    public float removeTime = 0f;

    //바뀐 수리 시간
    public float changedCompleteTime;

    public Pillar pillar;

    //활성화될 때
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
            Debug.Log("스킬 체크 성공");
        }
    }

    IEnumerator OnAndOffSkillCheck()
    {
        while (true)
        {
            //클릭하면 콜라이더 활성화
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

        //매치 성공
        if (isMatch)
        {
            pillar.isMatched = true;
            removeTime = Random.Range(pillar.fixTime, pillar.leftTime);

            //changedCompleteTime = pillar.completeTime - removeTime;
            pillar.completeTime -= removeTime;
            Debug.Log($"매치 성공 complete time {removeTime}초 감소");
        }
        //매치 실패
        else
        {
            pillar.isMatched = false;
            addTime = Random.Range(pillar.fixTime, pillar.leftTime);


            //changedCompleteTime = pillar.completeTime + addTime;
            pillar.completeTime += addTime;
            Debug.Log($"매치 실패 complete time {addTime}초 증가");
        }

        yield return new WaitForSeconds(0.5f);

        skillCheckObj.SetActive(false);
    }

    //비활성화될 때
    private void OnDisable()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        pillar = null;
    }

}
