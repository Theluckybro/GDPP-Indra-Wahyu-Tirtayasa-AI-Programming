var log = new System.Text.StringBuilder();
UnityEngine.GameObject Find(string path)
{
    var go = UnityEngine.GameObject.Find(path);
    if (go != null) return go;
    foreach (var t in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.Transform>())
        if (t.gameObject.scene.IsValid() && UnityEditor.AnimationUtility.CalculateTransformPath(t, null) == path) return t.gameObject;
    throw new System.Exception("not found: " + path);
}
UnityEditor.Undo.IncrementCurrentGroup();
UnityEditor.Undo.SetCurrentGroupName("Claude playtest fixes");

// 1. Crosshair image reference (null -> NullReferenceException every frame, Interact HUD never shown)
{
    var ui = Find("HUDManager/CrosshairUI").GetComponent<CrosshairUI>();
    var img = Find("HUDCanvas/Crosshair").GetComponent<UnityEngine.UI.Image>();
    var so = new UnityEditor.SerializedObject(ui);
    so.FindProperty("_crosshairImage").objectReferenceValue = img;
    so.ApplyModifiedProperties();
    log.AppendLine("crosshair image assigned");
}

// 2. Master Bedroom door must TRIGGER GE002 (Child Ghost), not finish it
{
    var door = Find("2nd Floor/Doors/Door Master Bedroom Locked/Door").GetComponent<Door>();
    var so = new UnityEditor.SerializedObject(door);
    var calls = so.FindProperty("OnDoorOpen.m_PersistentCalls.m_Calls");
    for (int i = 0; i < calls.arraySize; i++)
    {
        var c = calls.GetArrayElementAtIndex(i);
        if (c.FindPropertyRelative("m_MethodName").stringValue == "FinishEvent" && c.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue == "GE002")
        {
            c.FindPropertyRelative("m_MethodName").stringValue = "TriggerEvent";
            log.AppendLine("master door call " + i + ": FinishEvent(GE002) -> TriggerEvent(GE002)");
        }
    }
    so.ApplyModifiedProperties();
}

// 3. Big Guy ghost must be hidden until GE004 shows it
{
    var g = Find("BigGuyGhost");
    UnityEditor.Undo.RecordObject(g, "hide big guy");
    g.SetActive(false);
    log.AppendLine("BigGuyGhost inactive");
}

// 4. Stray AudioSources (sfx_door_pounding, Play On Awake) on a wall and a floor
foreach (var path in new[] { "1st Floor/Walls/Wall Dining S", "1st Floor/Floors/Floor Kitchen & Dining Room" })
{
    var g = Find(path);
    foreach (var a in g.GetComponents<UnityEngine.AudioSource>())
    {
        log.AppendLine("removed AudioSource(" + (a.clip ? a.clip.name : "null") + ") from " + path);
        UnityEditor.Undo.DestroyObjectImmediate(a);
    }
}

// 5. Walls on the Ground layer: priest sight ray ignores that layer, so it could see through them
foreach (var path in new[] { "1st Floor/Walls/Wall Dining S", "1st Floor/Walls/Wall Kitchen S" })
{
    var g = Find(path);
    UnityEditor.Undo.RecordObject(g, "layer");
    g.layer = UnityEngine.LayerMask.NameToLayer("Environment");
    log.AppendLine(path + " layer -> Environment");
}

// 6. Win condition: opening the Exit Door loads WinScreen
{
    var door = Find("1st Floor/Doors/Door Exit Locked/Door").GetComponent<Door>();
    var cursor = Find("DisplayCursor").GetComponent<DisplayCursor>();
    var loader = Find("SceneLoader").GetComponent<SceneLoader>();
    bool hasWin = false;
    for (int i = 0; i < door.OnDoorOpen.GetPersistentEventCount(); i++)
        if (door.OnDoorOpen.GetPersistentMethodName(i) == "LoadScene") hasWin = true;
    if (!hasWin)
    {
        UnityEditor.Undo.RecordObject(door, "win");
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(door.OnDoorOpen, new UnityEngine.Events.UnityAction(cursor.ShowCursor));
        UnityEditor.Events.UnityEventTools.AddStringPersistentListener(door.OnDoorOpen, new UnityEngine.Events.UnityAction<string>(loader.LoadScene), "WinScreen");
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(door);
        log.AppendLine("exit door -> ShowCursor + LoadScene(WinScreen)");
    }
}

UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
return log.ToString();
