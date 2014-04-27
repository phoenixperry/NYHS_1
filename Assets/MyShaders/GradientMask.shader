// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32458,y:32759|emission-121-OUT,alpha-109-A;n:type:ShaderForge.SFN_Tex2d,id:12,x:33140,y:32645,tex:deca4b74a27d39a47a7bf2876513e462,ntxv:0,isnm:False|TEX-1126-TEX;n:type:ShaderForge.SFN_Tex2d,id:109,x:32760,y:33110,ptlb:roundedCorner,ptin:_roundedCorner,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:121,x:32902,y:32832|A-12-RGB,B-122-RGB,T-1101-OUT;n:type:ShaderForge.SFN_Tex2d,id:122,x:33140,y:32863,tex:4ea2a300cf25896498655b95016bb042,ntxv:0,isnm:False|TEX-1132-TEX;n:type:ShaderForge.SFN_ValueProperty,id:1101,x:32962,y:33002,ptlb:Blend,ptin:_Blend,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2dAsset,id:1126,x:33347,y:32651,ptlb:yellow_asset,ptin:_yellow_asset,glob:False,tex:deca4b74a27d39a47a7bf2876513e462;n:type:ShaderForge.SFN_Tex2dAsset,id:1132,x:33360,y:32895,ptlb:organe_asset,ptin:_organe_asset,glob:False,tex:4ea2a300cf25896498655b95016bb042;proporder:109-1101-1126-1132;pass:END;sub:END;*/

Shader "Mine/GradientMask" {
    Properties {
        _roundedCorner ("roundedCorner", 2D) = "white" {}
        _Blend ("Blend", Float ) = 1
        _yellow_asset ("yellow_asset", 2D) = "white" {}
        _organe_asset ("organe_asset", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _roundedCorner; uniform float4 _roundedCorner_ST;
            uniform float _Blend;
            uniform sampler2D _yellow_asset; uniform float4 _yellow_asset_ST;
            uniform sampler2D _organe_asset; uniform float4 _organe_asset_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_1250 = i.uv0;
                float3 emissive = lerp(tex2D(_yellow_asset,TRANSFORM_TEX(node_1250.rg, _yellow_asset)).rgb,tex2D(_organe_asset,TRANSFORM_TEX(node_1250.rg, _organe_asset)).rgb,_Blend);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,tex2D(_roundedCorner,TRANSFORM_TEX(node_1250.rg, _roundedCorner)).a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
