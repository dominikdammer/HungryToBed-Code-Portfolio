using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.Reflection;
using Sirenix.Utilities;
using UnityEngine.VFX;

public class SOAttributeProcessor<T> : OdinAttributeProcessor<T> where T : ScriptableObject
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        attributes.Add(new LabelWidthAttribute(210));        

        if (member.GetReturnType() == typeof(Sprite))
        {
            attributes.Add(new PropertyOrderAttribute(-1));
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new HorizontalGroupAttribute("Character", 100));
            attributes.Add(new PreviewFieldAttribute(100, ObjectFieldAlignment.Center));
        }

        if (member.GetReturnType() == typeof(int))
        {
            attributes.Add(new RangeAttribute(0, 100));
            attributes.Add(new BoxGroupAttribute("Variables"));
        }

        if (member.Name == "health")
        {
            attributes.Add(new GUIColorAttribute("RGB(0, 1, 0)"));
            attributes.Add(new LabelTextAttribute(SdfIconType.HeartFill));
        }

        if (member.Name == "size")
        {
            attributes.Add(new VerticalGroupAttribute("Character/Right"));
        }

        if (member.Name == "dropAmount")
        {
            attributes.Add(new LabelTextAttribute(SdfIconType.DiamondFill));
        }

        if (member.GetReturnType() == typeof(string))
        {
            attributes.Add(new VerticalGroupAttribute("Character/Right"));
        }

        if (member.Name == "ammo")
        {
            attributes.Add(new PreviewFieldAttribute(100, ObjectFieldAlignment.Left));
            attributes.Add(new HorizontalGroupAttribute("Ammo", 100));
            attributes.Add(new HideLabelAttribute());
        }


        if (member.Name == "towerSize")
        {
            attributes.Add(new VerticalGroupAttribute("Character/Right", 100));
        }

        if (member.Name == "ammoSize")
        {
            attributes.Add(new VerticalGroupAttribute("Ammo/Right", 100));
        }

        if (member.GetReturnType() == typeof(float))
        {
            if (member.Name != "volume")
            {
                attributes.Add(new RangeAttribute(0, 100));
                attributes.Add(new BoxGroupAttribute("Variables"));
            }
            else
            {
                attributes.Add(new SuffixLabelAttribute("%"));
                attributes.Add(new VerticalGroupAttribute("Sound/Right"));
            }
        }



        if (member.GetReturnType() == typeof(AnimationClip))
        {
            attributes.Add(new BoxGroupAttribute("Animation"));
            attributes.Add(new PreviewFieldAttribute(30, ObjectFieldAlignment.Left));

            attributes.Add(new RequiredAttribute("Add animation"));
        }


        if (member.GetReturnType() == typeof(VisualEffect))
        {
            attributes.Add(new BoxGroupAttribute("Effect"));
            attributes.Add(new PreviewFieldAttribute(30, ObjectFieldAlignment.Left));

            attributes.Add(new RequiredAttribute("Add Effect"));
        }


        if (member.GetReturnType() == typeof(AudioClip))
        {
            attributes.Add(new BoxGroupAttribute("Sound"));
            attributes.Add(new PreviewFieldAttribute(30, ObjectFieldAlignment.Left));

            attributes.Add(new RequiredAttribute("Add Sound"));
        }


    }
}

