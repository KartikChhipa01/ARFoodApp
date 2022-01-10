//Preprocessor Directives Used
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//class that contains two variables objectName and the objectPrefab

[Serializable] //serialization is done. doing this will transform this class into a format that Unity 
               //can reconstruct
public class ObjectPrefab
{
    [SerializeField] public string objectName;

    [SerializeField] public GameObject objectPrefab;
}

public class UI : MonoBehaviour
{
    [SerializeField] private ObjectPrefab[] objectPrefabs; //an array of class type ObjectPrefab is created
                                                           //to store all the prefabs

    private ObjectPrefab selectedObject;                   //selectedObject is toggeled when a person clicks the dish from menu
                                                           //when a person clicks the dish from menu selected object changed to that 
                                                           //class object.

    [SerializeField] private Button btn1, btn2, btn3, btn4, btn5, btn6,btn7,btn8,btn9,btn10,btn11, MenuOpenButton, MenuCloseButton, ClearButton, PlaceObjectButton; //all the buttons used in the app

    [SerializeField] private GameObject toggleMenuPanel; //panel that contains the dish buttons

    public GameObject spawnedObject { get; private set; } //used property of type GameObject for spawnedObject because this GameObject is altered.

    public static event Action onPlacedObject;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); //array that stores the position of the points where the ray interacts with trackable
                                                                       //in current case this is a PlaneWithPolygon

    private ARPlaneManager arplanemanager;

    private ARRaycastManager arraycastmanager;

    public GameObject placementIndicator; //cross marker that appears in the app

    private Pose placementPose; //contains the position and rotation at which the ray interacts with the Plane

    private bool placementPoseIsValid = false;

    public GameObject MenuOpen; //contains menu button

    public GameObject MenuClose; //contains close button

    public GameObject Clear;

    public GameObject PlaceObject;

    void Start()
    {
        PlaceObject.gameObject.SetActive(false);
        if (spawnedObject == null) Clear.gameObject.SetActive(false);
        else Clear.gameObject.SetActive(true);
        MenuClose.gameObject.SetActive(false); //initially Menu Closing button is turned disabled and Menu Open button is set enabled
        MenuOpen.gameObject.SetActive(true);
        toggleMenuPanel.gameObject.SetActive(false); //dish Menu is Set Disabled
        arplanemanager.enabled = false; //plane detection is set disabled initially
    }
    void Awake()
    {
        arraycastmanager = GetComponent<ARRaycastManager>();

        arplanemanager = GetComponent<ARPlaneManager>();

        selectedObject = objectPrefabs[0]; //0th element of array of class ObjectPrefabs is used intially as selected object

        btn1.onClick.AddListener(() => SetObject("ravioli")); //if any of the dish button is selected then addListener Method is Called
                                                              //which referneces to the SetObject Method SetObject Method accepts a string
                                                              //which is the objectPrefab name this name is also set in unity inspector Panel
                                                              //the prefab the selected when the string matches the predefined objectPrefab name
                                                              //defined in the inspectorPanel
                                                              
        btn2.onClick.AddListener(() => SetObject("bread"));
        btn3.onClick.AddListener(() => SetObject("greencurry"));
        btn4.onClick.AddListener(() => SetObject("bhakharwadi"));
        btn5.onClick.AddListener(() => SetObject("hamburger"));
        btn6.onClick.AddListener(() => SetObject("fruitecake"));
        btn7.onClick.AddListener(() => SetObject("frankie"));
        btn8.onClick.AddListener(() => SetObject("capcicumpizza"));
        btn9.onClick.AddListener(() => SetObject("pie"));
        btn10.onClick.AddListener(() => SetObject("burger"));
        btn11.onClick.AddListener(() => SetObject("potatosalad"));
        MenuOpenButton.onClick.AddListener(() => ToggleMenuOpen());
        MenuCloseButton.onClick.AddListener(() => ToggleMenuClose());
        ClearButton.onClick.AddListener(() => ToggleClear());

    }

    void SetObject(string name)
    {
        PlaceObject.gameObject.SetActive(true);
        //DestroyImmediate(spawnedObject) //destroys the currently spawned object
        toggleMenuPanel.gameObject.SetActive(false); //disables the menu panel
        MenuOpen.gameObject.SetActive(true); //sets menu open button on
        MenuClose.gameObject.SetActive(false);
        arplanemanager.enabled = true; //enables plane detection
        selectedObject = objectPrefabs.Where(e => e.objectName == name).Single(); //changes the value of variable prefab to the one that is
                                                                                  //choosen in the menu
    }

    private void ToggleMenuOpen()
    {
        Clear.gameObject.SetActive(false);
        PlaceObject.gameObject.SetActive(false);
        MenuOpen.gameObject.SetActive(false);
        MenuClose.gameObject.SetActive(true);
        toggleMenuPanel.gameObject.SetActive(true);
        arplanemanager.enabled = false;
    }

    private void ToggleMenuClose()
    {
        if (spawnedObject == null) Clear.gameObject.SetActive(false);
        else Clear.gameObject.SetActive(true);
        MenuOpen.gameObject.SetActive(true);
        MenuClose.gameObject.SetActive(false);
        toggleMenuPanel.gameObject.SetActive(false);
        arplanemanager.enabled = true;
    }

    private void ToggleClear()
    {
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnedObject == null) Clear.gameObject.SetActive(false);
        else Clear.gameObject.SetActive(true);
        if (toggleMenuPanel.activeSelf) return;
        UpdatePlacementPose(); //placement position and rotation updating
        UpdatePlacementIndicator(); //placement indicator position and rotation updating
    }
    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); //finds the center of the sreen the stores it in the variable screenCenter
        var hits = new List<ARRaycastHit>(); //array to store the position of interaction
        arraycastmanager.Raycast(screenCenter, hits, TrackableType.Planes); //sends a ray from the center of the screen to interact with the plane and stores the position and rotation in 
        placementPoseIsValid = hits.Count > 0; //if the ray interacts with with the plane i.e. number of elements in the hits array is geater than one 
        if (placementPoseIsValid) //if placement position is valid
        {
            placementPose = hits[0].pose; //the first hit position is selected and stored in the placementPose variable
        }
    }
    void UpdatePlacementIndicator()
    {
        //if(spawnedObject==null&&placementPoseIsValid..........................
        if (placementPoseIsValid) //if no object is spawned and placement pose is valid i.e. ray is hitting a plane then the indicator is in active state
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation); //changes the position and rotation of the placement indicator

        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
    public void ARPlaceObject()
    {
        if (placementPoseIsValid && selectedObject != null)
        {
            spawnedObject = Instantiate(selectedObject.objectPrefab, placementPose.position, placementPose.rotation); //spawns the object at the position where the ray interacts with the plane

        }
    }
}
