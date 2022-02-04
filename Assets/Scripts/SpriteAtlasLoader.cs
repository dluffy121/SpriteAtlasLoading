using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasLoader : MonoBehaviour
{
    const string SEPARATOR = " | ";
    const string RQA = "Requested Atlases: ";
    const string RGA = "Registered Atlases: ";

    static SpriteAtlasLoader instance;
    public static SpriteAtlasLoader Instance => instance;

    Dictionary<string, Action<SpriteAtlas>> m_requestedAtlases = null;

    [SerializeField]
    List<SpriteAtlas> m_loadedAtlases = null;

    void OnEnable()
    {
        if(instance == null)
            instance = this;

        SpriteAtlasManager.atlasRequested += OnAtlasRequested;
        SpriteAtlasManager.atlasRegistered += OnAtlasRegistered;
    }

    void OnDisable()
    {
        SpriteAtlasManager.atlasRequested -= OnAtlasRequested;
        SpriteAtlasManager.atlasRegistered -= OnAtlasRegistered;

        instance = null;
    }

    void OnAtlasRequested(string spriteAtlasName, Action<SpriteAtlas> callback)
    {
        Debug.Log("Requested: " + spriteAtlasName);

        if (m_requestedAtlases == null)
            m_requestedAtlases = new Dictionary<string, Action<SpriteAtlas>>();

        m_requestedAtlases.Add(spriteAtlasName, callback);

        UpdateLogs();
    }

    void OnAtlasRegistered(SpriteAtlas spriteAtlas)
    {
        Debug.Log("Registered: " + spriteAtlas.name);

        if (m_loadedAtlases == null)
            m_loadedAtlases = new List<SpriteAtlas>();
        m_loadedAtlases.Add(spriteAtlas);

        UpdateLogs();
    }

    public void LoadAtlas(string spriteAtlasName)
    {
        if (string.IsNullOrEmpty(spriteAtlasName))
            return;
        if (m_requestedAtlases == null || !m_requestedAtlases.ContainsKey(spriteAtlasName))
            return;

        SpriteAtlas atlas = Resources.Load<SpriteAtlas>(spriteAtlasName);
        if (atlas == null)
            return;

        m_requestedAtlases[spriteAtlasName].Invoke(atlas);

        m_requestedAtlases.Remove(spriteAtlasName);

        UpdateLogs();
    }

    #region DebugLogging

    string m_strRequestedAtlas = null;
    string m_strRegisteredAtlas = null;

    private void UpdateLogs()
    {
        StringBuilder l_builder = new StringBuilder();
        foreach (var atlas in m_requestedAtlases.Keys)
        {
            l_builder.Append(atlas);
            l_builder.Append(SEPARATOR);
        }
        m_strRequestedAtlas = RQA + l_builder.ToString();
        l_builder.Clear();

        foreach (var atlas in m_loadedAtlases)
        {
            l_builder.Append(atlas.name);
            l_builder.Append(SEPARATOR);
        }
        m_strRegisteredAtlas = RGA + l_builder.ToString();
        l_builder.Clear();
    }

    void OnGUI()
    {
        GUIStyle l_textStyle = GUI.skin.textField;
        l_textStyle.fontSize = 45;
        GUI.TextArea(new Rect(50, Screen.height - 50 - 75 - 75 - 5, Screen.width - 100, 75), m_strRequestedAtlas, l_textStyle);
        GUI.TextArea(new Rect(50, Screen.height - 50 - 75, Screen.width - 100, 75), m_strRegisteredAtlas, l_textStyle);
    }

    #endregion
}
