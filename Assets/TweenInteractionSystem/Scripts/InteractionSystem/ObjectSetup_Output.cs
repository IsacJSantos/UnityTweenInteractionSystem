using DG.Tweening;
using System;
using UnityEngine;

public class ObjectSetup_Output : ObjectSetup
{
    [Space]

    [SerializeField]
    private Ease jumpEase;
    [SerializeField]
    private Ease scaleEase;
    [SerializeField]
    private float jumpPower = 0.3f;
    [SerializeField]
    private Vector3 ratation;

    private float currentTweenDuration;
    public override void MoveObject(Transform setupItemTargetPoint, Vector3 finalPosition, float duration, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        currentTweenDuration = duration;
        MoveObjectOut(finalPosition, duration, OnHitTargetPosition);
    }

    private void MoveObjectOut(Vector3 targetPosition, float time, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        ToggleColliders(false);

        if (disableTween)
        {
            DisableObject();
            return;
        }

        SetFirstPersonLayer();

        transform.DOScale(minScale, time).SetEase(scaleEase);

        transform.DOJump(targetPosition, jumpPower, 1, time)
            .SetEase(jumpEase)
            .OnComplete(() =>
            {
                OnHitTargetPosition?.Invoke(this);
            }).Play();

        transform.DOLocalRotate(ratation, time);
    }

    public override void ResetObject()
    {
        base.ResetObject();
        gameObject.SetActive(true);
        ToggleColliders(true);
    }

    public override float GetTotalTweenDuration()
    {
       return currentTweenDuration;
    }
}
