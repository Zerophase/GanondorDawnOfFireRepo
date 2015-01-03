using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(MasterCharacterController)), CanEditMultipleObjects]
public class CustomEditorCharController : Editor 
{
    private static Sprite[] totalSprites;
    private static string[] names;

    public static void threeD_Options()
    {
        remove2DController();

        createThirdPersonController();
    }

    private static void createThirdPersonController()
    {
        GameObject character = characterCapsule();
        createForward(character);
        createTargetLookAt(character);
        setUpMainCamera();
    }

    private static void setUpMainCamera()
    {
        if (Camera.main && 
            Camera.main.gameObject.GetComponent<ThirdPerson_Camera>() == null)
        {
            Camera.main.gameObject.AddComponent<ThirdPerson_Camera>();
        }
    }

    private static void createTargetLookAt(GameObject characterCapsule)
    {
        GameObject targetLookAt;
        if (GameObject.Find("targetLookAt") == null)
        {
            targetLookAt = new GameObject();
            targetLookAt.name = "targetLookAt";
            targetLookAt.transform.parent = characterCapsule.transform;
            targetLookAt.transform.localPosition = Vector3.zero;
        }
    }
    private static void createForward(GameObject characterCapsule)
    {
        GameObject forward;
        if (GameObject.Find("Forward") == null)
        {
            forward = GameObject.CreatePrimitive(PrimitiveType.Cube);
            forward.name = "Forward";
            DestroyImmediate(forward.GetComponent<BoxCollider>());
            forward.transform.parent = characterCapsule.transform;
            forward.transform.localPosition = new Vector3(0f, 0.5461181f, 0.728406f);
            forward.transform.localScale = new Vector3(0.2813494f, 0.2509388f, 0.9887459f);
        }
    }

    private static GameObject characterCapsule()
    {
        GameObject characterCapsule;
        if (GameObject.Find("CharacterCapsule") == null)
        {
            characterCapsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            characterCapsule.name = "CharacterCapsule";

            characterCapsule.AddComponent<CharacterController>();
            characterCapsule.AddComponent<ThirdPerson_Controller>();
            characterCapsule.AddComponent<ThirdPerson_Motor>();

            characterCapsule.transform.position = new Vector3(14.3024f, 1.776129f, 7.709145f);
            characterCapsule.transform.localScale = new Vector3(0.5f, 0.835f, 0.5f);

            characterCapsule.tag = "Player";
            return characterCapsule;
        }

        return null;
    }

    private static void remove2DController()
    {
        if (GameObject.Find("CharacterController"))
        {
            GameObject.Find("CharacterController/MainCamera").transform.parent = null;
            DestroyImmediate(GameObject.Find("CharacterController"));
        }
    }

    public static void twoD_Options()
    {
        remove3DController();

        createPlatformerController();
    }
    
    private static void createPlatformerController()
    {
        GameObject character;
        if (GameObject.Find("CharacterController") == null)
        {
            character = new GameObject();

            character.name = "CharacterController";
            character.transform.position = new Vector2(-9.04126f, 17.25893f);

            initializeSprites();
            addSprite(character);

            addRigidBody2D(character);

            addBoxCollider2D(ref character, new Vector2(0.8f, 1.09f), new Vector2(-0.07f, 0.08f));
            addBoxCollider2D(ref character, new Vector2(0.85f, 0.2f), new Vector2(0.0f, -0.53f));

            addAnimator(ref character);

            addCharacter(ref character);

            attachCamera(character);

            Physics2D.gravity = new Vector2(0.0f, -30.0f);
        }
    }

    private static void attachCamera(GameObject character)
    {
        Camera.main.transform.parent = character.transform;
        Camera.main.transform.localPosition = new Vector3(-0.06377244f, 0.1190827f, -10.0f);
    }

    private static void addCharacter(ref GameObject character)
    {
        Character c = character.AddComponent<Character>();

        c.GroundCheck = createCheck("GroundCheck", character, new Vector2(0.02172112f, -0.5318811f));
        c.WallCheck = createCheck("WallCheck", character, new Vector2(0.1219456f, -0.0362159f));
        
        c.WhatIsGround = 1 << 10;
        c.WhatIsWall = 1 << 9;
    }

    private static Transform createCheck(string name, GameObject parent, Vector2 localPosition)
    {
        GameObject g = new GameObject(name);
        g.transform.parent = parent.transform;
        g.transform.localPosition = localPosition;

        createLabel(g);

        return g.transform;
    }

    private static void createLabel(GameObject g)
    {
        Texture2D icon = Resources.Load<Texture2D>("Textures/Icon");
        var editorGUIUtility = typeof(EditorGUIUtility);
        BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
        object[] args = new object[] { g, icon };
        editorGUIUtility.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
    }

    private static void addAnimator(ref GameObject character)
    {
        Animator animator = character.AddComponent<Animator>();
        animator.animatePhysics = true;
        animator.applyRootMotion = false;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/AnimatorController");
    }

    private static void addBoxCollider2D(ref GameObject character, Vector2 size, Vector2 center)
    {
        BoxCollider2D boxCollider2D = character.AddComponent<BoxCollider2D>();
        boxCollider2D.size = size;
        boxCollider2D.center = center;
    }

    private static void addRigidBody2D(GameObject character)
    {
        character.AddComponent<Rigidbody2D>();
        character.GetComponent<Rigidbody2D>().fixedAngle = true;
    }

    private static void addSprite(GameObject character)
    {
        character.AddComponent<SpriteRenderer>();
        character.GetComponent<SpriteRenderer>().sprite = totalSprites[Array.IndexOf(names, "CharacterSheet_8")];
    }

    private static void initializeSprites()
    {
        totalSprites = Resources.LoadAll<Sprite>("Sprites/CharacterSheet");
        names = new string[totalSprites.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = totalSprites[i].name;
        }
    }

    private static void remove3DController()
    {
        if (GameObject.Find("CharacterCapsule"))
        {
            DestroyImmediate(GameObject.Find("CharacterCapsule"));
            DestroyImmediate(Camera.main.GetComponent<ThirdPerson_Camera>());
        }
    }
}
