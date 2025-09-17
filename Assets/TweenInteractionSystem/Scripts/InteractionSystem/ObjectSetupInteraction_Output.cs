using UnityEngine;

public class ObjectSetupInteraction_Output : ObjectSetupInteraction
{
    public override void MoveObject(ObjectSetup objectSetup, float duration)
    {
        objectSetup.MoveObject(currentSetupItem.targetTransform, currentSetupItem.targetTransform.position, duration, OnObjectHitTargetPosition);     
    }


    protected override void FinishHoldInteraction()
    {
        base.FinishHoldInteraction();
        DisableAllObjects();
        ToggleInteraction(false);
    }


    protected override void CancelInterction()
    {
        base.CancelInterction();
        ResetAllObjects();
        EnableAllObjects();     
    }

    private void OnObjectHitTargetPosition(ObjectSetup objectSetup)
    {
        currentSetupItem.OnObjectHitTargetPosition();
        objectSetup.DisableObject();
    }
}
