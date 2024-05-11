Shader "Hidden/Custom/Blur"
{
  HLSLINCLUDE
      #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
      TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

      float4 Greyscale(VaryingsDefault i) : SV_Target
      {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

            float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));

            color.rgb = luminance.xxx;

            return color;
      }
      float4 Blur3x3(VaryingsDefault i) : SV_Target
      {
          float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

          float4 s11 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f, -1.0f / 768.0f));
          float4 s12 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, -1.0f / 768.0f));
          float4 s13 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f, -1.0f / 768.0f));
 
          float4 s21 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f, 0));
          float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
          float4 s23 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f, 0));
 
          float4 s31 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f, 1.0f / 768.0f));
          float4 s32 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, 1.0f / 768.0f));
          float4 s33 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f, 1.0f / 768.0f));
 
          color = (color + s11 + s12 + s13 + s21 + s23 + s31 + s32 + s33) / 9;
          return color;
      }
      float4 Blur7x7(VaryingsDefault i) : SV_Target 
      {
          return (
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     -3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     -3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     -3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   -3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      -3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      -3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      -3.0f / 768.0f)) +
 
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     -2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     -2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     -2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   -2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      -2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      -2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      -2.0f / 768.0f)) +
 
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     -1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     -1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     -1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   -1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      -1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      -1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      -1.0f / 768.0f)) +
 
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     0)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     0)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     0)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   0)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      0)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      0)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      0)) +
 
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      1.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      1.0f / 768.0f)) +
 
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      2.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      2.0f / 768.0f)) +
 
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-3.0f / 1024.0f,     3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-2.0f / 1024.0f,     3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-1.0f / 1024.0f,     3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0,                   3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(1.0f / 1024.0f,      3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(2.0f / 1024.0f,      3.0f / 768.0f)) +
            SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(3.0f / 1024.0f,      3.0f / 768.0f))
          ) / 49;
        }
        

  ENDHLSL
  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Blur7x7
            //#pragma fragment Blur3x3
          ENDHLSL
      }
  }
}