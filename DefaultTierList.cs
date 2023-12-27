using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemLuck;

public class DefaultTierList
{
    public List<Rank<ItemDef>> Items { get; }
    public List<Rank<EquipmentDef>> Equipments { get; }

    private DefaultTierList(List<Rank<ItemDef>> items, List<Rank<EquipmentDef>> equipments)
    {
        Items = items;
        Equipments = equipments;
    }

    public static DefaultTierList Create()
    {
        List<Rank<ItemDef>> items = new List<Rank<ItemDef>>
        {
            // White -
            new(RoR2Content.Items.Hoof, Tier.S),//White - Paul's Goat Hoof 
            new(RoR2Content.Items.CritGlasses, Tier.S),//White - Lens-Maker's Glasses 
            new(RoR2Content.Items.SprintBonus, Tier.S),//White - Energy Drink 
            new(DLC1Content.Items.FragileDamageBonus, Tier.S),//White - Delicate Watch 
            new(DLC1Content.Items.AttackSpeedAndMoveSpeed, Tier.A),//White - Mocha 
            new(RoR2Content.Items.IgniteOnKill, Tier.A),//White - Gasoline 
            new(RoR2Content.Items.BleedOnHit, Tier.A),//White - Tri-Tip Dagger 
            new(RoR2Content.Items.Syringe, Tier.A),//White - Soldier's Syringe 
            new(RoR2Content.Items.Bear, Tier.A),//White - Tougher Times 
            new(RoR2Content.Items.HealWhileSafe, Tier.A),//White - Cautious Slug 
            new(DLC1Content.Items.OutOfCombatArmor, Tier.A),//White - Oddly-shaped Opal 
            new(RoR2Content.Items.BossDamageBonus, Tier.A),//White - Armor-Piercing Rounds 
            new(RoR2Content.Items.TreasureCache, Tier.B),//White - Rusted Key 
            new(RoR2Content.Items.StickyBomb, Tier.B),//White - Sticky Bomb 
            new(RoR2Content.Items.Crowbar, Tier.B),//White - Crowbar 
            new(RoR2Content.Items.ArmorPlate, Tier.B),//White - Repulsion Armor Plate 
            new(RoR2Content.Items.NearbyDamageBonus, Tier.B),//White - Focus Crystal 
            new(RoR2Content.Items.SecondarySkillMagazine, Tier.B),//White - Backup Magazine 
            new(RoR2Content.Items.BarrierOnKill, Tier.C),//White - Topaz Brooch 
            new(RoR2Content.Items.PersonalShield, Tier.C),//White - Personal Shield Generator 
            new(DLC1Content.Items.HealingPotion, Tier.C),//White - Power Elixir 
            new(RoR2Content.Items.Firework, Tier.C),//White - Bundle of Fireworks 
            new(RoR2Content.Items.Tooth, Tier.C),//White - Monster Tooth 
            new(DLC1Content.Items.GoldOnHurt, Tier.D),//White - Roll of Pennies 
            new(RoR2Content.Items.Mushroom, Tier.D),//White - Bustling Fungus 
            new(RoR2Content.Items.Medkit, Tier.D),//White - Medkit 
            new(RoR2Content.Items.StunChanceOnHit, Tier.D),//White - Stun Grenade 
            new(RoR2Content.Items.WardOnLevel, Tier.F),//White - Warbanner 
            new(RoR2Content.Items.FlatHealth, Tier.F),//White - Bison Steak 
            
            // Green - https://www.youtube.com/watch?v=9hmB2DvxRGg
            new(RoR2Content.Items.Feather, Tier.S),//Green - Hopoo Feather 
            new(RoR2Content.Items.IceRing, Tier.S),//Green - Runald's Band 
            new(RoR2Content.Items.FireRing, Tier.S),//Green - Kjaro's Band 
            new(RoR2Content.Items.Missile, Tier.S),//Green - AtG Missile Mk. 1 
            new(RoR2Content.Items.ChainLightning, Tier.A),//Green - Ukulele 
            new(RoR2Content.Items.ExplodeOnDeath, Tier.A),//Green - Will-o'-the-wisp 
            new(DLC1Content.Items.PrimarySkillShuriken, Tier.A),//Green - Shuriken 
            new(DLC1Content.Items.StrengthenBurn, Tier.B),//Green - Ignition Tank 
            new(RoR2Content.Items.JumpBoost, Tier.B),//Green - Wax Quail  
            new(RoR2Content.Items.SprintArmor, Tier.B),//Green - Rose Buckler 
            new(RoR2Content.Items.BonusGoldPackOnKill, Tier.C),//Green - Ghor's Tome 
            new(RoR2Content.Items.EquipmentMagazine, Tier.C),//Green - Fuel Cell 
            new(DLC1Content.Items.FreeChest, Tier.C),//Green - Shipping Request Form 
            new(RoR2Content.Items.SlowOnHit, Tier.C),//Green - Chronobauble 
            new(RoR2Content.Items.AttackSpeedOnCrit, Tier.C),//Green - Predatory Instincts 
            new(RoR2Content.Items.SprintOutOfCombat, Tier.C),//Green - Red Whip 
            new(RoR2Content.Items.ExecuteLowHealthElite, Tier.C),//Green - Old Guillotine 
            new(RoR2Content.Items.DeathMark, Tier.C),//Green - Death Mark 
            new(RoR2Content.Items.EnergizedOnEquipmentUse, Tier.D),//Green - War Horn 
            new(RoR2Content.Items.Bandolier, Tier.D),//Green - Bandolier 
            new(DLC1Content.Items.RegeneratingScrap, Tier.D),//Green - Regenerating Scrap 
            new(RoR2Content.Items.HealOnCrit, Tier.D),//Green - Harvester's Scythe 
            new(RoR2Content.Items.Squid, Tier.F),//Green - Squid Polyp 
            new(RoR2Content.Items.Thorns, Tier.F),//Green - Razorwire 
            new(RoR2Content.Items.Infusion, Tier.F),//Green - Infusion 
            new(RoR2Content.Items.WarCryOnMultiKill, Tier.F),//Green - Berzerker's Pauldron 
            new(RoR2Content.Items.Phasing, Tier.F),//Green - Old War Stealthkit 
            new(DLC1Content.Items.MoveSpeedOnKill, Tier.F),//Green - Hunter's Harpoon 
            new(RoR2Content.Items.Seed, Tier.F),//Green - Leeching Seed 
            new(RoR2Content.Items.TPHealingNova, Tier.F),//Green - Lepton Daisy 

            // Void - https://www.youtube.com/watch?v=s-UDQvmpvbY
            new(DLC1Content.Items.TreasureCacheVoid, Tier.S),//Void White - Encrusted Key 
            new(DLC1Content.Items.BearVoid, Tier.S),//Void White - Safer Spaces 
            new(DLC1Content.Items.ChainLightningVoid, Tier.S),//Void Green - Polylute 
            new(DLC1Content.Items.MissileVoid, Tier.S),//Void Green - Plasma Shrimp 
            new(DLC1Content.Items.ExplodeOnDeathVoid, Tier.A),//Void Green - Voidsent Flame 
            new(DLC1Content.Items.MushroomVoid, Tier.A),//Void White - Weeping Fungus 
            new(DLC1Content.Items.ExtraLifeVoid, Tier.A),//Void Red - Pluripotent Larva 
            new(DLC1Content.Items.EquipmentMagazineVoid, Tier.A),//Void Green - Lysate Cell 
            new(DLC1Content.Items.BleedOnHitVoid, Tier.B),//Void White - Needletick 
            new(DLC1Content.Items.SlowOnHitVoid, Tier.B),//Void Green - Tentabauble 
            new(DLC1Content.Items.CritGlassesVoid, Tier.D),//Void White - Lost Seer's Lenses 
            new(DLC1Content.Items.CloverVoid, Tier.D),//Void Red - Benthic Bloom 
            new(DLC1Content.Items.VoidMegaCrabItem, Tier.F),//Void Boss - Newly Hatched Zoea 
            new(DLC1Content.Items.ElementalRingVoid, Tier.F),//Void Green - Singularity Band 

            // Lunar - https://www.youtube.com/watch?v=ldc8PT054Bw
            new(RoR2Content.Items.ShieldOnly, Tier.S),//Lunar - Transcendence 
            new(RoR2Content.Items.AutoCastEquipment, Tier.S),//Lunar - Gesture of the Drowned 
            new(RoR2Content.Items.LunarSpecialReplacement, Tier.S),//Lunar - Essence of Heresy 
            new(RoR2Content.Items.LunarDagger, Tier.S),//Lunar - Shaped Glass 
            new(DLC1Content.Items.RandomlyLunar, Tier.A),//Lunar - Eulogy Zero 
            new(RoR2Content.Items.FocusConvergence, Tier.B),//Lunar - Focused Convergence 
            new(RoR2Content.Items.LunarBadLuck, Tier.B),//Lunar - Purity 
            new(RoR2Content.Items.LunarUtilityReplacement, Tier.B),//Lunar - Strides of Heresy 

            new(DLC1Content.Items.LunarSun, Tier.B),//Lunar - Egocentrism 
            new(RoR2Content.Items.LunarPrimaryReplacement, Tier.B),//Lunar - Visions of Heresy 
            new(RoR2Content.Items.LunarSecondaryReplacement, Tier.B),//Lunar - Hooks of Heresy 
            new(RoR2Content.Items.RepeatHeal, Tier.C),//Lunar - Corpsebloom 
            new(RoR2Content.Items.GoldOnHit, Tier.C),//Lunar - Brittle Crown 
            new(RoR2Content.Items.RandomDamageZone, Tier.C),//Lunar - Mercurial Rachis 
            new(RoR2Content.Items.LunarTrinket, Tier.D),//Lunar - Beads of Fealty 
            new(RoR2Content.Items.MonstersOnShrineUse, Tier.D),//Lunar - Defiant Gouge 
            new(DLC1Content.Items.HalfAttackSpeedHalfCooldowns, Tier.F),//Lunar - Light Flux Pauldron 
            new(DLC1Content.Items.HalfSpeedDoubleHealth, Tier.F),//Lunar - Stone Flux Pauldron 

            // Yellow - https://www.youtube.com/watch?v=k6SireTv8t4
            new(RoR2Content.Items.LightningStrikeOnHit, Tier.S),//Boss - Charged Perforator 
            new(RoR2Content.Items.FireballsOnHit, Tier.S),//Boss - Molten Perforator 
            new(RoR2Content.Items.BleedOnHitAndExplode, Tier.S),//Boss - Shatterspleen 
            new(RoR2Content.Items.ShinyPearl, Tier.S),//Boss - Irradiant Pearl 
            new(RoR2Content.Items.SprintWisp, Tier.S),//Boss - Little Disciple 
            new(RoR2Content.Items.RoboBallBuddy, Tier.A),//Boss - Empathy Cores 
            new(RoR2Content.Items.SiphonOnLowHealth, Tier.C),//Boss - Mired Urn 
            new(RoR2Content.Items.BeetleGland, Tier.C),//Boss - Queen's Gland 
            new(RoR2Content.Items.NovaOnLowHealth, Tier.D),//Boss - Genesis Loop 
            new(RoR2Content.Items.TitanGoldDuringTP, Tier.D),//Boss - Halcyon Seed 
            new(RoR2Content.Items.ParentEgg, Tier.D),//Boss - Planula 
            new(RoR2Content.Items.Pearl, Tier.D),//Boss - Pearl 
            new(RoR2Content.Items.Knurl, Tier.D),//Boss - Titanic Knurl 
            new(DLC1Content.Items.MinorConstructOnKill, Tier.F),//Boss - Defense Nucleus 

            // Red - https://www.youtube.com/watch?v=T7FwqTjO39s
            new(RoR2Content.Items.FallBoots, Tier.S),//Red - H3AD-5T v2 
            new(RoR2Content.Items.Clover, Tier.S),//Red - 57 Leaf Clover 
            new(DLC1Content.Items.DroneWeapons, Tier.S),//Red - Spare Drone Parts 
            new(RoR2Content.Items.ExtraLife, Tier.S),//Red - Dio's Best Friend 
            new(RoR2Content.Items.BounceNearby, Tier.S),//Red - Sentient Meat Hook 
            new(DLC1Content.Items.PermanentDebuffOnHit, Tier.S),//Red - Symbiotic Scorpion 
            new(RoR2Content.Items.Behemoth, Tier.S),//Red - Brilliant Behemoth 
            new(DLC1Content.Items.MoreMissile, Tier.A),//Red - Pocket I.C.B.M. 
            new(RoR2Content.Items.ShockNearby, Tier.A),//Red - Unstable Tesla Coil 
            new(RoR2Content.Items.Dagger, Tier.A),//Red - Ceremonial Dagger 
            new(RoR2Content.Items.ArmorReductionOnHit, Tier.A),//Red - Shattering Justice 
            new(DLC1Content.Items.CritDamage, Tier.A),//Red - Laser Scope 
            new(RoR2Content.Items.UtilitySkillMagazine, Tier.A),//Red - Hardlight Afterburner 
            new(RoR2Content.Items.Icicle, Tier.B),//Red - Frost Relic 
            new(RoR2Content.Items.Talisman, Tier.C),//Red - Soulbound Catalyst 
            new(RoR2Content.Items.LaserTurbine, Tier.C),//Red - Resonance Disc 
            new(RoR2Content.Items.Plant, Tier.C),//Red - Interstellar Desk Plant 
            new(RoR2Content.Items.AlienHead, Tier.C),//Red - Alien Head 
            new(RoR2Content.Items.IncreaseHealing, Tier.D),//Red - Rejuvenation Rack 
            new(RoR2Content.Items.KillEliteFrenzy, Tier.D),//Red - Brainstalks 
            new(DLC1Content.Items.ImmuneToDebuff, Tier.D),//Red - Ben's Raincoat 
            new(RoR2Content.Items.GhostOnKill, Tier.D),//Red - Happiest Mask 
            new(DLC1Content.Items.RandomEquipmentTrigger, Tier.D),//Red - Bottled Chaos 
            new(RoR2Content.Items.HeadHunter, Tier.F),//Red - Wake of Vultures 
            new(RoR2Content.Items.NovaOnHeal, Tier.F),//Red - N'kuhana's Opinion 
            new(RoR2Content.Items.BarrierOnOverHeal, Tier.F),//Red - Aegis 

            // N/A
            //new(RoR2Content.Items.ScrapWhite, Tier.S),//White - Item Scrap, White 
            //new(RoR2Content.Items.ScrapGreen, Tier.S),//Green - Item Scrap, Green 
            //new(RoR2Content.Items.ScrapRed, Tier.S),//Red - Item Scrap, Red 
            //new(RoR2Content.Items.ScrapYellow, Tier.S),//Boss - Item Scrap, Yellow 
            //new(DLC1Content.Items.DroneWeaponsBoost, Tier.S),//No Tier - ITEM_DRONEWEAPONSBOOST_NAME 
            //new(DLC1Content.Items.DroneWeaponsDisplay1, Tier.S),//No Tier - ITEM_DRONEWEAPONSDISPLAY1_NAME 
            //new(DLC1Content.Items.DroneWeaponsDisplay2, Tier.S),//No Tier - ITEM_DRONEWEAPONSDISPLAY2_NAME 
            //new(DLC1Content.Items.VoidmanPassiveItem, Tier.S),//No Tier - 
            //new(RoR2Content.Items.ArtifactKey, Tier.S),//Boss - Artifact Key
            //new(RoR2Content.Items.LevelBonus, Tier.S),//No Tier - ITEM_LEVELBONUS_NAME 
            //new(RoR2Content.Items.BoostHp, Tier.S),//No Tier - ITEM_BOOSTHP_NAME 
            //new(RoR2Content.Items.BoostDamage, Tier.S),//No Tier - ITEM_BOOSTDAMAGE_NAME 
            //new(RoR2Content.Items.CrippleWardOnLevel, Tier.S),//No Tier - ITEM_CRIPPLEWARDONLEVEL_NAME 
            //new(RoR2Content.Items.ExtraLifeConsumed, Tier.S),//No Tier - Dio's Best Friend (Consumed) 
            //new(RoR2Content.Items.Ghost, Tier.S),//No Tier - ITEM_GHOST_NAME 
            //new(RoR2Content.Items.HealthDecay, Tier.S),//No Tier - ITEM_HEALTHDECAY_NAME 
            //new(RoR2Content.Items.DrizzlePlayerHelper, Tier.S),//No Tier - ITEM_DRIZZLEPLAYERHELPER_NAME
            //new(RoR2Content.Items.TonicAffliction, Tier.S),//No Tier - Tonic Affliction 
            //new(RoR2Content.Items.MonsoonPlayerHelper, Tier.S),//No Tier - ITEM_MONSOONPLAYERHELPER_NAME 
            //new(RoR2Content.Items.InvadingDoppelganger, Tier.S),//No Tier - ITEM_INVADINGDOPPELGANGER_NAME 
            //new(RoR2Content.Items.CutHp, Tier.S),//No Tier - ITEM_CUTHP_NAME 
            //new(RoR2Content.Items.BoostAttackSpeed, Tier.S),//No Tier - ITEM_BOOSTATTACKSPEED_NAME 
            //new(RoR2Content.Items.AdaptiveArmor, Tier.S),//No Tier - ITEM_ADAPTIVEARMOR_NAME 
            //new(RoR2Content.Items.BoostEquipmentRecharge, Tier.S),//No Tier - ITEM_BOOSTEQUIPMENTRECHARGE_NAME 
            //new(RoR2Content.Items.TeamSizeDamageBonus, Tier.S),//No Tier - 
            //new(RoR2Content.Items.MinionLeash, Tier.S),//No Tier - ITEM_MINIONLEASH_NAME 
            //new(RoR2Content.Items.UseAmbientLevel, Tier.S),//No Tier - 
            //new(RoR2Content.Items.TeleportWhenOob, Tier.S),//No Tier - 
            //new(RoR2Content.Items.MinHealthPercentage, Tier.S),//No Tier - 
            //new(DLC1Content.Items.HealingPotionConsumed, Tier.S),//No Tier - Empty Bottle 
            //new(DLC1Content.Items.GummyCloneIdentifier, Tier.S),//No Tier - ITEM_GUMMYCLONEIDENTIFIER_NAME 
            //new(DLC1Content.Items.RegeneratingScrapConsumed, Tier.S),//No Tier - Regenerating Scrap (Consumed) 
            //new(DLC1Content.Items.ExtraLifeVoidConsumed, Tier.S),//No Tier - Pluripotent Larva (Consumed) 
            //new(DLC1Content.Items.FragileDamageBonusConsumed, Tier.S),//No Tier - Delicate Watch (Broken) 
            //new(DLC1Content.Items.ScrapWhiteSuppressed, Tier.S),//No Tier - Strange Scrap, White 
            //new(DLC1Content.Items.ScrapGreenSuppressed, Tier.S),//No Tier - Strange Scrap, Green 
            //new(DLC1Content.Items.ScrapRedSuppressed, Tier.S),//No Tier - Strange Scrap, Red 
            //new(DLC1Content.Items.ConvertCritChanceToCritDamage, Tier.S),//No Tier - ITEM_CONVERTCRITCHANCETOCRITDAMAGE_NAME 
            //new(RoR2Content.Items.SummonedEcho, Tier.S),//- 
            //new(RoR2Content.Items.CaptainDefenseMatrix, Tier.S),//Red - Defensive Microbots 
        };

        List<Rank<EquipmentDef>> equipements = new List<Rank<EquipmentDef>>
        {
            // Equipment - https://www.youtube.com/watch?v=GcO0QrzSPb4
            new(RoR2Content.Equipment.Tonic, Tier.S),//Lunar - Spinel Tonic 
            new(RoR2Content.Equipment.Recycle, Tier.S),//Normal - Recycler 
            new(DLC1Content.Equipment.BossHunter, Tier.S),//Normal - Trophy Hunter's Tricorn 
            new(DLC1Content.Equipment.MultiShopCard, Tier.S),//Normal - Executive Card 
            new(RoR2Content.Equipment.Lightning, Tier.A),//Normal - Royal Capacitor 
            new(RoR2Content.Equipment.GoldGat, Tier.A),//Normal - The Crowdfunder 
            new(RoR2Content.Equipment.Jetpack, Tier.A),//Normal - Milky Chrysalis 
            new(RoR2Content.Equipment.Saw, Tier.A),//Normal - Sawmerang 
            new(RoR2Content.Equipment.CommandMissile, Tier.B),//Normal - Disposable Missile Launcher 

            new(RoR2Content.Equipment.FireBallDash, Tier.B),//Normal - Volcanic Egg 
            new(RoR2Content.Equipment.Gateway, Tier.B),//Normal - Eccentric Vase 
            new(RoR2Content.Equipment.BFG, Tier.C),//Normal - Preon Accumulator 
            new(DLC1Content.Equipment.GummyClone, Tier.C),//Normal - Goobo Jr. 
            new(RoR2Content.Equipment.Cleanse, Tier.C),//Normal - Blast Shower 
            new(DLC1Content.Equipment.VendingMachine, Tier.C),//Normal - Remote Caffeinator 
            new(RoR2Content.Equipment.BurnNearby, Tier.C),//Lunar - Helfire Tincture 
            new(RoR2Content.Equipment.Blackhole, Tier.C),//Normal - Primordial Cube 
            
            // https://www.youtube.com/watch?v=GcO0QrzSPb4
            new(RoR2Content.Equipment.DeathProjectile, Tier.C),//Normal - Forgive Me Please 
            new(RoR2Content.Equipment.CritOnUse, Tier.C),//Normal - Ocular HUD 
            new(RoR2Content.Equipment.GainArmor, Tier.C),//Normal - Jade Elephant 
            new(RoR2Content.Equipment.Meteor, Tier.C),//Lunar - Glowing Meteorite 
            new(RoR2Content.Equipment.TeamWarCry, Tier.C),//Normal - Gorag's Opus 
            new(RoR2Content.Equipment.LifestealOnHit, Tier.D),//Normal - Super Massive Leech 
            new(RoR2Content.Equipment.PassiveHealing, Tier.D),//Normal - Gnarled Woodsprite 
            new(RoR2Content.Equipment.Fruit, Tier.D),//Normal - Foreign Fruit 
            new(RoR2Content.Equipment.DroneBackup, Tier.F),//Normal - The Back-up 
            new(DLC1Content.Equipment.Molotov, Tier.F),//Normal - Molotov (6-Pack) 
            new(RoR2Content.Equipment.Scanner, Tier.F),//Normal - Radar Scanner 
            new(RoR2Content.Equipment.CrippleWard, Tier.F),//Lunar - Effigy of Grief 

            // N/A
            //new(RoR2Content.Equipment.AffixRed, Tier.S),//Normal - Ifrit's Distinction
            //new(RoR2Content.Equipment.AffixBlue, Tier.S),//Normal - Silence Between Two Strikes
            //new(RoR2Content.Equipment.AffixWhite, Tier.S),//Normal - Her Biting Embrace 
            //new(RoR2Content.Equipment.AffixPoison, Tier.S),//Normal - N'kuhana's Retort
            //new(RoR2Content.Equipment.AffixHaunted, Tier.S),//Normal - Spectral Circlet
            //new(RoR2Content.Equipment.AffixEcho, Tier.S),//- 
            //new(RoR2Content.Equipment.AffixLunar, Tier.S),//Normal - Shared Design
            //new(DLC1Content.Equipment.EliteVoidEquipment, Tier.S),//Normal - EQUIPMENT_AFFIXVOID_NAME 
            //new(RoR2Content.Equipment.QuestVolatileBattery, Tier.S),//Normal - Fuel Array 
            //new(DLC1Content.Equipment.BossHunterConsumed, Tier.S),//Normal - Trophy Hunter's Tricorn (Consumed) 
            //new(RoR2Content.Equipment.LunarPotion, Tier.S),//Normal - EQUIPMENT_LUNARPOTION_NAME 
            //new(DLC1Content.Equipment.LunarPortalOnUse, Tier.S),//Lunar - Elegy of Extinction 
        };

        return new DefaultTierList(items, equipements);
    }
}
