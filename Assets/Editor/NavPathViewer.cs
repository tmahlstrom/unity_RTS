using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class NavPathViewer : Editor {

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawNavAgentPath(NavMeshAgent agent, GizmoType gizmoType){
        if (agent.path == null)
            return;

        Gizmos.color = Color.blue; 
        Vector3[] points = agent.path.corners;
        for(int i=0; i<points.Length-1; i++){
            Gizmos.DrawLine(points[i], points[i+1]);
        }

    }



}
