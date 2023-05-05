Shader "Custom/GlowShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        [HDR] _MaskColor("Mask Color", Color) = (0.4,0.4,0.4,1)
        _MaskMap("Mask Map", 2D) = "white" {}
        [HDR] _SubMaskColor("Sub Mask Color", Color) = (0.4,0.4,0.4,1)
        _SubMaskMap("Sub Mask Map", 2D) = "white" {}
        //_RimColor("Rim Color", Color) = (0.9,0.2,0.15,1)
        //_RimPower("Rim Spec", Range(1, 10)) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MaskMap;
        sampler2D _SubMaskMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskMap;
            float2 uv_SubMaskMap;
        };

        fixed4 _Color;
        fixed4 _MaskColor;
        fixed4 _SubMaskColor;
        //fixed4 _RimColor;
        //fixed4 _RimPower;


        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            fixed4 m = tex2D(_MaskMap, IN.uv_MaskMap) * _MaskColor;
            fixed4 m2 = tex2D(_SubMaskMap, IN.uv_SubMaskMap) * _SubMaskColor;
            o.Albedo = c.rgb;
            o.Emission = m.rgb + m2.rgb; // Glow Effect
            o.Alpha = (c.a + (m.a + m2.a)) * 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
