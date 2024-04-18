using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.Rendering;
using UnityEngine;

public class Tiktok : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    private int a;

    //바 이동 방향
    private bool moveLeft;

    //각도
    private float maxLeftAngle = 44.0f;
    private float maxRightAngle = 317.0f;

    private void OnEnable()
    {
        //시작 각도 설정하기

        moveLeft = false;  
        StartCoroutine(RotateTiktok());
    }

    //좌우로 회전
    IEnumerator RotateTiktok()
    {
        //좌우로 회전
        while (!Input.GetMouseButtonDown(0))
        {
            if (moveLeft == false)
            {
                if (0 < transform.eulerAngles.z || transform.eulerAngles.z < maxLeftAngle)
                {
                    a = -1;

                    if (maxRightAngle - 1 < transform.eulerAngles.z && transform.eulerAngles.z < maxRightAngle)
                    {
                        moveLeft = true;
                    }
                }
            }
            else
            {
                if (360f > transform.eulerAngles.z || transform.eulerAngles.z > maxRightAngle)

                    a = 1;
                if (maxLeftAngle - 1 < transform.eulerAngles.z && transform.eulerAngles.z < maxLeftAngle)
                {
                    moveLeft = false;
                }
            }

            transform.Rotate(0f, 0f, 10f * a * Time.deltaTime * speed);

            yield return null;
        }
    }   

}
