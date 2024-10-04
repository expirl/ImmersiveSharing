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
            // �ü� �������� Ray�� ��� �浹 ���� Ȯ��
            _ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(_ray, out _hit, _maxDetectionDistance, _layerMask))
            {
                // �浹�� ������Ʈ���� �Ÿ� ���
                var distance = Vector3.Distance(transform.position, _hit.transform.position);

                // �ּ� �Ÿ����� ������ Reset()�� ȣ���ϰ� ��ȣ�ۿ� �ߴ�
                if (distance < _minDetectionDistance)
                {
                    _reticle.Enable(true);  // reticle�� �׻� ���̰� ����
                    Reset();
                    return;
                }

                // �浹�� ������Ʈ�� reticle Ÿ������ �����ϰ� Ȱ��ȭ
                _reticle.SetTarget(_hit);
                _reticle.Enable(true);  // reticle �׻� Ȱ��ȭ

                // �浹�� ������Ʈ�� GazeInteractable�� �ִ��� Ȯ��
                var interactable = _hit.collider.transform.GetComponent<GazeInteractable>();
                currentMeshRenderer = _hit.collider.gameObject.GetComponent<MeshRenderer>();
                if (interactable == null)
                {
                    Reset();
                    return;
                }

                // ���ο� ��ȣ�ۿ� ����̸� �ʱ�ȭ�ϰ� GazeEnter ȣ��
                if (interactable != _interactable)
                {
                    Reset();
                    _enterStartTime = Time.time;  // ���ο� ��ȣ�ۿ� ���� �ð� ���
                    _interactable = interactable;
                    _interactable.GazeEnter(this, _hit.point);
                }

                // ��ȣ�ۿ� ����
                _interactable.GazeStay(this, _hit.point);

                // ����� Ȱ��ȭ �����ϰ�, �̹� Ȱ��ȭ�� ���¶��, �ٽ� Ȱ��ȭ�� �غ��ϵ��� �ʱ�ȭ
                if (_interactable.IsActivable)
                {
                    if (_interactable.IsActivated)
                    {
                        // Ȱ��ȭ�� ���¸� �����Ͽ� �ٽ� Ȱ��ȭ �����ϰ� ��
                        _interactable.IsActivated = false;
                    }

                    var timeToActivate = (_enterStartTime + _timeToActivate) - Time.time;
                    var progress = 1 - (timeToActivate / _timeToActivate);
                    progress = Mathf.Clamp(progress, 0, 1);

                    _reticle.SetProgress(progress);

                    // Ȱ��ȭ�� �Ϸ�Ǹ� ���൵�� �ð��� �ʱ�ȭ�ϰ� �ٽ� Ȱ��ȭ �����ϰ� ����
                    if (progress == 1)
                    {
                        _reticle.Enable(false);
                        _interactable.Activate();

                        Debug.Log("Gaze Active");
                        UpdateHeatmap();

                        // Ȱ��ȭ �ð��� ���൵�� �ʱ�ȭ
                        _enterStartTime = Time.time;
                        _reticle.SetProgress(0);

                        // Ȱ��ȭ ���¸� �ٽ� ������ ���ο� Ȱ��ȭ�� �غ�
                        _interactable.IsActivated = false;
                    }
                }

                return;
            }

            // �浹�� ������Ʈ�� ��� reticle�� �׻� Ȱ��ȭ
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