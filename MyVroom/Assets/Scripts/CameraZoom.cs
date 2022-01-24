using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineFreeLook FreeLookCamera;

    private CinemachineFreeLook.Orbit baseTopOrbit;
    private CinemachineFreeLook.Orbit baseMidOrbit;
    private CinemachineFreeLook.Orbit baseBotOrbit;

    private float baseOrthographicSize;

    private float PreviousFingerDistanceSquared = -1f;

    private float RatioZoom = 1f;

    public float MinZoom = 0.1f;
    public float MaxZoom = 2f;

    public float Sensitivity = 10f;

    // Start is called before the first frame update
    void Start()
    {
        FreeLookCamera = GetComponent<CinemachineFreeLook>();
        baseTopOrbit = FreeLookCamera.m_Orbits[0];
        baseMidOrbit = FreeLookCamera.m_Orbits[1];
        baseBotOrbit = FreeLookCamera.m_Orbits[2];

        baseOrthographicSize = FreeLookCamera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            float fingerDistanceSquared = Mathf.Abs((Input.touches[1].position - Input.touches[0].position).sqrMagnitude);

            if (Input.touches[1].phase == TouchPhase.Began)
            {
                PreviousFingerDistanceSquared = fingerDistanceSquared;
            }
            else if(Input.touches[0].phase == TouchPhase.Moved ||
                    Input.touches[1].phase == TouchPhase.Moved)
            {
                float deltaDistance = Mathf.Sqrt(fingerDistanceSquared) - Mathf.Sqrt(PreviousFingerDistanceSquared);

                RatioZoom = Mathf.Clamp(RatioZoom - (deltaDistance / Sensitivity), MinZoom, MaxZoom);

                if (!Camera.main.orthographic)
                {
                    FreeLookCamera.m_Orbits[0].m_Radius = baseTopOrbit.m_Radius * RatioZoom;
                    FreeLookCamera.m_Orbits[1].m_Radius = baseMidOrbit.m_Radius * RatioZoom;
                    FreeLookCamera.m_Orbits[2].m_Radius = baseBotOrbit.m_Radius * RatioZoom;

                    FreeLookCamera.m_Orbits[0].m_Height = baseTopOrbit.m_Height * RatioZoom;
                    FreeLookCamera.m_Orbits[1].m_Height = baseMidOrbit.m_Height * RatioZoom;
                    FreeLookCamera.m_Orbits[2].m_Height = baseBotOrbit.m_Height * RatioZoom;
                }
                else
                {
                    FreeLookCamera.m_Lens.OrthographicSize = baseOrthographicSize * RatioZoom;
                }

                PreviousFingerDistanceSquared = fingerDistanceSquared;
            }
        }
    }
}
