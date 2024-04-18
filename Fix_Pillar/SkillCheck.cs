using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class SkillCheck : MonoBehaviour
{
    public GameObject skillCheckObj;
    public Pillar pillar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSkillCheck());
        StartCoroutine(EndSkillCheck());
    }

    //랜덤 타임이 되면 스킬 체크 시작
    IEnumerator StartSkillCheck()
    {
        while (true)
        {
            if (pillar != null && pillar.ran != 0 && pillar.isFixing)
            {
                if (pillar.fixTime >= pillar.ran && pillar.isChecked == false)
                {
                    skillCheckObj.SetActive(true);

                    yield return null;
                }
            }
            yield return null;
        }
    }
    
    //5초가 지나면 자동으로 스킬 체크 종료
    IEnumerator EndSkillCheck()
    {
        while (true)
        {
            if (skillCheckObj.activeSelf)
            {
                yield return new WaitForSeconds(5.0f);

                if (skillCheckObj.activeSelf)
                {
                    pillar.isChecked = true;
                    float addTime = Random.Range(pillar.fixTime, pillar.leftTime);

                    pillar.completeTime += addTime;
                    Debug.Log($"시간 초과 complete time {addTime}초 증가");

                    skillCheckObj.SetActive(false);
                    Debug.Log("스킬 체크 종료");
                }
                yield return null;
            }
            yield return null;
        }
    }
}
