using UnityEngine;

public interface ISetupObjectItem
{
    public Transform targetTransform { get; }

    public void OnInitInteraction();
    public void OnFinishInteraction();
    public void OnCancelInteraction();
    public void OnObjectHitTargetPosition();
    public void OnObjectLeftTargetPosition();
}
