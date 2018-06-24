Shader "Custom/Desaturator"
{
    HLSLINCLUDE

        #include "PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

		float _SaturationPoints[360];
		float _OverSaturation;

		float3 rgb2hsv (float3 color)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(color.bg, K.wz), float4(color.gb, K.xy), step(color.b, color.g));
			float4 q = lerp(float4(p.xyw, color.r), float4(color.r, p.yzx), step(p.x, color.r));

			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		half4 HSVtoRGB(in half3 HSV)
		{
			half R = abs(HSV.x * 6 - 3) - 1;
			half G = 2 - abs(HSV.x * 6 - 2);
			half B = 2 - abs(HSV.x * 6 - 4);
			return half4(((saturate(half3(R,G,B)) - 1) * HSV.y + 1) * HSV.z,1);
		}

        float4 Frag(VaryingsDefault i) : SV_Target
        {
			float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				
			float3 hsv = rgb2hsv(color.rgb);

			float closest = _SaturationPoints[floor(hsv.x * 360)];
			// return float4(closest, closest, closest, 1.0f);
			float value = lerp(pow(hsv.z, 2), hsv.z, closest);

			return HSVtoRGB(float3(hsv.x, saturate(closest * hsv.y * (1.0 + _OverSaturation)), value));
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
