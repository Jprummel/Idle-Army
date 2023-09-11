using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StardustInteractive.Tools
{
    public class GameDataEditor : OdinMenuEditorWindow
    {
        [MenuItem("Stardust Interactive/Tools/Game Data Editor")]
        private static void Open()
        {
            var window = GetWindow<GameDataEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            tree.AddAllAssetsAtPath("AutoClickers", "Assets/Game/ScriptableObjects/AutoClickers", typeof(AutoClickerData), true).ForEach(this.AddDragHandles);
            tree.AddAllAssetsAtPath("Enemies", "Assets/Game/ScriptableObjects/Enemies", typeof(EnemyData), true).ForEach(this.AddDragHandles);
            tree.AddAllAssetsAtPath("Levels", "Assets/Game/ScriptableObjects/Levels", typeof(LevelData), true).ForEach(this.AddDragHandles);

            // Add drag handles to items, so they can be easily dragged
            tree.EnumerateTree().Where(x => x.Value as EnemyData).ForEach(AddDragHandles);

            // Add icons to objects that have them and need them in the editor.
            tree.EnumerateTree().AddIcons<AutoClickerData>(x => x.AutoClickerSprite);
            tree.EnumerateTree().AddIcons<EnemyData>(x => x.EnemySprite);

            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Autoclicker")))
                {
                    ScriptableObjectCreator.ShowDialog<AutoClickerData>("Assets/Game/ScriptableObjects/Enemies", obj =>
                    {
                        obj.Setup();
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Enemy")))
                {
                    ScriptableObjectCreator.ShowDialog<EnemyData>("Assets/Game/ScriptableObjects/Enemies", obj =>
                    {
                        obj.Setup();
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Level")))
                {
                    ScriptableObjectCreator.ShowDialog<LevelData>("Assets/Game/ScriptableObjects/Levels", obj =>
                    {
                        obj.Setup();
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete")))
                {
                    if (selected == null) { return; }
                    string[] unusedFolder = { "Assets/Game" };
                    foreach (var asset in AssetDatabase.FindAssets(selected.Name, unusedFolder))
                    {
                        var path = AssetDatabase.GUIDToAssetPath(asset);
                        AssetDatabase.DeleteAsset(path);
                    }
                }


            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}