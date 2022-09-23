using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MTreeExporterMonoCore))]
class TreeExporterGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (MTreeExporterMonoCore)target;
        
        if (GUILayout.Button("Execute Exportation"))
        {
            script.executeExportation();
        }
    }
}