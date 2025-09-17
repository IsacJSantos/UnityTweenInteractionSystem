
using System;

public class ObjectSetupInteraction_Input : ObjectSetupInteraction
{
    private void Start()
    {
        DisableAllObjects();
    }

    public override void MoveObject(ObjectSetup objectSetup, float duration)
    {
        objectSetup.EnableObject();
        currentSetupItem?.OnObjectLeftTargetPosition();
        objectSetup.MoveObject(currentSetupItem.targetTransform, objectSetup.getInitialPosition, duration, OnObjectHitTargetPosition);
    }

    protected override void CancelInterction()
    {
        base.CancelInterction();
        ResetAllObjects();
        DisableAllObjects();
    }

    protected override void FinishHoldInteraction()
    {
        base.FinishHoldInteraction();
        ToggleInteraction(false);
    }



    private void OnObjectHitTargetPosition(ObjectSetup objectSetup)
    {
        //currentSetupItemInterface.OnObjectHitTargetPosition();
    }


}
