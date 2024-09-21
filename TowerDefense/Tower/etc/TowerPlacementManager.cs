using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

    /* 
    Physics.Raycast(ray, out RaycastHit hit)는 Unity에서 광선을 발사하여 광선이 충돌한 첫 번째 객체를 감지

    Ray 생성: ray는 Ray 구조체로, 광선의 시작점과 방향을 정의합니다. 무한한 직선으로 발사

    Raycast 호출: Physics.Raycast(ray, out RaycastHit hit)는 ray를 사용하여 광선을 발사하고, 광선이 충돌한 첫 번째 객체를 감지

    ray: 발사할 광선입니다.
    out RaycastHit hit: 광선이 충돌한 객체에 대한 정보를 저장할 RaycastHit 구조 / out 키워드를 사용하여 이 변수에 함수 내부에서 값을 할당

    광선이 충돌한 경우: Physics.Raycast 메서드는 true를 반환하고, hit 변수에 충돌한 객체에 대한 정보가 저장
    광선이 아무것도 충돌하지 않은 경우: Physics.Raycast 메서드는 false를 반환하고, hit 변수에는 아무런 정보도 저장 안됨
     - RaycastHit 구조체의 정보 -

        hit.point: 충돌 지점의 세계 좌표.
        hit.normal: 충돌 지점의 표면 법선 벡터.
        hit.distance: 광선 시작점과 충돌 지점 사이의 거리.
        hit.collider: 충돌한 객체의 콜라이더.

    - 전체 동작 -
    위 코드는 광선을 발사하여 어떤 객체에 충돌했는지 확인하고, 충돌한 객체에 대한 정보를 hit 변수에 저장하는 역할을 함
    이를 통해 광선이 무엇과 충돌했는지, 그 충돌 지점이 어디인지 등을 알 수 있음
    */

public class TowerPlacementManager : MonoBehaviour
{
    // Inspector에서 할당할 타워 프리팹
    public GameObject towerPrefab;
    private CheckTowerCollision checkTowerCollision;

    // 타워를 설치 중인지 여부를 나타내는 불리언 변수
    [HideInInspector]
    public bool isPlacing = false;

    // 타워를 설치할 수 있는 레이어를 지정할 레이어 마스크
    // private LayerMask placeableLayer; 

    // private bool isNotTowerPlaced = true;

    // // 현재 위치를 따라다니는 임시 타워 객체
    private GameObject currentTower;

    // public string towerButtonTag = "TowerButton"; // TowerButton 오브젝트의 태그
    // private GameObject towerButton;
    // private TowerTrigger towerTrigger;

