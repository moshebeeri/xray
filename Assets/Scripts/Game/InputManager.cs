using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;
using Debug = UnityEngine.Debug;
using System.Linq;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private XRNode xRNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    public VideoPlayer videoPlayer = null;
    public VideoManager videoManager = null;

    public void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    private bool triggerIsPressed;
    private bool primaryButtonIsPressed;
    private bool primary2DAxisIsChosen;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;
    private bool gripIsPressed;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    private void Update()
    {
        // if(!videoManager.IsReady)
        //     return;
        XRInput();
        OculusInput();
        KeyboardInput();
    }

    private void XRInput()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
        // capturing trigger button press and release
        bool triggerButtonValue = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue) && triggerButtonValue && !triggerIsPressed)
        {
            triggerIsPressed = true;
            videoManager.PauseToggle();
        }
        else if (!triggerButtonValue && triggerIsPressed)
        {
            triggerIsPressed = false;
        }

        // capturing primary button press and release
        bool primaryButtonValue = false;
        InputFeatureUsage<bool> primaryButtonUsage = CommonUsages.primaryButton;

        if (device.TryGetFeatureValue(primaryButtonUsage, out primaryButtonValue) && primaryButtonValue && !primaryButtonIsPressed)
        {
            videoManager.PauseToggle();
            primaryButtonIsPressed = true;
        }
        else if (!primaryButtonValue && primaryButtonIsPressed)
        {
            primaryButtonIsPressed = false;
        }

        // capturing primary 2D Axis changes and release
        InputFeatureUsage<Vector2> primary2DAxisUsage = CommonUsages.primary2DAxis;
        // make sure the value is not zero and that it has changed
        if (primary2DAxisValue != prevPrimary2DAxisValue)
        {
            primary2DAxisIsChosen = false;
        }
        if (device.TryGetFeatureValue(primary2DAxisUsage, out primary2DAxisValue) && primary2DAxisValue != Vector2.zero && !primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = true;
        }
        else if (primary2DAxisValue == Vector2.zero && primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = false;
        }

        // capturing grip value
        float gripValue;
        InputFeatureUsage<float> gripUsage = CommonUsages.grip;

        if (device.TryGetFeatureValue(gripUsage, out gripValue) && gripValue > 0 && !gripIsPressed)
        {
            gripIsPressed = true;
        }
        else if (gripValue == 0 && gripIsPressed)
        {
            gripIsPressed = false;
        }
    }

    private void OculusInput()
    {
    //     if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.All))
    //     {
    //         Debug.Log("Button One");
    //         videoManager.PauseToggle();
    //     }
    //     if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
    //     {
    //         videoManager.PauseToggle();
    //     }
    //     if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
    //     {
    //         videoManager.PauseToggle();
    //     }
    //     if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
    //     {
    //         videoManager.PauseToggle();
    //     }
    //     if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
    //     {
    //         videoManager.PauseToggle();
    //     }
    }

    private void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Key Pressed");
            videoManager.PauseToggle();

        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {

        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {

        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
    }
}
