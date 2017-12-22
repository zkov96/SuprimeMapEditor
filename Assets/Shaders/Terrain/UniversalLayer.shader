// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33451,y:32697,varname:node_9361,prsc:2|normal-7327-RGB,emission-245-OUT,custl-9638-OUT;n:type:ShaderForge.SFN_Tex2d,id:7327,x:32124,y:32517,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:_Normal,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:56c0c3cee92bb8544affd92e4269948f,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:205,x:32124,y:32696,ptovrint:False,ptlb:Albedo,ptin:_Albedo,varname:node_205,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c252c3f5af7a0bb4a8b0439a9a0b6630,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:1738,x:32124,y:32877,ptovrint:False,ptlb:AlbedoColor,ptin:_AlbedoColor,varname:node_1738,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4980392,c2:0.4980392,c3:0.4980392,c4:1;n:type:ShaderForge.SFN_Multiply,id:1470,x:32378,y:32857,varname:node_1470,prsc:2|A-205-RGB,B-1738-RGB;n:type:ShaderForge.SFN_AmbientLight,id:7031,x:32378,y:32738,varname:node_7031,prsc:2;n:type:ShaderForge.SFN_Multiply,id:245,x:32590,y:32798,varname:node_245,prsc:2|A-7031-RGB,B-1470-OUT;n:type:ShaderForge.SFN_NormalVector,id:4707,x:31839,y:32982,prsc:2,pt:False;n:type:ShaderForge.SFN_LightVector,id:6851,x:31838,y:33118,varname:node_6851,prsc:2;n:type:ShaderForge.SFN_ViewReflectionVector,id:2477,x:31839,y:33235,varname:node_2477,prsc:2;n:type:ShaderForge.SFN_Dot,id:3335,x:32124,y:33032,varname:node_3335,prsc:2,dt:1|A-4707-OUT,B-6851-OUT;n:type:ShaderForge.SFN_Dot,id:5969,x:32122,y:33194,varname:node_5969,prsc:2,dt:1|A-6851-OUT,B-2477-OUT;n:type:ShaderForge.SFN_RemapRange,id:2090,x:31840,y:33353,varname:node_2090,prsc:2,frmn:0,frmx:1,tomn:1,tomx:11|IN-1377-OUT;n:type:ShaderForge.SFN_Slider,id:1377,x:31474,y:33353,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_1377,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4273504,max:1;n:type:ShaderForge.SFN_Exp,id:3353,x:32122,y:33351,varname:node_3353,prsc:2,et:1|IN-2090-OUT;n:type:ShaderForge.SFN_Power,id:5213,x:32381,y:33187,varname:node_5213,prsc:2|VAL-5969-OUT,EXP-3353-OUT;n:type:ShaderForge.SFN_Multiply,id:1828,x:32590,y:33014,varname:node_1828,prsc:2|A-1470-OUT,B-3335-OUT;n:type:ShaderForge.SFN_Multiply,id:9416,x:32590,y:33159,varname:node_9416,prsc:2|A-5213-OUT,B-2619-RGB;n:type:ShaderForge.SFN_Color,id:2619,x:32375,y:33351,ptovrint:False,ptlb:SpecColor,ptin:_SpecColor,varname:node_2619,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:6633,x:32795,y:33141,varname:node_6633,prsc:2|A-1828-OUT,B-9416-OUT;n:type:ShaderForge.SFN_Multiply,id:9638,x:33114,y:33240,varname:node_9638,prsc:2|A-6633-OUT,B-2431-RGB,C-3488-OUT;n:type:ShaderForge.SFN_LightColor,id:2431,x:32791,y:33258,varname:node_2431,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:3488,x:32783,y:33377,varname:node_3488,prsc:2;proporder:7327-205-1738-1377-2619;pass:END;sub:END;*/

Shader "Shader Forge/UniversalLayer" {
    Properties {
        _Normal ("Normal", 2D) = "bump" {}
        _Albedo ("Albedo", 2D) = "white" {}
        _AlbedoColor ("AlbedoColor", Color) = (0.4980392,0.4980392,0.4980392,1)
        _Gloss ("Gloss", Range(0, 1)) = 0.4273504
        _SpecColor ("SpecColor", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform float4 _AlbedoColor;
            uniform float _Gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_1470 = (_Albedo_var.rgb*_AlbedoColor.rgb);
                float3 emissive = (UNITY_LIGHTMODEL_AMBIENT.rgb*node_1470);
                float3 finalColor = emissive + (((node_1470*max(0,dot(i.normalDir,lightDirection)))+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2((_Gloss*10.0+1.0)))*_SpecColor.rgb))*_LightColor0.rgb*attenuation);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Albedo; uniform float4 _Albedo_ST;
            uniform float4 _AlbedoColor;
            uniform float _Gloss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Albedo_var = tex2D(_Albedo,TRANSFORM_TEX(i.uv0, _Albedo));
                float3 node_1470 = (_Albedo_var.rgb*_AlbedoColor.rgb);
                float3 finalColor = (((node_1470*max(0,dot(i.normalDir,lightDirection)))+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2((_Gloss*10.0+1.0)))*_SpecColor.rgb))*_LightColor0.rgb*attenuation);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
