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

    //�� �̵� ����
    private bool moveLeft;

    //����
    private float maxLeftAngle = 44.0f;
    private float maxRightAngle = 317.0f;

    private void OnEnable()
    {
        //���� ���� �����ϱ�

        moveLeft = false;  
        StartCoroutine(RotateTiktok());
    }

    //�¿�� ȸ��
    IEnumerator RotateTiktok()
    {
        //�¿�� ȸ��
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
