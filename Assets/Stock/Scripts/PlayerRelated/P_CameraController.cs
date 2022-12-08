using UnityEngine;
using Cinemachine;

public class P_CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineFramingTransposer cinemachineFramingTransposer;
    [SerializeField] private PlayerProporties playerProporties;
    private void Start()
    {
        IntializeCamera();
    }

    private void Update()
    {
        cinemachineFramingTransposer.m_TrackedObjectOffset = Vector2.Lerp(cinemachineFramingTransposer.m_TrackedObjectOffset, new Vector2(playerProporties.lookOffset.x, playerProporties.lookOffset.y) * InputController.Instance.LookValue(), Time.deltaTime * playerProporties.lookSpeed);
    }

    private void IntializeCamera()
    {
        cinemachineFramingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
}
