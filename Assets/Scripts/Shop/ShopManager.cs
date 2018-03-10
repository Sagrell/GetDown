using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public static ShopManager Instance;
    [Header("Managers")]
    public CubesManager cubesManager;

    [Space]
    [Header("References")]
    public GameObject showCube;
    public GameObject showCubePrefab;
    public GameObject gameCube;
    public GameObject destroyedVersion;

    DataManager dataManager;
    UserData data;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        dataManager = DataManager.Instance;
        data = dataManager.GetUserData();
    }
	

    public void Select(CubeModel cube)
    {
        Material material = cube.CubeMaterial;

        //Apply material to the cube
        gameCube.GetComponent<Renderer>().material = material;
        showCube.GetComponent<Renderer>().material = material;
        showCubePrefab.GetComponent<Renderer>().material = material;

        //Apply material to the destroyed version of cube
        Renderer[] cubeParts = destroyedVersion.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < cubeParts.Length; ++i)
        {
            cubeParts[i].material = material;
        }
        cubesManager.Deselect();
        cube.selected = true;
        cubesManager.Initialize();
        GuiManager.Instance.Show(cube);
    }

    public void Buy(CubeModel cube)
    {
        data.GoldAmount -= cube.cost;
        data.CubesUnlocked[cube.index] = true;
        dataManager.SaveUserData(data);
        cubesManager.Initialize();
        GuiManager.Instance.UpdateInfo();
        GuiManager.Instance.Show(cube);
    }
}
