using Cysharp.Threading.Tasks;
using Gameplay.CardAssembly;
using Gameplay.Extensions;
using Scellecs.Morpeh.Providers;
using ServiceLocatorSystem;
using Systems;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameplayAssembly.CardAssembly
{
    public class CardUIProvider : MonoProvider<CardUiComponent>
    {
        [FormerlySerializedAs("_canvas")] [SerializeField] private Canvas canvas;
        [FormerlySerializedAs("_canvasRect")] [SerializeField] private RectTransform canvasRect;
        [FormerlySerializedAs("_raycastDistance")] [SerializeField] private float raycastDistance;
        [FormerlySerializedAs("_rectTransform")] [SerializeField] private RectTransform rectTransform;

        private bool _isDragging;
        private Camera _mainCamera;
        private Vector3 _offset;

        private Ray _cardRay;
        private Ray _groundRay;
        private bool _isRayVisible;
        private bool _isRayHit;    
        private Vector3 _clickPoint;
        private Vector3 _stopDraggingPoint;
        private InputManager _inputManager;
        private Vector3 _startPosition;
    
        private void Start()
        {
            _mainCamera = Camera.main;
            var inputSystemLocator = ServiceLocatorController.Resolve<InputSystemLocator>();
            var inputManager = inputSystemLocator.Resolve<InputManager>();
            inputManager.onClick += StartDrag;
            inputManager.onDrag += DragCard;
            inputManager.onRelease += StopDrag;
            _startPosition = transform.localPosition;
        }

        private void StartDrag(Vector2 position)
        {
            Debug.Log($"StartDrag: {position}");
            _cardRay = _mainCamera.ScreenPointToRay(position);
            _isRayVisible = true;

            if (Physics.Raycast(_cardRay, out var hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    _isDragging = true;
                }
            }
            else
            {
                _isRayHit = false;
                _clickPoint = _cardRay.origin + _cardRay.direction * raycastDistance;
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

            if (_isDragging)
            {
                SetSpawningPoint(position);
                ReturnCard();
            }
  
            _isDragging = false;
        }

        private void SetSpawningPoint(Vector2 position)
        {
            _groundRay = _mainCamera.ScreenPointToRay(position);
            int layerMask = 1 << LayersHelper.Ground;
            if (Physics.Raycast(_groundRay, out var hit, Camera.main.farClipPlane,layerMask))
            {
                _isRayVisible = true;
                if (hit.collider.gameObject.layer == LayersHelper.Ground)
                {
                    ref var serializedData = ref GetData();
   
                    serializedData.position = hit.point;
                    serializedData.readyToSpawn = true;
                    _stopDraggingPoint = serializedData.position;
                    _isRayHit = true;
                }
            }
        }

        private void ReturnCard()
        {
            UniTask.DelayFrame(5).ContinueWith(() =>
            {
                transform.localPosition = _startPosition;
            });
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
                Gizmos.DrawRay(_groundRay.origin, _groundRay.direction * Camera.main.farClipPlane); 
 
                Gizmos.color = _isRayHit ? Color.green : Color.red;
                Gizmos.DrawSphere(_clickPoint, 1f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_stopDraggingPoint, 3f);
            }
        }
    }
}