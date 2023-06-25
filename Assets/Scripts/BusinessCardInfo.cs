using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BusinessCardInfo : MonoBehaviour
{
    public string m_name;
    public string m_bio;
    public string m_avatarID;
    public string m_linkedIn;
    public string m_website;

    public BusinessCardInfo(string name, string bio, string avatarID, string linkedIn, string website)
    {
        m_name = name;
        m_bio = bio;
        m_avatarID = avatarID;
        m_linkedIn = linkedIn;
        m_website = website;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static BusinessCardInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<BusinessCardInfo>(jsonString);
    }
}
