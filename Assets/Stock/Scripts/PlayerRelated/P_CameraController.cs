using UnityEngine;
using Cinemachine;

public class P_CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineFramingTransposer cinemachineFramingTransposer;
    private PlayerProperties playerProporties;
    private float currentZoom = 10;

    private void Start()
    {
        playerProporties = ScriptableManager.Instance.playerProperties;
        IntializeCamera();
    }

    private void Update()
    {
        if (InputController.Instance.ZoomValue() < 0 && currentZoom > playerProporties.zoomMinMax.x)
            currentZoom += InputController.Instance.ZoomValue();
        if (InputController.Instance.ZoomValue() > 0 && currentZoom < playerProporties.zoomMinMax.y)
            currentZoom += InputController.Instance.ZoomValue();

        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, currentZoom, Time.deltaTime * playerProporties.zoomSpeed);

        cinemachineFramingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
            cinemachineFramingTransposer.m_TrackedObjectOffset,
            new Vector3(playerProporties.lookOffset.x, playerProporties.lookOffset.y, 0) * InputController.Instance.LookValue(),
            Time.deltaTime * playerProporties.lookSpeed);
    }

    private void IntializeCamera()
    {
        cinemachineFramingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
}
