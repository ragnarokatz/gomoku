using UnityEngine;
using AssemblyCSharp;

public class ClientInit : MonoBehaviour
{
    private void HandleOnLog(Log.LogTypes type, string message)
    {
        switch (type)
        {
        case Log.LogTypes.Trace:
        default:
            Debug.Log(message);
            break;
        case Log.LogTypes.Error:
            Debug.LogError(message);
            break;
        case Log.LogTypes.Warning:
            Debug.LogWarning(message);
            break;
        }
    }

    private void Awake()
    {
        Log.OnLog += HandleOnLog;
    }

    private void OnDestroy()
    {
        Log.OnLog -= HandleOnLog;
    }
}
