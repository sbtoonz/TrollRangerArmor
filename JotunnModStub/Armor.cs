using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using System;

namespace ArmorMod
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("com.jotunn.jotunn")]
    internal class Armor : BaseUnityPlugin
    {
        public const string PluginGUID = "com.zarboz.RangerArmorSet";
        public const string PluginName = "RangerArmorSet";
        public const string PluginVersion = "1.0.0";
        public AssetBundle assetBundle;
        private void Awake()
        {
            AssetLoad();
            LoadItem(); 
            BoneReorder.ApplyOnEquipmentChanged();
            
        }
        
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("testarmor", typeof(Armor).Assembly);


        }
       
        private void LoadItem()
        {
            //piece_grill

            var chestplate = assetBundle.LoadAsset<GameObject>("Armor_TrollRanger_Chest");
            var armor = new CustomItem(chestplate, true,
                new ItemConfig
                {
                    Amount = 1,
                    Name = "Troll Ranger Chest gear",
                    CraftingStation = "forge",
                    Enabled = true,
                    Requirements = new []
                    {
                        new RequirementConfig {Item = "TrollHide", Amount = 15, AmountPerLevel = 5},
                        new RequirementConfig {Item = "LeatherScraps", Amount = 15, AmountPerLevel = 5}
                    }
                    
                }
                );
            armor.ItemDrop.m_itemData.m_shared.m_description = "Fine armor made from iron and troll hide";

            var armorpants = assetBundle.LoadAsset<GameObject>("Armor_TrollRanger_Legs");
            var pants = new CustomItem(armorpants, true,
                new ItemConfig
                {
                    Amount = 1,
                    Name = "Troll reinforced ranger pants",
                    CraftingStation = "forge",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "TrollHide", Amount = 15, AmountPerLevel = 5},
                        new RequirementConfig {Item = "LeatherScraps", Amount = 15, AmountPerLevel = 5}
                    }

                }
                );
            pants.ItemDrop.m_itemData.m_shared.m_description = "Fine pants made from troll hide and iron";
            var itemDrop = armor.ItemDrop;
            itemDrop.m_itemData.m_shared.m_armor = 100f;
            itemDrop.m_itemData.m_shared.m_armorPerLevel = 30f;
            itemDrop.m_itemData.m_shared.m_maxDurability = 2500;
            itemDrop.m_itemData.m_shared.m_maxQuality = 10;
            itemDrop.m_itemData.m_shared.m_toolTier = 10;
            ItemManager.Instance.AddItem(armor);
            ItemManager.Instance.AddItem(pants);
        }

    }
}