using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICardSlot : UI, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action<UICardSlot> OnStartDrag;
    public event Action<UICardSlot> OnStopDrag;
    public bool Selected { get; protected set; }

    [SerializeField] protected UICard _uICardPrefab;
    [SerializeField] protected Vector3 _selectedPosition;
    protected UICard _uICard = null;
    protected CanvasGroup _canvasGroup = null;
    protected bool _isDragging;
    protected PoolService _poolService = null;

    public void CreateUICard(int slotIndex)
    {
        _uICard = _poolService.Spawn(_uICardPrefab, transform);
        _uICard.Create(slotIndex);
    }

    public void DeleteUICard()
    {
        _poolService.Despawn(_uICard);
        _uICard = null;
    }

    public void SetInteractableCanvasGroup(bool interactable)
    {
        _canvasGroup.interactable = interactable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDragging)
            return;

        Selected = !Selected;

        ResetLocalPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;

        OnStartDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        OnStopDrag?.Invoke(this);
    }

    protected void ResetLocalPosition()
    {
        if (Selected)
            transform.localPosition = _selectedPosition;
        else
            transform.localPosition = Vector3.zero;
    }

    protected override void Awake()
    {
        base.Awake();

        _canvasGroup = GetComponent<CanvasGroup>();
    }
}
