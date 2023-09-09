namespace StardustInteractive.Saving
{
    /// <summary>
    /// Implement in any component that has state to save/restore.
    /// </summary>
    public interface ISaveable
    {
        void Save(string uniqueIdentifier, string saveFile);
        void Load(string uniqueIdentifier, string saveFile);
        void ResetData(string uniqueIdentifier, string saveFile);
    }
}