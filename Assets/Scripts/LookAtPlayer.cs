using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LookAtPlayer : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = FindAnyObjectByType<PlayerInteractor>().transform;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // 1) вычисляем вектор до целиF
        Vector3 dir = target.position - transform.position;
        // 2) обнуляем вертикальную составляющую, чтобы спрайт не наклонялся вверх/вниз
        dir.y = 0;
        if (dir.sqrMagnitude < 0.0001f)
            return;

        // 3) поворачиваем прямо лицом к цели
        transform.rotation = Quaternion.LookRotation(dir);
        // Если спрайт «задом» повернулся, можно добавить +180°:
        transform.Rotate(0, 180f, 0);
    }
}
