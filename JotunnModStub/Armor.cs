using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using HarmonyLib;
using System;
using UnityEngine;

namespace ArmorMod
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.jotunn.jotunn", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
    internal class Armor : BaseUnityPlugin
    {
        public const string PluginGUID = "com.zarboz.RangerArmorSet";
        public const string PluginName = "RangerArmorSet";
        public const string PluginVersion = "1.1.0";

       // internal static ManualLogSource Log;
        private Harmony _harmony;
        public AssetBundle assetBundle;

        public static ConfigEntry<float> configArmorVest;
        public static ConfigEntry<float> configArmorPerLevelVest;
        public static ConfigEntry<float> configDurabilityVest;
        public static ConfigEntry<float> configMaxDurabilityVest;
        public static ConfigEntry<float> configDurabilityPerLevelVest;
        public static ConfigEntry<int> configMaxLevelVest;
        public static ConfigEntry<float> configArmorLegs;
        public static ConfigEntry<float> configArmorPerLevelLegs;
        public static ConfigEntry<float> configDurabilityLegs;
        public static ConfigEntry<float> configMaxDurabilityLegs;
        public static ConfigEntry<float> configDurabilityPerLevelLegs;
        public static ConfigEntry<int> configMaxLevelLegs;
        private void Awake()
        {
            base.Config.SaveOnConfigSet = true;
            CreateConfigurationValues();
            SynchronizationManager.OnConfigurationSynchronized += delegate (object obj, ConfigurationSynchronizationEventArgs trasync)
            {
                if (trasync.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Configuration synchronization event received for Troll Ranger Set");
                }
                else
                {
                    Jotunn.Logger.LogMessage("Configuration synchronization event received for Troll Ranger Set");
                }
            };
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.zarboz.RangerArmorSet");
            AssetLoad();
            LoadItem(); 
            BoneReorder.ApplyOnEquipmentChanged();     
        }
        
        private void CreateConfigurationValues()
        {
            configArmorVest = base.Config.Bind("Vest", "Armor", 100f, new ConfigDescription("The amount of AC the item has", new AcceptableValueRange<float>(1f, 100f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configArmorPerLevelVest = base.Config.Bind("Vest", "Armor Per Level", 30f, new ConfigDescription("How much AC to add per level", new AcceptableValueRange<float>(1f, 100f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configDurabilityVest = base.Config.Bind("Vest", "Durability", 1000f, new ConfigDescription("The amount of Durability the item starts with", new AcceptableValueRange<float>(1f, 10000f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configMaxDurabilityVest = base.Config.Bind("Vest", "Maximum Durability", 2500f, new ConfigDescription("The maximum amount of Durability the item can have", new AcceptableValueRange<float>(1f, 100000f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configDurabilityPerLevelVest = base.Config.Bind("Vest", "Durability per Level", 50f, new ConfigDescription("How much durability to add per level", new AcceptableValueRange<float>(1f, 1000f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configMaxLevelVest = base.Config.Bind("Vest", "Level", 10, new ConfigDescription("What level you can upgrade too", new AcceptableValueRange<int>(1, 100), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configArmorLegs = base.Config.Bind("Leggings", "Armor", 100f, new ConfigDescription("The amount of AC the item has", new AcceptableValueRange<float>(1f, 100f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configArmorPerLevelLegs = base.Config.Bind("Leggings", "Armor Per Level", 30f, new ConfigDescription("How much AC to add per level", new AcceptableValueRange<float>(1f, 100f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configDurabilityLegs = base.Config.Bind("Leggings", "Durability", 1000f, new ConfigDescription("The amount of Durability the item starts with", new AcceptableValueRange<float>(1f, 10000f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configMaxDurabilityLegs = base.Config.Bind("Leggings", "Maximum Durability", 2500f, new ConfigDescription("The maximum amount of Durability the item can have", new AcceptableValueRange<float>(1f, 100000f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configDurabilityPerLevelLegs = base.Config.Bind("Leggings", "Durability per Level", 50f, new ConfigDescription("How much durability to add per level", new AcceptableValueRange<float>(1f, 1000f), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
            configMaxLevelLegs = base.Config.Bind("Leggings", "Level", 10, new ConfigDescription("What level you can upgrade too", new AcceptableValueRange<int>(1, 100), null, new ConfigurationManagerAttributes
            {
                IsAdminOnly = true
            }));
        }
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("testarmor", typeof(Armor).Assembly);
        }
       
        private void LoadItem()
        {
            try
            {
                //Chest
                var chestplate = assetBundle.LoadAsset<GameObject>("Armor_TrollRanger_Chest");
                var armor = new CustomItem(chestplate, true,
                    new ItemConfig
                    {
                        Amount = 1,
                        CraftingStation = "forge",
                        Enabled = true,
                        Requirements = new []
                        {
                            new RequirementConfig {Item = "Iron", Amount = 6, AmountPerLevel = 2},
                            new RequirementConfig {Item = "TrollHide", Amount = 15, AmountPerLevel = 5},
                            new RequirementConfig {Item = "LeatherScraps", Amount = 15, AmountPerLevel = 5}
                        }
                    
                    }
                    );
                armor.ItemDrop.m_itemData.m_shared.m_name = "$item_rangervest";
                armor.ItemDrop.m_itemData.m_shared.m_description = "$item_rangervest_desc";

                var itemDropV = armor.ItemDrop;
                    itemDropV.m_itemData.m_shared.m_armor = configArmorVest.Value;
                    itemDropV.m_itemData.m_shared.m_armorPerLevel = configArmorPerLevelVest.Value;
                    itemDropV.m_itemData.m_durability = configDurabilityVest.Value;
                    itemDropV.m_itemData.m_shared.m_maxDurability = configMaxDurabilityVest.Value;
                    itemDropV.m_itemData.m_shared.m_durabilityPerLevel = configDurabilityPerLevelVest.Value;
                    itemDropV.m_itemData.m_shared.m_maxQuality = configMaxLevelVest.Value;
                ItemManager.Instance.AddItem(armor);
                // Legs
                var armorpants = assetBundle.LoadAsset<GameObject>("Armor_TrollRanger_Legs");
                var pants = new CustomItem(armorpants, true,
                    new ItemConfig
                    {
                        Amount = 1,
                        CraftingStation = "forge",
                        Enabled = true,
                        Requirements = new[]
                        {
                            new RequirementConfig {Item = "Iron", Amount = 6, AmountPerLevel = 2},
                            new RequirementConfig {Item = "TrollHide", Amount = 15, AmountPerLevel = 5},
                            new RequirementConfig {Item = "LeatherScraps", Amount = 15, AmountPerLevel = 5}
                        }

                    }
                    );
                pants.ItemDrop.m_itemData.m_shared.m_name = "$item_rangerleggings";
                pants.ItemDrop.m_itemData.m_shared.m_description = "$item_rangerleggings_desc";
                var itemDropL = pants.ItemDrop;
                    itemDropL.m_itemData.m_shared.m_armor = configArmorLegs.Value;
                    itemDropL.m_itemData.m_shared.m_armorPerLevel = configArmorPerLevelLegs.Value;
                    itemDropL.m_itemData.m_shared.m_armor = configArmorLegs.Value;
                    itemDropL.m_itemData.m_shared.m_maxDurability = configMaxDurabilityLegs.Value;
                    itemDropL.m_itemData.m_shared.m_durabilityPerLevel = configDurabilityPerLevelLegs.Value;
                    itemDropL.m_itemData.m_shared.m_maxQuality = configMaxLevelLegs.Value;
                ItemManager.Instance.AddItem(pants);

            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Exception caught while adding Troll Ranger Armor: {ex}");
            }
            finally
            {
                assetBundle.Unload(false);
            }
        }
    }
}