    private void Awake() 
    {
        checkTowerCollision = towerPrefab.GetComponent<CheckTowerCollision>();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    // 정해진 시간간격마다 호출되는 Unity 메소드
    void FixedUpdate()
    {
        if(isPlacing)
        {
            // HandleTouches();
            FollowMouse();
            Debug.Log("FollowMouse Called!");

            // 마우스 왼쪽 버튼 클릭 시 타워 설치
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
    }

    // 마우스를 따라 타워를 이동시키는 메소드
    private void FollowMouse()
    {
        // 마우스 커서의 스크린 좌표를 가져옴
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // 2D 에서 z값은 10으로 해야 최대 레이어 9보다 앞에 있음

        // 스크린 좌표를 월드 좌표로 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 현재 타워의 위치를 마우스 커서 위치로 설정
        currentTower.transform.position = worldPosition;
    }

    // void HandleTouches()
    // {
    //     foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
    //     {
    //         Vector3 touchPosition = touch.screenPosition;
    //         touchPosition.z = 10f; // z값을 설정하여 2D 평면에서의 위치로 변환

    //         Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

    //         switch (touch.phase)
    //         {
    //             case UnityEngine.InputSystem.TouchPhase.Began:
    //                 // 터치가 시작된 경우 처리
    //                 break;

    //             case UnityEngine.InputSystem.TouchPhase.Moved:
    //             case UnityEngine.InputSystem.TouchPhase.Stationary:
    //                 // 터치가 이동 중이거나 정지 상태인 경우 타워 이동
    //                 FollowTouch(worldPosition, touch.finger.index);
    //                 break;

    //             case UnityEngine.InputSystem.TouchPhase.Ended:
    //             case UnityEngine.InputSystem.TouchPhase.Canceled:
    //                 // 터치가 끝나거나 취소된 경우 처리
    //                 break;
    //         }
    //     }
    // }

    // private void FollowTouch(Vector3 worldPosition, int touchIndex)
    // {
    //     currentTower.transform.position = worldPosition;
    // }

    // 타워 설치 시작 메소드
    public void StartPlacingTower()
    {
        // 타워 설치 모드 활성화
        isPlacing = true;

        // 타워 프리팹을 인스턴스화하여 현재 타워로 설정
        currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);

        // 타워의 Renderer 컴포넌트의 색상을 반투명하게 설정
        currentTower.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
    }

    public void DestroyPlacingTower()
    {
        // 타워 설치 모드 활성화
        isPlacing = false;
        Destroy(currentTower);
    }

    private void PlaceTower()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 10f;

        // 충돌 감지를 위한 Raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // 설치 가능한 위치에 클릭했는지 확인
        if (hit.collider != null && hit.collider.CompareTag("PlaceTower"))
        {
            Debug.Log("hit.collider: " + hit.collider);
            
            checkTowerCollision.isPlaced = true;

            // 설치 가능 위치
            Vector3 placementPosition = hit.collider.bounds.center; // 클릭한 영역의 중앙 위치
            
            // 새로운 타워를 설치하고 색상을 원래 색상으로 설정
            GameObject placedTower = Instantiate(towerPrefab, placementPosition, Quaternion.identity);
            placedTower.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            placedTower.name = "PlacedTower";

            // 설치된 타워의 Tower 스크립트를 가져와서 활성화
            // 간략하게 코드 짜는 법: Null-conditional operator(?.)를 사용하여 Tower 스크립트가 있는 경우에만 ActivateTower를 호출 (여기서는 안 씀)
            Tower towerComponent = placedTower.GetComponent<Tower>();
            if (towerComponent != null)
            {
                towerComponent.ActivateTower();
            }

            // 현재 위치를 따라다니는 임시 타워 삭제 및 참조 해제
            Destroy(currentTower);

            Debug.Log("NewTower Placed & currentTower Destroyed");
            
            // 타워 설치 모드 비활성화
            isPlacing = false;
        }        
    }
}




    // // 타워를 설치하는 메소드
    // private void PlaceTower()
    // {
    //     // 마우스 위치로부터 Ray를 생성
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //     // Raycast를 수행하여 충돌 검사
    //     if (Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         // 충돌한 객체의 레이어가 placeableLayer에 속하는지 검사
    //         if (((1 << hit.collider.gameObject.layer) & placeableLayer) != 0)
    //         {
    //             // 설치 가능 위치
    //             Vector3 placementPosition = hit.collider.bounds.center; // 클릭한 영역의 중앙 위치

    //             // 새로운 타워를 설치하고 색상을 원래 색상으로 설정
    //             GameObject placedTower = Instantiate(towerTrigger.towerPrefab, placementPosition, Quaternion.identity);
    //             placedTower.GetComponent<Renderer>().material.color = Color.white;

    //             // 설치된 타워의 Tower 스크립트를 가져와서 활성화
    //             // Null-conditional operator(?.)를 사용하여 Tower 스크립트가 있는 경우에만 ActivateTower를 호출
    //             placedTower.GetComponent<Tower>()?.ActivateTower(); // Null-conditional operator 사용

    //             // 현재 위치를 따라다니는 임시 타워 삭제 및 참조 해제
    //             Destroy(gameObject);

    //             // 타워 설치 모드 비활성화
    //             towerTrigger.isPlacing = false;
    //         }
    //         else
    //         {
    //             // 설치할 수 없는 위치일 때 로그 출력
    //             Debug.Log("설치할 수 없는 위치입니다.");
    //         }
    //     }
    // }



    // // 타워가 설치 가능한 위치인지 검사하는 메소드
    // private void CheckPlacement()
    // {    
    //     // 현재 타워가 null이면 반환
    //     if (currentTower == null)
    //     {
    //         return;
    //     }

    //     // 마우스 위치로부터 Ray를 생성
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //     // Raycast를 수행하여 충돌 검사
    //     if (Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         // 충돌한 객체의 레이어가 placeableLayer에 속하는지 검사하여 색상 설정
    //         // layerMask & placeableLayer가 참일 때 if문 실행
    //         currentTower.GetComponent<Renderer>().material.color = 
    //             (((1 << hit.collider.gameObject.layer) & placeableLayer) != 0) ? Color.blue : Color.red;
    //     }
    // }
