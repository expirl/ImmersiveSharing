#region Includes
using UnityEngine;
#endregion

namespace TS.GazeInteraction
{
    public class GazeInteractor : MonoBehaviour
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private float _maxDetectionDistance;
        [SerializeField] private float _minDetectionDistance;
        [SerializeField] private float _timeToActivate = 1.0f;
        [SerializeField] private LayerMask _layerMask;

        [Header("Heatmap")]
        [SerializeField] private HeadGazeHeatmap headGazeHeatmap;

        private Ray _ray;
        private RaycastHit _hit;
        private GazeReticle _reticle;
        private GazeInteractable _interactable;
        private float _enterStartTime;
        private MeshRenderer currentMeshRenderer;

        #endregion

        private void Start()
        {
            var instance = ResourcesManager.GetPrefab(ResourcesManager.FILE_PREFAB_RETICLE);
            var reticle = instance.GetComponent<GazeReticle>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (reticle == null) { throw new System.Exception("Missing GazeReticle"); }
#endif

            _reticle = Instantiate(reticle);
            _reticle.SetInteractor(this);
          //  _reticle.Enable(true); // Always enable the reticle
        }

        private void Update()
        {
            // 시선 방향으로 Ray를 쏘아 충돌 여부 확인
            _ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(_ray, out _hit, _maxDetectionDistance, _layerMask))
            {
                // 충돌된 오브젝트와의 거리 계산
                var distance = Vector3.Distance(transform.position, _hit.transform.position);

                // 최소 거리보다 가까우면 Reset()을 호출하고 상호작용 중단
                if (distance < _minDetectionDistance)
                {
                    _reticle.Enable(true);  // reticle을 항상 보이게 유지
                    Reset();
                    return;
                }

                // 충돌된 오브젝트를 reticle 타겟으로 설정하고 활성화
                _reticle.SetTarget(_hit);
                _reticle.Enable(true);  // reticle 항상 활성화

                // 충돌된 오브젝트에 GazeInteractable이 있는지 확인
                var interactable = _hit.collider.transform.GetComponent<GazeInteractable>();
                currentMeshRenderer = _hit.collider.gameObject.GetComponent<MeshRenderer>();
                if (interactable == null)
                {
                    Reset();
                    return;
                }

                // 새로운 상호작용 대상이면 초기화하고 GazeEnter 호출
                if (interactable != _interactable)
                {
                    Reset();
                    _enterStartTime = Time.time;  // 새로운 상호작용 시작 시간 기록
                    _interactable = interactable;
                    _interactable.GazeEnter(this, _hit.point);
                }

                // 상호작용 지속
                _interactable.GazeStay(this, _hit.point);

                // 대상이 활성화 가능하고, 이미 활성화된 상태라면, 다시 활성화를 준비하도록 초기화
                if (_interactable.IsActivable)
                {
                    if (_interactable.IsActivated)
                    {
                        // 활성화된 상태를 해제하여 다시 활성화 가능하게 함
                        _interactable.IsActivated = false;
                    }

                    var timeToActivate = (_enterStartTime + _timeToActivate) - Time.time;
                    var progress = 1 - (timeToActivate / _timeToActivate);
                    progress = Mathf.Clamp(progress, 0, 1);

                    _reticle.SetProgress(progress);

                    // 활성화가 완료되면 진행도와 시간을 초기화하고 다시 활성화 가능하게 설정
                    if (progress == 1)
                    {
                        _reticle.Enable(false);
                        _interactable.Activate();

                        Debug.Log("Gaze Active");
                        UpdateHeatmap();

                        // 활성화 시간과 진행도를 초기화
                        _enterStartTime = Time.time;
                        _reticle.SetProgress(0);

                        // 활성화 상태를 다시 해제해 새로운 활성화를 준비
                        _interactable.IsActivated = false;
                    }
                }

                return;
            }

            // 충돌된 오브젝트가 없어도 reticle을 항상 활성화
            _reticle.Enable(true);
            Reset();
        }

        private void UpdateHeatmap()
        {
            if (currentMeshRenderer != null && headGazeHeatmap != null)
            {
                headGazeHeatmap.addHitPoint(_hit.textureCoord.x * 4 - 2, _hit.textureCoord.y * 4 - 2, currentMeshRenderer);
            }
        }

        private void Reset()
        {
            _reticle.SetProgress(0);

            if (_interactable == null) { return; }
            _interactable.GazeExit(this);
            _interactable = null;
        }



#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * _maxDetectionDistance);
        }
#endif
    }
}