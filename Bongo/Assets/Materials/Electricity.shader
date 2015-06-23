// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'

    Shader "Electricity" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Electricity ("Electricity Texture (RG)", 2D) = "white" {}
        _Oppacity ("Oppacity", Range(0.01, 1)) = 0.5
        _OutlineWidth ("Outline Width", Range(0.01, 1)) = 0.05
        _OutlineColor("Outline Color", Color) = (0,0.7,0.8,1)
    }
     
    Category {
        SubShader {
            UsePass "Diffuse/BASE"
            Pass {
             Name "BASE"
             Tags { "LightMode" = "Always" }
             Lighting Off
             Cull Back
             ZWrite Off
             ZTest LEqual
             Blend SrcAlpha OneMinusSrcAlpha
             
             
    CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members viewAngle)
#pragma exclude_renderers d3d11 xbox360
    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"
     
    float4 _Electricity_ST;
    sampler2D _Electricity;
    float _Oppacity;
    float _OutlineWidth;
    float4 _OutlineColor;
     
    struct data {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
    };
     
    struct v2f {
        float4 position : POSITION;
        float2 uvmain : TEXCOORD2;
        float viewAngle;
    };
     
    v2f vert(data i){
        v2f o;
        o.position = mul(UNITY_MATRIX_MVP, i.vertex + float4(i.normal, 0)*_OutlineWidth);
        o.uvmain = TRANSFORM_TEX(i.texcoord, _Electricity);
        o.viewAngle = 1 - dot(normalize(ObjSpaceViewDir(i.vertex)), i.normal);
        return o;
    }
     
    half4 frag( v2f i ) : COLOR
    {  
        half4 col = lerp(tex2D(_Electricity, i.uvmain - _Time.xy), _OutlineColor, i.viewAngle);
        col.a *= _Oppacity*i.viewAngle;
       
        return col;
    }
    ENDCG
            }
        }
    }
     
    }

