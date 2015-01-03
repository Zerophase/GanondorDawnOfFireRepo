using UnityEngine;
using UnityEditor;
using System.Collections;

public class CustomWindow : EditorWindow
{

    [MenuItem("Character Controllers/2D/Platformer")]
    static void Platformer()
    {
        MasterCharacterController.TwoD_Camera();
        CustomEditorCharController.twoD_Options();
    }

    [MenuItem("Character Controllers/3D/Third Person")]
    static void ThirdPerson()
    {
        MasterCharacterController.ThreeD_Camera();
        CustomEditorCharController.threeD_Options();
    }
}
