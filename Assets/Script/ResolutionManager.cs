using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using System.IO;
using UnityEngine;

enum TypeEnum
{
    plane0D,
    plane1D,
    plane2D,
    plane3D
};
public class ResolutionManager : MonoBehaviour
{
    [Header("Resolution and type")]
    [SerializeField] TypeEnum type;
    [SerializeField] int sides;
    [SerializeField] int resolution;
    [SerializeField] bool changeResolution;
    [SerializeField] bool backgroundActive;
    [SerializeField] Color backgroundColor;
    [SerializeField] bool gridActive;
    [SerializeField] Color gridColor;
    [Header("File Names")]
    [SerializeField] string textureFileName;
    [Header("Pivot Transform (change model to show with \"N\" and \"M\")")]
    [SerializeField] List<GameObject> models;
    [SerializeField] Vector3 allWorldPosition;
    [SerializeField] Vector3 allWorldRotation;
    [Space]
    [SerializeField] Vector3 allLocalPosition;
    [SerializeField] Vector3 allLocalRotation;
    [SerializeField] Vector3 allLocalScale;
    [Space]
    [SerializeField] bool single;
    [SerializeField] bool createLivingPlane;

    TypeEnum currentType;
    int currentSides;
    string createdDirectoryPath;
    string defaultDirectoryPath;
    string modelName;
    GameObject pivot;
    List<GameObject> pivots;
    int counter;
    Vector3 cameraPosition;

    private void Awake()
    {
        pivots = new();
        createdDirectoryPath = "Assets/Other/Created/";
        defaultDirectoryPath = "Assets/Other/Default/";
        pivot = (GameObject)AssetDatabase.LoadAssetAtPath(defaultDirectoryPath + "DefaultPivot.prefab", typeof(GameObject));
    }

    private void Update()
    {
        SetGrid();
        SetPivot();
        ChangeModel();
        ChangeResolution();
        CreateLivingPlane();  
    }

