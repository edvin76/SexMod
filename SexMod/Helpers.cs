using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.View;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityModManagerNet;

namespace SexMod
{
    internal static class Helpers
    {



        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }


        public static string ReadLocResource()
        {
            // Determine path
            string Locale = SettingsRoot.Game.Main.Localization.m_CurrentValue.ToString() + ".json";

            //Log("1 - " + Locale);

            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = "";
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"


            foreach (string str in assembly.GetManifestResourceNames())
            {
                //  Log("2 - " + str);
                if (str.EndsWith(Locale))
                {

                    resourcePath = str;
                    //        Log("3 - " + resourcePath);
                    break;
                }

            }

            if (string.IsNullOrEmpty(resourcePath))
            {
                Locale = "enGB.json";
                resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(Locale));

            }
            //  Log("4 - " + resourcePath);

            using Stream stream = assembly.GetManifestResourceStream(resourcePath);
            using StreamReader reader = new(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /*
        public static string GetLocFilePath(string Locale = null)
        {
            if (Locale == null)
                return Path.Combine(UnityModManager.modsPath, Main.modName, "Localization", SettingsRoot.Game.Main.Localization.m_CurrentValue.ToString() + ".json");
            else
                return Path.Combine(UnityModManager.modsPath, Main.modName, "Localization", Locale + ".json");
        }
        */
        public static string GetPortraitsDir()
        {
            return Path.Combine(UnityModManager.ModsPath, "SexMod", "Portraits");

        }


        public static string GetTexturesDir()
        {
            return Path.Combine(UnityModManager.ModsPath, "SexMod", "Textures");

        }
        public static string GetAreasDir()
        {
            return Path.Combine(UnityModManager.ModsPath, "SexMod", "AreaMechanics");

        }


        public static void SaveMap(UnitEntityView prefab, string mapType, string prefix = "")
        {
            foreach (SkinnedMeshRenderer smr in prefab.GetComponentsInChildren<SkinnedMeshRenderer>())
            {

                if (smr.material != null)
                {
                    //Main.DebugLog("c");

                    Texture texture = smr.material.GetTexture(mapType);


                    if (texture != null)
                    {
                        Log("SaveMap smr: " + texture.name);

                        LoadingProcess.Instance.StartCoroutine(ExportTexture(texture, Path.Combine(GetTexturesDir(), prefix + texture.name + ".png")));

                    }
                }
            }


        }



        public static System.Collections.IEnumerator ExportSprite(Sprite sprite, string filePath)
        {
            /*
			if (File.Exists(filePath))
			{
				Log($"{filePath} exists. Skipping");
			}
			else 
			*/
            if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
            {
                Log($"Skipping tightly-packed sprite {sprite.name}");
            }

            else
            {


                var copy = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);

                Graphics.ConvertTexture(sprite.texture, copy);

                yield return null;

                if (sprite.packed)
                {
                    var spriteTexture = new Texture2D(
                        (int)sprite.textureRect.width,
                        (int)sprite.textureRect.height,
                        TextureFormat.RGBA32,
                        false);

                    Graphics.CopyTexture(
                        copy, 0, 0,
                        (int)sprite.textureRect.x,
                        (int)sprite.textureRect.y,
                        (int)sprite.textureRect.width,
                        (int)sprite.textureRect.height,
                        spriteTexture, 0, 0, 0, 0);

                    var old = copy;

                    UnityEngine.Object.Destroy(old);

                    copy = spriteTexture;
                }

                var request = AsyncGPUReadback.Request(copy, 0, TextureFormat.RGBA32, r =>
                {
                    var data = r.GetData<Color32>(0);

                    var bytes = ImageConversion.EncodeNativeArrayToPNG(
                        data,
                        copy.graphicsFormat,
                        (uint)copy.width,
                        (uint)copy.height)
                        .ToArray();

                    File.WriteAllBytes(filePath, bytes);

                    UnityEngine.Object.Destroy(copy);
                });

                while (!request.done)
                    yield return null;
            }

        }

        public static System.Collections.IEnumerator ExportTexture(Texture texture, string filePath)
        {



            var copy = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

            Graphics.ConvertTexture(texture, copy);

            yield return null;


            var request = AsyncGPUReadback.Request(copy, 0, TextureFormat.RGBA32, r =>
            {
                var data = r.GetData<Color32>(0);

                var bytes = ImageConversion.EncodeNativeArrayToPNG(
                    data,
                    copy.graphicsFormat,
                    (uint)copy.width,
                    (uint)copy.height)
                    .ToArray();

                File.WriteAllBytes(filePath, bytes);

                UnityEngine.Object.Destroy(copy);
            });

            while (!request.done)
                yield return null;


        }
        public static Texture2D ReadTexture(string path, bool isLinear = false)
        {
            if (!File.Exists(path))
                return null;

            Vector2Int size = ImageHeader.GetDimensions(path);


            byte[] array = File.ReadAllBytes(path);

            Texture2D texture2D;
            if (isLinear)
                texture2D = new Texture2D(size.x, size.y, TextureFormat.ARGB32, true, true);
            else
                texture2D = new Texture2D(size.x, size.y, TextureFormat.ARGB32, true);

            ImageConversion.LoadImage(texture2D, array);

            texture2D.Apply();


            return texture2D;
        }

