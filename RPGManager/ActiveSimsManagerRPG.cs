using Sims3.Gameplay.Academics;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using Sims3.UI.Controller;
using Sims3.UI.GameEntry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sims3.Gameplay.Lyralei.RPGManager
{
    public class ActiveSimsManagerRPG
    {
        public static List<SimDescription> mLastSimToPrepare = new List<SimDescription>();
        public static bool MoneySituationAlreadyRun = false;

        public static void GoThroughQuestionare(SimDescription sim, bool isBinSim, ulong ContentId)
        {
            try
            {
                if (sim != null)
                {
                    if (!sim.ToddlerOrBelow)
                    {
                        
                        bool skills = TwoButtonDialog.Show("[Skill Manager]" + '\n' + '\n' + "Would you Like RPG Manager to go through potential skills for " + sim.FullName + "?", "Yep!", "No, next!");

                        if (skills)
                        {
                            GetSkillChooser(sim);
                        }
                    }

                    if (!sim.ChildOrBelow)
                    {
                        if (!isBinSim)
                        {
                            if ( sim.CareerManager == null || sim.CareerManager.Occupation == null)
                            {
                                bool Career = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + "Would you Like RPG Manager to figure out a career for " + sim.FullName + "?", "Yep!", "No, next!");

                                if (Career)
                                {
                                    GetCareerChooser(sim);
                                }
                            }
                            else
                            {
                                bool Career = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + sim.FullName + " already seems to have a job. However, would you Like RPG Manager to figure out a career for " + sim.FullName + ", and override their current one?", "Yep!", "No, next!");

                                if (Career)
                                {
                                    GetCareerChooser(sim);
                                }
                            }
                        }

                        if (!MoneySituationAlreadyRun)
                        {
                            bool MoneySituation = TwoButtonDialog.Show("[Money Situation Manager]" + '\n' + '\n' + "Do you want to share to the RPG Manager what " + sim.Household.Name + "'s past Money Situation was? (With how much funding should the entire household start with?)", "Let's do this!", "No, next!");

                            if (MoneySituation)
                            {
                                GetMoneyChooser(sim, isBinSim, ContentId);
                                MoneySituationAlreadyRun = true;
                            }
                        }
                    }

                    if (!isBinSim && !sim.TeenOrBelow)
                    {
                        bool degree = TwoButtonDialog.Show("[Degree Manager]" + '\n' + '\n' + "Would you Like RPG Manager to figure out a (completed) degree for " + sim.FullName + "?", "Yep!", "No, next!");

                        if (degree)
                        {
                            GetDegreeChooser(sim);
                        }
                    }

                    if (!isBinSim && !sim.ToddlerOrBelow)
                    {
                        DoFriendship(sim);

                        DoLovers(sim);

                        DoEnemies(sim);

                        DoKnownNeighbors(sim);
                    }

                    DoPregnancy(sim, isBinSim, ContentId);

                    if(sim.TeenOrAbove)
                    {
                        DoSexualOrientation(sim);
                    }
                }
            }
            catch (Exception ex)
            {
                RPGManagerUtil.printException(ex);
            }
        }

        public static void DoSexualOrientation(SimDescription sim)
        {
            bool SexualOrientation = TwoButtonDialog.Show("[Gender Preference Manager]" + '\n' + '\n' + "Would you like " + sim.FullName + " to have a gender preference?", "Yep!", "No, next!");

            if (SexualOrientation)
            {
                ThreeButtonDialog.ButtonPressed btnPressed = ThreeButtonDialog.Show("[Gender Preference Manager]" + '\n' + '\n' + "Does " + sim.FullName + " prefer men, women or both?", "Men", "Women", "Both");

                switch (btnPressed)
                {
                    // prefers men
                    case ThreeButtonDialog.ButtonPressed.FirstButton:
                        sim.IncreaseGenderPreferenceMale();
                        sim.mGenderPreferenceFemale = 0;
                        sim.mGenderPreferenceMale = 200;
                        break;
                    // prefers women
                    case ThreeButtonDialog.ButtonPressed.SecondButton:
                        sim.IncreaseGenderPreferenceFemale();
                        sim.mGenderPreferenceFemale = 200;
                        sim.mGenderPreferenceMale = 0;
                        break;
                    // prefers both
                    case ThreeButtonDialog.ButtonPressed.ThirdButton:
                        sim.SetNeutralGenderPreference();
                        sim.mGenderPreferenceMale = 200;
                        sim.mGenderPreferenceFemale = 200;
                        break;
                }
            }
        }

        public static void DoSkilling(SimDescription sim)
        {
            bool skills = TwoButtonDialog.Show("[Skill Manager]" + '\n' + '\n' + "Would you Like RPG Manager to go through potential skills for " + sim.FullName + "?", "Yep!", "No, next!");

            if (skills)
            {
                GetSkillChooser(sim);
            }
        }

        public static void DoDegree(SimDescription sim)
        {
            if (!sim.TeenOrBelow)
            {
                bool degree = TwoButtonDialog.Show("[Degree Manager]" + '\n' + '\n' + "Would you Like RPG Manager to figure out a (completed) degree for " + sim.FullName + "?", "Yep!", "No, next!");

                if (degree)
                {
                    GetDegreeChooser(sim);
                }
            }
        }


        public static void DoCareer(SimDescription sim)
        {
            if (!sim.ChildOrBelow)
            {
                if (sim.CareerManager == null && sim.CareerManager.Occupation == null)
                {
                    bool Career = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + "Would you Like RPG Manager to figure out a career for " + sim.FullName + "?", "Yep!", "No, next!");

                    if (Career)
                    {
                        GetCareerChooser(sim);
                    }
                }
                else
                {
                    bool Career = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + sim.FullName + " Already seems to have a job. However, would you Like RPG Manager to figure out a career for " + sim.FullName + ", and override their current one?", "Yep!", "No, next!");

                    if (Career)
                    {
                        GetCareerChooser(sim);
                    }
                }
            }
        }

        public static void DoMoneySituation(SimDescription sim)
        {
            if (sim.ChildOrBelow) { return; }
            bool MoneySituation = TwoButtonDialog.Show("[Money Situation Manager]" + '\n' + '\n' +"Do you want to share to the RPG Manager what " + sim.Household.Name + "'s past Money Situation was? (With how much funding shall the Entire household will start with?)", "Let's do this!", "No, next!");

            if (MoneySituation)
            {
                GetMoneyChooser(sim, false, 0);
            }
        }

        public static void DoFriendship(SimDescription sim)
        {
            if (sim.ToddlerOrBelow) { return; }

            bool Friendships = TwoButtonDialog.Show("[Friendship Manager]" + '\n' + '\n' + "Would you like to handle " + sim.FullName + "'s friends? You will be able to choose to do this yourself or have the RPG manager do it for you.", "Let's do this!", "No, next!");

            if (Friendships)
            {
                bool Friendships2 = TwoButtonDialog.Show("[Friendship Manager]" + '\n' + '\n' + "Great! Do you want to Pick their friends, or have RPG Manager pick them?", "I'll pick them", "Let RPG Manager pick");

                // Let user Manually choose:
                if (Friendships2)
                {
                    List<Sim> UserChosenSims = FriendsPicker(sim, "Friend(s)", -1);

                    if (UserChosenSims.Count > 0)
                    {
                        bool FriendShipType = TwoButtonDialog.Show("[Friendship Manager]" + '\n' + '\n' + "Nice! Would you like to choose the type of friendships as well? (Best friend, Good friend, etc)", "Let me choose", "Randomize them");
                        if (FriendShipType)
                        {
                            LongTermRelationshipTypes[] names = new LongTermRelationshipTypes[] {
                                        LongTermRelationshipTypes.BestFriend,
                                        LongTermRelationshipTypes.BestFriendsForever,
                                        LongTermRelationshipTypes.DistantFriend,
                                        LongTermRelationshipTypes.GoodFriend,
                                        LongTermRelationshipTypes.OldFriend,
                                        LongTermRelationshipTypes.Friend,
                            };
                            foreach (Sim simFriend in UserChosenSims)
                            {
                                LongTermRelationshipTypes typeChosen = HaveUserChooseRelationshipType(simFriend, sim, names);
                                if(typeChosen == LongTermRelationshipTypes.Undefined) { continue; }

                                Relationship relationship2 = Relationship.Get(sim.CreatedSim, simFriend, true);

                                relationship2.LTR.ForceChangeState(typeChosen);
                            }
                        }
                    }
                }
                else // RPG Manager does the friends
                {
                    List<SimDescription> friends = new List<SimDescription>();
                    RPGManagerUtil.simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();

                    for (int i = 0; i < RPGManagerUtil.simsInWorld.Length; i++)
                    {
                        if(isAgeAppriopiate(RPGManagerUtil.simsInWorld[i].SimDescription, sim, false))
                        {
                            friends.Add(RPGManagerUtil.simsInWorld[i].SimDescription);
                        }
                    }

                    ValidateIfTownieIsGoodChoice(friends, sim, "Friends");

                    if (friends.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        int MaxFriends = GetMaxAmountOfSimsForSKillTraits(sim);

                        RandomUtil.RandomizeListOfObjects(friends);
                        int Index = 0;

                        if (MaxFriends != 0)
                        {
                            foreach (SimDescription simdesc in friends)
                            {
                                if (simdesc.CreatedSim == null) { return; }
                                if (simdesc == null) { return; }

                                Index++;
                                if (Index == MaxFriends) { break; }

                                Relationship relationship2 = Relationship.Get(sim, simdesc, true);
                                FigureOutLTRState("Friends", relationship2, sim);
                            }
                        }
                    }
                }
            }
        }

        public static void DoLovers(SimDescription sim)
        {
            
            bool Lovers = TwoButtonDialog.Show("[Relationship Manager]" + '\n' + '\n' + "Would you like to handle " + sim.FullName + "'s Crushes, exes or Partner? You will be able to choose to do this yourself or have the RPG manager do it for you.", "Let's do this!", "No, next!");

            if (Lovers)
            {
                bool Lovers2 = TwoButtonDialog.Show("[Relationship Manager]" + '\n' + '\n' + "Great! Do you want to Pick their Crushes/exes/partners, or have RPG Manager pick them?", "I'll pick them", "Let RPG Manager pick");

                if (Lovers2)
                {
                    List<Sim> UserChosenSims = FriendsPicker(sim, "Lover(s)/Ex(es)", -1);

                    if (UserChosenSims != null || UserChosenSims.Count != 0)
                    {
                        LongTermRelationshipTypes[] names = new LongTermRelationshipTypes[] {
                                LongTermRelationshipTypes.Ex,
                                LongTermRelationshipTypes.ExSpouse,
                                LongTermRelationshipTypes.Fiancee,
                                LongTermRelationshipTypes.Partner,
                                LongTermRelationshipTypes.RomanticInterest,
                                LongTermRelationshipTypes.Spouse,
                        };

                        foreach (Sim simFriend in UserChosenSims)
                        {
                            LongTermRelationshipTypes typeChosen = HaveUserChooseRelationshipType(simFriend, sim, names);
                            Relationship relationship2 = Relationship.Get(sim.CreatedSim, simFriend, true);

                            relationship2.LTR.ForceChangeState(typeChosen);
                            bool TweakLTR = TwoButtonDialog.Show("[Relationship Manager]" + '\n' + '\n' + "Would you like to set the relationship progress between " + sim.FullName + " and " + simFriend.FullName + "?", "Yes please", "No");
                            if (TweakLTR)
                            {
                                SetRelationshipLTR(simFriend, sim);
                            }
                        }
                    }
                }
                else
                {
                    List<SimDescription> friends = new List<SimDescription>();
                    RPGManagerUtil.simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();

                    for (int i = 0; i < RPGManagerUtil.simsInWorld.Length; i++)
                    {
                        if (isAgeAppriopiate(RPGManagerUtil.simsInWorld[i].SimDescription, sim, false))
                        {
                            friends.Add(RPGManagerUtil.simsInWorld[i].SimDescription);
                        }
                    }

                    ValidateIfTownieIsGoodChoice(friends, sim, "Relationship");

                    if (friends.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        int MaxFriends = GetMaxAmountOfLoversExesCrushes(sim);

                        RandomUtil.RandomizeListOfObjects(friends);
                        int Index = 0;

                        if (MaxFriends != 0)
                        {
                            foreach (SimDescription simdesc in friends)
                            {
                                if (simdesc.CreatedSim == null) { return; }
                                if (simdesc == null) { return; }

                                Index++;
                                if (Index == MaxFriends) { break; }

                                Relationship relationship2 = Relationship.Get(sim, simdesc, true);
                                FigureOutLTRState("Relationship", relationship2, sim);
                            }
                        }
                    }
                }
            }
        }


        public static void DoEnemies(SimDescription sim)
        {
            bool Enemies = TwoButtonDialog.Show("[Enemies Manager]" + '\n' + '\n' + "Would you like to handle " + sim.FullName + "'s Disliked or Enemies? You will be able to choose to do this yourself or have the RPG manager do it for you.", "Let's do this!", "No, next!");
            if (Enemies)
            {
                bool Enemies2 = TwoButtonDialog.Show("[Enemies Manager]" + '\n' + '\n' + "Great! Do you want to Pick their Disliked/Enemies/OldEnemies, or have RPG Manager pick them?", "I'll pick them", "Let RPG Manager pick");

                if (Enemies2)
                {
                    List<Sim> UserChosenSims = FriendsPicker(sim, "Enemy(ies)/Dislike(s)", -1);

                    if (UserChosenSims != null || UserChosenSims.Count != 0)
                    {
                        LongTermRelationshipTypes[] names = new LongTermRelationshipTypes[] {
                                LongTermRelationshipTypes.Disliked,
                                LongTermRelationshipTypes.Enemy,
                                LongTermRelationshipTypes.OldEnemies,
                        };
                        foreach (Sim simFriend in UserChosenSims)
                        {
                            LongTermRelationshipTypes typeChosen = HaveUserChooseRelationshipType(simFriend, sim, names);
                            Relationship relationship2 = Relationship.Get(sim.CreatedSim, simFriend, true);

                            relationship2.LTR.ForceChangeState(typeChosen);
                            bool TweakLTR = TwoButtonDialog.Show("[Enemies Manager]" + '\n' + '\n' + "Would you like to set the relationship progress between " + sim.FullName + " and " + simFriend.FullName + "?", "Yes please", "No");
                            if (TweakLTR)
                            {
                                SetRelationshipLTR(simFriend, sim);
                            }
                        }
                    }
                }
                else
                {
                    List<SimDescription> friends = new List<SimDescription>();
                    RPGManagerUtil.simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();

                    for (int i = 0; i < RPGManagerUtil.simsInWorld.Length; i++)
                    {
                        if (isAgeAppriopiate(RPGManagerUtil.simsInWorld[i].SimDescription, sim, false))
                        {
                            friends.Add(RPGManagerUtil.simsInWorld[i].SimDescription);
                        }
                    }

                    ValidateIfTownieIsGoodChoice(friends, sim, "Enemies");

                    if (friends.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        int MaxFriends = GetMaxAmountOfLoversExesCrushes(sim);

                        RandomUtil.RandomizeListOfObjects(friends);
                        int Index = 0;

                        if (MaxFriends != 0)
                        {
                            foreach (SimDescription simdesc in friends)
                            {
                                if (simdesc.CreatedSim == null) { return; }
                                if (simdesc == null) { return; }

                                Index++;
                                if (Index == MaxFriends) { break; }

                                Relationship relationship2 = Relationship.Get(sim, simdesc, true);
                                FigureOutLTRState("Enemies", relationship2, sim);
                            }
                        }
                    }
                }
            }
        }

        public static void DoKnownNeighbors(SimDescription sim)
        {
            bool knownNeighbors = TwoButtonDialog.Show("[Known Neighbors Manager]" + '\n' + '\n' + "Would you like to handle " + sim.FullName + "'s  known neighbors (or strangers)? You will be able to choose to do this yourself or have the RPG manager do it for you.", "Let's do this!", "No, next!");

            if (knownNeighbors)
            {
                bool knownNeighbors2 = TwoButtonDialog.Show("[Known Neighbors Manager]" + '\n' + '\n' + "Great! Do you want to Pick their known neighbors/strangers, or have RPG Manager pick them?", "I'll pick them", "Let RPG Manager pick");

                if (knownNeighbors2)
                {
                    List<Sim> UserChosenSims = FriendsPicker(sim, "Known Neighbor(s)", -1);

                    if (UserChosenSims != null || UserChosenSims.Count != 0)
                    {
                        LongTermRelationshipTypes[] names = new LongTermRelationshipTypes[] {
                                LongTermRelationshipTypes.Acquaintance,
                                LongTermRelationshipTypes.Stranger,
                            };

                        foreach (Sim simFriend in UserChosenSims)
                        {
                            LongTermRelationshipTypes typeChosen = HaveUserChooseRelationshipType(simFriend, sim, names);
                            Relationship relationship2 = Relationship.Get(sim.CreatedSim, simFriend, true);

                            relationship2.LTR.ForceChangeState(typeChosen);
                            bool TweakLTR = TwoButtonDialog.Show("[Known Neighbors Manager]" + '\n' + '\n' + "Would you like to set the relationship progress between " + sim.FullName + " and " + simFriend.FullName + "?", "Yes please", "No");
                            if (TweakLTR)
                            {
                                SetRelationshipLTR(simFriend, sim);
                            }
                        }

                    }
                }
                else
                {
                    List<SimDescription> friends = new List<SimDescription>();
                    RPGManagerUtil.simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();

                    for (int i = 0; i < RPGManagerUtil.simsInWorld.Length; i++)
                    {
                        if (isAgeAppriopiate(RPGManagerUtil.simsInWorld[i].SimDescription, sim, false))
                        {
                            friends.Add(RPGManagerUtil.simsInWorld[i].SimDescription);
                        }
                    }

                    ValidateIfTownieIsGoodChoice(friends, sim, "Known");

                    if (friends.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        int MaxFriends = GetMaxAmountOfSimsForSKillTraits(sim);

                        RandomUtil.RandomizeListOfObjects(friends);
                        int Index = 0;

                        if (MaxFriends != 0)
                        {
                            foreach (SimDescription simdesc in friends)
                            {
                                if (simdesc.CreatedSim == null) { return; }
                                if (simdesc == null) { return; }

                                Index++;
                                if (Index == MaxFriends) { break; }

                                Relationship relationship2 = Relationship.Get(sim, simdesc, true);
                                FigureOutLTRState("Known", relationship2, sim);
                            }
                        }
                    }
                }
            }
        }

        private static void FigureOutLTRState(string type, Relationship rel, SimDescription actor)
        {
            if (rel == null) { return; }
            if (rel.LTR == null) { return; }
            if (actor == null) { return; }

            if (type == "Enemies")
            {
                if(RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingDisliked))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Disliked);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingEnemy))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Enemy);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingOldEnemies))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.OldEnemies);
                    return;
                }
            }
            else if (type == "Friends")
            {
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingBestFriendForever))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.BestFriendsForever);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingBestFriend))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.BestFriend);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingGoodFriend))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.GoodFriend);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingOldFriend))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.OldFriend);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingFriend))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Friend);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingDistantFriend))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.DistantFriend);
                    return;
                }
            }
            else if (type == "Relationship")
            {
                SimDescription townie = rel.GetOtherSimDescription(actor);
                if(townie == null) { return; }


                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingEx))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Ex);
                    //MidlifeCrisisManager.OnHaveBreakup(townie, actor);

                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingExSpouse))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.ExSpouse);
                    //MidlifeCrisisManager.OnHaveBreakup(townie, actor);

                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingFiancee))
                {
                    if(townie.Partner != null)
                    {
                        SocialCallback.BreakUpDescriptions(townie, townie.Partner, townie.IsMarried);
                        //MidlifeCrisisManager.OnHaveBreakup(townie, townie.Partner);
                        RPGManagerUtil.print("Boken up:" + townie.FullName + " - " + townie.Partner.FullName);

                    }
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Fiancee);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingPartner))
                {
                    if (townie.Partner != null)
                    {
                        SocialCallback.BreakUpDescriptions(townie, townie.Partner, townie.IsMarried);
                        //MidlifeCrisisManager.OnHaveBreakup(townie, townie.Partner);
                        RPGManagerUtil.print("Boken up:" + townie.FullName + " - " + townie.Partner.FullName);
                    }
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Partner);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingRomanticInterest))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.RomanticInterest);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingSpouse))
                {
                    if (townie.Partner != null)
                    {
                        SocialCallback.BreakUpDescriptions(townie, townie.Partner, townie.IsMarried);
                        RPGManagerUtil.print("Boken up:" + townie.FullName + " - " + townie.Partner.FullName);
                        //MidlifeCrisisManager.OnHaveBreakup(townie, townie.Partner);
                    }
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Spouse);
                    return;
                }
            }
            else if(type == "Known")
            {
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingAcquaintance))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Acquaintance);
                    return;
                }
                if (RandomUtil.RandomChance(RPGManagerUtil.kChanceOfGettingStranger))
                {
                    rel.LTR.ForceChangeState(LongTermRelationshipTypes.Stranger);
                    return;
                }
            }
        }

        private static List<SimDescription> ValidateIfTownieIsGoodChoice(List<SimDescription> Collection, SimDescription actor, string type)
        {
            List<TraitNames> traits = new List<TraitNames>();
            foreach (KeyValuePair<ulong, Trait> mValue in actor.TraitManager.mValues)
            {
                traits.Add(mValue.Value.Guid);
            }
            TraitNames[] traitsActor = traits.ToArray();

            List<SkillNames> skills = new List<SkillNames>();
            foreach (KeyValuePair<ulong, Skill> mValue in actor.SkillManager.mValues)
            {
                skills.Add(mValue.Value.Guid);
            }
            SkillNames[] SkillsActor = skills.ToArray();

            List<SimDescription> TowniesToRemove = new List<SimDescription>();

            if (type == "Enemies")
            {
                foreach (SimDescription townie in Collection)
                {
                    foreach (Trait item in actor.TraitManager.List)
                    {
                        foreach (Trait item2 in townie.TraitManager.List)
                        {
                            if (item.TraitGuid == item2.TraitGuid)
                            {
                                TowniesToRemove.Add(townie);
                            }
                            else if (TraitManager.DoTraitsConflict(item.Guid, item2.Guid))
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (SimDescription townie in Collection)
                {
                    // We want to check their preference.
                    if (type == "Relationship")
                    {
                        if (actor.GenderPreferenceIsFemale() && townie.IsFemale)
                        {
                            continue;
                        }
                        else if (!actor.GenderPreferenceIsFemale() && townie.IsMale)
                        {
                            continue;
                        }
                        else
                        {
                            TowniesToRemove.Add(townie);
                        }
                    }

                    if (!townie.TraitManager.HasAnyElement(traitsActor))
                    {
                        if (RandomUtil.CoinFlip()) { TowniesToRemove.Add(townie); continue; }
                    }

                    if (!TownieManagerRPG.HasAnyElement(SkillsActor, townie))
                    {
                        if (RandomUtil.CoinFlip()) { TowniesToRemove.Add(townie); continue; }
                    }

                    if (!HasMutualKnownSim(actor, townie))
                    {
                        if (RandomUtil.CoinFlip()) { TowniesToRemove.Add(townie); continue; }
                    }

                }
            }

            foreach(SimDescription toRemove in TowniesToRemove)
            {
                Collection.Remove(toRemove);
            }
            return Collection;
        }
        
        private static bool HasMutualKnownSim(SimDescription actor, SimDescription townie)
        {
            List<bool> mContainsKnownPeople = new List<bool>();
            foreach(Relationship relTownie in Relationship.GetRelationships(townie))
            {
                foreach(Relationship relActor in Relationship.GetRelationships(actor))
                {
                    if(relActor.GetOtherSimDescriptionId(actor) == relTownie.GetOtherSimDescriptionId(townie))
                    {
                        mContainsKnownPeople.Add(true);
                    }
                }
            }

            if(mContainsKnownPeople.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DoPregnancy(SimDescription sim, bool isBinSim, ulong ContentId)
        {
            if (sim.IsFemale && sim.YoungAdultOrAbove)
            {
                bool PregnancyActor = TwoButtonDialog.Show("[Pregnancy Manager]" + '\n' + '\n' + "Would you like " + sim.FullName + " to be pregnant? (You'll be able to choose the sim impregating your sim)", "Good with me!", "No, next!");
                if (PregnancyActor)
                {
                    if(isBinSim)
                    {
                        List<SimDescription> BinSimsPregnancy = FriendsPicker(sim, "Sim who Impregnated " + sim.FullName, 1, isBinSim);
                        if (BinSimsPregnancy != null && BinSimsPregnancy.Count > 0)
                        {
                            ExportBinContents exportBinContents = (ExportBinContents)BinModel.Singleton.FindExportBinInfo(ContentId);
                            if (exportBinContents != null)
                            {
                                foreach(UISimInfo info in exportBinContents.HouseholdSims)
                                {
                                    if(info != null && info.SimName == sim.FullName)
                                    {
                                        info.mIsPregnant = true;
                                        RPGManagerUtil.simsToMakePregnant.Add(sim.FullName, BinSimsPregnancy[0].FullName);
                                    }
                                }
                            }
                        }
                        return;
                    }

                    List<Sim> UserChosenSims = FriendsPicker(sim, "Sim who Impregnated " + sim.FullName, 1);
                    
                    if (UserChosenSims != null && UserChosenSims.Count > 0)
                    {
                        if (Pregnancy.Start(sim.CreatedSim, UserChosenSims[0]))
                        {
                            bool PregnancyLength = TwoButtonDialog.Show("[Pregnancy Manager]" + '\n' + '\n' + "Would you like to manually set how far along they are?", "Yes please!", "No, next!");

                            if (PregnancyLength)
                            {
                                string Parsedpercentage = StringInputDialog.Show("RPG Manager", "Hours of how far along your sim is. (" + Pregnancy.kHourToStartContractions.ToString() + " hours == Starting contractions)" , "10", true);
                                float result = 10;

                                if (!float.TryParse(Parsedpercentage, out result))
                                {
                                    SimpleMessageDialog.Show("RPG Manager", "Wasn't a proper number! Will set it to the default of 10 hours...");
                                }
                                sim.Pregnancy.SetHourOfPregnancy(Math.Min((int)result, Pregnancy.kHourToStartContractions - 1));
                                sim.Pregnancy.SetPregoBlendShape();
                                sim.Pregnancy.HourlyCallback();

                                sim.Pregnancy.TryToGiveLeave();
                            }
                        }
                        else
                        {
                            SimpleMessageDialog.Show("RPG Manager", "[Pregnancy Manager]" + '\n' + '\n' + "Manager tried to make sim pregnant, but wasn't successful. This can be numerous of reasons (sims not being the same species, Household full, or simply a controversial choice.");
                        }
                    }
                }
            }
            else if (sim.IsMale && (sim.YoungAdultOrAbove && !sim.Elder))
            {
                bool PregnancyActor = TwoButtonDialog.Show("[Pregnancy Manager]" + '\n' + '\n' + "Would you like " + sim.FullName + " to impregnate a sim? (You'll be able to choose the sim to become pregnant)", "Good with me!", "No, next!");
                if (PregnancyActor)
                {
                    if (isBinSim)
                    {
                        List<SimDescription> BinSimsPregnancy = FriendsPicker(sim, "Sim who should be pregnant " + sim.FullName, -1, isBinSim);
                        if (BinSimsPregnancy != null && BinSimsPregnancy.Count > 0)
                        {
                            foreach (SimDescription impregnated in BinSimsPregnancy)
                            {
                                ExportBinContents exportBinContents = (ExportBinContents)BinModel.Singleton.FindExportBinInfo(sim.Household.HouseholdId);
                                if (exportBinContents != null)
                                {

                                    foreach (UISimInfo info in exportBinContents.HouseholdSims)
                                    {
                                        if (info != null && info.SimName == impregnated.FullName)
                                        {
                                            info.mIsPregnant = true;
                                            RPGManagerUtil.simsToMakePregnant.Add(impregnated.FullName, sim.FullName);
                                        }
                                    }
                                }
                            }
                        }
                        return;
                    }
                    List<Sim> UserChosenSims = FriendsPicker(sim, "Sim who should be pregnant " + sim.FullName, -1);

                    if (UserChosenSims != null && UserChosenSims.Count > 0)
                    {
                        foreach (Sim impregnated in UserChosenSims)
                        {
                            if (Pregnancy.Start(sim.CreatedSim, impregnated))
                            {
                                bool PregnancyLength = TwoButtonDialog.Show("[Pregnancy Manager]" + '\n' + '\n' + "Would you like to manually set how far along " + impregnated.FullName + " is?", "Yes please!", "No, next!");

                                if (PregnancyLength)
                                {
                                    string Parsedpercentage = StringInputDialog.Show("RPG Manager", "Hours of how far along " + impregnated.SimDescription.FullName + " is. (" + Pregnancy.kHourToStartContractions.ToString() + " hours == Starting contractions)", "10", true);
                                    float result = 10;


                                    if (!float.TryParse(Parsedpercentage, out result))
                                    {
                                        SimpleMessageDialog.Show("RPG Manager", "Wasn't a proper number! Will set it to the default of 10 hours...");
                                    }
                                    impregnated.SimDescription.Pregnancy.SetHourOfPregnancy(Math.Min((int)result, Pregnancy.kHourToStartContractions - 1));
                                    impregnated.SimDescription.Pregnancy.SetPregoBlendShape();
                                    impregnated.SimDescription.Pregnancy.HourlyCallback();


                                    impregnated.SimDescription.Pregnancy.TryToGiveLeave();
                                }
                            }
                            else
                            {
                                SimpleMessageDialog.Show("RPG Manager", "[Pregnancy Manager]" + '\n' + '\n' + "Manager tried to make sim pregnant, but wasn't successful. This can be numerous of reasons (sims not being the same species, Household full, or simply a controversial choice.");
                            }
                        }
                    }
                }
            }
        }

        private static void SetRelationshipLTR(Sim friend, SimDescription actor)
        {
            if(friend != null && actor != null && actor.CreatedSim != null)
            {
                Relationship rel = Relationship.Get(actor.CreatedSim, friend, false);

                if (rel == null) { return; }

                bool isPositive = TwoButtonDialog.Show("Does the relationship progress need to be positive or negative? (with other words: should it go in the red or green bar of the relationship?)", "Green/positive", "Red/Negative");

                string Parsedpercentage = StringInputDialog.Show("RPG Manager", "Amount of progress of the relationship: (Range is: 0 to 100.)", "20", true);
                float result = 20;

                if (!float.TryParse(Parsedpercentage, out result))
                {
                    SimpleMessageDialog.Show("RPG Manager", "Wasn't a proper number! Will set it to the default of 20...");
                }

                if(!isPositive)
                {
                    rel.LTR.UpdateLiking(-Math.Abs(result));
                }
                else
                {
                    rel.LTR.UpdateLiking(result);
                }
            }
        }


        private static LongTermRelationshipTypes HaveUserChooseRelationshipType(Sim sim, SimDescription actor, LongTermRelationshipTypes[] names)
        {
            List<ObjectListPickerInfo> typeInfo = new List<ObjectListPickerInfo>();
            LongTermRelationshipTypes typeChosen = LongTermRelationshipTypes.Undefined;

            foreach (LongTermRelationshipTypes type in names)
            {
                string nameType = LTRData.Get(type).GetName(actor, sim.SimDescription);
                ObjectListPickerInfo o = new ObjectListPickerInfo(sim.GetLocalizedName() + " is a: " + nameType, type);
                typeInfo.Add(o);
            }

            string typeReturned = ObjectListPickerDialog.Show(typeInfo).ToString();
            if (!string.IsNullOrEmpty(typeReturned))
            {
                 typeChosen = TownieManagerRPG.ParseEnum<LongTermRelationshipTypes>(typeReturned);
                return typeChosen;
            }
            else
            {
                // In case the user clicked nothing...
                 typeChosen = RandomUtil.GetRandomObjectFromList(names);
                return typeChosen;
            }


        }

        private static int GetMaxAmountOfSimsForSKillTraits(SimDescription simDesc)
        {
            if (simDesc != null)
            {
                TraitNames[] traitsLessFriends = new TraitNames[] { TraitNames.Loner, TraitNames.SociallyAwkward, TraitNames.Shy, TraitNames.Grumpy, TraitNames.Inappropriate, TraitNames.Insane };
                TraitNames[] traitsMoreFriends = new TraitNames[] { TraitNames.SocialButterfly, TraitNames.Charismatic, TraitNames.GoodSenseOfHumor, TraitNames.Schmoozer, TraitNames.Friendly, TraitNames.PartyAnimal, TraitNames.BornSalesman };

                if (simDesc.TraitManager.HasAnyElement(traitsLessFriends))
                {
                    return RandomUtil.GetInt(1, 4);
                }
                else if(simDesc.TraitManager.HasAnyElement(traitsMoreFriends))
                {
                    return RandomUtil.GetInt(5, 7);
                }
            }
            return 3;
        }

        private static int GetMaxAmountOfLoversExesCrushes(SimDescription simDesc)
        {
            if (simDesc != null)
            {
                TraitNames[] traitsLessFriends = new TraitNames[] { TraitNames.Loner, TraitNames.SociallyAwkward, TraitNames.Shy, TraitNames.Grumpy };
                TraitNames[] traitsMoreFriends = new TraitNames[] { TraitNames.SocialButterfly, TraitNames.CommitmentIssues, TraitNames.Charismatic, TraitNames.GoodSenseOfHumor, TraitNames.Schmoozer, TraitNames.PartyAnimal, TraitNames.Inappropriate, TraitNames.Insane };

                if (simDesc.TraitManager.HasAnyElement(traitsLessFriends))
                {
                    return RandomUtil.GetInt(1, 2);
                }
                else if (simDesc.TraitManager.HasAnyElement(traitsMoreFriends))
                {
                    return RandomUtil.GetInt(2, 5);
                }
            }
            return 1;
        }

        private static void GetMoneyChooser(SimDescription sim, bool isBinSim, ulong ContentId)
        {
            List<ObjectListPickerInfo> typeInfo = new List<ObjectListPickerInfo>();

            string[] options = new string[] {
                "Very poor ",
                "Can barely make ends meet ",
                "More Middle class ",
                "Richer than middle class, but also not incredibly rich ",
                "Rich ",
                "Royally rich ",
                "Customly set"
            };

            for(int i = 0; i < options.Length; i++)
            {
                if(i == 6) { continue; }
                options[i] = options[i] + " ($" + RPGManagerUtil.kMoneyPerOptionToBeAddedOrSubstracted[i].ToString() + ")";
            }

            int index = 0;
            foreach (string type in options)
            {
                ObjectListPickerInfo o = new ObjectListPickerInfo(type.ToString(), index);
                typeInfo.Add(o);
                index++;
            }

            string typeReturned = ObjectListPickerDialog.Show(typeInfo).ToString();
            if (!string.IsNullOrEmpty(typeReturned))
            {
                if(int.Parse(typeReturned) == 6)
                {
                    string Parsedpercentage = StringInputDialog.Show("RPG Manager", "[Money Situation Manager]" + '\n' + '\n' + "Customly set the starter money of this household: (NOTE, don't use . or , when typing it out!)", "20000", true);
                    float result = 20000;

                    if (!float.TryParse(Parsedpercentage, out result))
                    {
                        SimpleMessageDialog.Show("RPG Manager", "[Money Situation Manager]" + '\n' + '\n' + "Wasn't a proper number! Will set it to the default of 20.000...");
                    }
                    sim.Household.ModifyFamilyFunds((int)result);
                    if(isBinSim)
                    {
                        BinManagerRPG.UpdateFamilyFundsToUIInfo(sim.Household.HouseholdId, (int)result, ContentId);
                    }
                }
                else
                {
                    sim.Household.ModifyFamilyFunds(RPGManagerUtil.kMoneyPerOptionToBeAddedOrSubstracted[int.Parse(typeReturned)]);
                    if (isBinSim)
                    {
                        BinManagerRPG.UpdateFamilyFundsToUIInfo(sim.Household.HouseholdId, RPGManagerUtil.kMoneyPerOptionToBeAddedOrSubstracted[int.Parse(typeReturned)], ContentId);
                    }
                }
            }
        }

        private static void GetCareerChooser(SimDescription sim)
        {
            TownieManagerRPG.UserCanConfirmJob = TwoButtonDialog.Show("[Job Manager]" + '\n' + '\n' + "Before the RPG manager begins, do you want to manually choose from the suggested careers for " + sim.FullName + "?", "Yep!", "Nah, assign randomly");
            TownieManagerRPG.TownieJobFixer(sim.CreatedSim);
        }

        private static void GetDegreeChooser(SimDescription sim)
        {
            if(sim.SkillManager == null || sim.SkillManager.NumSkillsSimHas == 0) { return; }

            ThreeButtonDialog.ButtonPressed degree2 = ThreeButtonDialog.Show("[Degree Manager]" + '\n' + '\n' + "Cool! Would you like the RPG manager check their current Career and basing it off that?", "yes please!", "Let Me choose", "Let RPG Suggest");

            switch (degree2)
            {
                // Check current career
                case ThreeButtonDialog.ButtonPressed.FirstButton:
                    if (sim.CareerManager == null || sim.CareerManager.Occupation == null)
                    {
                        SimpleMessageDialog.Show("RPG Manager", "[Degree Manager]" + '\n' + '\n' + "Hrm, it seems that " + sim.FullName + " is Unemployed! RPG Manager will let you manually pick a degree...");
                        List<AcademicDegreeNames> degreesChosen = DegreePicker(sim);
                        if (degreesChosen != null && degreesChosen.Count != 0)
                        {
                            foreach (AcademicDegreeNames name in degreesChosen)
                            {
                                TownieManagerRPG.SetAcademicDegreeAndComplete(sim, name, false);
                            }
                        }
                        break;
                    }
                    TownieManagerRPG.TownieDegreeFixer(sim.CreatedSim, false);
                    break;

                // Let User Pick
                case ThreeButtonDialog.ButtonPressed.SecondButton:
                    List<AcademicDegreeNames> degreesChosen2 = DegreePicker(sim);
                    if (degreesChosen2 != null && degreesChosen2.Count != 0)
                    {
                        foreach (AcademicDegreeNames name in degreesChosen2)
                        {
                            TownieManagerRPG.SetAcademicDegreeAndComplete(sim, name, false);
                        }
                    }
                    break;

                // have RPG suggest
                case ThreeButtonDialog.ButtonPressed.ThirdButton:

                    TownieManagerRPG.TownieDegreeFixer(sim.CreatedSim, true);
                    if (TownieManagerRPG.AssignedActiveDegree.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("[Degree Manager] is suggesting the following degree for " + sim.FullName + ":");

                        foreach (AcademicDegreeNames degree in TownieManagerRPG.AssignedActiveDegree)
                        {
                            sb.AppendLine("* " + degree);
                        }
                        sb.AppendLine();
                        sb.AppendLine("Do you accept the suggested degree(s)? (If no, then you'll be able to pick the degree yourself)");

                        bool AcceptedDegree = TwoButtonDialog.Show(sb.ToString(), "Accepted!", "No, Let me choose!");
                        if (!AcceptedDegree)
                        {
                            List<AcademicDegreeNames> degreesChosen = DegreePicker(sim);
                            if (degreesChosen != null && degreesChosen.Count != 0)
                            {
                                foreach (AcademicDegreeNames name in degreesChosen)
                                {
                                    TownieManagerRPG.SetAcademicDegreeAndComplete(sim, name, false);
                                }
                            }
                        }
                        else
                        {
                            foreach (AcademicDegreeNames degree in TownieManagerRPG.AssignedActiveDegree)
                            {
                                TownieManagerRPG.SetAcademicDegreeAndComplete(sim, degree, false);
                            }
                        }
                    }
                    else
                    {
                        SimpleMessageDialog.Show("RPG Manager", "[Degree manager] tried to find a potential degree for your sim, but none could be suggested...");
                    }
                    break;
            }
            TownieManagerRPG.AssignedActiveDegree.Clear();
        }

        private static List<AcademicDegreeNames> DegreePicker(SimDescription sim)
        {
            AcademicDegreeNames[] skillsToPick = (AcademicDegreeNames[])Enum.GetValues(typeof(AcademicDegreeNames));

            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo("Pick any Degree for " + sim.FirstName, "Pick any Degree for " + sim.FirstName, 256));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
            List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();

            foreach (AcademicDegreeNames type in skillsToPick)
            {
                List<ObjectPicker.ColumnInfo> list6 = new List<ObjectPicker.ColumnInfo>();

                AcademicDegreeStaticData degree;


                GenericManager<AcademicDegreeNames, AcademicDegreeStaticData, AcademicDegree>.sDictionary.TryGetValue((ulong)type, out degree);

                if(degree == null) { continue; }

                list6.Add(new ObjectPicker.TextColumn(Localization.LocalizeString(degree.DegreeName)));
                ObjectPicker.RowInfo item3 = new ObjectPicker.RowInfo(type, list6);
                list2[0].RowInfo.Add(item3);
            }

            List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();
            list4.Add(new ObjectPicker.TextColumn("None"));
            ObjectPicker.RowInfo item = new ObjectPicker.RowInfo("None", list4);
            list2[0].RowInfo.Add(item);


            if (list2.Count > 0)
            {
                List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show("Degree Picker", Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, 1, list3, false);
                if (list5 != null && list5.Count > 0)
                {
                    
                    List<AcademicDegreeNames> chosenSkills = new List<AcademicDegreeNames>();
                    foreach (ObjectPicker.RowInfo item2 in list5)
                    {
                        if (item2.Item.ToString() == "None")
                        {
                            return null;
                        }
                        AcademicDegreeNames name = TownieManagerRPG.ParseEnum<AcademicDegreeNames>(item2.Item.ToString());
                        chosenSkills.Add(name);
                    }
                    return chosenSkills;
                }
            }
            return null;

        }

        public static bool isAgeAppriopiate(Sim target, Sim Actor, bool hasNoTargetOnPurpose)
        {
            if (Actor == null || Actor.SimDescription == null)
            {
                return false;
            }

            if (hasNoTargetOnPurpose)
            {
                return true;
            }
            else if (!hasNoTargetOnPurpose && target == null)
            {
                return false;
            }
            else if (!hasNoTargetOnPurpose && target.SimDescription == null)
            {
                return false;
            }

            if (target.IsPet || Actor.IsPet)
            {
                return false;
            }

            if ((Actor.SimDescription.YoungAdultOrAbove && target.SimDescription.TeenOrBelow) || (Actor.SimDescription.TeenOrBelow && target.SimDescription.YoungAdultOrAbove))
            {
                return false;
            }

            if ((Actor.SimDescription.Teen && target.SimDescription.ChildOrBelow) || (Actor.SimDescription.ChildOrBelow && target.SimDescription.Teen))
            {
                return false;
            }

            if (Actor.SimDescription.ToddlerOrBelow || target.SimDescription.ToddlerOrBelow)
            {
                return false;
            }
            return true;
        }

        public static bool isAgeAppriopiate(SimDescription target, SimDescription Actor, bool hasNoTargetOnPurpose)
        {
            if (Actor == null)
            {
                return false;
            }



            if (hasNoTargetOnPurpose)
            {
                return true;
            }
            else if (!hasNoTargetOnPurpose && target == null)
            {
                return false;
            }

            if (Actor.SimDescriptionId == target.SimDescriptionId)
            {
                return false;
            }

            if (target.IsPet || Actor.IsPet)
            {
                return false;
            }

            if ((Actor.YoungAdultOrAbove && target.TeenOrBelow) || (Actor.TeenOrBelow && target.YoungAdultOrAbove))
            {
                return false;
            }

            if ((Actor.Teen && target.ChildOrBelow) || (Actor.ChildOrBelow && target.Teen))
            {
                return false;
            }

            if (Actor.ToddlerOrBelow || target.ToddlerOrBelow)
            {
                return false;
            }
            return true;
        }


        public static List<Sim> FriendsPicker(SimDescription sim, string TypeOfRelationship, int amountOfAccepted)
        {
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo("Pick any " + TypeOfRelationship + " for " + sim.FullName, "Pick any " + TypeOfRelationship + " for " + sim.FullName, 256));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
            List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();


            foreach (Sim household in sim.Household.Sims)
            {
                if (household != null && household.SimDescription != null && household.SimDescription.CreatedSim != null)
                {
                    if (isAgeAppriopiate(household, sim.CreatedSim, false))
                    {
                        if (household.InWorld && !household.IsPet && !household.IsPerformingAService && household.Household.LotHome != null && sim.SimDescriptionId != household.SimDescription.SimDescriptionId)
                        {
                            List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();

                            list4.Add(new ObjectPicker.ThumbAndTextColumn(household.GetThumbnailKey(), household.FullName));
                            ObjectPicker.RowInfo item = new ObjectPicker.RowInfo(household, list4);
                            list2[0].RowInfo.Add(item);
                        }
                    }
                }
            }


            Sim[] simsInWorld = Sims3.Gameplay.Queries.GetObjects<Sim>();
            foreach (Sim friend in simsInWorld)
            {
                if (friend != null && friend.SimDescription != null && friend.SimDescription.CreatedSim != null)
                {
                    if (isAgeAppriopiate(friend, sim.CreatedSim, false))
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
            }
            

            if (list2.Count > 0)
            {
                List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show(TypeOfRelationship + " Picker", Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, amountOfAccepted, list3, false);
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

        private static List<SimDescription> FriendsPicker(SimDescription sim, string TypeOfRelationship, int amountOfAccepted, bool isBinSim)
        {
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo("Pick any " + TypeOfRelationship + " for " + sim.FullName, "Pick any "+ TypeOfRelationship + " for " + sim.FullName, 256));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
            List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();

            if(isBinSim)
            {
                foreach (SimDescription household in sim.Household.AllSimDescriptions)
                {
                    if (household != null)
                    {
                        if (isAgeAppriopiate(household, sim, false))
                        {
                            if (!household.IsPet)
                            {
                                List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();

                                list4.Add(new ObjectPicker.ThumbAndTextColumn(household.GetThumbnailKey(ThumbnailSize.Medium, 0), household.FullName));
                                ObjectPicker.RowInfo item = new ObjectPicker.RowInfo(household, list4);
                                list2[0].RowInfo.Add(item);
                            }
                        }
                    }
                }
            }

            if (list2.Count > 0)
            {
                List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show(TypeOfRelationship + " Picker", Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, amountOfAccepted, list3, false);
                if (list5 != null)
                {
                    if(list5.Count > 0)
                    {
                        List<SimDescription> chosenSims = new List<SimDescription>();
                        foreach (ObjectPicker.RowInfo item2 in list5)
                        {
                            SimDescription friendChosen = item2.Item as SimDescription;
                            chosenSims.Add(friendChosen);
                        }
                        return chosenSims;
                    }
                }
            }
            return new List<SimDescription>();
        }

        private static void GetSkillChooser(SimDescription sim)
        {
            if (sim.ToddlerOrBelow) { return; }
            TownieManagerRPG.AssignedActiveSkills.Clear();
            TownieManagerRPG.AssignSkillsOnDynamicLevel(sim.SimDescriptionId, true);

            StringBuilder sb = new StringBuilder();

            if(TownieManagerRPG.AssignedActiveSkills.Count > 0)
            {
                sb.AppendLine("[Skills Manager] is suggesting the following skills for " + sim.FullName + ":");

                foreach (KeyValuePair<SkillNames, int> kpv in TownieManagerRPG.AssignedActiveSkills)
                {
                    sb.AppendLine("* " + kpv.Key + " (Chance To Get: " + kpv.Value + ")");
                }
                sb.AppendLine();
                sb.AppendLine("Do you accept the suggested skill(s)? (If no, then you'll be able to pick the skills yourself)");

                bool Acceptedskills = TwoButtonDialog.Show(sb.ToString(), "Accepted!", "No, Let me choose!");

                if (!Acceptedskills)
                {
                    SkillNames[] skillsToPick = (SkillNames[])Enum.GetValues(typeof(SkillNames));

                    List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
                    list.Add(new ObjectPicker.HeaderInfo("Pick any Skill for " + sim.FirstName, "Pick any Skill for " + sim.FirstName, 256));
                    List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
                    list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
                    List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();

                    foreach (SkillNames type in skillsToPick)
                    {
                        List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();

                        Skill skill;
                        GenericManager<SkillNames, Skill, Skill>.sDictionary.TryGetValue((ulong)type, out skill);

                        if (skill != null)
                        {
                            ResourceKey skillUIIconKey = skill.SkillUIIconKey;

                            //list4.Add(new ObjectPicker.ThumbAndTextColumn(new ThumbnailKey(skillUIIconKey, ThumbnailSize.Medium), Localization.LocalizeString(skill.Name)));
                            list4.Add(new ObjectPicker.TextColumn(Localization.LocalizeString(skill.Name)));
                            ObjectPicker.RowInfo item = new ObjectPicker.RowInfo(type, list4);
                            list2[0].RowInfo.Add(item);
                        }
                    }

                    if (list2.Count > 0)
                    {
                        List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show("Skill Picker", Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, -1, list3, false);
                        if (list5 != null)
                        {
                            if(list5.Count > 0)
                            {
                                List<SkillNames> chosenSkills = new List<SkillNames>();
                                foreach (ObjectPicker.RowInfo item2 in list5)
                                {
                                    SkillNames name = TownieManagerRPG.ParseEnum<SkillNames>(item2.Item.ToString());
                                    chosenSkills.Add(name);
                                }
                                SkillNames[] array = chosenSkills.ToArray();

                                TownieManagerRPG.SKillingUpHelperFunction(sim.SimDescriptionId, array, false);
                            }
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<SkillNames, int> kpv in TownieManagerRPG.AssignedActiveSkills)
                    {
                        TownieManagerRPG.SKillingUpHelperFunction(sim, kpv.Key, false);
                    }
                }
            }
            else
            {
                SimpleMessageDialog.Show("RPG Manager", "RPG manager tried to find a potential skill for your sim, but they must be either boring (or perfect!) that none could be suggested...");
            }

        }
    }
}
