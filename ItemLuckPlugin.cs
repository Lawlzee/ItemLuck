using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions.Options;
using RiskOfOptions;
using RoR2;
using UnityEngine;
using System.IO;
using RiskOfOptions.OptionConfigs;
using R2API;
using RoR2.ExpansionManagement;

namespace ItemLuck
{
    [BepInDependency("com.rune580.riskofoptions")]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ItemLuckPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "Lawlzee.ItemLuck";
        public const string PluginAuthor = "Lawlzee";
        public const string PluginName = "ItemLuck";
        public const string PluginVersion = "1.0.0";

        public static ConfigEntry<bool> ModEnabled;
        public static ConfigEntry<float> ItemLuck;
        public static TierList TierList;

        //private ItemDef _itemLuckItem;
        //private ItemDef _voidItemLuckItem;

        public static bool UpdateLuckEnabled => ModEnabled.Value && ItemLuck.Value != 0;

        public void Awake()
        {
            Log.Init(Logger);

            //On.RoR2.Items.ContagiousItemManager.Init += ContagiousItemManager_Init;

            ModEnabled = Config.Bind("Configuration", "Mod enabled", true, "Mod enabled");
            ModSettingsManager.AddOption(new CheckBoxOption(ModEnabled));

            ItemLuck = Config.Bind("Configuration", "Item luck", 0f, "todo");

            UpdatableStepSliderOption option = new UpdatableStepSliderOption(ItemLuck, new StepSliderConfig() { min = -5, max = 5, increment = 0.05f, formatString = "{0:+0.##;-0.##;0.##}" });
            ModSettingsManager.AddOption(option);

            ItemLuck.SettingChanged += (_, _) =>
            {
                UpdatableOptions.UpdateDescriptions();
                option.UpdatePanel();
                //choiceOption.description = itemConfig.Value.ToString();
            };

            var texture = LoadTexture("icon.png");
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            ModSettingsManager.SetModIcon(sprite);

            Hooks.Init(Config);

            //_itemLuckItem = CreateLuckItem(
            //    tokenName: "ItemLuck",
            //    model: "RabbitFoot.png",
            //    texture: "BorderedRabbitFoot.png",
            //    itemTier: ItemTier.Tier3,
            //    name: "Lucky Rabbit's Foot",
            //    pickup: "Increases item luck",
            //    description: "Increases <style=cIsUtility>item luck</style> by <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style>",
            //    lore: "In the depths of the enchanted forest, there once lived a particularly fortunate rabbit. Legend has it that this rabbit's foot was said to bring luck to anyone who possessed it. The rabbit's foot is believed to hold the essence of the rabbit's incredible luck, making it a highly sought-after item among adventurers. When carried, it increases the odds of finding valuable items and hidden treasures, making it a must-have for treasure hunters and explorers.",
            //    expansion: null);
            //
            //_voidItemLuckItem = CreateLuckItem(
            //    tokenName: "VoidItemLuck",
            //    model: "Feather.png",
            //    texture: "BorderedFeather.png",
            //    itemTier: ItemTier.VoidTier3,
            //    name: "Unlucky Crow's Feather",
            //    pickup: "Decreases item luck",
            //    description: "Decreases <style=cIsUtility>item luck</style> by <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style>. <style=cIsVoid>Corrupts all Lucky Rabbit's Foot</style>.",
            //    lore: "The Unlucky Crow's Feather is a cursed artifact, said to be plucked from the tail of a legendary shadowy crow that brings misfortune to those who cross its path. Possessing this feather might make you seem more intimidating, but it will undoubtedly reduce your chances of finding good items. It is believed that this feather is tied to the mysterious Crow of Ill Omen, a creature that brings bad luck and misfortune to anyone who dares to collect its feathers.",
            //    expansion: ExpansionCatalog.expansionDefs.FirstOrDefault(def => def.nameToken == "DLC1_NAME"));
        }

        private ItemDef CreateLuckItem(
            string tokenName,
            string model,
            string texture,
            ItemTier itemTier,
            string name,
            string pickup,
            string description,
            string lore,
            ExpansionDef expansion)
        {
            var texture2d = LoadTexture(texture);
            var sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0, 0));


            var prefab = ItemModelFactory.Create(LoadTexture(model), tokenName, 1 / 25f, 0.6f);
            DontDestroyOnLoad(prefab);

            var itemDef = ScriptableObject.CreateInstance<ItemDef>();
            itemDef.name = "ITEM_" + tokenName;
            itemDef.nameToken = "ITEM_" + tokenName + "_NAME";
            itemDef.pickupToken = "ITEM_" + tokenName + "_PICKUP";
            itemDef.descriptionToken = "ITEM_" + tokenName + "_DESCRIPTION";
            itemDef.loreToken = "ITEM_" + tokenName + "_LORE";
            itemDef.pickupModelPrefab = prefab;
            itemDef.pickupIconSprite = sprite;
            itemDef.hidden = false;
            itemDef.deprecatedTier = itemTier;
            itemDef.requiredExpansion = expansion;

            LanguageAPI.Add(itemDef.nameToken, name);
            LanguageAPI.Add(itemDef.pickupToken, pickup);
            LanguageAPI.Add(itemDef.descriptionToken, description);
            LanguageAPI.Add(itemDef.loreToken, lore);

            ItemAPI.Add(new CustomItem(itemDef, default(ItemDisplayRule[])));

            return itemDef;
        }

        private Texture2D LoadTexture(string name)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(File.ReadAllBytes(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Info.Location), name)));
            return texture;
        }


        //private void ContagiousItemManager_Init(On.RoR2.Items.ContagiousItemManager.orig_Init orig)
        //{
        //    ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem] = ItemCatalog.itemRelationships[DLC1Content.ItemRelationshipTypes.ContagiousItem]
        //        .AddToArray(new ItemDef.Pair()
        //        {
        //            itemDef1 = _itemLuckItem,
        //            itemDef2 = _voidItemLuckItem
        //        });
        //
        //    orig();
        //}

        //private float GetItemLuckValue()
        //{
        //    float itemLuck = _itemLuck.Value;
        //    foreach (CharacterMaster readOnlyInstances in CharacterMaster.readOnlyInstancesList)
        //    {
        //        itemLuck += readOnlyInstances.inventory.GetItemCount(_itemLuckItem);
        //        itemLuck -= readOnlyInstances.inventory.GetItemCount(_voidItemLuckItem);
        //    }
        //    return itemLuck;
        //}

        
    }
}
