// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32561,y:32683|emission-45-RGB,alpha-9-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33103,y:32693,ptlb:invisible,ptin:_invisible,tex:0912c2000dc03684ab8fa95123c0fc37,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3,x:33178,y:32909,ptlb:visible,ptin:_visible,tex:a84588c5617df3f4bb34c94c6181d6eb,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:9,x:32839,y:32865|A-2-A,B-3-A,T-10-OUT;n:type:ShaderForge.SFN_ValueProperty,id:10,x:33086,y:33100,ptlb:node_wipe,ptin:_node_wipe,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:45,x:33040,y:32509,ptlb:image,ptin:_image,tex:253a1ce48002f7542b447920e66dcce6,ntxv:0,isnm:False;proporder:3-2-10-45;pass:END;sub:END;*/

Shader "Shader Forge/openNodeWipe" {
    Properties {
        _visible ("visible", 2D) = "white" {}
        _invisible ("invisible", 2D) = "white" {}
        _node_wipe ("node_wipe", Float ) = 1
        _image ("image", 2D) = "white" {}
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
            uniform sampler2D _invisible; uniform float4 _invisible_ST;
            uniform sampler2D _visible; uniform float4 _visible_ST;
            uniform float _node_wipe;
            uniform sampler2D _image; uniform float4 _image_ST;
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
                float2 node_50 = i.uv0;
                float3 emissive = tex2D(_image,TRANSFORM_TEX(node_50.rg, _image)).rgb;
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,lerp(tex2D(_invisible,TRANSFORM_TEX(node_50.rg, _invisible)).a,tex2D(_visible,TRANSFORM_TEX(node_50.rg, _visible)).a,_node_wipe));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
