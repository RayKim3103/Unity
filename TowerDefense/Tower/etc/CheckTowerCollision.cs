using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTowerCollision : MonoBehaviour
{
    public bool isPlaced = false;
    public bool isBlue = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlaced != true)
        {
            // 충돌이 시작되면 색상을 파란색으로 변경합니다.
            if (other.gameObject.tag == "PlaceTower")
            {
                // 마우스 위치를 월드 좌표로 변환
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 10f;

                // 충돌 감지를 위한 Raycast
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("PlaceTower"))
                {
                    isBlue = true;
                    gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 0.5f);
                    Debug.Log("OnTriggerEnter!! & ray hitted!! : Tower Color is BLUE");
                }
                else
                {
                    isBlue = false;
                    gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
                }
            }
        }

    }

        void OnTriggerStay2D(Collider2D other)
    {
        if (isPlaced != true)
        {
            // 충돌 중이면 색상을 파란색으로 유지합니다.
            if (other.gameObject.tag == "PlaceTower")
            {
                // 마우스 위치를 월드 좌표로 변환
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 10f;

                // 충돌 감지를 위한 Raycast
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("PlaceTower"))
                {
                    gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 0.5f);
                }
                else
                {
                    isBlue = false;
                    gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isPlaced != true)
        {
            // 충돌이 끝나면 원래 색상으로 되돌립니다.
            if (other.gameObject.tag == "PlaceTower")
            {
                // 마우스 위치를 월드 좌표로 변환
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 10f;

                // 충돌 감지를 위한 Raycast
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (!hit.collider.CompareTag("PlaceTower"))
                {
                    isBlue = false;
                    gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
                }
            }

        }
    }
}
