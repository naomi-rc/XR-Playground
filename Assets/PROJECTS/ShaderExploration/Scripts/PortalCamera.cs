using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    private class Portal
    {
        public Renderer Renderer;
        public bool IsPlaced;
        public Transform transform;
    }

    [SerializeField] private Portal[] portals = new Portal[2];
    [SerializeField] private Camera portalCamera; //Disabled - only renders when told to render
    [SerializeField] private int iterations = 7; //Number of recursive iterations - avoid too high of a value

    // One render texture per portal - reused for each frame
    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;

    private Camera mainCamera;


    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }
    void Start()
    {
        portals[0].Renderer.material.mainTexture = tempTexture1;
        portals[1].Renderer.material.mainTexture = tempTexture2;
    }

    // Call
    private void OnEnable()
    {
        // Callback called immediately before camera rendering
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        // Callback called immediately after camera rendering
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    /**
     *  src : ScriptableRenderContext - contains states
     *  camera : Camera - main camera
     * */
    private void UpdateCamera(ScriptableRenderContext src, Camera camera)
    {
        // Do not render if either portal has not been placed
        if (!portals[0].IsPlaced || !portals[1].IsPlaced)
        {
            return;
        }

        // Make sure portal is visible before rendering and set virtual camera to render to one of the render textures
        if (portals[0].Renderer.isVisible)
        {
            portalCamera.targetTexture = tempTexture1;
            for(int i = iterations - 1; 1 >= 0; --i){
                RenderCamera(portals[0], portals[1], i, src);
            }
        }

        if (portals[1].Renderer.isVisible)
        {
            portalCamera.targetTexture = tempTexture2;
            for (int i = iterations - 1; 1 >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i, src);
            }
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, int iterationID, ScriptableRenderContext src)
    {
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
