using System.Collections;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public BoxObject boxObject;
    public ObjectSetupInteraction tableInteraction;
    public float interactionTime;

    public bool startInteraction;
    public bool cancelInteraction;

    private void OnValidate()
    {
        if (startInteraction) 
        {
            startInteraction = false;
            StartInteraction(interactionTime);
        }

        if (cancelInteraction) 
        {
            cancelInteraction = false;
            StopAllCoroutines();
            tableInteraction.OnCancelInterction();
        }
    }

    public void StartInteraction(float interactionTime) 
    {
        StopAllCoroutines();
        StartCoroutine(_StartInteraction());
    }

    public void CancelInteraction() 
    {
        StopAllCoroutines();
        tableInteraction.OnCancelInterction();
    }
    IEnumerator _StartInteraction() 
    {
        tableInteraction.OnStartInterction(boxObject.GetComponent<ISetupObjectItem>(), interactionTime);
        yield return new WaitForSeconds(interactionTime);
        tableInteraction.OnFinishInterction();
    }
}
