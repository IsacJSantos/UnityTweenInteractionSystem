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

    public override void MoveObject(Transform setupItemTransform, Vector3 targetPosition, float time, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        MoveObjectIn(setupItemTransform, OnHitTargetPosition);
    }

    private void MoveObjectIn(Transform setupItemTransform, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        if (disableTween)
        {
            EnableObject();
            return;
        }

        SetLayer(true);

        transform.position = setupItemTransform.position;
        transform.localScale = Vector3.one * minScale;
        transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));

        Vector3 targetPosition = GetTargetPositionWithOffset();

        DoMoveTween(setupItemTransform, targetPosition, OnHitTargetPosition);

    }

    private Vector3 GetTargetPositionWithOffset()
    {
        Vector3 localInitialPosition = parentTransform.InverseTransformPoint(initialPosition);

        Vector3 resultPosition = new Vector3((localInitialPosition.x + targetPositionOffset.x),
                                             (localInitialPosition.y + targetPositionOffset.y),
                                             (localInitialPosition.z + targetPositionOffset.z));

        return parentTransform.TransformPoint(resultPosition);
    }

    void DoMoveTween(Transform setupItemTransform, Vector3 targetPosition, Action<ObjectSetup> OnHitTargetPosition = null)
    {
        transform.DOScale(initialScale, scaleDuration).SetEase(scaleEase).Play();

        transform.DORotate(initialRotation, 0.8f);

        Vector3 jumpPosition = setupItemTransform.TransformPoint(setupItemTransform.localPosition + jumpOffset);

        Vector3[] waypoints = new Vector3[] { jumpPosition, targetPosition };

        transform.DOPath(waypoints, jumpDuration, PathType.CatmullRom)
                 .OnUpdate(() =>
                 {
                     if (Vector3.Distance(transform.position, targetPosition) < 1)
                         SetLayer(false);
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

    public override void ResetObject()
    {
        base.ResetObject();
        DisableObject();
    }
}