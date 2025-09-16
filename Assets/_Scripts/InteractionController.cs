using System.Collections;
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
            StartCoroutine(_StartInteraction());
        }

        if (CancelInteraction) 
        {
            CancelInteraction = false;
            StopAllCoroutines();
            tableInteraction.OnCancelInterction();
        }
    }

    IEnumerator _StartInteraction() 
    {
        tableInteraction.OnStartInterction(boxObject.GetComponent<ISetupObjectItem>(), interactionTime);
        yield return new WaitForSeconds(interactionTime);
        tableInteraction.OnFinishInterction();
    }
}
