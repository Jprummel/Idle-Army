using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StardustInteractive.Saving
{
    /// <summary>
    /// To be placed on any GameObject that has ISaveable components that
    /// require saving.
    ///
    /// This class gives the GameObject a unique ID in the scene file. The ID is
    /// used for saving and restoring the state related to this GameObject. This
    /// ID can be manually override to link GameObjects between scenes (such as
    /// recurring characters, the player or a score board). Take care not to set
    /// this in a prefab unless you want to link all instances between scenes.
    /// </summary>
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        // CONFIG DATA
        [Tooltip("The unique ID is automatically generated in a scene file if " +
        "left empty. Do not set in a prefab unless you want all instances to " + 
        "be linked.")]
        [SerializeField] private string m_UniqueIdentifier = "";

        // CACHED STATE
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return m_UniqueIdentifier;
        }

        public void Save(string saveFile)
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                saveable.Save(m_UniqueIdentifier, saveFile);
            }
        }

        public void Load(string saveFile)
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                saveable.Load(m_UniqueIdentifier, saveFile);
            }
        }

        public void Reset(string saveFile)
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                saveable.ResetData(m_UniqueIdentifier, saveFile);
            }
        }
        // PRIVATE

#if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("m_UniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}