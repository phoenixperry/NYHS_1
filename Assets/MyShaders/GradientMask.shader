// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32457,y:32690|diff-121-OUT,alpha-1159-OUT;n:type:ShaderForge.SFN_Tex2d,id:12,x:33140,y:32645,tex:deca4b74a27d39a47a7bf2876513e462,ntxv:0,isnm:False|TEX-1126-TEX;n:type:ShaderForge.SFN_Tex2d,id:109,x:33093,y:33072,ptlb:rounded,ptin:_rounded,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:121,x:32890,y:32792|A-12-RGB,B-122-RGB,T-1101-OUT;n:type:ShaderForge.SFN_Tex2d,id:122,x:33140,y:32863,tex:4ea2a300cf25896498655b95016bb042,ntxv:0,isnm:False|TEX-1132-TEX;n:type:ShaderForge.SFN_ValueProperty,id:1101,x:32962,y:33002,ptlb:Blend,ptin:_Blend,glob:False,v1:0;n:type:ShaderForge.SFN_Tex2dAsset,id:1126,x:33347,y:32651,ptlb:yellow_asset,ptin:_yellow_asset,glob:False,tex:deca4b74a27d39a47a7bf2876513e462;n:type:ShaderForge.SFN_Tex2dAsset,id:1132,x:33360,y:32895,ptlb:organe_asset,ptin:_organe_asset,glob:False,tex:4ea2a300cf25896498655b95016bb042;n:type:ShaderForge.SFN_ValueProperty,id:1142,x:32872,y:33262,ptlb:alpha_blend,ptin:_alpha_blend,glob:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:1144,x:33246,y:33242,ptlb:invisible_shader,ptin:_invisible_shader,tex:10350849f8eb00847b6e7aab7bb62aa3,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:1159,x:32772,y:33074|A-1144-A,B-109-A,T-1142-OUT;proporder:109-1101-1126-1132-1142-1144;pass:END;sub:END;*/

Shader "Mine/GradientMask" {
    Properties {
        _rounded ("rounded", 2D) = "white" {}
        _Blend ("Blend", Float ) = 0
        _yellow_asset ("yellow_asset", 2D) = "white" {}
        _organe_asset ("organe_asset", 2D) = "white" {}
        _alpha_blend ("alpha_blend", Float ) = 0
        _invisible_shader ("invisible_shader", 2D) = "white" {}
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
            uniform float4 _LightColor0;
            uniform sampler2D _rounded; uniform float4 _rounded_ST;
            uniform float _Blend;
            uniform sampler2D _yellow_asset; uniform float4 _yellow_asset_ST;
            uniform sampler2D _organe_asset; uniform float4 _organe_asset_ST;
            uniform float _alpha_blend;
            uniform sampler2D _invisible_shader; uniform float4 _invisible_shader_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_1173 = i.uv0;
                finalColor += diffuseLight * lerp(tex2D(_yellow_asset,TRANSFORM_TEX(node_1173.rg, _yellow_asset)).rgb,tex2D(_organe_asset,TRANSFORM_TEX(node_1173.rg, _organe_asset)).rgb,_Blend);
/// Final Color:
                return fixed4(finalColor,lerp(tex2D(_invisible_shader,TRANSFORM_TEX(node_1173.rg, _invisible_shader)).a,tex2D(_rounded,TRANSFORM_TEX(node_1173.rg, _rounded)).a,_alpha_blend));
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _rounded; uniform float4 _rounded_ST;
            uniform float _Blend;
            uniform sampler2D _yellow_asset; uniform float4 _yellow_asset_ST;
            uniform sampler2D _organe_asset; uniform float4 _organe_asset_ST;
            uniform float _alpha_blend;
            uniform sampler2D _invisible_shader; uniform float4 _invisible_shader_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_1174 = i.uv0;
                finalColor += diffuseLight * lerp(tex2D(_yellow_asset,TRANSFORM_TEX(node_1174.rg, _yellow_asset)).rgb,tex2D(_organe_asset,TRANSFORM_TEX(node_1174.rg, _organe_asset)).rgb,_Blend);
/// Final Color:
                return fixed4(finalColor * lerp(tex2D(_invisible_shader,TRANSFORM_TEX(node_1174.rg, _invisible_shader)).a,tex2D(_rounded,TRANSFORM_TEX(node_1174.rg, _rounded)).a,_alpha_blend),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
