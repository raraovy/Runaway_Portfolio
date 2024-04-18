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

    //���� Ÿ���� �Ǹ� ��ų üũ ����
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
    
    //5�ʰ� ������ �ڵ����� ��ų üũ ����
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
                    Debug.Log($"�ð� �ʰ� complete time {addTime}�� ����");

                    skillCheckObj.SetActive(false);
                    Debug.Log("��ų üũ ����");
                }
                yield return null;
            }
            yield return null;
        }
    }
}
