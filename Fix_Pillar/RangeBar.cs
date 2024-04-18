using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBar : MonoBehaviour
{
    private void OnEnable()
    {
        RotateRangeBar();
    }

    //랜덤한 범위 생성
    void RotateRangeBar()
    {
        int dir = Random.Range(1, 3);
        if (dir == 1)
        {
            float angle = Random.Range(0f, 40f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
        }
        else if (dir == 2)
        {
            float angle = Random.Range(320f, 360f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
        }
    }
}
