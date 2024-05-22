using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float damage;
    private float speed;

    public void Seek(Transform _target, float _damage, float _speed)
    {
        target = _target;
        damage = _damage;
        speed = _speed;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        UnitCombat targetHealth = target.GetComponent<UnitCombat>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
