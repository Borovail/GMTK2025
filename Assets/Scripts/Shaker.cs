using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shaker : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private List<Cocktail> _cocktails;
    [HideInInspector] public Cocktail Cocktail;
    private List<Resource> _resources = new();

    private UIDraggable _draggable;

    private void Awake()
    {
        _draggable = GetComponent<UIDraggable>();
        _draggable.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        ShakeAnimation();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggable = eventData.pointerDrag.GetComponent<UIDraggable>();
        _resources.Add(draggable.Resource);
        draggable.DropSuccessful();
    }

    public void ShakeAnimation(float duration = 1f, float magnitude = 0.05f)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Нормализуем прогресс [0..1]
            float progress = elapsed / duration;
            // Делаем затухание (чем ближе конец, тем меньше смещение)
            float damper = 1f - Mathf.Clamp01((progress - 0.5f) * 2f); // пиковая амплитуда в середине

            // случайное смещение в круге радиуса 1
            Vector2 rand = Random.insideUnitCircle * magnitude * damper;
            transform.position = originalPos + (Vector3)rand;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // в конце — сброс позиции
        transform.position = originalPos;

        Shake();
    }

    private void Shake()
    {
        Cocktail = _cocktails.FirstOrDefault(c => new HashSet<Resource>(_resources).SetEquals(c.Recipe));
        if (Cocktail == null) Cocktail = _cocktails[0];
        _draggable.enabled = true;
    }
}
