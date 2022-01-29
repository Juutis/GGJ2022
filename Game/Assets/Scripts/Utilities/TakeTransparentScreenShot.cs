// Date   : 24.02.2018 09:31
// Project: snipergame
// Author : bradur

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Rendering;

public class TakeTransparentScreenShot : MonoBehaviour
{

    void Start()
    {

    }

    private bool takeShot = false;

    [SerializeField]
    private RenderTexture rt;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            takeShot = true;
        }
    }

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }


    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }


    private void OnPostRender()
    {
        if (takeShot)
        {
            takeShot = false;
            //GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
            int width = Screen.width;
            int height = Screen.height;
            Debug.Log($"Width: {width} Height: {height} ");

            //RenderTexture rt = new RenderTexture(width, height, 32);
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
            screenShot.Apply();
            byte[] pngShot = screenShot.EncodeToPNG();
            string path = $"{Application.dataPath}/screenshot_{Random.Range(0, 1024).ToString()}.png";
            Debug.Log(path);
            File.WriteAllBytes(path, pngShot);
            Destroy(screenShot);
        }
    }
}
