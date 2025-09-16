using UnityEngine;
using DG.Tweening;

public class BoxObject : MonoBehaviour, ISetupObjectItem
{
    [SerializeField]
    private Animation animation;
    [SerializeField]
    private Transform targetPoint;
    public Transform targetTransform => targetPoint;

   

    public void OnCancelInteraction()
    {
        animation.Play("BoxClose");
    }

    public void OnFinishInteraction()
    {
        animation.Play("BoxClose");
    }

    public void OnInitInteraction()
    {
        animation.Play("BoxOpen");
    }

    public void OnObjectHitTargetPosition()
    {
        transform.DOPunchPosition(new Vector3(0, 0.1f, 0), 0.2f); 
    }

    public void OnObjectLeftTargetPosition()
    {
        transform.DOShakeRotation(0.35f,10f);
    }
}
