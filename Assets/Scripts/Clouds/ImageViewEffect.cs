using System;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class ImageViewEffect : MonoBehaviour
{
    public Shader shader;
    public Transform container;
    Material material;

    [SerializeField] Vector3 CloudOffset; //Allows for scrolling of clouds
    [SerializeField] float scrollSpeed = 0;
    [SerializeField] float CloudScaleMultiplier = 70f;    //multiplies CloudDeform
    [SerializeField] Vector3 CloudDeform = new Vector3(2, 1, 1);   //Changes size of clouds
    [SerializeField][Range(0, 1.0f)]float DensityThreshold = .5f; //Controls how high the noise has to be to draw a cloud, if too low, space will be empty
    [SerializeField] float DensityMultiplier = 5;    //Increases density of clouds
    [SerializeField] int NumSteps = 100;

    [SerializeField] float lightAbsorptionTowardSun = 1;
    [SerializeField] float cloudLightAbsorption = 1;
    [SerializeField] float phaseVal = 1;
    [SerializeField] Vector3 cloudColor = new Vector3(50, 50, 50);



    [ImageEffectOpaque]
    //When image is rendered to screen, we change it with this function, the screen is just a render texture
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //If we don't have a shader, the view shouldn't just fail, we'll just give it a shader that passes through, just copies source to destination
        if(shader == null)
        {
            shader = Shader.Find("Hidden/PassthroughDefault");
            //throw new NotImplementedException("Shader not defined on ImageViewEffect");
        }

        //If material is not set, initialize using the shader
        if(material == null)
        {
            material = new Material(shader);
        }

        //If shader is changed, this will update it in editor
        if(material.shader != shader)
        {
            material = new Material(shader);
        }

        WorleyNoiseGenerator noise = FindObjectOfType<WorleyNoiseGenerator>();
        noise.Generate();

        material.SetVector("BoundsMin", container.position - container.localScale / 2);
        material.SetVector("BoundsMax", container.position + container.localScale / 2);
        material.SetTexture("ShapeNoise", noise.noiseTexture);

        //material.SetVector("CloudOffset", CloudOffset);
        material.SetInt("NumSteps", NumSteps);
        material.SetVector("CloudScale", CloudScaleMultiplier * CloudDeform);
        material.SetVector("CloudOffset", CloudOffset);
        material.SetFloat("DensityMultiplier", DensityMultiplier);
        material.SetFloat("DensityThreshold", DensityThreshold);

        material.SetFloat("lightAbsorptionTowardSun", lightAbsorptionTowardSun);
        material.SetFloat("cloudLightAbsorption", cloudLightAbsorption);
        material.SetFloat("phaseVal", phaseVal);

        material.SetVector("cloudColor", cloudColor);

        CloudOffset = new Vector3(CloudOffset.x + scrollSpeed * Time.deltaTime, 0, CloudOffset.z + scrollSpeed * Time.deltaTime);

        //Blit sets _MainTex on material to source texture, sets render target to destination texture, and draws full-screen quad
        //Allows us to modify source then copy it to destination
        Graphics.Blit(source, destination, material);
    }
}
