using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public abstract class ObjectSetupInteraction : MonoBehaviour
{
    [SerializeField]
    private InteractionType interactionType;
    [SerializeField]
    private InteractionTweenType tweenType;
    [SerializeField]
    [Tooltip("The time between objects if the Tween Type is setted to Fixed Time")]
    protected float tweenFixedTime;
    [SerializeField]
    protected ObjectSetup[] objectSetups;

    [Space(10)]

    [SerializeField]
    [Tooltip("The interaction signs in this setup interaction")]
    private List<Transform> interactionSigns;
    private Vector3 interactionSignDefaultScale;
    [SerializeField]
    private Ease interactionSignsTweenEase = Ease.OutBack;
    [SerializeField]
    protected GameObject setupSignParent;


    protected ISetupObjectItem currentSetupItem;

    protected const float START_DELAY = .8f;
    protected const float TIME_PER_OBJECT_OFFSET = .03f;

    private void Awake()
    {
        RemoveNullFieldsFromSignList();

        if (HasInstalationSigns())
            interactionSignDefaultScale = interactionSigns[0].localScale;
    }

    protected virtual void StartInteraction(ISetupObjectItem setupObjectItemInterface, float interactionTime)
    {
        currentSetupItem = setupObjectItemInterface;

        float timePerObject = GetTimePerObject(interactionTime);

        StartCoroutine(_MoveObjects(timePerObject));
    }

    private float GetTimePerObject(float interactionTime)
    {
        if (tweenType == InteractionTweenType.DependsOnInteractionTime)
            return ((interactionTime - START_DELAY) / objectSetups.Length) - TIME_PER_OBJECT_OFFSET;
        else
            return tweenFixedTime;
    }

    #region Interaction Methods
    public void OnStartInterction(ISetupObjectItem setupObjectItemInterface, float interactionTime)
    {
        StartInteraction(setupObjectItemInterface, interactionTime);
    }

    public void OnCancelInterction()
    {
        CancelInterction();
    }
    protected virtual void CancelInterction()
    {
        StopAllCoroutines();

        if (HasInstalationSigns())
            ResetSigns();

        currentSetupItem?.OnCancelInteraction();
        currentSetupItem = null;
    }

    public void OnFinishInterction()
    {
        FinishHoldInteraction();
    }
    protected virtual void FinishHoldInteraction()
    {
        StopAllCoroutines();
        currentSetupItem?.OnFinishInteraction();
        currentSetupItem = null;
    }
    #endregion

    public void ResetAllObjects()
    {
        for (int i = 0; i < objectSetups.Length; i++)
        {
            objectSetups[i].ResetObject();
        }
    }

    public void EnableAllObjects()
    {
        for (int i = 0; i < objectSetups.Length; i++)
        {
            objectSetups[i].EnableObject();
        }
    }

    public void DisableAllObjects()
    {
        for (int i = 0; i < objectSetups.Length; i++)
        {
            objectSetups[i].DisableObject();
        }
    }

    public void ToggleInteraction(bool active)
    {
        // Do something
    }

    protected IEnumerator _MoveObjects(float timePerObject)
    {
        currentSetupItem.OnInitInteraction();

        OnStartMovingObjects();

        yield return new WaitForSeconds(START_DELAY);

        ObjectSetup lastObjectSetup = null;
        do
        {
            for (int i = 0; i < objectSetups.Length; i++)
            {
                lastObjectSetup = objectSetups[i];
                MoveObject(lastObjectSetup, timePerObject);
                yield return new WaitForSeconds(timePerObject);
            }

            if (interactionType == InteractionType.Loop)
            {
                yield return new WaitForSeconds(lastObjectSetup.GetTotalTweenDuration());
                ResetAllObjects();
            }

        } while (interactionType == InteractionType.Loop);
    }

    public virtual void OnStartMovingObjects()
    {
        if (HasInstalationSigns())
            StartCoroutine(_HideSigns());
    }

    public abstract void MoveObject(ObjectSetup objectSetup, float duration);

    #region Interaction Signs
    protected bool HasInstalationSigns()
    {
        return interactionSigns.Count > 0;
    }

    IEnumerator _HideSigns()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(.5f);

        foreach (var sign in interactionSigns)
        {
            if (sign != null)
                sign.DOScale(Vector3.zero, 0.5f).SetEase(interactionSignsTweenEase).Play();
            yield return waitForSeconds;
        }
    }

    protected void ResetSigns()
    {
        foreach (var sign in interactionSigns)
        {
            if (sign != null)
            {
                sign.DOKill();
                sign.localScale = interactionSignDefaultScale;
            }
        }
    }

    private void RemoveNullFieldsFromSignList()
    {
        List<Transform> notNullSigns = new List<Transform>();
        int count = interactionSigns.Count;
        for (int i = 0; i < count; i++)
        {
            if (interactionSigns[i] != null)
                notNullSigns.Add(interactionSigns[i]);
        }
        interactionSigns = notNullSigns;
    }
    #endregion


}
[System.Serializable]
public enum InteractionType
{
    /// <summary>
    /// Plays the animation one time
    /// </summary>
    Once,
    /// <summary>
    /// Plays the animation in a loop while the interaction exists
    /// </summary>
    Loop
}
[System.Serializable]
public enum InteractionTweenType
{
    /// <summary>
    /// The time between objects will depend on the given interaction time
    /// </summary>
    DependsOnInteractionTime,
    /// <summary>
    /// The time between objects will be fixed
    /// </summary>
    FixedTime
}