using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Root;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WrathCommonerClass.Classes
{
    public static class ExposeClass
    {
        public static void SetBaseAttackBonus(this BlueprintCharacterClass c, BlueprintStatProgressionReference bab){
            typeof(BlueprintCharacterClass).GetField("m_BaseAttackBonus", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, bab);
        }
        public static void SetFortitudeSave(this BlueprintCharacterClass c, BlueprintStatProgressionReference fort)
        {
            typeof(BlueprintCharacterClass).GetField("m_FortitudeSave", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, fort);
        }
        public static void SetReflexSave(this BlueprintCharacterClass c, BlueprintStatProgressionReference reflex)
        {
            typeof(BlueprintCharacterClass).GetField("m_WillSave", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, reflex);
        }
        public static void SetWillSave(this BlueprintCharacterClass c, BlueprintStatProgressionReference will)
        {
            typeof(BlueprintCharacterClass).GetField("m_ReflexSave", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, will);
        }
        public static void SetHitDice(this BlueprintCharacterClass c, DiceType hd)
        {
            typeof(BlueprintCharacterClass).GetField("HitDie", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, hd);
        }
        public static void SetDifficulty(this BlueprintCharacterClass c, int difficulty)
        {
            typeof(BlueprintCharacterClass).GetField("m_Difficulty", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, difficulty);
        }

        public static BlueprintUnitFactReference GetDefaultBuild(this BlueprintCharacterClass c)
        {
            return (BlueprintUnitFactReference)typeof(BlueprintCharacterClass).GetField("m_DefaultBuild", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
        }
        public static void SetDefaultBuild(this BlueprintCharacterClass c, BlueprintUnitFactReference defaultBuild)
        {
            typeof(BlueprintCharacterClass).GetField("m_DefaultBuild", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, defaultBuild);
        }

        public static void AddLevelEntry(this BlueprintProgression bp, LevelEntry le)
        {
            var entries = bp.LevelEntries.ToList();
            entries.Add(le);
            bp.LevelEntries = entries.ToArray();
        }

        public static void AddFeature(this BlueprintFeatureSelection fs, BlueprintFeature feat)
        {
            var features = ((BlueprintFeatureReference[])typeof(BlueprintFeatureSelection).GetField("m_Features", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fs)).ToList();
            features.Add(BlueprintReference<BlueprintFeature>.CreateTyped<BlueprintFeatureReference>(feat));
            typeof(BlueprintFeatureSelection).GetField("m_Features", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(fs, features.ToArray());

        }

        public static BlueprintProgressionReference GetProgression(this BlueprintCharacterClass c)
        {
            return (BlueprintProgressionReference)typeof(BlueprintCharacterClass).GetField("m_Progression", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
        }
        public static void SetProgression(this BlueprintCharacterClass c, BlueprintProgressionReference m_Progression)
        {
            typeof(BlueprintCharacterClass).GetField("m_Progression", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, m_Progression);
        }

        public static void SetCharacterClasses(this ProgressionRoot progressionRoot, BlueprintCharacterClassReference[] classes)
        {
            typeof(ProgressionRoot).GetField("m_CharacterClasses", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(progressionRoot, classes);
        }
        public static BlueprintCharacterClassReference[] GetCharacterClasses(this ProgressionRoot progressionRoot)
        {
            return (BlueprintCharacterClassReference[])typeof(ProgressionRoot).GetField("m_CharacterClasses", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(progressionRoot);
        }
    }
}