        private static Dictionary<string, LocalizedString> Strings = new();
        internal static LocalizedString CreateString(string key, string value)
        {
            var localizedString = new LocalizedString() { m_Key = key };
            LocalizationManager.Instance.CurrentPack.PutString(key, value);
            if (!Strings.ContainsKey(key))
                Strings.Add(key, localizedString);
            return localizedString;
        }

        internal static LocalizedString GetString(string key)
        {
            return Strings.ContainsKey(key) ? Strings[key] : default;
        }

        public static void SetLocalisedName(this BlueprintUnit unit, string key, string name)
        {
            if (unit.LocalizedName.String.Key != key)
            {
                unit.LocalizedName = ScriptableObject.CreateInstance<SharedStringAsset>();
                unit.LocalizedName.String = CreateString(key, name);
            }
        }




        public static void Log(string str)
        {
            Main.log.Log(str);
        }

        public static void Log(object obj)
        {
            Main.log.Log(obj?.ToString() ?? "null");
        }


        public static void DebugError(Exception ex)
        {
            if (Main.log != null) Main.log.Log(ex.ToString() + "\n" + ex.StackTrace);
        }

        internal static Exception Error(string message)
        {
            // modEntry?.Logger?.Log(message);
            return new InvalidOperationException(message);
        }


        public class ProcessLog : IDisposable
        {
            private Stopwatch stopWatch = new Stopwatch();
            private string message;
            public ProcessLog(string message)
            {
                this.message = message;
                stopWatch.Start();
            }

            public void Dispose()
            {
                stopWatch.Stop();
                Main.log.Log($"{message}: [{stopWatch?.Elapsed:ss\\.ff}]");
            }
        }

        public static T CreateElement<T>(SimpleBlueprint owner) where T : Element
        {
            var element = Element.CreateInstance(typeof(T));
            element.name = $"{element.GetType()}${System.Guid.NewGuid()}";
            return (T)element;
        }

        public static T Create<T>(string name, string guid) where T : BlueprintScriptableObject, new()
        {
            T asset = new()
            {
                name = name,
                AssetGuid = guid.ToGUID()
            };

            return asset;
        }

        public static T CreateESO<T>(string name, string guid) where T : ElementsScriptableObject, new()
        {
            T asset = new()
            {
                name = name,
                AssetGuid = guid.ToGUID()
            };

            return asset;
        }

        public static T CreateAndAddESO<T>(string name, string guid) where T : ElementsScriptableObject, new()
        {
            var asset = CreateESO<T>(name, guid);

            AddESO(guid, asset);

            return asset;
        }

        public static void AddESO<T>(string guid, T blueprint) where T : ElementsScriptableObject
        {
            if (ResourcesLibrary.TryGetScriptable<T>(guid) == null)
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(guid.ToGUID(), blueprint);
        }

        public static T CreateAndAdd<T>(string name, string guid) where T : BlueprintScriptableObject, new()
        {
            var asset = Create<T>(name, guid);

            AddBlueprint(guid, asset);

            return asset;
        }

        public static void AddBlueprint<T>(string guid, T blueprint) where T : BlueprintScriptableObject
        {
            if (ResourcesLibrary.TryGetBlueprint<T>(guid) == null)
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(guid.ToGUID(), blueprint);
        }

        public static T CopyAndAdd<T>(T original, string name, string guid) where T : BlueprintScriptableObject
        {
            var copy = ObjectCopy<T>(original);
            copy.name = name;
            copy.AssetGuid = guid.ToGUID();

            AddBlueprint(guid, copy);

            return copy;
        }

        public static T CopyAndAddESO<T>(T original, string name, string guid) where T : ElementsScriptableObject
        {
            var copy = ObjectCopy<T>(original);
            copy.name = name;
            copy.AssetGuid = guid.ToGUID();

            AddESO(guid, copy);

            return copy;
        }

        public static T ObjectCopy<T>(T original)
        {
            var result = (T)ObjectDeepCopier.Clone(original);
            return result;
        }

  /*      internal static class Empties
        {
            public static readonly ActionList Actions = new() { Actions = new GameAction[0] };
            public static readonly ConditionsChecker Conditions = new() { Conditions = new Condition[0] };
            public static readonly ContextDiceValue DiceValue = new()
            {
                DiceType = DiceType.Zero,
                DiceCountValue = 0,
                BonusValue = 0
            };
            public static readonly LocalizedString String = new();
            public static readonly PrefabLink PrefabLink = new();
            public static readonly ShowCheck ShowCheck = new();
            public static readonly CueSelection CueSelection = new();
            public static readonly CharacterSelection CharacterSelection = new();
            public static readonly DialogSpeaker DialogSpeaker = new() { NoSpeaker = true };
        }*/
    }
}

