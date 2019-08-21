Shader "_old_Sprites/Default"
{
  Properties
  {
      _MainTex ("Sprite Texture",2D) = "white"{}
      _Color ("Tint",Color) = (1,1,1,1)
      _RendererColor ("RendererColor",Color) = (1,1,1,1)
      _AlphaTex ("External Alpha",2D) = "white"{}
  }
  SubShader
  {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 0
      Pass // ind: 1, name: 
      {
        Tags
        { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        }
        LOD 0
        // m_ProgramMask = 6
        CGPROGRAM
#pragma target 3.0
#pragma vertex vertexShader
#pragma fragment fragmentShader
#pragma multi_compile ETC1_EXTERNAL_ALPHA PIXELSNAP_ON

        // ======================
        // SerializedProgram: progVertex

        // ++++++++++++++++
        // Sub Program: 1 for m_GpuProgramType: #5
        // m_BlobIndex: 0
        // m_ShaderHardwareTier: 0


        //  m_ShaderRequirements: 1
        float4 vertexShader(float4 v:POSITION) : SV_POSITION
        {
            return UnityObjectToClipPos(v); // IS STUB!!!!
        }
        //
        // ++++++++++++++++

        // ++++++++++++++++
        // Sub Program: 4 for m_GpuProgramType: #5
        // m_BlobIndex: 1
        // m_ShaderHardwareTier: 0

#ifdef ETC1_EXTERNAL_ALPHA

        //  m_ShaderRequirements: 1
        float4 vertexShader(float4 v:POSITION) : SV_POSITION
        {
            return UnityObjectToClipPos(v); // IS STUB!!!!
        }
#endif // ETC1_EXTERNAL_ALPHA
        //
        // ++++++++++++++++

        // ++++++++++++++++
        // Sub Program: 7 for m_GpuProgramType: #5
        // m_BlobIndex: 2
        // m_ShaderHardwareTier: 0

#ifdef PIXELSNAP_ON

        //  m_ShaderRequirements: 1
        float4 vertexShader(float4 v:POSITION) : SV_POSITION
        {
            return UnityObjectToClipPos(v); // IS STUB!!!!
        }
#endif // PIXELSNAP_ON
        //
        // ++++++++++++++++

        // ++++++++++++++++
        // Sub Program: 10 for m_GpuProgramType: #5
        // m_BlobIndex: 3
        // m_ShaderHardwareTier: 0

#ifdef ETC1_EXTERNAL_ALPHA
#ifdef PIXELSNAP_ON

        //  m_ShaderRequirements: 1
        float4 vertexShader(float4 v:POSITION) : SV_POSITION
        {
            return UnityObjectToClipPos(v); // IS STUB!!!!
        }
#endif // PIXELSNAP_ON
#endif // ETC1_EXTERNAL_ALPHA
        //
        // ++++++++++++++++

        //
        // ======================


        // ======================
        // SerializedProgram: progFragment

        // ++++++++++++++++
        // Sub Program: 1 for m_GpuProgramType: #5
        // m_BlobIndex: 4
        // m_ShaderHardwareTier: 0


        //  m_ShaderRequirements: 1
        fixed4 fragmentShader() : SV_Target
        {
            return fixed4(1,1,1,1); // IS STUB!!!!
        }
        //
        // ++++++++++++++++

        // ++++++++++++++++
        // Sub Program: 4 for m_GpuProgramType: #5
        // m_BlobIndex: 5
        // m_ShaderHardwareTier: 0

#ifdef ETC1_EXTERNAL_ALPHA

        //  m_ShaderRequirements: 1
        fixed4 fragmentShader() : SV_Target
        {
            return fixed4(1,1,1,1); // IS STUB!!!!
        }
#endif // ETC1_EXTERNAL_ALPHA
        //
        // ++++++++++++++++

        // ++++++++++++++++
        // Sub Program: 7 for m_GpuProgramType: #5
        // m_BlobIndex: 6
        // m_ShaderHardwareTier: 0

#ifdef PIXELSNAP_ON

        //  m_ShaderRequirements: 1
        fixed4 fragmentShader() : SV_Target
        {
            return fixed4(1,1,1,1); // IS STUB!!!!
        }
#endif // PIXELSNAP_ON
        //
        // ++++++++++++++++

        // ++++++++++++++++
        // Sub Program: 10 for m_GpuProgramType: #5
        // m_BlobIndex: 7
        // m_ShaderHardwareTier: 0

#ifdef ETC1_EXTERNAL_ALPHA
#ifdef PIXELSNAP_ON

        //  m_ShaderRequirements: 1
        fixed4 fragmentShader() : SV_Target
        {
            return fixed4(1,1,1,1); // IS STUB!!!!
        }
#endif // PIXELSNAP_ON
#endif // ETC1_EXTERNAL_ALPHA
        //
        // ++++++++++++++++

        //
        // ======================


        ENDCG
      } // end phase
  }
  FallBack ""
  /* Disabemble: 
   https://blogs.unity3d.com/ru/2015/08/27/plans-for-graphics-features-deprecation/


 Block#0 Platform: 5 raw_size: 5836
0800000078050000F803000070090000700500004400000034050000980F0000AC060000441600003C000000E00E000054000000801600004C000000340F000064000000324F040C0500000002000000010000000000000000000000010000000C000000504958454C534E41505F4F4EE80400002376657273696F6E203130300A0A236966646566205645525445580A6174747269627574652076656334205F676C65735665727465783B0A6174747269627574652076656334205F676C6573436F6C6F723B0A6174747269627574652076656334205F676C65734D756C7469546578436F6F7264303B0A756E69666F726D2068696768702076656334205F53637265656E506172616D733B0A756E69666F726D206869676870206D61743420756E6974795F4F626A656374546F576F726C643B0A756E69666F726D206869676870206D61743420756E6974795F4D617472697856503B0A756E69666F726D206C6F77702076656334205F52656E6465726572436F6C6F723B0A756E69666F726D206C6F77702076656334205F436F6C6F723B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420746D707661725F313B0A20206869676870207665633420746D707661725F323B0A20206869676870207665633420746D707661725F333B0A2020746D707661725F332E77203D20312E303B0A2020746D707661725F332E78797A203D205F676C65735665727465782E78797A3B0A2020746D707661725F32203D2028756E6974795F4D61747269785650202A2028756E6974795F4F626A656374546F576F726C64202A20746D707661725F3329293B0A2020746D707661725F31203D2028285F676C6573436F6C6F72202A205F436F6C6F7229202A205F52656E6465726572436F6C6F72293B0A20206869676870207665633420706F735F343B0A2020706F735F342E7A77203D20746D707661725F322E7A773B0A20206869676870207665633220746D707661725F353B0A2020746D707661725F35203D20285F53637265656E506172616D732E7879202A20302E35293B0A2020706F735F342E7879203D202828666C6F6F72280A20202020282828746D707661725F322E7879202F20746D707661725F322E7729202A20746D707661725F3529202B207665633228302E352C20302E3529290A202029202F20746D707661725F3529202A20746D707661725F322E77293B0A2020676C5F506F736974696F6E203D20706F735F343B0A2020786C765F434F4C4F52203D20746D707661725F313B0A2020786C765F544558434F4F524430203D205F676C65734D756C7469546578436F6F7264302E78793B0A7D0A0A0A23656E6469660A23696664656620465241474D454E540A756E69666F726D2073616D706C65723244205F4D61696E5465783B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420635F313B0A20206C6F7770207665633420746D707661725F323B0A2020746D707661725F32203D202874657874757265324420285F4D61696E5465782C20786C765F544558434F4F52443029202A20786C765F434F4C4F52293B0A2020635F312E77203D20746D707661725F322E773B0A2020635F312E78797A203D2028746D707661725F322E78797A202A20746D707661725F322E77293B0A2020676C5F46726167446174615B305D203D20635F313B0A7D0A0A0A23656E6469660A0D000000000000000100000000000000000000000000000000000000324F040C050000000200000001000000000000000000000000000000BB0300002376657273696F6E203130300A0A236966646566205645525445580A6174747269627574652076656334205F676C65735665727465783B0A6174747269627574652076656334205F676C6573436F6C6F723B0A6174747269627574652076656334205F676C65734D756C7469546578436F6F7264303B0A756E69666F726D206869676870206D61743420756E6974795F4F626A656374546F576F726C643B0A756E69666F726D206869676870206D61743420756E6974795F4D617472697856503B0A756E69666F726D206C6F77702076656334205F52656E6465726572436F6C6F723B0A756E69666F726D206C6F77702076656334205F436F6C6F723B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420746D707661725F313B0A20206869676870207665633420746D707661725F323B0A2020746D707661725F322E77203D20312E303B0A2020746D707661725F322E78797A203D205F676C65735665727465782E78797A3B0A2020746D707661725F31203D2028285F676C6573436F6C6F72202A205F436F6C6F7229202A205F52656E6465726572436F6C6F72293B0A2020676C5F506F736974696F6E203D2028756E6974795F4D61747269785650202A2028756E6974795F4F626A656374546F576F726C64202A20746D707661725F3229293B0A2020786C765F434F4C4F52203D20746D707661725F313B0A2020786C765F544558434F4F524430203D205F676C65734D756C7469546578436F6F7264302E78793B0A7D0A0A0A23656E6469660A23696664656620465241474D454E540A756E69666F726D2073616D706C65723244205F4D61696E5465783B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420635F313B0A20206C6F7770207665633420746D707661725F323B0A2020746D707661725F32203D202874657874757265324420285F4D61696E5465782C20786C765F544558434F4F52443029202A20786C765F434F4C4F52293B0A2020635F312E77203D20746D707661725F322E773B0A2020635F312E78797A203D2028746D707661725F322E78797A202A20746D707661725F322E77293B0A2020676C5F46726167446174615B305D203D20635F313B0A7D0A0A0A23656E6469660A000D000000000000000100000000000000000000000000000000000000324F040C05000000030000000200000000000000000000000100000013000000455443315F45585445524E414C5F414C504841001B0500002376657273696F6E203130300A0A236966646566205645525445580A6174747269627574652076656334205F676C65735665727465783B0A6174747269627574652076656334205F676C6573436F6C6F723B0A6174747269627574652076656334205F676C65734D756C7469546578436F6F7264303B0A756E69666F726D206869676870206D61743420756E6974795F4F626A656374546F576F726C643B0A756E69666F726D206869676870206D61743420756E6974795F4D617472697856503B0A756E69666F726D206C6F77702076656334205F52656E6465726572436F6C6F723B0A756E69666F726D206C6F77702076656334205F436F6C6F723B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420746D707661725F313B0A20206869676870207665633420746D707661725F323B0A2020746D707661725F322E77203D20312E303B0A2020746D707661725F322E78797A203D205F676C65735665727465782E78797A3B0A2020746D707661725F31203D2028285F676C6573436F6C6F72202A205F436F6C6F7229202A205F52656E6465726572436F6C6F72293B0A2020676C5F506F736974696F6E203D2028756E6974795F4D61747269785650202A2028756E6974795F4F626A656374546F576F726C64202A20746D707661725F3229293B0A2020786C765F434F4C4F52203D20746D707661725F313B0A2020786C765F544558434F4F524430203D205F676C65734D756C7469546578436F6F7264302E78793B0A7D0A0A0A23656E6469660A23696664656620465241474D454E540A756E69666F726D20686967687020666C6F6174205F456E61626C6545787465726E616C416C7068613B0A756E69666F726D2073616D706C65723244205F4D61696E5465783B0A756E69666F726D2073616D706C65723244205F416C7068615465783B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420635F313B0A20206C6F7770207665633420636F6C6F725F323B0A20206C6F7770207665633420746D707661725F333B0A2020746D707661725F33203D2074657874757265324420285F4D61696E5465782C20786C765F544558434F4F524430293B0A2020636F6C6F725F322E78797A203D20746D707661725F332E78797A3B0A20206C6F7770207665633420746D707661725F343B0A2020746D707661725F34203D2074657874757265324420285F416C7068615465782C20786C765F544558434F4F524430293B0A2020686967687020666C6F617420746D707661725F353B0A2020746D707661725F35203D206D69782028746D707661725F332E772C20746D707661725F342E782C205F456E61626C6545787465726E616C416C706861293B0A2020636F6C6F725F322E77203D20746D707661725F353B0A20206C6F7770207665633420746D707661725F363B0A2020746D707661725F36203D2028636F6C6F725F32202A20786C765F434F4C4F52293B0A2020635F312E77203D20746D707661725F362E773B0A2020635F312E78797A203D2028746D707661725F362E78797A202A20746D707661725F362E77293B0A2020676C5F46726167446174615B305D203D20635F313B0A7D0A0A0A23656E6469660A000D000000000000000100000000000000000000000000000000000000324F040C05000000000000000000000000000000000000000100000013000000455443315F45585445524E414C5F414C504841000000000000000000000000000100000000000000000000000000000000000000324F040C05000000000000000000000000000000000000000200000013000000455443315F45585445524E414C5F414C504841000C000000504958454C534E41505F4F4E0000000000000000000000000100000000000000000000000000000000000000324F040C05000000030000000200000000000000000000000200000013000000455443315F45585445524E414C5F414C504841000C000000504958454C534E41505F4F4E480600002376657273696F6E203130300A0A236966646566205645525445580A6174747269627574652076656334205F676C65735665727465783B0A6174747269627574652076656334205F676C6573436F6C6F723B0A6174747269627574652076656334205F676C65734D756C7469546578436F6F7264303B0A756E69666F726D2068696768702076656334205F53637265656E506172616D733B0A756E69666F726D206869676870206D61743420756E6974795F4F626A656374546F576F726C643B0A756E69666F726D206869676870206D61743420756E6974795F4D617472697856503B0A756E69666F726D206C6F77702076656334205F52656E6465726572436F6C6F723B0A756E69666F726D206C6F77702076656334205F436F6C6F723B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420746D707661725F313B0A20206869676870207665633420746D707661725F323B0A20206869676870207665633420746D707661725F333B0A2020746D707661725F332E77203D20312E303B0A2020746D707661725F332E78797A203D205F676C65735665727465782E78797A3B0A2020746D707661725F32203D2028756E6974795F4D61747269785650202A2028756E6974795F4F626A656374546F576F726C64202A20746D707661725F3329293B0A2020746D707661725F31203D2028285F676C6573436F6C6F72202A205F436F6C6F7229202A205F52656E6465726572436F6C6F72293B0A20206869676870207665633420706F735F343B0A2020706F735F342E7A77203D20746D707661725F322E7A773B0A20206869676870207665633220746D707661725F353B0A2020746D707661725F35203D20285F53637265656E506172616D732E7879202A20302E35293B0A2020706F735F342E7879203D202828666C6F6F72280A20202020282828746D707661725F322E7879202F20746D707661725F322E7729202A20746D707661725F3529202B207665633228302E352C20302E3529290A202029202F20746D707661725F3529202A20746D707661725F322E77293B0A2020676C5F506F736974696F6E203D20706F735F343B0A2020786C765F434F4C4F52203D20746D707661725F313B0A2020786C765F544558434F4F524430203D205F676C65734D756C7469546578436F6F7264302E78793B0A7D0A0A0A23656E6469660A23696664656620465241474D454E540A756E69666F726D20686967687020666C6F6174205F456E61626C6545787465726E616C416C7068613B0A756E69666F726D2073616D706C65723244205F4D61696E5465783B0A756E69666F726D2073616D706C65723244205F416C7068615465783B0A76617279696E67206C6F7770207665633420786C765F434F4C4F523B0A76617279696E67206869676870207665633220786C765F544558434F4F5244303B0A766F6964206D61696E2028290A7B0A20206C6F7770207665633420635F313B0A20206C6F7770207665633420636F6C6F725F323B0A20206C6F7770207665633420746D707661725F333B0A2020746D707661725F33203D2074657874757265324420285F4D61696E5465782C20786C765F544558434F4F524430293B0A2020636F6C6F725F322E78797A203D20746D707661725F332E78797A3B0A20206C6F7770207665633420746D707661725F343B0A2020746D707661725F34203D2074657874757265324420285F416C7068615465782C20786C765F544558434F4F524430293B0A2020686967687020666C6F617420746D707661725F353B0A2020746D707661725F35203D206D69782028746D707661725F332E772C20746D707661725F342E782C205F456E61626C6545787465726E616C416C706861293B0A2020636F6C6F725F322E77203D20746D707661725F353B0A20206C6F7770207665633420746D707661725F363B0A2020746D707661725F36203D2028636F6C6F725F32202A20786C765F434F4C4F52293B0A2020635F312E77203D20746D707661725F362E773B0A2020635F312E78797A203D2028746D707661725F362E78797A202A20746D707661725F362E77293B0A2020676C5F46726167446174615B305D203D20635F313B0A7D0A0A0A23656E6469660A0D000000000000000100000000000000000000000000000000000000324F040C0500000000000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000324F040C0500000000000000000000000000000000000000010000000C000000504958454C534E41505F4F4E0000000000000000000000000100000000000000000000000000000000000000
  */
}