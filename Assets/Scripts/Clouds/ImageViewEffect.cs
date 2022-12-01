using System;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class ImageViewEffect : MonoBehaviour
{
    public Shader shader;
    public Transform container;
    public Material noise;
    Material material;

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

        material.SetVector("BoundsMin", container.position - container.localScale / 2);
        material.SetVector("BoundsMax", container.position + container.localScale / 2);
        material.SetTexture("ShapeNoise", noise.GetTexture("_MainTex"));

        //Blit sets _MainTex on material to source texture, sets render target to destination texture, and draws full-screen quad
        //Allows us to modify source then copy it to destination
        Graphics.Blit(source, destination, material);
    }
}
