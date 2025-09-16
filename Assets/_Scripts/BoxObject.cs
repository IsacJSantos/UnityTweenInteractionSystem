using UnityEngine;

public class BoxObject : MonoBehaviour, ISetupObjectItem
{
    [SerializeField]
    private Transform itemPoint;
    public Transform targetTransform => itemPoint;


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
