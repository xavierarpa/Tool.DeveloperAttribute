using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class DeveloperAttribute : PropertyAttribute{
    public readonly Enum dev = default;
    public readonly string message = "";
    public readonly DeveloperMessage messageType = DeveloperMessage.None;
    public readonly bool showVar = true;
    public readonly float height;

    public DeveloperAttribute(
        Enum dev,
        DeveloperMessage messageType = DeveloperMessage.Info,
        string message = default,
        bool showVar=true,
        float height = 20
    ){
        this.dev = dev;
        this.message = message;
        this.messageType = messageType;
        this.showVar = showVar;
        this.height = height;
    }
}


#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(DeveloperAttribute))]
public class DeveloperPropertyDrawer : PropertyDrawer
{
    private const float HEIGHT = 20f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        DeveloperAttribute developer = attribute as DeveloperAttribute;
        return HEIGHT + developer.height;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        //RangeAttribute att = attribute as RangeAttribute;
        DeveloperAttribute developer = attribute as DeveloperAttribute;

        //INIT
        Color init_contentColor = GUI.contentColor;
        Color init_color = GUI.color;
        Color init_backgroundColor  = GUI.backgroundColor;



        GUI.contentColor = Color.white;

        MessageType message = MessageType.Info;
        switch (developer.messageType)
        {
            case DeveloperMessage.None:
                GUI.backgroundColor = Color.white;
                GUI.color = Color.black;
                message = MessageType.None;
                break;
            case DeveloperMessage.Info:
                GUI.backgroundColor = Color.black;
                message = MessageType.Info;
                break;
            case DeveloperMessage.Warning:
                GUI.backgroundColor = Color.yellow;
                message = MessageType.Warning;
                break;
            default:
            //case DeveloperMessage.Error:
                GUI.backgroundColor = Color.red;
                message = MessageType.Error;
                break;
        }

        float height = developer.height;
        string devName = developer.dev.Equals(-1) ? "" : $"{developer.dev} :";
        Rect r_helpBox = new Rect(pos.position, pos.size);
        r_helpBox.height = height;
        EditorGUI.HelpBox(r_helpBox, $"{devName} {developer.message}", message);


        if (developer.showVar){
            Rect r_field = new Rect(pos.position, pos.size);
            r_field.height = height;
            r_field.y = r_helpBox.y + height;
            r_field.x = pos.x;
            EditorGUI.LabelField(r_field, label);
            EditorGUI.PropertyField(r_field, prop, label);
        }




        //RESTORE
        GUI.contentColor = init_contentColor;
        GUI.color = init_color;
        GUI.backgroundColor = init_backgroundColor;
    }
}
#endif
