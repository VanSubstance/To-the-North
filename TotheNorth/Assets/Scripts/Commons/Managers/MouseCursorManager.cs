using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    private static Dictionary<MouseCursorType, Texture2D> textures = new Dictionary<MouseCursorType, Texture2D>();
    // Start is called before the first frame update
    void Start()
    {
        LoadCursors();
        GlobalComponent.Common.Event.mouseCursorManager = this;
        GlobalStatus.Loading.System.MouseCursorManager = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetMouseCursor(MouseCursorType cursorType)
    {
        Cursor.SetCursor(textures[cursorType] == null ? textures[MouseCursorType.NORMAL] : textures[cursorType], Vector2.zero, CursorMode.Auto);
    }

    private void LoadCursors()
    {
        textures[MouseCursorType.NORMAL] = Resources.Load<Texture2D>($"{PathInfo.Image.Common.MouseCursor}NORMAL");
        textures[MouseCursorType.QUESTION] = Resources.Load<Texture2D>($"{PathInfo.Image.Common.MouseCursor}QUESTION");
        textures[MouseCursorType.BUTTON] = Resources.Load<Texture2D>($"{PathInfo.Image.Common.MouseCursor}BUTTON");
    }
}
