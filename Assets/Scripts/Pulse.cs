using DG.Tweening;
using UnityEngine;

public class Pulse : MonoBehaviour
{

    [Tooltip("Во сколько раз увеличивать размер")]
    [SerializeField] private float pulseScale = 1.2f;

    [Tooltip("Длительность одного «пульса» туда-обратно (секунды)")]
    [SerializeField] private float pulseDuration = 0.6f;

    [Tooltip("Кривая ускорения-плавления")]
    [SerializeField] private Ease easeType = Ease.InOutSine;

    private Tween pulseTween;

    private void OnEnable()
    {
        pulseTween = transform
            .DOScale(transform.localScale * pulseScale, pulseDuration / 2f)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        pulseTween.Kill();
    }
}
