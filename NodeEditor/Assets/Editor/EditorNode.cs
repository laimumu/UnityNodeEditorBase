﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The visual representation of a logic unit such as an object or function.
/// </summary>
public class EditorNode
{
    public static readonly Vector2 kDefaultSize = new Vector2(100f, 80f);
    
    /// <summary>
    /// The space reserved between knobs.
    /// </summary>
    public const float kKnobOffset = 15f;

    /// <summary>
    /// The space reserved for the header (title) of the node.
    /// </summary>
    public const float kHeaderHeight = 15f;

    /// <summary>
    /// The rect of the node in canvas space.
    /// </summary>
    public Rect bodyRect;

    /// <summary>
    /// A texture associated with the node.
    /// </summary>
    public Texture2D iconTex;

    public string name = "Node";

    /// <summary>
    /// How much additional offset to apply when resizing.
    /// </summary>
    public const float resizePaddingX = 20f;

    public readonly bool bCanHaveMultipleOutputs;

    private List<EditorOutputKnob> _outputs;
    private List<EditorInputKnob> _inputs;

    private GUIStyle _iconNameStyle;
    private GUIContent _iconNameContent;

    public EditorNode(string nodeName, Texture2D icon, Vector2 size, bool canHaveMultipleOuts = true)
    {
        name = nodeName;
        iconTex = icon;
        bodyRect.size = size;

        _outputs = new List<EditorOutputKnob>();
        _inputs = new List<EditorInputKnob>();

        bCanHaveMultipleOutputs = canHaveMultipleOuts;
    }

    public EditorInputKnob AddInput()
    {
        var input = new EditorInputKnob(this);
        _inputs.Add(input);

        return input;
    }

    public EditorOutputKnob AddOutput()
    {
        var output = new EditorOutputKnob(this);
        _outputs.Add(output);

        return output;
    }

    /// <summary>
    /// Called when the output knob had an input connection removed.
    /// </summary>
    /// <param name="removedInput"></param>
    public virtual void OnInputConnectionRemoved(EditorInputKnob removedInput) { }

    /// <summary>
    /// Called when the output knob made a connection to an input knob.
    /// </summary>
    /// <param name="addedInput"></param>
    public virtual void OnNewInputConnection(EditorInputKnob addedInput) { }

    public IEnumerable<EditorOutputKnob> Outputs
    {
        get { return _outputs; }
    }

    public IEnumerable<EditorInputKnob> Inputs
    {
        get { return _inputs; }
    }

    /// <summary>
    /// Get the Y value of the top header.
    /// </summary>
    public float HeaderTop
    {
        get { return bodyRect.yMin + kHeaderHeight; }
    }

    #region Styles and Contents

    public string NiceName
    {
        get { return ObjectNames.NicifyVariableName(name); }
    }

    public GUIContent IconNameContent
    {
        get
        {
            if (_iconNameContent == null) {
                _iconNameContent = new GUIContent(NiceName, iconTex);
            }
            return _iconNameContent;
        }
    }

    public GUIStyle IconNameStyle
    {
        get
        {
            if (_iconNameStyle == null) {
                SetupStyle();
            }

            return _iconNameStyle;
        }
    }

    /// <summary>
    /// Sets up the style to render the node.
    /// </summary>
    public void SetupStyle()
    {
        _iconNameStyle = new GUIStyle();
        _iconNameStyle.normal.textColor = Color.white;
        _iconNameStyle.alignment = TextAnchor.LowerCenter;

        _iconNameStyle.imagePosition = ImagePosition.ImageAbove;

        // Test if the name fits
        Vector2 contentSize = _iconNameStyle.CalcSize(new GUIContent(NiceName));

        // Resize width of the node body.
        if (contentSize.x > bodyRect.width - resizePaddingX) {
            bodyRect.width = contentSize.x + resizePaddingX;
        }

        _iconNameStyle.fixedHeight = bodyRect.height - 5f;
        _iconNameStyle.fixedWidth = bodyRect.width;
    }

    #endregion
}
