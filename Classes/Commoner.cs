using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace WrathCommonerClass.Classes
{
    static class Commoner
    {
        public static void Load()
        {
            var cleric = Resources.GetBlueprint<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0");
            var bp = Utils.CreateBlueprint<BlueprintCharacterClass>("Commoner");
            var bpRef = BlueprintReference<BlueprintCharacterClass>.CreateTyped<BlueprintCharacterClassReference>(bp);
            bp.name = "CommonerClass";
            bp.LocalizedName = Utils.CreateString("Commoner.Name", "Commoner");
            bp.LocalizedDescription = Utils.CreateString("Commoner.Description", "Has no limitations, yet nothing given.");
            bp.LocalizedDescriptionShort = Utils.CreateString("Commoner.DescriptionShort", "Has no limitations, yet nothing given.");
            bp.m_Icon = cleric.Icon;
            bp.SkillPoints = 2;
            bp.SetHitDice(Kingmaker.RuleSystem.DiceType.D6);
            bp.SetBaseAttackBonus(BlueprintRoot.Instance.Progression.StatProgressions.BABLow);
            bp.SetFortitudeSave(BlueprintRoot.Instance.Progression.StatProgressions.SavesLow);
            bp.SetReflexSave(BlueprintRoot.Instance.Progression.StatProgressions.SavesLow);
            bp.SetWillSave(BlueprintRoot.Instance.Progression.StatProgressions.SavesLow);
            bp.SetDifficulty(BlueprintCharacterClass.MaxDifficulty);
            bp.ClassSkills = new StatType[]
            {
                StatType.SkillAthletics,
                StatType.SkillMobility,
                StatType.SkillPerception
            };

            bp.PrimaryColor = cleric.PrimaryColor;
            bp.SecondaryColor = cleric.SecondaryColor;
            bp.SetProgression(BlueprintReference<BlueprintProgression>.CreateTyped<BlueprintProgressionReference>(CommonerProgression(bpRef)));

            var classes = BlueprintRoot.Instance.Progression.GetCharacterClasses().ToList();
            classes.Add(bpRef);
            classes.Sort((x, y) =>
            {
                if (x.Get().PrestigeClass != y.Get().PrestigeClass) return x.Get().PrestigeClass ? 1 : -1;
                return x.Get().Name.CompareTo(y.Get().Name);
            });
            BlueprintRoot.Instance.Progression.SetCharacterClasses(classes.ToArray());
        }

        public static BlueprintProgression CommonerProgression(BlueprintCharacterClassReference commoner)
        {
            var bp = Utils.CreateBlueprint<BlueprintProgression>("CommonerProgression");
            bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            typeof(BlueprintProgression).GetField("m_Classes", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, new BlueprintProgression.ClassWithLevel[] { new BlueprintProgression.ClassWithLevel { m_Class = commoner } });
            bp.IsClassFeature = true;
            //bp.UIDeterminatorsGroup = Array.Empty<BlueprintFeatureBase>();
            typeof(BlueprintUnitFact).GetField("m_DisplayName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerProgression.Name", ""));
            var firstLevel = new LevelEntry();
            firstLevel.Level = 1;
            
            //Resources.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd")
            firstLevel.Features.Add(CommonerProficiencies());
            bp.AddLevelEntry(firstLevel);
            return bp;
        }

        public static BlueprintFeature CommonerProficiencies()
        {
            var bp = Utils.CreateBlueprint<BlueprintFeature>("CommonerProficienciesFeature");
            typeof(BlueprintUnitFact).GetField("m_DisplayName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerWeaponProficiency.Name", "Commoner Weapon Proficiency"));
            typeof(BlueprintUnitFact).GetField("m_Description", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerWeaponProficiency.Description", "The commoner is proficient with simple weapons. He is not proficient with any other weapons, nor is he proficient with any type of armor or shield."));
            typeof(BlueprintUnitFact).GetField("m_DescriptionShort", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerWeaponProficiency.DescriptionShort", "The commoner is proficient with simple weapons. He is not proficient with any other weapons, nor is he proficient with any type of armor or shield."));
            bp.IsClassFeature = true;
            var addFacts = new AddFacts();
            addFacts.OwnerBlueprint = bp;
            typeof(AddFacts).GetField("m_Facts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(addFacts, new BlueprintUnitFactReference[]{
                BlueprintReference<BlueprintUnitFact>.CreateTyped<BlueprintUnitFactReference>(Resources.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd"))
            });
            var comps = bp.ComponentsArray.ToList();
            comps.Add(addFacts);
            bp.ComponentsArray = comps.ToArray();
            return bp;
        }
    }
}
