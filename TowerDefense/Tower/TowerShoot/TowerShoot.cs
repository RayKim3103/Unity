using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public float speed = 10f; // 투사체의 이동 속도
    public float damage = 1.0f; // 투사체의 피해량
    protected Transform target; // 목표 적의 Transform

    // 매 프레임마다 호출되는 Unity 메소드
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();
    }

    // 목표 적을 설정하는 메소드
    public void SetTarget(GameObject target) => this.target = target.transform;

    // 목표를 향해 이동하고, 목표에 도달하면 적에게 피해를 줌
    void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45);
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            OnHitTarget();
        }
    }

    // 목표에 도달했을 때 호출되는 메소드
    protected virtual void OnHitTarget()
    {
        target.GetComponent<AttackEnemy>()?.TakeDamage(damage);
        Destroy(gameObject);
    }
}
