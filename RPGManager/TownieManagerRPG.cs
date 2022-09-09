using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Academics;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Counters;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Objects.Fishing;
using Sims3.Gameplay.Objects.Gardening;
using Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.Hud;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sims3.Gameplay.Lyralei.RPGManager
{
    public class TownieManagerRPG
    {
        public static Dictionary<ulong, List<SkillNames>> assignedSKills = new Dictionary<ulong, List<SkillNames>>();
        public static Dictionary<SkillNames, int> AssignedActiveSkills = new Dictionary<SkillNames, int>();

        public static Dictionary<ulong, AcademicDegreeNames> assignedDegree = new Dictionary<ulong, AcademicDegreeNames>();
        public static List<AcademicDegreeNames> AssignedActiveDegree = new List<AcademicDegreeNames>();

        public static Dictionary<ulong, OccupationNames> assignedJob = new Dictionary<ulong, OccupationNames>();


        public static void TownieHobbyFixer(Sim sim)
        {
            try
            {
                ulong simId = sim.SimDescription.SimDescriptionId;
                // Premades we want to edit:
                switch (simId)
                {
                    case 257u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Cooking, SkillNames.Gardening };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 260u:
                        {
                            SkillNames[] Skills = new SkillNames[] { SkillNames.Foosball, SkillNames.BallFighting, SkillNames.Snowboarding, SkillNames.Trampoline, SkillNames.Waterskiing, SkillNames.Windsurfing, SkillNames.Skating };
                            SKillingUpHelperFunction(simId, Skills, false);

                            SimDescription simDescription = SimDescription.Find(260u);
                            if (simDescription == null) { break; }

                            if (simDescription.SkillManager.GetElement(SkillNames.Athletic).SkillLevel == 1)
                            {
                                simDescription.SkillManager.AddSkillPoints(SkillNames.Athletic, 3f);
                            }
                            break;
                        }
                    case 259u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.ChildAthletic, SkillNames.ChildPiano, SkillNames.ChildGardening, SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 218u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.SocialNetworking, SkillNames.Bartending };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 150u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 163u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 169u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Athletic, SkillNames.BallFighting };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 171u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.ChildAthletic };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 231u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.SocialNetworking };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 234u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 377u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 1226u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Athletic };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 154u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Painting, SkillNames.Photography, SkillNames.Sculpting };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 2314u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Painting, SkillNames.Cooking, SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 138u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.ChildAthletic, SkillNames.Logic, SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 193u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Athletic, SkillNames.Logic };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 197u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Logic };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 6340u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Cooking };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 329u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Athletic, SkillNames.Logic, SkillNames.Painting, SkillNames.Piano, SkillNames.Mooch };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 8318u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 8319u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Charisma };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 244u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Cooking, SkillNames.Piano, SkillNames.Mooch, SkillNames.MartialArts };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 245u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.Cooking, SkillNames.Piano, SkillNames.Charisma, SkillNames.Gardening };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                    case 246u:
                        {
                            SkillNames[] skills = new SkillNames[] { SkillNames.ChildPiano, SkillNames.Painting, SkillNames.ChildGardening, SkillNames.Chess, SkillNames.Collecting };
                            SKillingUpHelperFunction(simId, skills, false);
                            break;
                        }
                }
                AssignSkillsOnDynamicLevel(simId, false);
            }
            catch(Exception ex)
            {
                RPGManagerUtil.printException(ex);
                RPGManagerUtil.WriteErrorXMLFile("Error Townie Manager", ex, null);
            }
        }

        public static void SKillingUpHelperFunction(ulong simId, SkillNames[] skills, bool SimplyWantInfo)
        {
            SimDescription simDescription = SimDescription.Find(simId);
            if (simDescription == null) { return; }

            if (!assignedSKills.ContainsKey(simDescription.SimDescriptionId))
            {
                assignedSKills.Add(simDescription.SimDescriptionId, new List<SkillNames>());
            }

            foreach (SkillNames skill in skills)
            {
                if (!AssignedActiveSkills.ContainsKey(skill) && !AssignedActiveSkills.ContainsKey(QuicklyCheckIfChildSkillIsNecessary(skill)))
                {
                    if (simDescription.ChildOrBelow && QuicklyCheckIfChildSkillIsNecessary(skill) != SkillNames.None)
                    {
                        AssignedActiveSkills.Add(QuicklyCheckIfChildSkillIsNecessary(skill), 100);
                    }
                    else
                    {
                        AssignedActiveSkills.Add(skill, 100);
                    }
                }
                if (!simDescription.SkillManager.HasElement(skill) && !SimplyWantInfo)
                {
                    simDescription.SkillManager.AddElement(skill);
                    simDescription.SkillManager.GetElement(skill).ForceSkillLevelUp(RandomUtil.GetInt(1, 5));
                    if (!assignedSKills[simDescription.SimDescriptionId].Contains(skill))
                    {
                        assignedSKills[simDescription.SimDescriptionId].Add(skill);
                    }
                }
            }
        }

        public static void SKillingUpHelperFunction(SimDescription simDescription, SkillNames[] skills, bool SimplyWantInfo)
        {
            if (simDescription == null) { return; }

            if (!assignedSKills.ContainsKey(simDescription.SimDescriptionId))
            {
                assignedSKills.Add(simDescription.SimDescriptionId, new List<SkillNames>());
            }

            foreach (SkillNames skill in skills)
            {
                if (!AssignedActiveSkills.ContainsKey(skill) && !AssignedActiveSkills.ContainsKey(QuicklyCheckIfChildSkillIsNecessary(skill)))
                {
                    if (simDescription.ChildOrBelow && QuicklyCheckIfChildSkillIsNecessary(skill) != SkillNames.None)
                    {
                        AssignedActiveSkills.Add(QuicklyCheckIfChildSkillIsNecessary(skill), 100);
                    }
                    else
                    {
                        AssignedActiveSkills.Add(skill, 100);
                    }
                }

                if (!simDescription.SkillManager.HasElement(skill) && !SimplyWantInfo)
                {
                    if (simDescription.ChildOrBelow)
                    {
                        SkillNames NewSkill = QuicklyCheckIfChildSkillIsNecessary(skill);

                        if (NewSkill == SkillNames.None) { return; }
                        simDescription.SkillManager.AddElement(NewSkill);
                        simDescription.SkillManager.GetElement(NewSkill).ForceSkillLevelUp(RandomUtil.GetInt(1, 5));
                        if(!assignedSKills[simDescription.SimDescriptionId].Contains(NewSkill))
                        {
                            assignedSKills[simDescription.SimDescriptionId].Add(NewSkill);
                        }
                        return;
                    }

                    simDescription.SkillManager.AddElement(skill);
                    simDescription.SkillManager.GetElement(skill).ForceSkillLevelUp(RandomUtil.GetInt(1, 5));
                    if (!assignedSKills[simDescription.SimDescriptionId].Contains(skill))
                    {
                        assignedSKills[simDescription.SimDescriptionId].Add(skill);
                    }

                    //simDescription.SkillManager.AddSkillPoints(skill, 3f);
                }
            }
        }

        public static void SKillingUpHelperFunction(SimDescription simDescription, SkillNames skill, bool SimplyWantInfo)
        {
            if (simDescription == null) { return; }

            if(!assignedSKills.ContainsKey(simDescription.SimDescriptionId))
            {
                assignedSKills.Add(simDescription.SimDescriptionId, new List<SkillNames>());
            }

            if(!AssignedActiveSkills.ContainsKey(skill) && !AssignedActiveSkills.ContainsKey(QuicklyCheckIfChildSkillIsNecessary(skill)))
            {
                if (simDescription.ChildOrBelow && QuicklyCheckIfChildSkillIsNecessary(skill) != SkillNames.None)
                {
                    AssignedActiveSkills.Add(QuicklyCheckIfChildSkillIsNecessary(skill), 100);
                }
                else
                {
                    AssignedActiveSkills.Add(skill, 100);
                }
            }

            if (!simDescription.SkillManager.HasElement(skill) && !SimplyWantInfo)
            {
                if(simDescription.ChildOrBelow)
                {
                    SkillNames NewSkill = QuicklyCheckIfChildSkillIsNecessary(skill);

                    if(NewSkill == SkillNames.None) { return; }
                    simDescription.SkillManager.AddElement(NewSkill);
                    simDescription.SkillManager.GetElement(NewSkill).ForceSkillLevelUp(RandomUtil.GetInt(1, 5));
                    if (!assignedSKills[simDescription.SimDescriptionId].Contains(NewSkill))
                    {
                        assignedSKills[simDescription.SimDescriptionId].Add(NewSkill);
                    }
                    return;
                }

                simDescription.SkillManager.AddElement(skill);
                simDescription.SkillManager.GetElement(skill).ForceSkillLevelUp(RandomUtil.GetInt(1, 5));

                if (!assignedSKills[simDescription.SimDescriptionId].Contains(skill))
                {
                    assignedSKills[simDescription.SimDescriptionId].Add(skill);
                }

                //simDescription.SkillManager.AddSkillPoints(skill, 3f);
            }

        }

        private static SkillNames QuicklyCheckIfChildSkillIsNecessary(SkillNames skill)
        {

            switch(skill)
            {
                case SkillNames.Athletic:
                    {
                        return SkillNames.ChildAthletic;
                    }
                case SkillNames.BassGuitar:
                    {
                        return SkillNames.ChildBassGuitar;
                    }
                case SkillNames.Cooking:
                    {
                        return SkillNames.ChildCooking;
                    }
                case SkillNames.Drums:
                    {
                        return SkillNames.ChildDrums;
                    }
                case SkillNames.Gardening:
                    {
                        return SkillNames.ChildGardening;
                    }
                case SkillNames.Guitar:
                    {
                        return SkillNames.ChildGuitar;
                    }
                case SkillNames.Piano:
                    {
                        return SkillNames.ChildPiano;
                    }
                default:
                    return SkillNames.None;
            }
        }

        private static void SetProperPercentageForActiveSim(int percentage, SkillNames skill)
        {
            if(AssignedActiveSkills.ContainsKey(skill))
            {
                AssignedActiveSkills[skill] = percentage;
            }
        }

        private static void SetProperPercentageForActiveSim(int percentage, SkillNames[] skills)
        {
            foreach(SkillNames skill in skills)
            {
                if (AssignedActiveSkills.ContainsKey(skill))
                {
                    AssignedActiveSkills[skill] = percentage;
                }
            }
        }


        /// <summary>
        /// Loops through the house items AND traits to see what the chances are that this sim will get a skill assigned that EA may have forgot to do.
        /// </summary>
        /// <param name="simId">Sim Description Id.</param>
        public static void AssignSkillsOnDynamicLevel(ulong simId, bool SimplyWantInfo)
        {
            SimDescription simDescription = SimDescription.Find(simId);

            if (simDescription == null) { return; }
            if (simDescription.TraitManager == null) { return; }

            int Points = 0;
            GiveSimsWhohaveAppropiateTraitSkills(simDescription, SimplyWantInfo);

            Lot homeLot = simDescription.LotHome;
            if (homeLot == null) { return; }

            Plant[] PlantsCurrentLot = homeLot.GetObjects<Plant>();
            FishingSpot[] FishingspotHome = homeLot.GetObjects<FishingSpot>();
            Easel[] EaselHome = homeLot.GetObjects<Easel>();
            Stove[] StoveHome = homeLot.GetObjects<Stove>();
            ChessTable[] ChessTableHome = homeLot.GetObjects<ChessTable>();
            Sims3.Gameplay.Objects.HobbiesSkills.Piano[] PianoHome = homeLot.GetObjects<Sims3.Gameplay.Objects.HobbiesSkills.Piano>();
            Sims3.Gameplay.Objects.HobbiesSkills.Guitar[] GuitarHome = homeLot.GetObjects<Sims3.Gameplay.Objects.HobbiesSkills.Guitar>();
            Computer[] ComputerHome = homeLot.GetObjects<Computer>();
            BarAdvanced[] BarHome = homeLot.GetObjects<BarAdvanced>();
            Drums[] DrumsHome = homeLot.GetObjects<Drums>();
            BassGuitar[] BassHome = homeLot.GetObjects<BassGuitar>();
            ScienceResearchStation[] ResearchHome = homeLot.GetObjects<ScienceResearchStation>();

            if (PlantsCurrentLot != null && PlantsCurrentLot.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.HatesOutdoors))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (simDescription.TraitManager.HasElement(TraitNames.LovesTheOutdoors))
                {
                    Points += 40;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Gardening, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Gardening);
                }
                Points = 0;
            }

            if (FishingspotHome != null && FishingspotHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.HatesOutdoors))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (simDescription.TraitManager.HasElement(TraitNames.LovesTheOutdoors))
                {
                    Points += 40;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Fishing, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Fishing);
                }
                Points = 0;
            }

            if (EaselHome != null && EaselHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.CantStandArt))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Painting, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Painting);
                }
                Points = 0;
            }

            if (StoveHome != null && StoveHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.CouchPotato))
                {
                    Points = 0;
                }
                else if(simDescription.Bodyshape == SimIFace.CAS.BodyshapeType.Fat)
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Cooking, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Cooking);
                }
                Points = 0;
            }

            if (ChessTableHome != null && ChessTableHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.AbsentMinded) || simDescription.TraitManager.HasElement(TraitNames.Insane))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Chess, SkillNames.Logic }, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, new SkillNames[] { SkillNames.Chess, SkillNames.Logic });
                }
                Points = 0;
            }

            if (PianoHome != null && PianoHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.CouchPotato) || simDescription.TraitManager.HasElement(TraitNames.CantStandArt))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Piano, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Piano);
                }
                Points = 0;
            }

            if (GuitarHome != null && GuitarHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.CouchPotato) || simDescription.TraitManager.HasElement(TraitNames.CantStandArt))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Guitar, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Guitar);
                }
                Points = 0;
            }

            if (ComputerHome != null && ComputerHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.AbsentMinded) || simDescription.TraitManager.HasElement(TraitNames.AntiTV))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (simDescription.TraitManager.HasElement(TraitNames.ComputerWhiz))
                {
                    Points += 40;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Logic, SkillNames.Writing, SkillNames.VideoGame }, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, new SkillNames[] { SkillNames.Logic, SkillNames.Writing, SkillNames.VideoGame });
                }
                Points = 0;
            }


            if (BarHome != null && BarHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.AbsentMinded))
                {
                    Points = 0;
                }

                if (simDescription.TraitManager.HasElement(TraitNames.PartyAnimal))
                {
                    Points += 40;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Bartending, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Bartending);
                }
                Points = 0;
            }

            if (DrumsHome != null && DrumsHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.CantStandArt))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Drums, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.Drums);
                }
                Points = 0;
            }
            if (BassHome != null && BassHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.CantStandArt))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.BassGuitar, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, SkillNames.BassGuitar);
                }
                Points = 0;
            }
            if (ResearchHome != null && ResearchHome.Length > 0)
            {
                Points += 10;
                if (simDescription.TraitManager.HasElement(TraitNames.AbsentMinded))
                {
                    Points = 0;
                }
                else
                {
                    Points += 30;
                }

                if (simDescription.TraitManager.HasElement(TraitNames.Genius) || simDescription.TraitManager.HasElement(TraitNames.Eccentric))
                {
                    Points += 30;
                }

                if (RandomUtil.RandomChance(Points))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Science, SkillNames.Logic }, SimplyWantInfo);
                    SetProperPercentageForActiveSim(Points, new SkillNames[] { SkillNames.Science, SkillNames.Logic });
                }
            }

            // Tackle jobs.
            HandleJobsSKillsForTownies(simDescription, SimplyWantInfo);
            HandleLifetimeWishesForTownies(simDescription, SimplyWantInfo);
        }

        private static void HandleLifetimeWishesForTownies(SimDescription simDescription, bool SimplyWantInfo)
        {
            if (simDescription != null && CASLogic.Instance != null)
            {
                IInitialMajorWish wish = CASLogic.Instance.GetLifetimeWish(simDescription.LifetimeWish);
                if(wish == null) { return; }

                switch(wish.PrimitiveId)
                {
                    // Perfect aquarium
                    case 3074810660:
                        {
                            SKillingUpHelperFunction(simDescription, SkillNames.Fishing, SimplyWantInfo);
                            break;
                        }
                    // Perfect body/mind
                    case 1457442002:
                        {
                            SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Athletic, SkillNames.Logic }, SimplyWantInfo);
                            break;
                        }
                    // Super Popular
                    case 3034414675:
                        {
                            SKillingUpHelperFunction(simDescription, SkillNames.Charisma, SimplyWantInfo);
                            break;
                        }
                    // Master of all Arts
                    case 1432152990:
                        {
                            SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Painting, SkillNames.Guitar }, SimplyWantInfo);
                            break;
                        }
                    // Perfect garden
                    case 3631643856:
                        {
                            SKillingUpHelperFunction(simDescription, SkillNames.Gardening, SimplyWantInfo);
                            break;
                        }
                    // Renaissance Sim
                    case 2281530272:
                        {
                            SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Painting, SkillNames.Guitar, SkillNames.Logic }, SimplyWantInfo);
                            break;
                        }
                    // Illustrious Author
                    case 3588921655:
                        {
                            SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Writing, SkillNames.Painting }, SimplyWantInfo);
                            break;
                        }
                    // The Tinkerer
                    case 797244627:
                        {
                            SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Handiness, SkillNames.Logic }, SimplyWantInfo);
                            break;
                        }
                    // The Culinary Librarian
                    case 2393916713:
                        {
                            SKillingUpHelperFunction(simDescription, SkillNames.Cooking , SimplyWantInfo);
                            break;
                        }
                    // Chess Legend
                    case 2287246491:
                        {
                            SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Chess, SkillNames.Logic }, SimplyWantInfo);
                            break;
                        }
                    // Professional Author
                    case 39298250:
                        {
                            SKillingUpHelperFunction(simDescription, SkillNames.Writing, SimplyWantInfo);
                            break;
                        }
                }
                //SimpleMessageDialog.Show("", "NAME: " + wish.GetMajorWishName(simDescription) + " , ID: " + wish.PrimitiveId.ToString());

            }
        }

        private static void HandleJobsSKillsForTownies(SimDescription simDescription, bool SimplyWantInfo)
        {
            if (simDescription != null)
            {
                if (simDescription.CareerManager == null) { return; }
                if (simDescription.CareerManager.Occupation == null) { return; }
                if (simDescription.CareerManager.Occupation.PerfFactors == null) { return; }

                List<PerfFactor> metrics = simDescription.CareerManager.Occupation.PerfFactors;

                if(metrics == null) { return; }

                foreach(PerfFactor factor in metrics)
                {
                    foreach(ISkillJournalEntry entry in simDescription.SkillManager.SkillJournalEntries)
                    {
                        if(entry.SkillGuid == factor.SkillGuid)
                        {
                            SKillingUpHelperFunction(simDescription, (SkillNames)entry.SkillGuid, SimplyWantInfo);
                        }
                    }
                }
            }
        }

        public static void TownieDegreeFixer(Sim sim, bool SimplyWantsInfo)
        {
            if(sim == null) { return; }
            if(sim.SimDescription == null) { return; }
            SimDescription simDescription = sim.SimDescription;

            if (simDescription.TeenOrBelow) { return; }

            if (simDescription.CareerManager == null) { return; }
            if (simDescription.CareerManager.Occupation == null) { return; }
            if (simDescription.OccupationAsAcademicCareer != null) { return; }

            OccupationNames names = simDescription.CareerManager.Occupation.mCareerGuid;
            
            switch (names)
            {
                case OccupationNames.Undefined:
                case OccupationNames.Any:
                    return;
                case OccupationNames.Medical:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Science, SimplyWantsInfo);
                    }
                    break;
                case OccupationNames.Political:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Communications, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Criminal:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Business, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Culinary:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.FineArts, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.LawEnforcement:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Technology, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Music:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.FineArts, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Military:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Technology, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Science:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Science, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Business:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Business, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Journalism:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Communications, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.ProfessionalSports:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.PhysEd, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.PTDaySpaSpecialist:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.PhysEd, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.PTMausoleum:
                    if (RandomUtil.RandomChance(30f) && simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Science, SimplyWantsInfo);
                    }
                    break;
                case OccupationNames.Firefighter:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        if(RandomUtil.CoinFlip())
                        {
                            SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Technology, SimplyWantsInfo);
                        }
                        else
                        {
                            SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.PhysEd, SimplyWantsInfo);
                        }
                    }
                    break;
                case OccupationNames.PrivateEye:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Technology, SimplyWantsInfo);

                    }
                    break;

                case OccupationNames.Lifeguard:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.PhysEd, SimplyWantsInfo);

                    }
                    break;

                case OccupationNames.BotBuilder:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Technology, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Film:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.FineArts, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.FortuneTeller:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Communications, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.GameDeveloper:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Technology, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.SportsAgent:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.PhysEd, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.ArtAppraiser:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.FineArts, SimplyWantsInfo);

                    }
                    break;
                case OccupationNames.Astronomer:
                    if (simDescription.CareerManager.DegreeManager != null)
                    {
                        SetAcademicDegreeAndComplete(simDescription, AcademicDegreeNames.Science, SimplyWantsInfo);

                    }
                    break;
            }
            
        }

        public static void SetAcademicDegreeAndComplete(SimDescription simDescription, AcademicDegreeNames name, bool SimplyWantsInfo)
        {
            AcademicDegreeManager degreeMngr = simDescription.CareerManager.DegreeManager;
            if(degreeMngr == null) { return; }
            

            if(SimplyWantsInfo && !AssignedActiveDegree.Contains(name))
            {
                AssignedActiveDegree.Add(name);
                return; 
            }

            if (name != AcademicDegreeNames.Undefined)
            {
                AcademicDegree academicDegree = degreeMngr.AddNewDegree(name, 0);
                if(academicDegree == null) { return; }
                academicDegree.EarnedNumberOfCreditsTowardDegree = academicDegree.RequiredNumberOfCredit;
                academicDegree.SetGpa(100f);
                degreeMngr.SetLastStudiedDegree(name);
                degreeMngr.SetEnrollmentData(name, 0);
                if(academicDegree.IsDegreeCompleted)
                {
                    if(!assignedDegree.ContainsKey(simDescription.SimDescriptionId))
                    {
                        assignedDegree.Add(simDescription.SimDescriptionId, name);
                    }
                    else if(assignedDegree.ContainsKey(simDescription.SimDescriptionId) && assignedDegree[simDescription.SimDescriptionId] != name)
                    {
                        assignedDegree[simDescription.SimDescriptionId] = name;
                    }
                    DoCorrectSkillForDegree(simDescription, name);
                }
            }
        }

        private static void DoCorrectSkillForDegree(SimDescription simDescription, AcademicDegreeNames name)
        {
            SkillNames[] ScienceSkills = new SkillNames[] { SkillNames.Science, SkillNames.Gardening, SkillNames.Fishing, SkillNames.Logic };
            SkillNames[] BusinessSkills = new SkillNames[] { SkillNames.Charisma, SkillNames.SocialNetworking };
            SkillNames[] CommunicationsSkills = new SkillNames[] { SkillNames.Writing, SkillNames.Charisma, SkillNames.Photography };
            SkillNames[] FineArtsSkills = new SkillNames[] { SkillNames.StreetArt, SkillNames.Painting, SkillNames.Cooking, SkillNames.Guitar, SkillNames.Piano, SkillNames.BassGuitar, SkillNames.Sculpting ,SkillNames.Drums };
            SkillNames[] PhysEdSkills = new SkillNames[] { SkillNames.Athletic, SkillNames.Dancing, SkillNames.Snowboarding, SkillNames.Waterskiing, SkillNames.Windsurfing };
            SkillNames[] TechnologySkills = new SkillNames[] { SkillNames.Handiness, SkillNames.Logic, SkillNames.Inventing };

            switch (name)
            {
                case AcademicDegreeNames.Undefined:
                    break;
                case AcademicDegreeNames.Business:
                    SKillingUpHelperFunction(simDescription, BusinessSkills, false);

                    break;
                case AcademicDegreeNames.Technology:
                    SKillingUpHelperFunction(simDescription, TechnologySkills, false);

                    break;
                case AcademicDegreeNames.Science:
                    SKillingUpHelperFunction(simDescription, ScienceSkills, false);

                    break;
                case AcademicDegreeNames.FineArts:
                    SKillingUpHelperFunction(simDescription, FineArtsSkills, false);

                    break;
                case AcademicDegreeNames.Communications:
                    SKillingUpHelperFunction(simDescription, CommunicationsSkills, false);

                    break;
                case AcademicDegreeNames.PhysEd:
                    SKillingUpHelperFunction(simDescription, PhysEdSkills, false);

                    break;
            }
        }


        private static void GiveSimsWhohaveAppropiateTraitSkills(SimDescription simDescription, bool SimplyWantInfo)
        {
            // First we handle the prominent sims that really need those skills.
            if (simDescription != null)
            {
                if (simDescription.TraitManager.HasElement(TraitNames.Angler))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Fishing }, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.NaturalCook) || simDescription.TraitManager.HasElement(TraitNames.BornToCook))
                {
                    if (simDescription.ChildOrBelow)
                    {
                        SKillingUpHelperFunction(simDescription, SkillNames.ChildCooking, SimplyWantInfo);
                    }
                    else
                    {
                        SKillingUpHelperFunction(simDescription, SkillNames.Cooking, SimplyWantInfo);
                    }
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Schmoozer) || simDescription.TraitManager.HasElement(TraitNames.SocialButterfly) || simDescription.TraitManager.HasElement(TraitNames.Charismatic) || simDescription.TraitManager.HasElement(TraitNames.Flirty))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Charisma, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.GreenThumb) || simDescription.TraitManager.HasElement(TraitNames.SuperGreenThumb))
                {
                    if (simDescription.ChildOrBelow)
                    {
                        SKillingUpHelperFunction(simDescription, SkillNames.ChildGardening, SimplyWantInfo);
                    }
                    else
                    {
                        SKillingUpHelperFunction(simDescription, SkillNames.Gardening, SimplyWantInfo);
                    }
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Athletic) || simDescription.TraitManager.HasElement(TraitNames.LovesToSwim))
                {
                    if (simDescription.ChildOrBelow)
                    {
                        SKillingUpHelperFunction(simDescription, SkillNames.ChildAthletic, SimplyWantInfo);
                    }
                    else
                    {
                        SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Athletic, SkillNames.Snowboarding, SkillNames.PingPong, SkillNames.Skating, SkillNames.Trampoline }, SimplyWantInfo);
                        if (simDescription.TraitManager.HasElement(TraitNames.LovesToSwim)) { SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Waterskiing, SkillNames.Windsurfing, SkillNames.ScubaDiving }, SimplyWantInfo); }
                    }
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Genius) || simDescription.TraitManager.HasElement(TraitNames.ComputerWhiz) || simDescription.TraitManager.HasElement(TraitNames.Perceptive))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Chess, SkillNames.Logic }, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Artistic))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Painting, SkillNames.Sculpting, SkillNames.Writing, SkillNames.Photography }, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.AvantGarde))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.StreetArt, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.SavvySculptor))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Sculpting, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Virtuoso))
                {
                    if (simDescription.ChildOrBelow)
                    {
                        SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.ChildPiano, SkillNames.ChildGuitar, SkillNames.ChildBassGuitar }, SimplyWantInfo);
                    }
                    else
                    {
                        SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Piano, SkillNames.Guitar, SkillNames.BassGuitar, SkillNames.Karaoke }, SimplyWantInfo);
                    }
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Eccentric))
                {
                    SKillingUpHelperFunction(simDescription, new SkillNames[] { SkillNames.Inventing, SkillNames.Handiness }, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Handy))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Handiness, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Equestrian))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Riding, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.GathererTrait))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Collecting, SimplyWantInfo);
                }
                else if (simDescription.TraitManager.HasElement(TraitNames.Mooch))
                {
                    SKillingUpHelperFunction(simDescription, SkillNames.Mooch, SimplyWantInfo);
                }
            }
        }

        public static string GetTownieNamesAndSkillsInStringFormat(bool isXML)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;

            sb.AppendLine("Lyralei's RPG Manager - Townie Manager [Skills] Log");
            sb.AppendLine();

            foreach (KeyValuePair<ulong, List<SkillNames>> kpv in TownieManagerRPG.assignedSKills)
            {
                if (kpv.Value.Count == 0) { continue; }
                SimDescription simDescription = SimDescription.Find(kpv.Key);
                if (simDescription == null) { continue; }

                if (!isXML) { count++; }

                sb.AppendLine(simDescription.FullName);

                

                sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/SKills:HasSkills"));

                foreach (SkillNames skill in kpv.Value)
                {
                    sb.AppendLine("			- " + skill);
                }
                sb.AppendLine();

                if (count == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
                {
                    sb.AppendLine("[CUT]");
                    sb.AppendLine("Lyralei's RPG Manager - Townie Manager [Skills] Log");
                    count = 0;
                }
            }
            return sb.ToString();
        }

        public static string GetTownieNamesDegreesInStringFormat(bool isXML)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;

            sb.AppendLine("Lyralei's RPG Manager - Townie Manager [Degree] Log");
            sb.AppendLine();

            foreach (KeyValuePair<ulong, AcademicDegreeNames> kpv in assignedDegree)
            {
                SimDescription simDescription = SimDescription.Find(kpv.Key);
                if (simDescription == null) { continue; }

                if (!isXML) { count++; }

                sb.AppendLine(simDescription.FullName);
                sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/Degrees:hasDegrees") + kpv.Value.ToString());

                if(assignedSKills.Count > 0 && assignedSKills.ContainsKey(kpv.Key) )
                {
                    sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/Degrees:hasDegreeSkills"));
                    foreach (SkillNames skill in assignedSKills[kpv.Key])
                    {
                        sb.AppendLine("             - " + skill);
                    }
                }
                sb.AppendLine();

                if (count == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
                {
                    sb.AppendLine("[CUT]");
                    sb.AppendLine("Lyralei's RPG Manager - Townie Manager [Degree] Log");
                    count = 0;
                }
            }
            return sb.ToString();
        }

        public static string GetTownieNamesJobsInStringFormat(bool isXML)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;

            sb.AppendLine("Lyralei's RPG Manager - Townie Manager [Jobs] Log");
            sb.AppendLine();

            foreach (KeyValuePair<ulong, OccupationNames> kpv in assignedJob)
            {
                SimDescription simDescription = SimDescription.Find(kpv.Key);
                if (simDescription == null) { continue; }

                if (!isXML) { count++; }

                sb.AppendLine(simDescription.FullName);

                if(simDescription.CareerManager.OccupationAsCareer != null)
                {
                    sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/Jobs:hasJob") + kpv.Value.ToString() + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/Jobs:levels") + simDescription.CareerManager.OccupationAsCareer.CurLevel.Level.ToString() + ")");
                }
                else
                {
                    sb.AppendLine(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/Jobs:hasJob") + kpv.Value.ToString());
                }
                sb.AppendLine();

                if (count == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
                {
                    sb.AppendLine("[CUT]");
                    sb.AppendLine("Lyralei's RPG Manager - Townie Manager [Jobs] Log");
                    count = 0;
                }
            }
            return sb.ToString();
        }

        public static void TownieJobFixer(Sim sim)
        {
            try
            {
                if (sim != null && sim.SimDescription != null)
                {
                    if (sim.CareerManager != null && sim.CareerManager.Occupation != null) { return; }

                    if (sim.SimDescription.ChildOrBelow) { return; }

                    List<OccupationNames> names = GetPotentialCareer(sim.SimDescription);
                    if (names.Count == 0) { return; }

                    RemoveAnyInvalidCareers(names);

                    int CareerLevel = 1;

                    OccupationNames ConfirmedCareer = OccupationNames.Undefined;
                    if (UserCanConfirmJob)
                    {
                        
                        ThreeButtonDialog.ButtonPressed showingData = ThreeButtonDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Job/OptionButtonCareer", new object[] { sim.FullName }), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Job/OptionButtonCareer:option1"), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Job/OptionButtonCareer:option2"), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Job/OptionButtonCareer:option3"));

                        switch (showingData)
                        {
                            case ThreeButtonDialog.ButtonPressed.FirstButton:
                                names.Add(OccupationNames.Undefined);
                                names.Add(OccupationNames.Any);

                                List<ObjectListPickerInfo> typeInfo = new List<ObjectListPickerInfo>();

                                foreach (OccupationNames type in names)
                                {
                                    if (type == OccupationNames.Any || type == OccupationNames.Undefined)
                                    {
                                        ObjectListPickerInfo o = new ObjectListPickerInfo(type.ToString(), type);
                                        typeInfo.Add(o);
                                    }
                                    else
                                    {
                                        ObjectListPickerInfo o = new ObjectListPickerInfo(Occupation.GetLocalizedCareerName(type, sim.SimDescription), type);
                                        typeInfo.Add(o);
                                    }
                                }

                                object typeReturned = ObjectListPickerDialog.Show(typeInfo);
                                if (typeReturned != null)
                                {
                                    string stringify = typeReturned.ToString();
                                    ConfirmedCareer = ParseEnum<OccupationNames>(stringify);
                                    if (ConfirmedCareer == OccupationNames.Any)
                                    {
                                        names.Remove(OccupationNames.Any);
                                        names.Remove(OccupationNames.Undefined);
                                        ConfirmedCareer = RandomUtil.GetRandomObjectFromList(names);
                                    }
                                    if (ConfirmedCareer == OccupationNames.Undefined)
                                    {
                                        SimpleMessageDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Mains:Header"), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Job/keepUnemployed:Text", new object[] { sim.FullName }));
                                        return;
                                    }
                                }
                                else
                                {
                                    names.Remove(OccupationNames.Any);
                                    names.Remove(OccupationNames.Undefined);
                                    // In case the user clicked nothing...
                                    ConfirmedCareer = RandomUtil.GetRandomObjectFromList(names);
                                }
                                
                                string Level = StringInputDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Mains:Header") + " - " + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies:Header"), sim.FullName + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Careers/MainText:TextPt1") + Occupation.GetLocalizedCareerName(ConfirmedCareer, sim.SimDescription) + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Careers/MainText:TextPt2"), "1", true);
                                if (!int.TryParse(Level, out CareerLevel))
                                {
                                    
                                    SimpleMessageDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Mains:Header") + " - " + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies:Header"), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Townies/Careers/MainText:Error"));
                                }

                                break;
                            case ThreeButtonDialog.ButtonPressed.SecondButton:
                                ConfirmedCareer = RandomUtil.GetRandomObjectFromList(names);
                                break;
                            case ThreeButtonDialog.ButtonPressed.ThirdButton:
                                return;
                        }

                        if (ConfirmedCareer == OccupationNames.Undefined) { return; }

                        CareerLocation careerLocation = GetCareerLocation(ConfirmedCareer);

                        AcquireOccupationParameters occupationParameters = new AcquireOccupationParameters(ConfirmedCareer, careerLocation, false, false);
                        occupationParameters.CharacterImportRequest = true;
                        occupationParameters.GiftPromotionAwards = true;

                        if (sim.AcquireOccupation(occupationParameters))
                        {
                            if(CareerLevel > 10) { CareerLevel = 10; }
                            for (int i = 1; i < CareerLevel; i++)
                            {
                                sim.SimDescription.CareerManager.Occupation.PromoteSim();
                            }

                            if (!assignedJob.ContainsKey(sim.SimDescription.mSimDescriptionId))
                            {
                                assignedJob.Add(sim.SimDescription.mSimDescriptionId, ConfirmedCareer);
                            }
                            else if (assignedJob[sim.SimDescription.mSimDescriptionId] != ConfirmedCareer)
                            {
                                assignedJob[sim.SimDescription.mSimDescriptionId] = ConfirmedCareer;
                            }
                        }
                    }
                    else
                    {
                        ConfirmedCareer = RandomUtil.GetRandomObjectFromList(names);

                        CareerLocation careerLocation = GetCareerLocation(ConfirmedCareer);

                        AcquireOccupationParameters occupationParameters = new AcquireOccupationParameters(ConfirmedCareer, careerLocation, false, false);
                        occupationParameters.CharacterImportRequest = true;
                        occupationParameters.GiftPromotionAwards = true;

                        if (sim.AcquireOccupation(occupationParameters))
                        {
                            CareerLevel = RandomUtil.GetInt(1, 10);
                            for (int i = 1; i < CareerLevel; i++)
                            {
                                sim.SimDescription.CareerManager.Occupation.PromoteSim();
                            }

                            if (!assignedJob.ContainsKey(sim.SimDescription.mSimDescriptionId))
                            {
                                assignedJob.Add(sim.SimDescription.mSimDescriptionId, ConfirmedCareer);
                            }
                            else if (assignedJob[sim.SimDescription.mSimDescriptionId] != ConfirmedCareer)
                            {
                                assignedJob[sim.SimDescription.mSimDescriptionId] = ConfirmedCareer;
                            }
                        }
                        
                        SimpleMessageDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Mains:Header"), sim.FullName + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Managers/Jobs/Text:NOwHasJob") + Occupation.GetLocalizedCareerName(ConfirmedCareer, sim.SimDescription));
                    }
                }
            }
            catch (Exception ex)
            {
                RPGManagerUtil.printException(ex);
                RPGManagerUtil.WriteErrorXMLFile("Error Townie Manager", ex, null);
            }
        }

        public static CareerLocation GetCareerLocation(OccupationNames careerGuid)
        {
            CareerLocation result = null;

            RabbitHole[] rabbitholes = Sims3.Gameplay.Queries.GetObjects<RabbitHole>();

            foreach(RabbitHole rabbithole in rabbitholes)
            {
                rabbithole.CareerLocations.TryGetValue((ulong)careerGuid, out result);

                if(result != null)
                {
                    return result;
                }
            }
            return result;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        

        public static bool UserCanConfirmJob = false;
        private static List<OccupationNames> GetPotentialCareer(SimDescription simDescription)
        {
            List<OccupationNames> CollectedNames = new List<OccupationNames>();

            if(simDescription.Teen)
            {
                CollectedNames.Add(OccupationNames.PTBookstoreClerk);
                CollectedNames.Add(OccupationNames.PTDaySpaReceptionist);
                CollectedNames.Add(OccupationNames.PTGroceryStoreClerk);
                CollectedNames.Add(OccupationNames.PTMausoleum);
                if (simDescription.IsAlien)
                {
                    CollectedNames.Add(OccupationNames.PTAlienTestSubject);
                }
                return CollectedNames;
            }


            // Check trait first...
            if(simDescription.TraitManager.HasAnyElement(new TraitNames[] { TraitNames.BookWorm, TraitNames.Artistic, TraitNames.Friendly, TraitNames.Charismatic }))
            {
                CollectedNames.Add(OccupationNames.Journalism);
                if (simDescription.TraitManager.HasElement(TraitNames.Artistic)) { CollectedNames.Add(OccupationNames.Writer);}

                CollectedNames.Add(OccupationNames.PTBookstoreClerk);
                CollectedNames.Add(OccupationNames.Political);

                if (simDescription.TraitManager.HasElement(TraitNames.Artistic)) { CollectedNames.Add(OccupationNames.Painter); }
            }
            bool hasCat;
            bool hasDog;
            bool hasHorse;
            simDescription.Household.HasPetsOfType(out hasDog, out hasHorse, out hasCat);

            if (simDescription.TraitManager.HasElement(TraitNames.Virtuoso)) { CollectedNames.Add(OccupationNames.Music); CollectedNames.Add(OccupationNames.RockBand); }
            if (simDescription.TraitManager.HasElement(TraitNames.NaturalCook)) { CollectedNames.Add(OccupationNames.Culinary); }
            if (simDescription.TraitManager.HasElement(TraitNames.Athletic)) { CollectedNames.Add(OccupationNames.SportsAgent); CollectedNames.Add(OccupationNames.ProfessionalSports);}
            if (simDescription.TraitManager.HasElement(TraitNames.Handy)) { CollectedNames.Add(OccupationNames.Military); CollectedNames.Add(OccupationNames.Firefighter); CollectedNames.Add(OccupationNames.Inventor); }
            if (simDescription.TraitManager.HasElement(TraitNames.FamilyOriented) || simDescription.TraitManager.HasElement(TraitNames.Nurturing)) { CollectedNames.Add(OccupationNames.Education); CollectedNames.Add(OccupationNames.Daycare); }
            if (simDescription.TraitManager.HasElement(TraitNames.Genius)) { CollectedNames.Add(OccupationNames.Science); CollectedNames.Add(OccupationNames.Medical); }
            if (simDescription.TraitManager.HasElement(TraitNames.Perceptive)) { CollectedNames.Add(OccupationNames.PrivateEye); }
            if (simDescription.TraitManager.HasElement(TraitNames.SavvySculptor)) { CollectedNames.Add(OccupationNames.Sculptor); }
            if (simDescription.TraitManager.HasElement(TraitNames.Equestrian) && hasHorse) { CollectedNames.Add(OccupationNames.Rider); }
            if (simDescription.TraitManager.HasElement(TraitNames.BotFan)) { CollectedNames.Add(OccupationNames.BotBuilder); }
            if (simDescription.TraitManager.HasElement(TraitNames.Evil) || simDescription.TraitManager.HasElement(TraitNames.MeanSpirited)) { CollectedNames.Add(OccupationNames.Criminal); }

            if (simDescription.IsAlien)
            {
                CollectedNames.Add(OccupationNames.PTAlienTestSubject);
            }
            // Now check skills...

            if (simDescription.SkillManager.HasElement(SkillNames.Athletic))
            {
                if(simDescription.SkillManager.HasElement(SkillNames.Handiness))
                {
                    if (!CollectedNames.Contains(OccupationNames.Military)) { CollectedNames.Add(OccupationNames.Military); }
                    if (!CollectedNames.Contains(OccupationNames.Firefighter)) { CollectedNames.Add(OccupationNames.Firefighter); }
                }
                if (!CollectedNames.Contains(OccupationNames.SportsAgent)) { CollectedNames.Add(OccupationNames.SportsAgent); }
                if (!CollectedNames.Contains(OccupationNames.ProfessionalSports)) { CollectedNames.Add(OccupationNames.ProfessionalSports); }
                if (!CollectedNames.Contains(OccupationNames.Criminal)) { CollectedNames.Add(OccupationNames.Criminal); }
                if (!CollectedNames.Contains(OccupationNames.LawEnforcement)) { CollectedNames.Add(OccupationNames.LawEnforcement); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Handiness))
            {
                if (!CollectedNames.Contains(OccupationNames.Science)) { CollectedNames.Add(OccupationNames.Science); }
                if (!CollectedNames.Contains(OccupationNames.Inventor)) { CollectedNames.Add(OccupationNames.Inventor); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Painting))
            {
                if (!CollectedNames.Contains(OccupationNames.Painter)) { CollectedNames.Add(OccupationNames.Painter); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Sculpting))
            {
                if (!CollectedNames.Contains(OccupationNames.Sculptor)) { CollectedNames.Add(OccupationNames.Sculptor); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Fishing))
            {
                if (!CollectedNames.Contains(OccupationNames.Fisher)) { CollectedNames.Add(OccupationNames.Fisher); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Gardening))
            {
                if (!CollectedNames.Contains(OccupationNames.Gardener)) { CollectedNames.Add(OccupationNames.Gardener); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Photography))
            {
                if (!CollectedNames.Contains(OccupationNames.Photographer)) { CollectedNames.Add(OccupationNames.Photographer); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Nectar))
            {
                if (!CollectedNames.Contains(OccupationNames.NectarMaker)) { CollectedNames.Add(OccupationNames.NectarMaker); }
            }
            if (simDescription.SkillManager.HasElement(SkillNames.Charisma))
            {
                if (!CollectedNames.Contains(OccupationNames.Political)) { CollectedNames.Add(OccupationNames.Political); }
                if (!CollectedNames.Contains(OccupationNames.Journalism)) { CollectedNames.Add(OccupationNames.Journalism); }
                if (!CollectedNames.Contains(OccupationNames.Business)) { CollectedNames.Add(OccupationNames.Business); }
            }

            if (simDescription.SkillManager.HasElement(SkillNames.Logic))
            {
                if (!CollectedNames.Contains(OccupationNames.Science)) { CollectedNames.Add(OccupationNames.Science); }
                if (!CollectedNames.Contains(OccupationNames.Inventor)) { CollectedNames.Add(OccupationNames.Inventor); }
                if (!CollectedNames.Contains(OccupationNames.LawEnforcement)) { CollectedNames.Add(OccupationNames.LawEnforcement); }
            }
            return CollectedNames;
        }

        private static List<OccupationNames> RemoveAnyInvalidCareers(List<OccupationNames> names)
        {
            List<OccupationNames> editedList = names;
            if(names != null && names.Count > 0)
            {
                for(int i = 0; i < names.Count; i++)
                {
                    CareerLocation careerLocation = GetCareerLocation(names[i]);

                    AcquireOccupationParameters occupationParameters = new AcquireOccupationParameters(names[i], careerLocation, false, false);
                    occupationParameters.CharacterImportRequest = true;
                    occupationParameters.GiftPromotionAwards = true;

                    OccupationType occupationType = Occupation.GetOccupationType(occupationParameters.TargetJob);
                    if (occupationParameters.Location == null && occupationType == OccupationType.None)
                    {
                        editedList.Remove(names[i]);
                    }
                    else if(occupationParameters.Location == null && (occupationType == OccupationType.Rabbithole || occupationType == OccupationType.ActiveCareer))
                    {
                        editedList.Remove(names[i]);
                    }
                }
                return editedList;
            }
            return names;
        }

        public static List<Sim> TowniePicker(string TypeOfRelationship, int amountOfAccepted)
        {
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObjects/TowniePicker/Header:PickSim"), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObjects/TowniePicker/Header:PickSim"), 256));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
            List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();


            Sim[] simsInWorld = Queries.GetObjects<Sim>();
            foreach (Sim friend in simsInWorld)
            {
                if (friend != null && friend.SimDescription != null && friend.SimDescription.CreatedSim != null)
                {
                    if (friend.InWorld && !friend.IsPet && !friend.IsPerformingAService && friend.Household.LotHome != null && !friend.IsInActiveHousehold)
                    {
                        List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();

                        list4.Add(new ObjectPicker.ThumbAndTextColumn(friend.GetThumbnailKey(), friend.FullName));
                        ObjectPicker.RowInfo item = new ObjectPicker.RowInfo(friend, list4);
                        list2[0].RowInfo.Add(item);
                    }
                }
            }


            if (list2.Count > 0)
            {
                
                List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show(TypeOfRelationship + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObjects/TowniePicker/Header:Picker"), Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, amountOfAccepted, list3, false);
                if (list5 != null)
                {
                    if (list5.Count > 0)
                    {
                        List<Sim> chosenSims = new List<Sim>();
                        foreach (ObjectPicker.RowInfo item2 in list5)
                        {
                            Sim friendChosen = item2.Item as Sim;
                            chosenSims.Add(friendChosen);
                        }
                        return chosenSims;
                    }
                }
            }
            return new List<Sim>();
        }

        public static List<Household> TownieHouseholdPicker(string TypeOfRelationship, int amountOfAccepted)
        {
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            
            list.Add(new ObjectPicker.HeaderInfo(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObjects/TownieHouseholdPicker/Header:Main"), Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObjects/TownieHouseholdPicker/Header:Main"), 256));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
            List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();


            Household[] simsInWorld = Sims3.Gameplay.Queries.GetObjects<Household>();
            foreach (Household friend in simsInWorld)
            {
                if (friend != null && friend.Sims.Count > 0)
                {
                    if (friend.InWorld && !friend.IsServiceNpcHousehold && friend.LotHome != null && !friend.IsActive)
                    {
                        List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();
                        ResourceKey objectDescKey = new ResourceKey(friend.HouseholdId, 188367198u, 0u);

                        list4.Add(new ObjectPicker.ThumbAndTextColumn(new ThumbnailKey(objectDescKey, ThumbnailSize.Medium), friend.Name));
                        ObjectPicker.RowInfo item = new ObjectPicker.RowInfo(friend, list4);
                        list2[0].RowInfo.Add(item);
                    }
                }
            }


            if (list2.Count > 0)
            {
                List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show(TypeOfRelationship + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObjects/TowniePicker/Header:Picker"), Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, amountOfAccepted, list3, false);
                if (list5 != null)
                {
                    if (list5.Count > 0)
                    {
                        List<Household> chosenSims = new List<Household>();
                        foreach (ObjectPicker.RowInfo item2 in list5)
                        {
                            Household friendChosen = item2.Item as Household;
                            chosenSims.Add(friendChosen);

                        }
                        return chosenSims;
                    }
                }
            }
            return new List<Household>();
        }

        // Because Skills didn't have a "Has Any Element", I implemented a new one.
        public static bool HasAnyElement(SkillNames[] guids, SimDescription desc)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                if (desc.SkillManager.HasElement((ulong)guids[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
