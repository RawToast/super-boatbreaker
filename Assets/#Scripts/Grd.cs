#if UNITY_EDITOR
using UnityEditor;
#endif

using Managers;
using UnityEngine;

static class Grd {
    public static Score Score;
    public static Lives Lives;
    public static Level Level;
    
    static Grd() {
        GameObject g = safeFind("App");

        Score = (Score)SafeComponent( g, "Score" );
        Lives = (Lives)SafeComponent( g, "Lives" );
        Level = (Level)SafeComponent(g, "Level");
        
        #if UNITY_EDITOR
            Application.LoadLevel(System.IO.Path.GetFileNameWithoutExtension(EditorPrefs.GetString("SceneAutoLoader.PreviousScene")));
        #endif
    }

    // when Grid wakes up, it checks everything is in place
    // it uses these trivial routines to do so
    private static GameObject safeFind(string s) {
        GameObject g = GameObject.Find(s);
        if (g == null) Woe("GameObject " + s + "  not on _preload.");
        return g;
    }

    private static Component SafeComponent(GameObject g, string s) {
        Component c = g.GetComponent(s);
        if (c == null) Woe("Component " + s + " not on _preload.");
        return c;
    }

    private static void Woe(string error) {
        Debug.Log(">>> Cannot proceed... " + error);
        Debug.Log(">>> It is very likely you just forgot to launch");
        Debug.Log(">>> from scene zero, the _preload scene.");
    }
    // be sure to read this:
    // http://stackoverflow.com/a/35891919/294884
    
}

