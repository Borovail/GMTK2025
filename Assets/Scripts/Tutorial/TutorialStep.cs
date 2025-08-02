using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public interface ITutorialStep
{
    public void StartStep();
    public bool IsStepCompleted();
    public void EndStep();

}

public abstract class ArrowTutorialStep : ITutorialStep
{
    public static Transform Arrow;
    protected Lookable Obj;

    protected Vector3 ArrowPosition;
    protected Quaternion ArrowRotation = Quaternion.identity;
    protected string Text;

    public ArrowTutorialStep(Lookable lookable = null)
    {
        Obj = lookable;
    }

    public virtual void StartStep()
    {
        Arrow.gameObject.SetActive(true);
        Arrow.SetPositionAndRotation(ArrowPosition, ArrowRotation);
        Obj.ObjectLookedAt.AddListener(ObjectLookedAt);
        Obj.ObjectLookedAway.AddListener(ObjectLookedAway);
    }
    protected virtual void ObjectLookedAt()
    {
        PlayerUI.Instance.Text(true, Text);
    }

    protected virtual void ObjectLookedAway()
    {
        PlayerUI.Instance.Text(false);
    }

    public abstract bool IsStepCompleted();

    public virtual void EndStep()
    {
        Arrow.gameObject.SetActive(false);
        PlayerUI.Instance.Text(false);
        Obj.ObjectLookedAt.RemoveListener(ObjectLookedAt);
        Obj.ObjectLookedAway.RemoveListener(ObjectLookedAway);
    }
}


public class GhostTutorialStep : ITutorialStep
{
    private List<UIDraggable> _draggables;
    private IEnumerable<UIDraggable> _draggablesIEnumerable;
    private int index;

    private Transform _target;
    private UIDraggable _currentDraggable;

    private GameObject ghost;

    public GhostTutorialStep(IEnumerable<UIDraggable> graggables, Transform target)
    {
        _draggablesIEnumerable = graggables;
        _target = target;
    }

    public GhostTutorialStep(List<UIDraggable> graggables, Transform target)
    {
        _draggables = graggables;
        _target = target;
    }

    private void CreateGhostDragLoop(float duration = 2f, float visibleAlpha = 0.5f)
    {
        Vector3 startPos = _currentDraggable.transform.position;
        ghost = UnityEngine.Object.Instantiate(
            _currentDraggable.gameObject, startPos,
 _currentDraggable.transform.rotation, _currentDraggable.transform
        );
        ghost.name = _currentDraggable.name + "_Ghost";

        var renderers = ghost.GetComponentsInChildren<Image>();
        foreach (var sr in renderers)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
            sr.raycastTarget = false;
        }

        Sequence seq = DOTween.Sequence().SetTarget(ghost).SetAutoKill(false);

        seq.AppendCallback(() =>
        {
            ghost.transform.position = startPos;
            foreach (var sr in renderers)
            {
                var c = sr.color;
                c.a = Mathf.Clamp01(visibleAlpha);
                sr.color = c;
            }
        });

        seq.Append(ghost.transform
            .DOMove(_target.position, duration)
            .SetEase(Ease.Linear)
        );

        seq.AppendCallback(() =>
        {
            foreach (var sr in renderers)
            {
                var c = sr.color;
                c.a = 0f;
                sr.color = c;
            }
        });

        seq.SetLoops(-1, LoopType.Restart);
    }

    public virtual void StartStep()
    {
        _draggables ??= _draggablesIEnumerable.ToList();
        _currentDraggable = _draggables[index++];
        CreateGhostDragLoop();
    }

    public bool IsStepCompleted()
    {
        if (_currentDraggable.IsDropSuccessful && index < _draggables.Count)
        {
            EndStep();

            StartStep();
            return false;
        }

        return _draggables.Count == index && _currentDraggable.IsDropSuccessful;
    }

    public virtual void EndStep()
    {
        UnityEngine.Object.Destroy(ghost);
        DOTween.Kill(ghost, complete: false);
    }
}
