// Fantasy Adventure Environment
// Copyright Staggart Creations
// staggart.xyz

using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using FAE;

namespace FAE
{
    [CustomEditor(typeof(PigmentMapGenerator))]
    public class PigmentMapGeneratorInspector : Editor
    {
        PigmentMapGenerator pmg;

        // Use this for initialization
        void OnEnable()
        {
            pmg = (PigmentMapGenerator)target;

        }

        private string terrainInfo;

        // Update is called once per frame
        public override void OnInspectorGUI()
        {

                if (pmg.workflow == TerrainUVUtil.Workflow.Terrain && pmg.terrain.terrainData.splatPrototypes.Length == 0)
                {
                    EditorGUILayout.HelpBox("Assign at least one texture to your terrain", MessageType.Error);
                    return;
                }

            if (pmg.workflow == TerrainUVUtil.Workflow.Terrain)
            {
                //Functionality to be implemented in a later update
                pmg.heightmapChannel = (PigmentMapGenerator.HeightmapChannel)EditorGUILayout.EnumPopup("Height source material", pmg.heightmapChannel);
                EditorGUILayout.HelpBox("This is the texture whose painted weight will determine the grass height \n\nThe effect can be controlled through the \"Heightmap influence\" parameter on the FAE/Grass shader", MessageType.None);
                EditorGUILayout.Space();
            }
            else if (pmg.workflow == TerrainUVUtil.Workflow.Mesh)
            {
                //Field to assign heightmap texture
                EditorGUILayout.LabelField("Input grass heightmap (optional)", EditorStyles.boldLabel);
                pmg.inputHeightmap = EditorGUILayout.ObjectField(pmg.inputHeightmap, typeof(Texture2D), false) as Texture2D;
                EditorGUILayout.LabelField("The texture needs its Read/Write option enabled", EditorStyles.helpBox);
                //Channel dropdown?
            }

            terrainInfo = string.Format("Terrain size: {0}x{1} \nTerrain position: X: {2} Z: {3}", pmg.targetSize.x, pmg.targetSize.z, pmg.targetPosition.x, pmg.targetPosition.z);
            terrainInfo.Replace("\\n", "\n"); //Break lines

            //EditorGUILayout.HelpBox(terrainInfo, MessageType.Info);

            //Button
            EditorGUILayout.LabelField("Renderer", EditorStyles.boldLabel);
            if (GUILayout.Button("Generate", GUILayout.Height(40f)))
            {
                pmg.Generate();
            }
            pmg.useAlternativeRenderer = EditorGUILayout.ToggleLeft("Use alternative renderer", pmg.useAlternativeRenderer);
            EditorGUILayout.LabelField("Some third-party terrain shaders require you to use this", EditorStyles.helpBox);

            EditorGUILayout.Space();

            //Pigment map preview
            EditorGUILayout.BeginHorizontal();
            if (pmg.pigmentMap)
            {
                EditorGUILayout.LabelField(string.Format("Output pigment map ({0}x{0})", pmg.pigmentMap.height), EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();

            if (pmg.pigmentMap)
            {
                Texture nothing = EditorGUILayout.ObjectField(pmg.pigmentMap, typeof(Texture), false, GUILayout.Width(150f), GUILayout.Height(150f)) as Texture;
                nothing.name = "Pigmentmap";
            }

            if (pmg.workflow == TerrainUVUtil.Workflow.Terrain)
            {
                if (pmg.hasTerrainData)
                {
                    EditorGUILayout.LabelField("The output texture file is stored next to the TerrainData asset", EditorStyles.helpBox);
                }
                else
                {
                    EditorGUILayout.LabelField("The output texture file is stored next to the scene file", EditorStyles.helpBox);
                }
            }
            else if (pmg.workflow == TerrainUVUtil.Workflow.Mesh)
            {
                EditorGUILayout.LabelField("The output texture file is stored next to the material file", EditorStyles.helpBox);
            }

            //Placeholder for realtime updates functionality
            if (GUI.changed)
            {
                Apply();
            }
        }

        private void Apply()
        {

        }

    }
}
