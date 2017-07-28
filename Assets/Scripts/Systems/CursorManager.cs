using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    [System.Serializable]//lets unity identify custom class for scripts public variables (shows public variables on inspector)
    public class TheCursor
    {
        public string tag; //tag that indicates where the mouse is on
        public Texture2D cursorTexture;
    }

    public List<TheCursor> cursorList = new List<TheCursor>();

	// Use this for initialization
	void Start () {
        SetCursorTexture(cursorList[0].cursorTexture);
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            for(int i = 0; i < cursorList.Count; i++)
            {
                if(hit.collider.tag == cursorList[i].tag)
                {
                    SetCursorTexture(cursorList[i].cursorTexture);
                    return;
                }
            }
        }
        SetCursorTexture(cursorList[0].cursorTexture);
	}

    void SetCursorTexture(Texture2D tex)
    {
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto); //changes the texure of the cursor
    }
}
