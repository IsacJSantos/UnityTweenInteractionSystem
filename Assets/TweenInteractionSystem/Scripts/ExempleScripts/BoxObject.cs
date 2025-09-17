using UnityEngine;
using DG.Tweening;

public class BoxObject : MonoBehaviour, ISetupObjectItem
{
    [SerializeField]
    private Animation m_animation;
    [SerializeField]
    private Transform targetPoint;
    public Transform targetTransform => targetPoint;

   

    public void OnCancelInteraction()
    {
        m_animation.Play("BoxClose");
    }

    public void OnFinishInteraction()
    {
        m_animation.Play("BoxClose");
    }

    public void OnInitInteraction()
    {
        m_animation.Play("BoxOpen");
    }

    public void OnObjectHitTargetPosition()
    {
        transform.DOPunchPosition(new Vector3(0, -0.08f, 0), 0.2f); 
    }

    public void OnObjectLeftTargetPosition()
    {
        transform.DOShakeRotation(0.35f,10f);
    }
}
