using System;

public sealed class CustomFieltypeEditorAttribute : Attribute
{
    public string[] Extensions { get; private set; }
    public bool MultiObjectEditing { get; private set; }

    /// <summary>
    /// Turns the class into an editor class for the specified filetype
    /// </summary>
    /// <param name="aExtension">Extension (Without the dot)</param>
    public CustomFieltypeEditorAttribute(params string[] aExtension)
    {
        Extensions = aExtension;
    }

    public bool HasExtension(string aExtension)
    {
        for (int i = 0; i < Extensions.Length; i++)
        {
            if (Extensions[i] == aExtension)
                return true;
        }
        return false;
    }
}
