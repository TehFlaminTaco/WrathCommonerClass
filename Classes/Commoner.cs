using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
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

            bp.PrimaryColor = cleric.PrimaryColor;
            bp.SecondaryColor = cleric.SecondaryColor;
            bp.SetDefaultBuild(cleric.GetDefaultBuild());
            bp.SetProgression(BlueprintReference<BlueprintProgression>.CreateTyped<BlueprintProgressionReference>(CommonerProgression()));

            var classes = BlueprintRoot.Instance.Progression.GetCharacterClasses().ToList();
            classes.Add(BlueprintReference<BlueprintCharacterClass>.CreateTyped<BlueprintCharacterClassReference>(bp));
            classes.Sort((x, y) =>
            {
                if (x.Get().PrestigeClass != y.Get().PrestigeClass) return x.Get().PrestigeClass ? 1 : -1;
                return x.Get().Name.CompareTo(y.Get().Name);
            });
            BlueprintRoot.Instance.Progression.SetCharacterClasses(classes.ToArray());
        }

        public static BlueprintProgression CommonerProgression()
        {
            var bp = Utils.CreateBlueprint<BlueprintProgression>("CommonerProgression");
            typeof(BlueprintUnitFact).GetField("m_DisplayName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("Commoner.Name", "Commoner"));
            var firstLevel = new LevelEntry();
            firstLevel.Level = 1;
            
            //Resources.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd")
            firstLevel.Features.Add(CommonerProficiencies());
            bp.AddLevelEntry(firstLevel);
            return bp;
        }

        public static BlueprintParametrizedFeature CommonerProficiencies()
        {
            var bp = Utils.CreateBlueprint<BlueprintParametrizedFeature>("CommonerProficienciesFeature");
            typeof(BlueprintUnitFact).GetField("m_DisplayName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerWeaponProficiency.Name", "Commoner Weapon Proficiency"));
            typeof(BlueprintUnitFact).GetField("m_Description", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerWeaponProficiency.Description", "The commoner is proficient with one simple weapon. He is not proficient with any other weapons, nor is he proficient with any type of armor or shield."));
            typeof(BlueprintUnitFact).GetField("m_DescriptionShort", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("CommonerWeaponProficiency.DescriptionShort", "The commoner is proficient with one simple weapon. He is not proficient with any other weapons, nor is he proficient with any type of armor or shield."));
            bp.ParameterType = FeatureParameterType.WeaponCategory;
            bp.WeaponSubCategory = Kingmaker.Enums.WeaponSubCategory.Simple;
            bp.IsClassFeature = true;
            var addFacts = new AddFacts();
            addFacts.OwnerBlueprint = bp;
            typeof(AddFacts).GetField("m_Facts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(addFacts, new BlueprintUnitFactReference[]{
                BlueprintReference<BlueprintUnitFact>.CreateTyped<BlueprintUnitFactReference>(SimpleWeaponChoice())
            });
            var comps = bp.ComponentsArray.ToList();
            comps.Add(addFacts);
            bp.ComponentsArray = comps.ToArray();
            return bp;
        }

        public static BlueprintParametrizedFeature SimpleWeaponChoice()
        {
            var bp = Utils.CreateBlueprint<BlueprintParametrizedFeature>("SimpleWeaponProficiencySelectionFeature");
            //bp.Group = FeatureGroup.CombatFeat;
            typeof(BlueprintUnitFact).GetField("m_DisplayName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("SimpleWeaponProficiencySelection.Name", "Single Simple Weapon Proficiency"));
            typeof(BlueprintUnitFact).GetField("m_Description", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("SimpleWeaponProficiencySelection.Description", "Proficiency in just one simple weapon."));
            typeof(BlueprintUnitFact).GetField("m_DescriptionShort", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, Utils.CreateString("SimpleWeaponProficiencySelection.DescriptionShort", "Proficiency in just one simple weapon."));
            bp.ParameterType = FeatureParameterType.WeaponCategory;
            bp.WeaponSubCategory = Kingmaker.Enums.WeaponSubCategory.Simple;
            bp.IsClassFeature = true;
            var comps = bp.ComponentsArray.ToList();
            var addProfs = new AddProficienciesParameterized();
            addProfs.OwnerBlueprint = bp;
            comps.Add(addProfs);
            bp.ComponentsArray = comps.ToArray();
            
            //foreach(BlueprintItemWeapon weap in BlueprintRoot.Instance.Progression.)
            //bp.AddFeature(SimpleWeaponProficiency("fuck"));
            return bp;
        }

        /*public static BlueprintFeature SimpleWeaponProficiency(BlueprintWeaponType item)
        {
            var bp = Utils.CreateBlueprint<BlueprintFeature>("SimpleWeaponProficiency" + item.name + "Feature");
            (typeof(BlueprintFeature)).GetField("m_Icon", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bp, item.Icon);
            bp.name = item.name;
            var comps = bp.ComponentsArray.ToList();
            //comps.Add(new AddProficiency());
            bp.ComponentsArray = comps.ToArray();
            //bp.Description = item.Description;
            //bp.DescriptionShort = item.Description;
            return bp;
        }*/

        [ComponentName("Add proficiencies parameterized")]
        [AllowedOn(typeof(BlueprintUnit), false)]
        [AllowedOn(typeof(BlueprintUnitFact), false)]
        [AllowMultipleComponents]
        public class AddProficienciesParameterized : UnitFactComponentDelegate
        {   
            protected override void OnTurnOn()
            {
                if(Param.WeaponCategory is WeaponCategory wc){
                    base.Owner.Proficiencies.Add(wc);
                }
            }
            
            protected override void OnTurnOff()
            {
                if (Param.WeaponCategory is WeaponCategory wc)
                {
                    base.Owner.Proficiencies.Remove(wc);
                }
            }

            protected override void OnFactAttached()
            {
                if (Param.WeaponCategory is WeaponCategory wc)
                {
                    base.Owner.Proficiencies.Add(wc);
                }
            }
        }
    }
}
