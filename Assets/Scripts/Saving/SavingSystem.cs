using Michsky.LSS;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StardustInteractive.Saving
{
    /// <summary>
    /// This component provides the interface to the saving system. It provides
    /// methods to save and restore a scene.
    ///
    /// This component should be created once and shared between all subsequent scenes.
    /// </summary>
    public class SavingSystem : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField] private LSS_Manager m_LoadingScreenManager;
        #endregion
        /// <summary>
        /// Will load the last scene that was saved and restore the state. This
        /// must be run as a coroutine.
        /// </summary>
        /// <param name="saveFile">The save file to consult for loading.</param>
        public void LoadLastScene(string saveFile)
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (ES3.KeyExists("LastSceneBuildIndex", filePath: saveFile))
            {
                int lastScene = ES3.Load<int>("LastSceneBuildIndex", filePath: saveFile);
                if (lastScene == 0)
                {
                    buildIndex = 1;
                }
                else
                {
                    buildIndex = lastScene;
                }
            }
            else
            {
                buildIndex = 1;
            }

            m_LoadingScreenManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(buildIndex));
            m_LoadingScreenManager.onLoadingStart.AddListener(() => Load(saveFile));
        }

        /// <summary>
        /// Save the current scene to the provided save file.
        /// </summary>
        public void Save(string saveFile)
        {
            string saveFileWithExtension = $"{saveFile}.es3";
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.Save(saveFileWithExtension);
            }
            ES3.Save("LastSceneBuildIndex", SceneManager.GetActiveScene().buildIndex, saveFileWithExtension);
        }

        /// <summary>
        /// Delete the state in the given save file.
        /// </summary>
        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public void Load(string saveFile)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.Load($"{Application.persistentDataPath}/{saveFile}.es3");
            }
        }

        public bool SaveFileExists(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            return File.Exists(path);
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if(Path.GetExtension(path) == ".es3")
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        // PRIVATE
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".es3");
        }
    }
}