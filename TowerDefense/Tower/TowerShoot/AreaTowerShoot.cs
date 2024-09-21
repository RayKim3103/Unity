using UnityEngine;

public class AreaTowerShoot : TowerShoot
{
    public float areaRadius = 2f; // 폭발 범위

    // 목표에 도달했을 때 호출되는 메소드를 재정의 (override)
    protected override void OnHitTarget()
    {
        // 목표 적의 EnemyMovement 컴포넌트를 가져옴
        EnemyMovement enemy = target.GetComponent<EnemyMovement>();

        // 목표 적이 EnemyMovement 컴포넌트를 가지고 있다면
        if (enemy != null)
        {
            Explosion();
        }

        // 기본 기능을 호출하여 데미지를 입힘
        base.OnHitTarget();
    }

    protected void Explosion()
    {
        // 원점(transform.position)을 중심으로 반경(radius) 안에 있는 콜라이더를 모두 탐색
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, areaRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // 탐색된 콜라이더의 태그가 "Enemy"인 경우
            if (hitCollider.CompareTag("Enemy"))
            {
                // 적에게 데미지를 준다 (가정: 적 오브젝트에 Enemy 스크립트가 붙어 있다고 가정)
                AttackEnemy enemy = hitCollider.GetComponent<AttackEnemy>();
                if (enemy != null && enemy.transform != target)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    // Gizmo를 그려서 반경을 시각적으로 확인할 수 있게 함
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}