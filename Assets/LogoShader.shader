// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32547,y:32671|emission-2-RGB,alpha-8-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:32985,y:32688,ptlb:image,ptin:_image,tex:deb50e372bee54c2596e17acfea28943,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7,x:32985,y:32872,ptlb:node_7,ptin:_node_7,tex:10350849f8eb00847b6e7aab7bb62aa3,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:8,x:32780,y:32889|A-7-A,B-2-A,T-10-OUT;n:type:ShaderForge.SFN_ValueProperty,id:10,x:32935,y:33102,ptlb:alpha_blend,ptin:_alpha_blend,glob:False,v1:0;proporder:2-7-10;pass:END;sub:END;*/

Shader "Shader Forge/LogoShader" {
    Properties {
        _image ("image", 2D) = "white" {}
        _node_7 ("node_7", 2D) = "white" {}
        _alpha_blend ("alpha_blend", Float ) = 0
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
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _image; uniform float4 _image_ST;
            uniform sampler2D _node_7; uniform float4 _node_7_ST;
            uniform float _alpha_blend;
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
                float2 node_19 = i.uv0;
                float4 node_2 = tex2D(_image,TRANSFORM_TEX(node_19.rg, _image));
                float3 emissive = node_2.rgb;
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,lerp(tex2D(_node_7,TRANSFORM_TEX(node_19.rg, _node_7)).a,node_2.a,_alpha_blend));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
