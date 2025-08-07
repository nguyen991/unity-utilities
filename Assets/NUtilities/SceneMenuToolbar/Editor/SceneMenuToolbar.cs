using System;
using System.Collections.Generic;
using System.Linq;
using Paps.UnityToolbarExtenderUIToolkit;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace NUtilities.SceneMenuToolbar.Editor
{
    [MainToolbarElement(id: "SceneMenuToolbar")]
    public class SceneMenuToolbar : VisualElement
    {
        public void InitializeElement()
        {
            // set up the style for the toolbar
            style.flexDirection = FlexDirection.Column;
            style.flexGrow = 1;

            // load data from SceneMenuToolbarObject
            var data = GetData();

            // add header label
            Add(new Label("Scene Toolbars")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 16,
                    marginBottom = 8
                }
            });

            // add spacing
            Spacing();

            // add text field for scene directory
            var listView = new ListView(data.sceneDirectories, 20, () =>
            {
                var element = new VisualElement()
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        flexGrow = 1,
                        marginBottom = 2,
                        marginTop = 2,
                        marginLeft = 2,
                        marginRight = 2,
                        alignItems = Align.Center,
                    }
                };

                // add text field
                var textField = new TextField()
                {
                    label = "Element",
                    style =
                    {
                        flexGrow = 1,
                    }
                };
                element.Add(textField);

                // add icon button
                var button = new Button()
                {
                    style =
                    {
                        backgroundImage = new StyleBackground(EditorGUIUtility.IconContent("Folder Icon").image as Texture2D),
                        height = 20,
                        width = 20,
                    }
                };
                element.Add(button);

                return element;
            }, (element, i) =>
            {
                // text field
                var textField = element.Q<TextField>();
                textField.label = "Element " + (i + 1);
                textField.value = data.sceneDirectories[i];
                textField.RegisterValueChangedCallback(evt =>
                {
                    while (data.sceneDirectories.Count <= i)
                    {
                        data.sceneDirectories.Add(string.Empty);
                    }
                    data.sceneDirectories[i] = evt.newValue;
                    EditorUtility.SetDirty(data);
                });

                // icon button to open folder picker
                var button = element.Q<Button>();
                button.clickable = null;
                button.clicked += () => PickSceneDirectoryField(data, i);
            })
            {
                headerTitle = "Scene Directories",
                selectionType = SelectionType.Single,
                style = { flexGrow = 1 },
                reorderable = true,
                showAddRemoveFooter = true,
                showFoldoutHeader = true,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            };
            listView.itemsRemoved += _ =>
            {
                Refresh();
            };
            
            Add(listView);
            Spacing();
            
            // get all scenes in the directory
            var scenes = GetScenes(data.sceneDirectories);
            
            // add dropdown for quick open scenes
            var openScenes = new List<string>(scenes);
            openScenes.Insert(0, "---");
            var sceneDropdown = new DropdownField("Open Scene", openScenes, 0);
            sceneDropdown.RegisterValueChangedCallback(evt =>
            {
                // open the selected scene
                var selectedScene = evt.newValue;
                if (!string.IsNullOrEmpty(selectedScene) && selectedScene != "---")
                {
                    EditorSceneManager.OpenScene(selectedScene);
                }
            });
            Add(sceneDropdown);
            Spacing();
            
            // add dropdown for select boost scene
            var boostSceneDropdown = new DropdownField("Boost Scene", scenes, scenes.IndexOf(data.sceneBoostPath));
            boostSceneDropdown.RegisterValueChangedCallback(evt =>
            {
                data.sceneBoostPath = evt.newValue;
                EditorUtility.SetDirty(data);
            });
            Add(boostSceneDropdown);
            Spacing();
            
            // add horizontal line
            Add(new VisualElement
            {
                style =
                {
                    height = 1,
                    backgroundColor = Color.gray,
                    marginTop = 8,
                    marginBottom = 8
                }
            });
            
            // add boost button
            Add(new Button(() =>
            {
                // check if boost scene path is set
                if (string.IsNullOrEmpty(data.sceneBoostPath) || EditorApplication.isPlaying)
                    return;
                
                // open the boost scene
                if (System.IO.File.Exists(data.sceneBoostPath) && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorPrefs.SetString($"scene_boost", SceneManager.GetActiveScene().path);
                    EditorSceneManager.OpenScene(data.sceneBoostPath);
                    EditorApplication.isPlaying = true;
                }
                else
                {
                    Debug.LogWarning($"Scene not found: {data.sceneBoostPath}");
                }
            })
            {
                text = "Boost Scene",
                style =
                {
                    height = 25,
                    flexGrow = 1,
                    backgroundColor = Color.green,
                    color = Color.black
                }
            });
            
            // add refresh button
            Add(new Button(Refresh)
            {
                text = "Refresh",
                style =
                {
                    height = 25,
                    flexGrow = 1,
                    backgroundColor = Color.gray,
                    color = Color.white
                }
            });
        }

        private void Refresh()
        {
            Clear();
            InitializeElement();
        }
        
        private void PickSceneDirectoryField(SceneMenuToolbarObject data, int index)
        {
            var path = EditorUtility.OpenFolderPanel("Select Scene Directory", "Assets", "");
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Substring(path.LastIndexOf("Assets/", StringComparison.Ordinal));
                data.sceneDirectories[index] = path;
                EditorUtility.SetDirty(data);
                Refresh();
            }
        }
        
        private void Spacing(float height = 8f)
        {
            Add(new VisualElement
            {
                style =
                {
                    height = height,
                    flexGrow = 1
                }
            });
        }

        private static readonly List<string> EmptyScenes = new List<string>();
        private List<string> GetScenes(List<string> paths)
        {
            if (paths.Count == 0)
            {
                Debug.LogWarning("Scene directory is empty");
                return EmptyScenes;
            }

            return AssetDatabase.FindAssets("t:Scene", paths.Where(p => !string.IsNullOrEmpty(p)).ToArray())
                .Distinct()
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToList();
        }
        
        private SceneMenuToolbarObject GetData()
        {
            var filters = AssetDatabase.FindAssets("t:SceneMenuToolbarObject");
            if (filters.Length > 0)
            {
                // load the existing instance
                var path = AssetDatabase.GUIDToAssetPath(filters[0]);
                var existingObject = AssetDatabase.LoadAssetAtPath<SceneMenuToolbarObject>(path);
                if (existingObject != null)
                {
                    return existingObject;
                }
            }
            
            // create a new instance if none exists
            var newObject = ScriptableObject.CreateInstance<SceneMenuToolbarObject>();
            AssetDatabase.CreateAsset(newObject, "Assets/SceneMenuToolbar/SceneMenuToolbarObject.asset");
            AssetDatabase.SaveAssets();
            return newObject;
        }
    }
    
    [InitializeOnLoad]
    public static class SceneMenuToolbarExtender
    {
        static SceneMenuToolbarExtender()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && EditorPrefs.GetString("scene_boost") != string.Empty)
            {
                var scene = EditorPrefs.GetString("scene_boost");
                EditorSceneManager.OpenScene(scene);
                EditorPrefs.DeleteKey("scene_boost");
            }
        }
    }
}

