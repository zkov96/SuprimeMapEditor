// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:4,bdst:5,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:720,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33425,y:32771,varname:node_3138,prsc:2|emission-608-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32734,y:32999,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Vector4Property,id:5209,x:32027,y:32695,ptovrint:False,ptlb:Position,ptin:_Position,varname:node_5209,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_TexCoord,id:5564,x:32027,y:32481,varname:node_5564,prsc:2,uv:0;n:type:ShaderForge.SFN_Distance,id:8007,x:32531,y:32675,varname:node_8007,prsc:2|A-5564-UVOUT,B-5209-XYZ;n:type:ShaderForge.SFN_OneMinus,id:76,x:32734,y:32675,varname:node_76,prsc:2|IN-7535-OUT;n:type:ShaderForge.SFN_Divide,id:7535,x:32531,y:32550,varname:node_7535,prsc:2|A-8007-OUT,B-1923-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1923,x:32027,y:32426,ptovrint:False,ptlb:Size,ptin:_Size,varname:node_1923,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Vector1,id:4336,x:32903,y:32517,varname:node_4336,prsc:2,v1:1;n:type:ShaderForge.SFN_Color,id:9437,x:32734,y:32828,ptovrint:False,ptlb:OutColor,ptin:_OutColor,varname:node_9437,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:0;n:type:ShaderForge.SFN_Lerp,id:5535,x:33077,y:32901,varname:node_5535,prsc:2|A-9437-RGB,B-7241-RGB,T-3840-OUT;n:type:ShaderForge.SFN_If,id:608,x:33227,y:32726,varname:node_608,prsc:2|A-7535-OUT,B-4336-OUT,GT-9437-RGB,EQ-5535-OUT,LT-5535-OUT;n:type:ShaderForge.SFN_Power,id:3840,x:32973,y:32675,varname:node_3840,prsc:2|VAL-76-OUT,EXP-174-OUT;n:type:ShaderForge.SFN_Slider,id:174,x:32374,y:32821,ptovrint:False,ptlb:Power,ptin:_Power,varname:node_174,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2991453,max:1;proporder:7241-5209-1923-9437-174;pass:END;sub:END;*/

Shader "Shader Forge/TestBrush" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Position ("Position", Vector) = (0,0,0,0)
        _Size ("Size", Float ) = 1
        _OutColor ("OutColor", Color) = (1,1,1,0)
        _Power ("Power", Range(0, 1)) = 0.2991453
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend DstColor DstAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float4 _Position;
            uniform float _Size;
            uniform float4 _OutColor;
            uniform float _Power;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_7535 = (distance(i.uv0,_Position.rgb)/_Size);
                float node_608_if_leA = step(node_7535,1.0);
                float node_608_if_leB = step(1.0,node_7535);
                float3 node_5535 = lerp(_OutColor.rgb,_Color.rgb,pow((1.0 - node_7535),_Power));
                float3 emissive = lerp((node_608_if_leA*node_5535)+(node_608_if_leB*_OutColor.rgb),node_5535,node_608_if_leA*node_608_if_leB);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
