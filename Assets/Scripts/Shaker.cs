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

    public UIDraggable Draggable;
    public bool Shaked;

    private Coroutine _coroutine;

    private void Awake()
    {
        Draggable = GetComponent<UIDraggable>();
        Draggable.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || eventData.button != PointerEventData.InputButton.Left) return;

        ShakeAnimation();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<UIDraggable>(out var draggable))
        {
            _resources.Add(draggable.Resource);
            draggable.DropSuccessful();
            Shaked = false;
        }
    }

    public void ShakeAnimation(float duration = 1f, float magnitude = 0.05f)
    {
        if (!Shaked && _coroutine == null)
            _coroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
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
        Draggable.enabled = true;
        Shaked = true;
        _coroutine = null;
    }
}
