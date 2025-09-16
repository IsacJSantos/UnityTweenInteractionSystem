
using System;

public class ObjectSetupInteraction_Input : ObjectSetupInteraction
{
    private void Start()
    {
        DisableAllObjects();
    }

    public override void MoveObject(ObjectSetup objectSetup, float time)
    {
        objectSetup.EnableObject();
        currentSetupItemInterface?.OnObjectLeftTargetPosition();
        objectSetup.MoveObject(currentSetupItemInterface.targetTransform, objectSetup.getInitialPosition, time, OnObjectHitTargetPosition);
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
