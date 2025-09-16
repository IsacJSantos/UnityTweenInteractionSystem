using UnityEngine;

public class ObjectSetupInteraction_Output : ObjectSetupInteraction
{
    public override void MoveObject(ObjectSetup objectSetup, float time)
    {
        objectSetup.MoveObject(currentSetupItemInterface.targetTransform, currentSetupItemInterface.targetTransform.position, time, OnObjectHitTargetPosition);     
    }


    protected override void FinishHoldInteraction()
    {
        base.FinishHoldInteraction();
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
        currentSetupItemInterface.OnObjectHitTargetPosition();
        objectSetup.DisableObject();
    }
}
