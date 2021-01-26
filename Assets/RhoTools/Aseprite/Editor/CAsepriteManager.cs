using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhoTools.Aseprite
{

    [InitializeOnLoad]
    public class CAsepriteManager : Editor
    {
        static CAsepriteManager()
        {
            
        }

        static void DragCallback()
        {
            if (Event.current == null)
                return;
            
            // happens when an acceptable item is released over the GUI window
            if (Event.current.type == EventType.DragPerform)
            {
                Debug.Log("Dragging asset");
                // get all the drag and drop information ready for processing.
                DragAndDrop.AcceptDrag();
                // used to emulate selection of new objects.
                var selectedObjects = new List<GameObject>();
                // run through each object that was dragged in.
                foreach (var objectRef in DragAndDrop.objectReferences)
                {
                    // if the object is the particular asset type...
                    if (objectRef is CAsepriteObject)
                    {
                        // we create a new GameObject using the asset's name.
                        var gameObject = new GameObject(objectRef.name);
                        // we attach component X, associated with asset X.
                       // var componentX = gameObject.AddComponent<ComponentX>();
                        // we place asset X within component X.
                       // componentX.assetX = objectRef as CAsepriteObject;
                        // add to the list of selected objects.
                        selectedObjects.Add(gameObject);
                    }
                }
                // we didn't drag any assets of type AssetX, so do nothing.
                if (selectedObjects.Count == 0) return;
                // emulate selection of newly created objects.
                Selection.objects = selectedObjects.ToArray();

                // make sure this call is the only one that processes the event.
                Event.current.Use();
            }
        }
    }
}
