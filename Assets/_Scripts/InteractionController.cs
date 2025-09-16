using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public BoxObject boxObject;
    public ObjectSetupInteraction tableInteraction;
    public float interactionTime;

    public bool StartInteraction;
    public bool CancelInteraction;

    private void OnValidate()
    {
        if (StartInteraction) 
        {
            StartInteraction = false;
            tableInteraction.OnStartInterction(boxObject.GetComponent<ISetupObjectItem>(), interactionTime);
        }

        if (CancelInteraction) 
        {
            CancelInteraction = false;
            tableInteraction.OnCancelInterction();
        }
    }
}
