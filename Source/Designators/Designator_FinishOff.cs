﻿using HugsLib.Utils;
using RimWorld;
using Verse;

namespace AllowTool {
	public class Designator_FinishOff : Designator_SelectableThings {
		private const int MeleeSkillLevelRequired = 6;

		public static bool IsValidDesignationTarget(Thing t) {
			var p = t as Pawn;
			return p?.def != null && !p.Dead && p.Downed;
		}

		public static AcceptanceReport PawnMeetsSkillRequirement(Pawn pawn, Pawn targetPawn) {
			var skillPass = pawn?.skills != null && (!AllowToolController.Instance.FinishOffSkillRequirement || pawn.skills.GetSkill(SkillDefOf.Melee).Level >= MeleeSkillLevelRequired);
			var animalTarget = targetPawn?.RaceProps != null && targetPawn.RaceProps.Animal;
			return skillPass || animalTarget ? true : new AcceptanceReport("Finish_off_pawnSkillRequired".Translate(MeleeSkillLevelRequired));
		}

		public static AcceptanceReport FriendlyPawnIsValidTarget(Thing t) {
			var result = !AllowToolUtility.PawnIsFriendly(t) || HugsLibUtility.ShiftIsHeld;
			return result ? true : new AcceptanceReport("Finish_off_floatMenu_reason_friendly".Translate());
		}

		protected override DesignationDef Designation {
			get { return AllowToolDefOf.FinishOffDesignation; }
		}

		public override string Desc {
			get {
				if (AllowToolController.Instance.FinishOffSkillRequirement) {
					return $"{base.Desc}\n\n{"Finish_off_skillRequired".Translate(MeleeSkillLevelRequired)}";
				}
				return base.Desc;
			}
		}

		public Designator_FinishOff(ThingDesignatorDef def) : base(def) {
		}

		public override AcceptanceReport CanDesignateThing(Thing t) {
			if (!IsValidDesignationTarget(t) || t.HasDesignation(AllowToolDefOf.FinishOffDesignation)) return false;
			return FriendlyPawnIsValidTarget(t);
		}

		public override void DesignateThing(Thing t) {
			if (!CanDesignateThing(t).Accepted) return;
			t.ToggleDesignation(AllowToolDefOf.FinishOffDesignation, true);
		}
	}
}