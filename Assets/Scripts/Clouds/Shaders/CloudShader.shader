Shader "Hidden/CloudShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct VertData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct FragData
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewVector : TEXCOORD1;
            };

            FragData vert(VertData v)
            {
                FragData fragDat;
                fragDat.vertex = UnityObjectToClipPos(v.vertex);
                fragDat.uv = v.uv;

                //Camera space forward is -z, unity's is +z so we have to flip the z
                float3 viewVector = mul(unity_CameraInvProjection, float4(v.uv * 2 - 1, 0, -1));
                fragDat.viewVector = mul(unity_CameraToWorld, float4(viewVector, 0));

                return fragDat;
            }

            //Screen so far
            sampler2D _MainTex;
            //Depth of objects in scene from current view position
            sampler2D _CameraDepthTexture;

            Texture3D<float4> ShapeNoise;
            Texture3D<float4> DetailNoise;
            SamplerState samplerShapeNoise;
            SamplerState samplerDetailNoise;

            //Set by ImageViewEffect script
            float3 BoundsMin;
            float3 BoundsMax;
            float3 CloudOffset; //Allows for scrolling of clouds
            float3 CloudScale;   //Changes size of clouds
            float DensityThreshold; //Controls how high the noise has to be to draw a cloud, if too low, space will be empty
            float DensityMultiplier;    //Increases density of clouds
            int NumSteps;

            float lightAbsorptionTowardSun;
            float cloudLightAbsorption;
            float phaseVal;

            float4 cloudCol;   //Color of cloud

            float sampleDensity(float3 position)
            {
                float3 uvw = float3(position.x * CloudScale.x, position.y * CloudScale.y, position.z * CloudScale.z) * 0.001 + CloudOffset * 0.01;
                float4 shape = ShapeNoise.SampleLevel(samplerShapeNoise, uvw, 0);
                float density = max(0, shape.r - DensityThreshold) * DensityMultiplier;
                return density;
            }

            float2 rayBoxDist(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 rayDir)
            {
                float3 t0 = (boundsMin - rayOrigin) / rayDir;
                float3 t1 = (boundsMax - rayOrigin) / rayDir;
                float3 tmin = min(t0, t1);
                float3 tmax = max(t0, t1);

                float distA = max(max(tmin.x, tmin.y), tmin.z);
                float distB = min(tmax.x, min(tmax.y, tmax.z));

                float distToBox = max(0, distA);
                float distInsideBox = max(0, distB - distToBox);
                return float2(distToBox, distInsideBox);
            }

            //Returns light transmittance, amount of light that makes it through the cloud, by marching through the box and summing the samples
            float3 lightMarch(float3 position)
            {
                float3 lightDir = _WorldSpaceLightPos0.xyz; //Get position of light, that is our direction
                float distInsideBox = rayBoxDist(BoundsMin, BoundsMax, position, 1/lightDir).y; //Raycast distance inside the cloud container

                float stepSize = distInsideBox / NumSteps;  //Calculate step size
                float totalDensity = 0;

                for(int step = 0; step < NumSteps; step++)
                {
                    position += lightDir * stepSize;
                    totalDensity += max(0, sampleDensity(position) * stepSize); //Ensure value is at least 0, get density from noise and add to total
                }

                float transmittance = exp(-totalDensity * lightAbsorptionTowardSun);
                return DensityThreshold + transmittance * (1-DensityThreshold);   //blends transmittance based on a threshold
            }

            float4 frag(FragData input) : SV_Target
            {
                //Sample main texture color
                float4 col = tex2D(_MainTex, input.uv);

                //Get position of camera in world space
                float3 rayOrigin = _WorldSpaceCameraPos;
                //Direction of ray is same as viewVector, like (eyePosition - lookAtPosition) or transform.forward of camera
                float3 rayDir = normalize(input.viewVector);

                //Get depth of objects in the frame (stored in _CameraDepthTexture) and sample the texture at our current UV
                float nonLinearDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, input.uv);
                //Depth is stored non-linearly in depth texture, get depth by converting it to linear eye depth and multiplying it by distance from eye
                float depth = LinearEyeDepth(nonLinearDepth) * length(input.viewVector);

                //Get Vector2 where distance to point on box we're looking at is x component and distance to point on the other side of the box is y component
                float2 rayBoxInfo = rayBoxDist(BoundsMin, BoundsMax, rayOrigin, rayDir);
                float distToBox = rayBoxInfo.x;
                float distInsideBox = rayBoxInfo.y;

                float distTravelled = 0;
                float stepSize = distInsideBox / NumSteps;  //Find size of steps based on number of samples to take for density
                float distLimit = min(depth - distToBox, distInsideBox);    //The furthest our distance can be

                //March through volume (step through cloud container in increments)
                float transmittance = 1;   //As cloud density increases, light makes it through exponentially less
                float light = 0; 

                while(distTravelled < distLimit)
                {
                    float3 rayPos = rayOrigin + rayDir * distTravelled;   //position of ray is at the point we look at on the box + step distance
                    float density = sampleDensity(rayPos);

                    if(density > 0)
                    {
                        float lightTransmittance = lightMarch(rayPos);  //Calculate light transmittance through cloud
                        light += density * stepSize * transmittance * lightTransmittance * phaseVal;
                        transmittance *= exp(-density * stepSize * cloudLightAbsorption);

                        //Break out early if transmittance is almost 0
                        if(transmittance < 0.01)
                        {
                            break;
                        }
                    }
                    
                    //Increment to next step
                    distTravelled += stepSize;
                }

                   

                //if(transmittance < 1)
                //{
                //    float increase = lerp(.4f, 0, transmittance);
                //    col.a = .5f;
                //    return col * transmittance + float4(increase, increase, increase, 0);
                //}

                //If distance to inside of box is more than 0 (point is not behind or right on top of us) and distance to box less than depth of objects (box point is not behind an object)
                //bool rayHitBox = distInsideBox > 0 && distToBox < depth;
                //if (rayHitBox)
                //{
                //    col = 0;
                //}

                float3 clodCol = light * _LightColor0;    //Color of Unity's light * factor of amount of energy light has
                col = col * transmittance + cloudCol;

                return col;
            }
            ENDCG
        }
    }
}
