using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TIMCommonAssetCtrl : MonoBehaviour
{
    [Header("Intro")]
    public GameObject Group_Intro;
    public float intro_delay;

    [Header("Marker Guide")]
    public GameObject Group_MarkerGuide;
    
    [Header("Close Popup")]
    public GameObject Group_PopupClose;

    [Header("Finish Popup")]
    public GameObject Group_PopupFinish;

    [Header("마커인식")]
    public GameObject ImageTarget;

    public delegate void Del();
    public static Del Close;

    private void OnEnable()
    {
        Close += ShowPopupClose;
    }

    private void OnDisable()
    {
        Close -= ShowPopupClose;
    }

    private void Start()
    {
        StartCoroutine(HideIntro(intro_delay));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPopupClose();
        }
    }

    //인트로 감추기
    IEnumerator HideIntro(float delay)
    {
        yield return new WaitForSeconds(delay);
        Group_Intro.SetActive(false);
        ImageTarget.GetComponent<TrackingTarget>().enabled = true;
    }
    // AR 마커 인식 가이드 감추기
    public void HideMarkerGuide()
    {
        Group_MarkerGuide.SetActive(false);
    }

    // 스크린샷 촬영
    public void CaptureScreenShot()
    {
        StartCoroutine(TakeScreenshot());
    }

    // 종료 UI 호출 
    public void ShowPopupClose()
    {
        Group_PopupClose.SetActive(true);
    }

    // 종료 UI 해제
    public void HidePopupClose()
    {
        Group_PopupClose.SetActive(false);
    }

    // 컨텐츠 종료 UI 호출 
    public void ShowPopupFinsh()
    {
        Group_PopupClose.SetActive(true);
    }

    // 컨텐츠 종료 UI 해제
    public void HidePopupFinish()
    {
        Group_PopupClose.SetActive(false);
    }

    // 게임 종료
    public void SetAppFinish()
    {
        Application.Quit();
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        //INITIAL SETUP
        string dirPath = Application.persistentDataPath + "/ScreenShot/";
        string myFilename = "keris_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string myDefaultLocation = dirPath + myFilename;
        //EXAMPLE OF DIRECTLY ACCESSING THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
        //EXAMPLE OF BACKING INTO THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = Application.persistentDataPath + "/../../../../DCIM/Camera/";
        //EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
        string myFolderLocation = "/storage/emulated/0/DCIM/ScreenShot/";
        string myScreenshotLocation = myFolderLocation + myFilename;

        //ENSURE THAT FOLDER LOCATION EXISTS
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }

        //TAKE THE SCREENSHOT AND AUTOMATICALLY SAVE IT TO THE DEFAULT LOCATION.
        //Application.CaptureScreenshot(myFilename);
        //ScreenCapture.CaptureScreenshot(myFilename);

        DirectoryInfo dir = new DirectoryInfo(dirPath);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(dirPath);
        }

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame(); // Ted 191226, Texture2D 객체의 랜더링이 될 때 까지 기다림
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        DestroyImmediate(screenShot);
        File.WriteAllBytes(myDefaultLocation, bytes);

#if UNITY_ANDROID
        //MOVE THE SCREENSHOT WHERE WE WANT IT TO BE STORED
        System.IO.File.Move(myDefaultLocation, myScreenshotLocation);

        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE

        //AUTO LAUNCH/VIEW THE SCREENSHOT IN THE PHOTO GALLERY
        Application.OpenURL(myScreenshotLocation);
        //AFTERWARDS IF YOU MANUALLY GO TO YOUR PHOTO GALLERY, 
        //YOU WILL SEE THE FOLDER WE CREATED CALLED "myFolder"
#endif
    }

}
