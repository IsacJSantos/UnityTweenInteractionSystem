using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class ObjectSetupInteraction : MonoBehaviour
{
    [SerializeField]
    private ObjcetSetupInteractionType interactionType;
    [SerializeField]
    private ObjcetSetupInteractionTweenType tweenType;
    [SerializeField]
    [Tooltip("The time between objects if the Tween Type is setted to Fixed Time")]
    protected float tweenFixedTime;
    [SerializeField]
    protected ObjectSetup[] objectSetups;
    [SerializeField]
    [Tooltip("The instalation signs in this setup interaction")]
    private List<Transform> signs;
    private Vector3 signDefaultScale;
    [SerializeField]
    private Ease signsTweenEase = Ease.OutBack;
    [SerializeField]
    protected GameObject setupSignParent;


    protected ISetupObjectItem currentSetupItemInterface;

    protected const float startDelay = .8f;
    protected const float timePerObjectOffset = .03f;

    protected bool settedUp;

    private void Awake()
    {
        RemoveNullFieldsFromSignList();

        if (HasInstalationSigns())
            signDefaultScale = signs[0].localScale;
    }

    protected virtual void StartInteraction(ISetupObjectItem setupObjectItemInterface, float interactionTime)
    {
        currentSetupItemInterface = setupObjectItemInterface;

        float timePerObject = tweenType == ObjcetSetupInteractionTweenType.FixedTime ?
           tweenFixedTime :
           ((interactionTime - startDelay) / objectSetups.Length) - timePerObjectOffset;
       
        StartCoroutine(_MoveObjects(timePerObject));
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

        currentSetupItemInterface?.OnCancelInteraction();
        currentSetupItemInterface = null;
    }

    public void OnFinishInterction()
    {
        FinishHoldInteraction();
    }
    protected virtual void FinishHoldInteraction()
    {
        StopAllCoroutines();
        currentSetupItemInterface?.OnFinishInteraction();
        currentSetupItemInterface = null;
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
        currentSetupItemInterface.OnInitInteraction();

        OnStartMovingObjects();

        yield return new WaitForSeconds(startDelay);

        do
        {
            for (int i = 0; i < objectSetups.Length; i++)
            {
                ObjectSetup objectSetup = objectSetups[i];
                MoveObject(objectSetup, timePerObject);
                yield return new WaitForSeconds(timePerObject);
            }

            if (interactionType == ObjcetSetupInteractionType.Loop)
                ResetAllObjects();

        } while (interactionType == ObjcetSetupInteractionType.Loop);
    }

    public virtual void OnStartMovingObjects()
    {
        if (HasInstalationSigns())
            StartCoroutine(_HideSigns());
    }

    public abstract void MoveObject(ObjectSetup objectSetup, float time);

    IEnumerator _HideSigns()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(.5f);

        foreach (var sign in signs)
        {
            if (sign != null)
                sign.DOScale(Vector3.zero, 0.5f).SetEase(signsTweenEase).Play();
            yield return waitForSeconds;
        }
    }

    protected void ResetSigns()
    {
        foreach (var sign in signs)
        {
            if (sign != null)
            {
                sign.DOKill();
                sign.localScale = signDefaultScale;
            }
        }
    }

    protected bool HasInstalationSigns()
    {
        return signs.Count > 0;
    }

    private void RemoveNullFieldsFromSignList()
    {
        List<Transform> notNullSigns = new List<Transform>();
        int count = signs.Count;
        for (int i = 0; i < count; i++)
        {
            if (signs[i] != null)
                notNullSigns.Add(signs[i]);
        }
        signs = notNullSigns;
    }
}
[System.Serializable]
public enum ObjcetSetupInteractionType
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
public enum ObjcetSetupInteractionTweenType
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