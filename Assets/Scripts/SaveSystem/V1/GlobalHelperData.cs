using UnityEngine;

[System.Serializable]
public class GlobalHelperData
{
    public bool baseDataSaved;

    public GlobalHelperData(GlobalHelper GH)
    {
        baseDataSaved = GH.baseDataSaved;
    }
}
