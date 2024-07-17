using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MoonSharp.Interpreter;
using KSP.Rendering.Planets;

namespace WorldVis
{
    [CustomEditor(typeof(ScaledSpaceTextureGenerator))]
    class ScaledSpaceTextureGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Build Object"))
            {
                GenerateTextures();
            }
            DrawDefaultInspector();
        }

        private void GenerateTextures()
        {
            var sstg = (ScaledSpaceTextureGenerator)target;
            var pqs = FindAnyObjectByType<PQS>();

            var argsBuffer = pqs.PQSRenderer.GetField<PQSRenderer, ComputeBuffer>("_drawArgsBuffer");

            var pqsRendererDrawArgsBuffer = new uint[4];
            argsBuffer.GetData(pqsRendererDrawArgsBuffer);

            sstg.GenerateTextures(
                pqs.ScaledSpaceTextureGeneratorSettings,
                pqs.data.materialSettings.surfaceMaterial,
                null,
                (int)pqsRendererDrawArgsBuffer[0],
                0
                );
        }
    }
}
