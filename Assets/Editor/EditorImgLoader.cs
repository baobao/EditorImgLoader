using UnityEngine;
using UnityEditor;

/// <summary>
/// エディタ拡張で非同期処理サンプル
/// </summary>
public class EditorImgLoader : EditorWindow 
{
    [MenuItem("Window/EditorImgLoader")]
    static void Init ()
    {
        var w = EditorWindow.GetWindow<EditorImgLoader>();
        w.titleContent = new GUIContent("EditorImgLoader");
        w.Show();
    }

    string m_imgURL = "https://upload.wikimedia.org/wikipedia/commons/8/8a/Official_unity_logo.png";
    WWW w;
    Texture2D tex;

    #region Unity Method

    void OnEnable ()
    {
        EditorApplication.update += OnUpdate;
    }

    void OnDisable () 
    {
        EditorApplication.update -= OnUpdate;
        EditorUtility.ClearProgressBar();
        Clear();
    }

    void OnGUI ()
    {
        m_imgURL = EditorGUILayout.TextField("画像URL", m_imgURL);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load", GUILayout.Width(100f), GUILayout.Height(40f)))
        {
            w = new WWW(m_imgURL);
        }

        if (GUILayout.Button("Reset", GUILayout.Width(100f), GUILayout.Height(40f)))
        {
            Clear();
        }
        GUILayout.EndHorizontal();

        if (tex != null)
        {
            EditorGUI.DrawTextureTransparent(new Rect(0, 70f, tex.width * 0.5f, tex.height * 0.5f), tex);
        }
    }

    #endregion

    void Loading ()
    {
        EditorUtility.DisplayProgressBar("Loading...", "画像をロードしています", w.progress);

        w.MoveNext();
        if (w.isDone)
        {
            tex = w.texture;
            w = null;
            EditorUtility.ClearProgressBar();
        }
    }

    void OnUpdate()
    {
        if (w != null)
            Loading();
    }

    void Clear ()
    {
        if(tex != null)
            DestroyImmediate(tex);
        if(w != null)
            w.Dispose();
        w = null;
    }
}
