using DG.Tweening;
using System;
using UnityEngine;
public class ObjectSetup_Input : ObjectSetup
{
    [SerializeField]
    private Ease scaleEase;
    [SerializeField]
    private Ease jumpEase;
    [SerializeField]
    private Ease moveEase;

    [Space]

    [SerializeField]
    private Vector3 jumpOffset;
    [SerializeField]
    private Vector3 targetPositionOffset;

    [Space]

    [SerializeField]
    private float jumpPower = 0.3f;
    [SerializeField]
    private float scaleDuration = 1.6f;
    [SerializeField]
    private float jumpDuration = 1.4f;
    [SerializeField]
    private float finalMoveDelay = 0.15f;
    [SerializeField]
    private float finalMoveDuration = .5f;

    public override void MoveObject(Transform setupItemTargetPoint, Vector3 finalPosition, float duration, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        MoveObjectIn(setupItemTargetPoint, OnHitTargetPosition);
    }

    private void MoveObjectIn(Transform setupItemTargetPoint, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        if (disableTween)
        {
            EnableObject();
            return;
        }

        SetFirstPersonLayer();

        transform.position = setupItemTargetPoint.position;
        transform.localScale = Vector3.one * minScale;
        transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        Vector3 targetPosition = GetTargetPositionWithOffset();

        DoMoveTween(setupItemTargetPoint, targetPosition, OnHitTargetPosition);

    }

    private Vector3 GetTargetPositionWithOffset()
    {
        Vector3 localInitialPosition = parentTransform.InverseTransformPoint(initialPosition);

        Vector3 resultPosition = new Vector3((localInitialPosition.x + targetPositionOffset.x),
                                             (localInitialPosition.y + targetPositionOffset.y),
                                             (localInitialPosition.z + targetPositionOffset.z));

        return parentTransform.TransformPoint(resultPosition);
    }

    void DoMoveTween(Transform setupItemTargetPoint, Vector3 targetPosition, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        transform.DOScale(initialScale, scaleDuration).SetEase(scaleEase).Play();

        transform.DORotate(initialRotation, 0.8f);

        Vector3 jumpPosition = setupItemTargetPoint.TransformPoint(setupItemTargetPoint.localPosition + jumpOffset);

        Vector3[] waypoints = new Vector3[] { jumpPosition, targetPosition };

        transform.DOPath(waypoints, jumpDuration, PathType.CatmullRom)
                 .OnUpdate(() =>
                 {
                     if (CanChangeToDefaultLayer(targetPosition))
                         SetDefaultLayer();
                 })
                 .OnComplete(() =>
                 {
                     transform.DOMove(initialPosition, finalMoveDuration)
                              .SetEase(moveEase)
                              .OnComplete(() =>
                              {
                                  OnHitTargetPosition?.Invoke(this);
                              })
                              .SetDelay(finalMoveDelay);
                 });
    }

    private bool CanChangeToDefaultLayer(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < 1;
    }

    public override void ResetObject()
    {
        base.ResetObject();
        DisableObject();
    }

    public override float GetTotalTweenDuration()
    {
        return jumpDuration +
               finalMoveDelay +
               finalMoveDuration;
    }
}