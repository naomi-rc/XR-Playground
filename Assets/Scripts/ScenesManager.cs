using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace ContactCard
{
    public class ScenesManager : MonoBehaviour
    {
        public void LoadScene(SceneAsset sceneAsset)
        {
            SceneManager.LoadScene(sceneAsset.name);
        }        
    }
}

