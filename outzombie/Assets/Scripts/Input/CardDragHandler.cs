using Systems;
using UnityEngine;

public class CardDragHandler : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private RectTransform _rectTransform;
    
    private bool isDragging;
    private Camera mainCamera;
    private Vector3 offset;

    private Ray currentRay;
    private bool isRayVisible;
    private bool isRayHit;    
    private Vector3 clickPoint;

    private void Start()
    {
        mainCamera = Camera.main;

        CustomInputManager.Instance.OnClick += StartDrag;
        CustomInputManager.Instance.OnDrag += DragCard;
        CustomInputManager.Instance.OnRelease += StopDrag;
    }

    private void StartDrag(Vector2 position)
    {
        Debug.Log($"StartDrag: {position}");
        currentRay = mainCamera.ScreenPointToRay(position);
        isRayVisible = true;

        if (Physics.Raycast(currentRay, out var hit))
        {
            isRayHit = true;
            clickPoint = hit.point; 
            
            if (hit.collider.gameObject == gameObject)
            {
                isDragging = true;
            }
        }
        else
        {
            isRayHit = false;
            clickPoint = currentRay.origin + currentRay.direction * _raycastDistance;
        }
    }

    private void DragCard(Vector2 position)
    {
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect, 
                position, 
                mainCamera, 
                out var localPosition
            );

            transform.localPosition = new Vector3(localPosition.x, localPosition.y, transform.localPosition.z);
        }
    }
    
    private void StopDrag(Vector2 position)
    {
        Debug.Log($"StopDrag: {position}");
        isDragging = false;
    }

    private void OnDestroy()
    {
        if (CustomInputManager.Instance != null)
        {
            CustomInputManager.Instance.OnClick -= StartDrag;
            CustomInputManager.Instance.OnDrag -= DragCard;
        }
    }

    private void OnDrawGizmos()
    {
        if (isRayVisible)
        {
            Gizmos.color = isRayHit ? Color.green : Color.red;
            Gizmos.DrawRay(currentRay.origin, currentRay.direction * _raycastDistance); 
 
            Gizmos.color = isRayHit ? Color.green : Color.red;
            Gizmos.DrawSphere(clickPoint, 10f);
        }
    }
}