using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HallwayPostAndCam : MonoBehaviour
{
    public PlayerTrigger trigger;
    public PostProcessVolume postVolume;
    public CinemachineFreeLook cam;
    public CinemachineVirtualCamera camTransfer;
    public GameObject enemy;
    public AudioSource music;
    public float shiftSpeed;
    private ColorGrading cg;
    private CinemachineComposer toprig;
    private CinemachineComposer midrig;
    private CinemachineComposer botrig;
    private bool executed = false;

    private void Start()
    {
        cg = postVolume.profile.GetSetting<ColorGrading>();
        toprig = cam.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
        midrig = cam.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
        botrig = cam.GetRig(2).GetCinemachineComponent<CinemachineComposer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger.Active())
        {
            // Lerp post saturation
            postProcessingShift();
            // Adjust Camera settings
            cameraShift();
            if (!executed && cam.m_Lens.FieldOfView > 64f)
            {
                camTransfer.gameObject.SetActive(true);
                enemy.GetComponent<Rigidbody>().useGravity = true;
                enemy.GetComponent<MeshRenderer>().enabled = true;
                music.enabled = true;
                music.Play();
                executed = true;
            }
        }

    }

    void postProcessingShift()
    {
        if ( cg.saturation < 10f)
        {
            cg.saturation.value = Mathf.Lerp(cg.saturation, 10f, shiftSpeed * Time.deltaTime);
        }
    }
    void cameraShift()
    {
        cam.m_Orbits[0].m_Height = Mathf.Lerp(cam.m_Orbits[0].m_Height, 4f, shiftSpeed * Time.deltaTime);
        cam.m_Orbits[0].m_Radius = Mathf.Lerp(cam.m_Orbits[0].m_Radius, 7, shiftSpeed * Time.deltaTime);
        cam.m_Orbits[1].m_Height = Mathf.Lerp(cam.m_Orbits[1].m_Height, 0.4f, shiftSpeed * Time.deltaTime);
        cam.m_Orbits[1].m_Radius = Mathf.Lerp(cam.m_Orbits[1].m_Radius, 7, shiftSpeed * Time.deltaTime);
        cam.m_Orbits[2].m_Height = Mathf.Lerp(cam.m_Orbits[2].m_Height, -1f, shiftSpeed * Time.deltaTime);
        cam.m_Orbits[2].m_Radius = Mathf.Lerp(cam.m_Orbits[2].m_Radius, 7, shiftSpeed * Time.deltaTime);
        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 65f, shiftSpeed * Time.deltaTime);
        toprig.m_TrackedObjectOffset = Vector3.Lerp(toprig.m_TrackedObjectOffset,
            new Vector3(toprig.m_TrackedObjectOffset.x, 1.38f, toprig.m_TrackedObjectOffset.z), Time.deltaTime);
        midrig.m_TrackedObjectOffset = Vector3.Lerp(midrig.m_TrackedObjectOffset,
            new Vector3(midrig.m_TrackedObjectOffset.x, 1.38f, midrig.m_TrackedObjectOffset.z), Time.deltaTime);
        botrig.m_TrackedObjectOffset = Vector3.Lerp(botrig.m_TrackedObjectOffset,
            new Vector3(botrig.m_TrackedObjectOffset.x, 1.38f, botrig.m_TrackedObjectOffset.z), Time.deltaTime);
    }
}
