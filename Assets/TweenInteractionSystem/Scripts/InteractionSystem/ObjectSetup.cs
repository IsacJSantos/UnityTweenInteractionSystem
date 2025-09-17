using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class ObjectSetup : MonoBehaviour
{
    [SerializeField]
    protected Renderer[] visualRenderers;
    [SerializeField]
    protected Collider[] objectColliders;

    [SerializeField]
    protected float minScale = .3f;
    [SerializeField]
    protected bool disableTween;

    protected Vector3 initialPosition;
    public Vector3 getInitialPosition { get => initialPosition; }

    protected Vector3 initialScale;
    protected Vector3 initialRotation;

    protected LayerMask defaultLayer;
    protected LayerMask firstPersonLayer;

    protected Transform parentTransform;

#if UNITY_EDITOR
    [Space]
    [Header("EDITOR")]
    public bool quickSetup;
#endif

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (quickSetup)
        {
            quickSetup = false;
            QuickSetup();
        }
    }
#endif
    protected virtual void Awake()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        initialRotation = transform.eulerAngles;

        parentTransform = transform.parent;

        if (visualRenderers[0] != null)
            defaultLayer = visualRenderers[0].gameObject.layer;
        else
            defaultLayer = LayerMask.NameToLayer("Default");

        firstPersonLayer = LayerMask.NameToLayer("Default");
    }


    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public abstract void MoveObject(Transform setupItemTargetPoint, Vector3 finalPosition, float duration, Action<ObjectSetup> OnHitTargetPosition = null);


    public virtual void ResetObject()
    {
        transform.DOKill();
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.eulerAngles = initialRotation;
        SetLayer(false);
    }

    protected void SetLayer(bool firstPerson)
    {
        int length = visualRenderers.Length;
        for (int i = 0; i < length; i++)
        {
            visualRenderers[i].gameObject.layer = firstPerson ? firstPersonLayer : defaultLayer;
        }
    }

    protected void ToggleColliders(bool active)
    {
        int length = objectColliders.Length;
        for (int i = 0; i < length; i++)
        {
            if (objectColliders[i] != null)
                objectColliders[i].enabled = active;
        }
    }

    public abstract float GetTotalTweenDuration();

#if UNITY_EDITOR
    private void QuickSetup()
    {
        List<Renderer> renders = new List<Renderer>();
        renders.AddRange(GetComponentsInChildren<Renderer>());

        visualRenderers = renders.ToArray();

        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(GetComponentsInChildren<Collider>());

        objectColliders = colliders.ToArray();

        EditorUtility.SetDirty(gameObject);
    }
#endif
}
