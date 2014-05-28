// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|diff-1502-OUT,alpha-23-OUT;n:type:ShaderForge.SFN_Tex2d,id:7,x:33370,y:32592,ptlb:image,ptin:_image,tex:b48f6390da55d4060873679bbb022a4d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:21,x:33244,y:33135,ptlb:visible,ptin:_visible,tex:ebc649d0c7b45455caffd1da7471ae93,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:22,x:33289,y:32900,ptlb:invisible,ptin:_invisible,tex:10350849f8eb00847b6e7aab7bb62aa3,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:23,x:32967,y:32918|A-22-A,B-21-A,T-24-OUT;n:type:ShaderForge.SFN_ValueProperty,id:24,x:33029,y:33123,ptlb:alpha_blend,ptin:_alpha_blend,glob:False,v1:1;n:type:ShaderForge.SFN_Lerp,id:31,x:33144,y:32670|A-7-RGB,B-33-OUT,T-35-OUT;n:type:ShaderForge.SFN_Desaturate,id:33,x:33370,y:32749|COL-7-RGB;n:type:ShaderForge.SFN_ValueProperty,id:35,x:33144,y:32812,ptlb:desaturate,ptin:_desaturate,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:1502,x:32957,y:32712|A-1503-RGB,B-31-OUT;n:type:ShaderForge.SFN_Color,id:1503,x:33144,y:32526,ptlb:TintColor,ptin:_TintColor,glob:False,c1:1,c2:1,c3:1,c4:1;proporder:7-21-22-24-35-1503;pass:END;sub:END;*/

Shader "MaskedTexture" {
    Properties {
        _image ("image", 2D) = "white" {}
        _visible ("visible", 2D) = "white" {}
        _invisible ("invisible", 2D) = "white" {}
        _alpha_blend ("alpha_blend", Float ) = 1
        _desaturate ("desaturate", Float ) = 1
        _TintColor ("TintColor", Color) = (1,1,1,1)
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
            uniform sampler2D _image; uniform float4 _image_ST;
            uniform sampler2D _visible; uniform float4 _visible_ST;
            uniform sampler2D _invisible; uniform float4 _invisible_ST;
            uniform float _alpha_blend;
            uniform float _desaturate;
            uniform float4 _TintColor;
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
                float2 node_1512 = i.uv0;
                float4 node_7 = tex2D(_image,TRANSFORM_TEX(node_1512.rg, _image));
                float node_33 = dot(node_7.rgb,float3(0.3,0.59,0.11));
                finalColor += diffuseLight * (_TintColor.rgb*lerp(node_7.rgb,float3(node_33,node_33,node_33),_desaturate));
/// Final Color:
                return fixed4(finalColor,lerp(tex2D(_invisible,TRANSFORM_TEX(node_1512.rg, _invisible)).a,tex2D(_visible,TRANSFORM_TEX(node_1512.rg, _visible)).a,_alpha_blend));
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
            uniform sampler2D _image; uniform float4 _image_ST;
            uniform sampler2D _visible; uniform float4 _visible_ST;
            uniform sampler2D _invisible; uniform float4 _invisible_ST;
            uniform float _alpha_blend;
            uniform float _desaturate;
            uniform float4 _TintColor;
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
                float2 node_1513 = i.uv0;
                float4 node_7 = tex2D(_image,TRANSFORM_TEX(node_1513.rg, _image));
                float node_33 = dot(node_7.rgb,float3(0.3,0.59,0.11));
                finalColor += diffuseLight * (_TintColor.rgb*lerp(node_7.rgb,float3(node_33,node_33,node_33),_desaturate));
/// Final Color:
                return fixed4(finalColor * lerp(tex2D(_invisible,TRANSFORM_TEX(node_1513.rg, _invisible)).a,tex2D(_visible,TRANSFORM_TEX(node_1513.rg, _visible)).a,_alpha_blend),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
