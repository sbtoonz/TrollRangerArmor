using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using System;

namespace ArmorMod
{
	[HarmonyPatch]
	public class TRALocal
	{
		private static Localization lcl;
		public static Dictionary<string, string> t;
		private static Dictionary<string, string> english = new Dictionary<string, string>() {

			{"item_rangervest", "Troll Ranger Vest"},
			{"item_rangervest_desc", "Fine vest made from iron and troll hide"},
			{"item_rangerleggings", "Troll Ranger Leggings"},
			{"item_rangerleggings_desc", "Fine leggings made from iron and troll hide"}

		};
		private static Dictionary<string, string> russian = new Dictionary<string, string>() {

			{"item_rangervest", "Troll Ranger Vest"},
			{"item_rangervest_desc", "Fine vest made from iron and troll hide"},
			{"item_rangerleggings", "Troll Ranger Leggings"},
			{"item_rangerleggings_desc", "Fine leggings made from iron and troll hide"}

		};
		private static Dictionary<string, string> german = new Dictionary<string, string>() {

			{"item_rangervest", "Troll Ranger Vest"},
			{"item_rangervest_desc", "Fine vest made from iron and troll hide"},
			{"item_rangerleggings", "Troll Ranger Leggings"},
			{"item_rangerleggings_desc", "Fine leggings made from iron and troll hide"}

		};
		private static Dictionary<string, string> turkish = new Dictionary<string, string>() {

			{"item_rangervest", "Troll Ranger Vest"},
			{"item_rangervest_desc", "Fine vest made from iron and troll hide"},
			{"item_rangerleggings", "Troll Ranger Leggings"},
			{"item_rangerleggings_desc", "Fine leggings made from iron and troll hide"}

		};

		public static void init(string lang, Localization l)
		{
			lcl = l;
			if (lang == "Russian")
			{
				t = russian;
			}
			else if (lang == "English")
			{
				t = english;
			}
			else if (lang == "Turkish")
			{
				t = turkish;
			}
			else
			{
				t = german;
			}
		}
		public static void AddWord(object[] element)
		{
			MethodInfo meth = AccessTools.Method(typeof(Localization), "AddWord", null, null);
			meth.Invoke(lcl, element);
		}
		public static void UpdateDictinary()
		{
			string missing = "Missing Words:";
			foreach (var el in english)
			{
				if (t.ContainsKey(el.Key))
				{
					AddWord(new object[] { el.Key, t[el.Key] });
					continue;
				}
				AddWord(new object[] { el.Key, el.Value });
				missing += el.Key;
			}
		}

		[HarmonyPatch(typeof(Localization), "SetupLanguage")]
		public static class TRALocalizationPatch
		{
			public static void Postfix(Localization __instance, string language)
			{
				init(language, __instance);
				UpdateDictinary();
			}
		}
	}
}
