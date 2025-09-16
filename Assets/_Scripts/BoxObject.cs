using UnityEngine;

public class BoxObject : MonoBehaviour, ISetupObjectItem
{
    [SerializeField]
    private Transform targetPoint;
    public Transform targetTransform => targetPoint;


    public void OnCancelInteraction()
    {
       
    }

    public void OnFinishInteraction()
    {
      
    }

    public void OnInitInteraction()
    {
       
    }

    public void OnObjectHitTargetPosition()
    {
       
    }

    public void OnObjectLeftTargetPosition()
    {
        
    }
}
