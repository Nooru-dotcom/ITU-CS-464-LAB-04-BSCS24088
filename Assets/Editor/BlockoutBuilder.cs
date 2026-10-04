using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class BlockoutBuilder
{
    static Material Mat(Color c)
    {
        Directory.CreateDirectory("Assets/Materials");
        string path = "Assets/Materials/BO_" + ColorUtility.ToHtmlStringRGB(c) + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m) return m;
        var sh = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        m = new Material(sh) { color = c };
        AssetDatabase.CreateAsset(m, path);
        return m;
    }

    static GameObject P(PrimitiveType t, string n, Vector3 pos, Vector3 s, Color c, Transform parent, Vector3? rot = null)
    {
        var g = GameObject.CreatePrimitive(t);
        g.name = n;
        g.transform.SetParent(parent);
        g.transform.position = pos;
        g.transform.localScale = s;
        if (rot.HasValue) g.transform.eulerAngles = rot.Value;
        g.GetComponent<Renderer>().sharedMaterial = Mat(c);
        return g;
    }

    static void NewScene() => EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

    static void Save(string name)
    {
        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/" + name + ".unity");
    }

    [MenuItem("Tools/Blockout/Level 1 - Sky Run")]
    public static void Level1()
    {
        NewScene();
        var root = new GameObject("Level1_SkyRun").transform;
        Color grass = new Color(.3f, .7f, .3f), stone = new Color(.6f, .6f, .65f), red = new Color(.85f, .2f, .2f),
              gold = new Color(1f, .85f, .2f), blue = new Color(.2f, .4f, .9f);

        P(PrimitiveType.Cube, "Start", new Vector3(0, 0, 0), new Vector3(8, 1, 8), grass, root);
        P(PrimitiveType.Capsule, "PlayerStart", new Vector3(0, 2, 0), Vector3.one, blue, root);

        // stepping platforms with gaps, rising
        for (int i = 1; i <= 6; i++)
            P(PrimitiveType.Cube, "Step" + i, new Vector3((i % 2 == 0 ? 2 : -2), i * 0.5f, i * 8), new Vector3(4, 1, 4), stone, root);

        // narrow beam
        P(PrimitiveType.Cube, "Beam", new Vector3(0, 3, 60), new Vector3(1, 1, 12), stone, root);

        // obstacle platform: pillars + sweeper bar
        P(PrimitiveType.Cube, "ObstaclePlatform", new Vector3(0, 3, 72), new Vector3(8, 1, 8), grass, root);
        P(PrimitiveType.Cylinder, "Pillar_L", new Vector3(-3, 4.5f, 72), new Vector3(1, 1.5f, 1), red, root);
        P(PrimitiveType.Cylinder, "Pillar_R", new Vector3(3, 4.5f, 72), new Vector3(1, 1.5f, 1), red, root);
        P(PrimitiveType.Cylinder, "SweeperBar", new Vector3(0, 4.3f, 72), new Vector3(0.3f, 3.5f, 0.3f), red, root, new Vector3(0, 0, 90));
        P(PrimitiveType.Cylinder, "SweeperHub", new Vector3(0, 4f, 72), new Vector3(0.6f, 0.5f, 0.6f), stone, root);

        // ramp up
        P(PrimitiveType.Cube, "Ramp", new Vector3(0, 4.9f, 84), new Vector3(4, 0.5f, 10), stone, root, new Vector3(-20, 0, 0));

        // goal platform
        P(PrimitiveType.Cube, "GoalPlatform", new Vector3(0, 6.5f, 96), new Vector3(8, 1, 8), grass, root);
        P(PrimitiveType.Capsule, "Guard", new Vector3(-2, 8, 96), Vector3.one, red, root);
        P(PrimitiveType.Sphere, "GoalOrb", new Vector3(0, 8.5f, 97), Vector3.one * 1.5f, gold, root);

        // coins over gaps
        for (int i = 1; i <= 5; i++)
            P(PrimitiveType.Sphere, "Coin" + i, new Vector3(0, i * 0.5f + 2.5f, i * 8 + 4), Vector3.one * 0.5f, gold, root);

        Save("Level1_SkyRun");
    }

    [MenuItem("Tools/Blockout/Level 2 - The Vault")]
    public static void Level2()
    {
        NewScene();
        var root = new GameObject("Level2_Vault").transform;
        Color floor = new Color(.25f, .25f, .3f), wall = new Color(.55f, .55f, .6f), red = new Color(.85f, .2f, .2f),
              gold = new Color(1f, .85f, .2f), blue = new Color(.2f, .4f, .9f), green = new Color(.2f, .8f, .4f);

        P(PrimitiveType.Cube, "Floor", new Vector3(0, -0.5f, 0), new Vector3(40, 1, 40), floor, root);
        // outer walls
        P(PrimitiveType.Cube, "Wall_N", new Vector3(0, 2, 20), new Vector3(40, 4, 1), wall, root);
        P(PrimitiveType.Cube, "Wall_S", new Vector3(0, 2, -20), new Vector3(40, 4, 1), wall, root);
        P(PrimitiveType.Cube, "Wall_E", new Vector3(20, 2, 0), new Vector3(1, 4, 40), wall, root);
        P(PrimitiveType.Cube, "Wall_W", new Vector3(-20, 2, 0), new Vector3(1, 4, 40), wall, root);

        // inner walls: x, z, sizeX, sizeZ (gaps left as doorways)
        float[,] w = {
            {-10,-10,20,1}, {8,-10,12,1},
            {-5, 0,1,20}, {5,-3,1,14},
            {12, 5,14,1}, {-12,10,12,1}, {0,14,1,10}
        };
        for (int i = 0; i < w.GetLength(0); i++)
            P(PrimitiveType.Cube, "InnerWall" + i, new Vector3(w[i, 0], 2, w[i, 1]), new Vector3(w[i, 2], 4, w[i, 3]), wall, root);

        // cover pillars
        Vector2[] pil = { new Vector2(-14, -4), new Vector2(-14, 4), new Vector2(10, -16), new Vector2(14, 0), new Vector2(-8, 16) };
        for (int i = 0; i < pil.Length; i++)
            P(PrimitiveType.Cylinder, "Pillar" + i, new Vector3(pil[i].x, 2, pil[i].y), new Vector3(1.5f, 2, 1.5f), wall, root);

        // guards
        Vector2[] gd = { new Vector2(0, -6), new Vector2(10, 2), new Vector2(-10, 14), new Vector2(14, 14) };
        for (int i = 0; i < gd.Length; i++)
            P(PrimitiveType.Capsule, "Guard" + i, new Vector3(gd[i].x, 1, gd[i].y), Vector3.one, red, root);

        // keys / pickups
        Vector2[] pk = { new Vector2(-16, -16), new Vector2(16, -14), new Vector2(-16, 16) };
        for (int i = 0; i < pk.Length; i++)
            P(PrimitiveType.Sphere, "Key" + i, new Vector3(pk[i].x, 1, pk[i].y), Vector3.one * 0.8f, gold, root);

        // player start + exit door + vault
        P(PrimitiveType.Capsule, "PlayerStart", new Vector3(-17, 1, -17), Vector3.one, blue, root);
        P(PrimitiveType.Cube, "ExitDoor", new Vector3(17, 1.5f, 17), new Vector3(3, 3, 0.5f), green, root);
        P(PrimitiveType.Cube, "VaultCore", new Vector3(0, 0.75f, 18), new Vector3(3, 1.5f, 3), gold, root);

        Save("Level2_Vault");
    }
}