using UnityEngine;

public class VSyncSetup : MonoBehaviour
{
    [SerializeField]
    private bool vSyncEnabled;
    [SerializeField]
    private int target;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = vSyncEnabled ? target : -1;
    }
}