Shader "Hidden/Custom/Greyscale"
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
  ENDHLSL
  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Greyscale
            //#pragma fragment Blur3x3
          ENDHLSL
      }
  }
}