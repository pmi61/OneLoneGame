using UnityEngine;
using System.Collections;
using UnityEditor;
//[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : MonoBehaviour{

	//void OnSceneGUI() {
	//	FieldOfView fow = (FieldOfView)target;
	//	Handles.color = Color.white;
	//	Handles.DrawWireArc (fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);
	//	Vector3 viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2, false);
	//	Vector3 viewAngleB = fow.DirFromAngle (fow.viewAngle / 2, false);

 //       viewAngleA.y = viewAngleA.z;
 //       viewAngleA.z = 0;

 //       viewAngleB.y = viewAngleB.z;
 //       viewAngleB.z = 0;
        
	//	Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
	//	Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

	//	Handles.color = Color.red;
	//	foreach (Transform visibleTarget in fow.visibleTargets) {
	//		Handles.DrawLine (fow.transform.position, visibleTarget.position);
	//	}
	//}

}
