using UnityEngine;

// 타워의 기본 동작을 정의하는 추상 클래스
public class Tower : MonoBehaviour
{
    public float detectionRadius = 15f; // 탐지 반경
    public float fireRate = 1f; // 초당 발사 속도
    protected float fireCooldown = 0f; // 발사 대기 시간
    protected bool isActive = false; // 타워 활성화 여부
    private DrawAttackRange attackRangeDrawer;

    public GameObject towerShootPrefab; // 타워 투사체 프리팹

    protected void Start()
    {
        // DrawAttackRange 컴포넌트를 가져옵니다.
        attackRangeDrawer = GetComponent<DrawAttackRange>();

        // 반경에 따라 공격 범위를 업데이트합니다.
        if (attackRangeDrawer != null)
        {
            attackRangeDrawer.UpdateCircle(detectionRadius);
        }
    }

    // 매 프레임마다 호출되는 Unity 메소드
    protected void Update()
    {
        if (!isActive) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown > 0f) return;

        GameObject target = FindClosestEnemy();
        if (target == null) return;

        RotateTowardsTarget(target.transform.position);
        FireTowerShoot(target);
        fireCooldown = 1f / fireRate;
    }

    // 타워를 활성화하는 메소드
    public void ActivateTower() => isActive = true;

    // 가장 가까운 적을 찾는 메소드
    protected GameObject FindClosestEnemy()
    {
        GameObject closestEnemy = null;
        float minDistance = detectionRadius;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                closestEnemy = enemy;
                minDistance = distance;
            }
        }
        return closestEnemy;
    }

    // 타겟을 향해 회전하는 메소드
    protected void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 타워 발사를 정의하는 추상 메소드 (자식 클래스에서 구현)
    protected virtual void FireTowerShoot(GameObject target)
    {
        TowerShoot towerShoot = Instantiate(towerShootPrefab, transform.position, Quaternion.identity)
            .GetComponent<TowerShoot>();

        towerShoot?.SetTarget(target);
    }

    // 디버깅용: 타워의 기본 공격범위를 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
