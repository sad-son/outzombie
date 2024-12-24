using ServiceLocatorSystem;
using Systems;
using UnityEngine;
using UnityEngine.Serialization;

public class CardDragHandler : MonoBehaviour
{
    [FormerlySerializedAs("_canvas")] [SerializeField] private Canvas canvas;
    [FormerlySerializedAs("_canvasRect")] [SerializeField] private RectTransform canvasRect;
    [FormerlySerializedAs("_raycastDistance")] [SerializeField] private float raycastDistance;
    [FormerlySerializedAs("_rectTransform")] [SerializeField] private RectTransform rectTransform;
    
    private bool _isDragging;
    private Camera _mainCamera;
    private Vector3 _offset;

    private Ray _currentRay;
    private bool _isRayVisible;
    private bool _isRayHit;    
    private Vector3 _clickPoint;
    private InputManager _inputManager;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        var inputSystemLocator = ServiceLocatorController.Resolve<InputSystemLocator>();
        var inputManager = inputSystemLocator.Resolve<InputManager>();
        inputManager.onClick += StartDrag;
        inputManager.onDrag += DragCard;
        inputManager.onRelease += StopDrag;
    }

    private void StartDrag(Vector2 position)
    {
        Debug.Log($"StartDrag: {position}");
        _currentRay = _mainCamera.ScreenPointToRay(position);
        _isRayVisible = true;

        if (Physics.Raycast(_currentRay, out var hit))
        {
            _isRayHit = true;
            _clickPoint = hit.point; 
            
            if (hit.collider.gameObject == gameObject)
            {
                _isDragging = true;
            }
        }
        else
        {
            _isRayHit = false;
            _clickPoint = _currentRay.origin + _currentRay.direction * raycastDistance;
        }
    }

    private void DragCard(Vector2 position)
    {
        if (_isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, 
                position, 
                _mainCamera, 
                out var localPosition
            );

            transform.localPosition = new Vector3(localPosition.x, localPosition.y, transform.localPosition.z);
        }
    }
    
    private void StopDrag(Vector2 position)
    {
        Debug.Log($"StopDrag: {position}");
        _isDragging = false;
    }

    private void OnDestroy()
    {
        if (_inputManager != null)
        {
            _inputManager.onClick -= StartDrag;
            _inputManager.onDrag -= DragCard;
        }
    }

    private void OnDrawGizmos()
    {
        if (_isRayVisible)
        {
            Gizmos.color = _isRayHit ? Color.green : Color.red;
            Gizmos.DrawRay(_currentRay.origin, _currentRay.direction * raycastDistance); 
 
            Gizmos.color = _isRayHit ? Color.green : Color.red;
            Gizmos.DrawSphere(_clickPoint, 10f);
        }
    }
}