    void SetGrid()
    {
        cameraPosition = Camera.main.transform.position + new Vector3(0, 0, -10);
        if (backgroundActive) Camera.main.backgroundColor = backgroundColor;
        else Camera.main.backgroundColor = Color.clear;
        if (gridActive)
        {
            switch (currentType)
            {
                case TypeEnum.plane0D:
                    Debug.DrawRay(cameraPosition + new Vector3(0, -0.5f, 0), Vector3.up, gridColor);
                    break;
                case TypeEnum.plane2D:
                    for (int i = 0; i < currentSides - 1; i++) Debug.DrawRay(cameraPosition + new Vector3(currentSides / 2 - 1 - i, -0.5f, 0), Vector3.up, gridColor);
                    break;
                case TypeEnum.plane3D:
                    for (int i = 0; i < currentSides - 1; i++)
                        Debug.DrawRay(
                        cameraPosition + new Vector3(currentSides / 2 - 1 - i, -(float)(currentSides / 2 + 1) / 2, 0),
                        Vector3.up * (currentSides / 2 + 1),
                        gridColor);
                    for (int i = 0; i < currentSides / 2; i++)
                        Debug.DrawRay(
                        cameraPosition + new Vector3(currentSides / 2, (-currentSides / 2 - 1) * 0.5f + 1 + i, 0),
                        Vector3.left * currentSides,
                        gridColor);
                    break;
            }
        }
    }
    void SetPivot()
    {
        for (int i = 0; i < pivots.Count; i++)
        {
            SpriteController spriteController = pivots[i].GetComponent<SpriteController>();
            spriteController.AllLocalPosition = allLocalPosition;
            spriteController.AllLocalScale = allLocalScale;
            switch (currentType)
            {
                case TypeEnum.plane0D:
                    spriteController.AllLocalRotation = allLocalRotation;
                    if (i == 0)
                    {
                        spriteController.AllWorldPosition = allWorldPosition + cameraPosition + new Vector3(0.5f, 0, 0);
                        spriteController.AllWorldRotation = allWorldRotation;
                    }
                    else
                    {
                        spriteController.AllWorldPosition = allWorldPosition + cameraPosition + new Vector3(-0.5f, 0, 0);
                        spriteController.AllWorldRotation = allWorldRotation + new Vector3(0, 180, 0);
                    }
                    break;
                case TypeEnum.plane1D:
                    spriteController.AllLocalRotation = allLocalRotation;
                    spriteController.AllWorldPosition = allWorldPosition + cameraPosition;
                    spriteController.AllWorldRotation = allWorldRotation;
                    break;
                case TypeEnum.plane2D:
                    spriteController.AllLocalRotation = allLocalRotation;
                    spriteController.AllWorldPosition = allWorldPosition + cameraPosition + new Vector3((float)currentSides / 2 - i - 0.5f, 0, 0);
                    spriteController.AllWorldRotation = allWorldRotation + new Vector3(0, 45 * i, 0);
                    break;
                case TypeEnum.plane3D:
                    if (i == 0)
                    {
                        spriteController.AllWorldPosition = allWorldPosition + cameraPosition + new Vector3((float)currentSides / 2 - 0.5f, ((float)currentSides / 2 + 1) * 0.5f - 0.5f, 0);
                        spriteController.AllWorldRotation = allWorldRotation + new Vector3(-90, 0, 0);
                        spriteController.AllLocalRotation = allLocalRotation;
                    }
                    else
                    {
                        spriteController.AllWorldPosition = allWorldPosition + cameraPosition + new Vector3((float)currentSides / 2 - 0.5f - (i - 1) + currentSides * ((i - 1) / currentSides), (currentSides / 2 + 1) * 0.5f - 1.5f - (i - 1) / currentSides, 0);
                        spriteController.AllWorldRotation = allWorldRotation + new Vector3(-90 + 360 / (float)currentSides * ((i - 1) / currentSides + 1), 0, 0);
                        spriteController.AllLocalRotation = allLocalRotation + new Vector3(0, 360 / (float)currentSides * (i - 1), 0);
                    }
                    break;
            }
        }
    }
    void ChangeModel()
    {
        if (models.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M))
            {
                if (Input.GetKeyDown(KeyCode.N)) counter--;
                if (Input.GetKeyDown(KeyCode.M)) counter++;
                if (counter == -1) counter = models.Count - 1;
                else if (counter >= models.Count) counter = 0;
                for (int i = 0; i < pivots.Count; i++) if (pivots[i].transform.childCount != 0) Destroy(pivots[i].transform.GetChild(0).gameObject);
                for (int i = 0; i < pivots.Count; i++) if (models.Count != 0 && models[counter] != null) Instantiate(models[counter]).transform.parent = pivots[i].transform;
            }
        }
        else
        {
            counter = 0;
            for (int i = 0; i < pivots.Count; i++) if (pivots[counter].transform.childCount != 0) Destroy(pivots[i].transform.GetChild(0).gameObject);
        }
    }
    void ChangeResolution()
    {
        if (sides <= 1) sides = 2;
        if (sides % 2 == 1) sides += 1;
        if (resolution < 1) resolution = 1;
        if (changeResolution)
        {
            changeResolution = false;
            GameObject[] pivotsToRemove = GameObject.FindGameObjectsWithTag("Pivot");
            foreach (GameObject pivot in pivotsToRemove) Destroy(pivot);
            pivots.Clear();
            currentSides = sides;
            if (GetCount() > 7) RemoveResolution(7);
            switch (type)
            {
                case TypeEnum.plane0D:
                    AddResolution(2 * resolution, resolution, "TemporaryResolution");
                    for (int i = 0; i < 2; i++) CreatePivot();
                    Camera.main.orthographicSize = 0.5f;
                    currentType = TypeEnum.plane0D;
                    break;
                case TypeEnum.plane1D:
                    AddResolution(resolution, resolution, "TemporaryResolution");
                    CreatePivot();
                    Camera.main.orthographicSize = 0.5f;
                    currentType = TypeEnum.plane1D;
                    break;
                case TypeEnum.plane2D:
                    AddResolution(sides * resolution, resolution, "TemporaryResolution");
                    for (int i = 0; i < currentSides; i++) CreatePivot();
                    Camera.main.orthographicSize = 0.5f;
                    currentType = TypeEnum.plane2D;
                    break;
                case TypeEnum.plane3D:
                    AddResolution(sides * resolution, (sides / 2 + 1) * resolution, "TemporaryResolution");
                    for (int i = 0; i < (currentSides / 2 - 1) * currentSides + 2; i++) CreatePivot();
                    Camera.main.orthographicSize = (float)(sides / 2 + 1) / 2;
                    currentType = TypeEnum.plane3D;
                    break;
            }
            SetResolution(7);
        }
        void CreatePivot()
        {
            GameObject createdPivot = Instantiate(pivot);
            createdPivot.transform.position = cameraPosition;
            createdPivot.name = "Pivot_" + pivots.Count;
            pivots.Add(createdPivot);
        }
        void AddResolution(int width, int height, string label)
        {
            Type gameViewSize = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
            Type gameViewSizes = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            Type gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
            Type generic = typeof(ScriptableSingleton<>).MakeGenericType(gameViewSizes);
            MethodInfo getGroup = gameViewSizes.GetMethod("GetGroup");
            object instance = generic.GetProperty("instance").GetValue(null, null);
            object group = getGroup.Invoke(instance, new object[] { (int)GameViewSizeGroupType.Standalone });
            Type[] types = new Type[] { gameViewSizeType, typeof(int), typeof(int), typeof(string) };
            ConstructorInfo constructorInfo = gameViewSize.GetConstructor(types);
            object entry = constructorInfo.Invoke(new object[] { 1, width, height, label });
            MethodInfo addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize");
            addCustomSize.Invoke(group, new object[] { entry });
        }
        void RemoveResolution(int index)
        {
            Type gameViewSizes = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            Type generic = typeof(ScriptableSingleton<>).MakeGenericType(gameViewSizes);
            MethodInfo getGroup = gameViewSizes.GetMethod("GetGroup");
            object instance = generic.GetProperty("instance").GetValue(null, null);
            object group = getGroup.Invoke(instance, new object[] { (int)GameViewSizeGroupType.Standalone });
            MethodInfo removeCustomSize = getGroup.ReturnType.GetMethod("RemoveCustomSize");
            removeCustomSize.Invoke(group, new object[] { index });
        }
        int GetCount()
        {
            Type gameViewSizes = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            Type generic = typeof(ScriptableSingleton<>).MakeGenericType(gameViewSizes);
            MethodInfo getGroup = gameViewSizes.GetMethod("GetGroup");
            object instance = generic.GetProperty("instance").GetValue(null, null);
            PropertyInfo currentGroupType = instance.GetType().GetProperty("currentGroupType");
            GameViewSizeGroupType groupType = (GameViewSizeGroupType)(int)currentGroupType.GetValue(instance, null);
            object group = getGroup.Invoke(instance, new object[] { (int)groupType });
            MethodInfo getBuiltinCount = group.GetType().GetMethod("GetBuiltinCount");
            MethodInfo getCustomCount = group.GetType().GetMethod("GetCustomCount");
            return (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
        }
        void SetResolution(int index)
        {
            Type gameView = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            PropertyInfo selectedSizeIndex = gameView.GetProperty("selectedSizeIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            EditorWindow window = EditorWindow.GetWindow(gameView);
            selectedSizeIndex.SetValue(window, index, null);
        }
    }
    void CreateLivingPlane()
    {
        if (createLivingPlane && models.Count != 0)
        {
            createLivingPlane = false;
            backgroundActive = false;
            gridActive = false;
            if (string.IsNullOrEmpty(textureFileName)) textureFileName = "Texture_";
            createdDirectoryPath = "Assets/Other/Created";
            defaultDirectoryPath = "Assets/Other/Default";

            for (int i = models.Count - 1; i >= 0; i--) if (models[i] == null) models.Remove(models[i]);

            if (single) StartCoroutine(LivingPlaneSingle());
            else StartCoroutine(LivingPlaneMultiple());
        }
        else if (Input.GetKeyDown(KeyCode.P) && models.Count == 0) print("You dumb sh*t, put some f*cking model in the inspector!");

        IEnumerator LivingPlaneMultiple()
        {
            yield return new WaitForSeconds(0.1f);
            counter = 0;
            for (int i = 0; i < models.Count; i++)
            {
                yield return StartCoroutine(LivingPlaneSingle());
                counter++;
            }
        }

        IEnumerator LivingPlaneSingle()
        {
            if (single) yield return new WaitForSeconds(0.1f);

            if (counter != models.Count)
            {
                modelName = models[counter].name;
                for (int i = 0; i < pivots.Count; i++)
                {
                    if (pivots[i].transform.childCount != 0) Destroy(pivots[i].transform.GetChild(0).gameObject);
                    if (models.Count != 0 && models[counter] != null) Instantiate(models[counter]).transform.parent = pivots[i].transform;
                }
            }

            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(CreateTexture());

            Camera.main.backgroundColor = backgroundColor;
        }

        IEnumerator CreateTexture()
        {
            yield return new WaitForEndOfFrame();
            int width = Screen.width;
            int height = Screen.height;
            Texture2D screenshotTexture = new(width, height, TextureFormat.ARGB32, false);
            Rect rect = new(0, 0, width, height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.EncodeToPNG();
            screenshotTexture.Apply();

            if (!AssetDatabase.LoadAssetAtPath(createdDirectoryPath + "/" + textureFileName + modelName + ".png", typeof(Texture2D))) AssetDatabase.CopyAsset(defaultDirectoryPath +  "/DefaultTexture.png", createdDirectoryPath + "/" + textureFileName + modelName + ".png");
            Texture2D newTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(createdDirectoryPath + "/" + textureFileName + modelName + ".png", typeof(Texture2D));
            newTexture.Reinitialize(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            newTexture.SetPixels32(screenshotTexture.GetPixels32());
            newTexture.Apply();

            byte[] bytes = newTexture.EncodeToPNG();
            File.WriteAllBytes(createdDirectoryPath + "/" + textureFileName + modelName + ".png", bytes);
        }
    }
}