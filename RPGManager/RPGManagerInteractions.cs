using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.RabbitHoles;
using Sims3.Gameplay.PetSystems;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sims3.Gameplay.Lyralei.RPGManager
{
    public class RPGManagerInteractions
    {
        public class RerunTownieRPG : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                RPGManagerUtil.RunTownieRPGFixer();
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunTownieRPG>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction:RerunTownieRPG");
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }
        public class RunTownieSkills : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Sim[] sims = Sims3.Gameplay.Queries.GetObjects<Sim>();
                for (int i = 0; i < sims.Length; i++)
                {
                    Sim sim = sims[i];
                    if (sim != null || sim.SimDescription != null || sim.SimDescription.CreatedSim != null)
                    {
                        if (sim.InWorld && !sim.IsPet && !sim.IsPerformingAService && sim.Household != null && sim.Household.LotHome != null)
                        {
                            // We need to skip our active household sims.
                            if (sim.IsInActiveHousehold) { continue; }
                            TownieManagerRPG.TownieHobbyFixer(sim);
                        }
                    }
                }

                if (TownieManagerRPG.assignedSKills.Count > 0)
                {
                    ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sims as Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");

                    switch (XmlWriter)
                    {
                        case ThreeButtonDialog.ButtonPressed.FirstButton:
                            RPGManagerUtil.WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Skills] Log", null, TownieManagerRPG.GetTownieNamesAndSkillsInStringFormat(true));
                            SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
                            break;
                        case ThreeButtonDialog.ButtonPressed.SecondButton:
                            string TownieData = TownieManagerRPG.GetTownieNamesAndSkillsInStringFormat(false);

                            string[] subs = TownieData.Split(new string[] { "[CUT]" }, StringSplitOptions.None);

                            foreach (string split in subs)
                            {
                                StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
                                StyledNotification.Show(format);
                            }
                            break;
                        case ThreeButtonDialog.ButtonPressed.ThirdButton:
                            break;
                    }
                    TownieManagerRPG.assignedSKills.Clear();
                }
                else
                {
                    SimpleMessageDialog.Show("Lyralei's RPG Manager", "The Townie manager of the mod tried to find any townies that need skills assigned, but couldn't find any good candidates! (or, there are no sims in your world!)");
                }

                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RunTownieSkills>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Skill Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RunTownieDegree : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                Sim[] sims = Sims3.Gameplay.Queries.GetObjects<Sim>();
                for (int i = 0; i < sims.Length; i++)
                {
                    Sim sim = sims[i];
                    if (sim != null || sim.SimDescription != null || sim.SimDescription.CreatedSim != null)
                    {
                        if (sim.InWorld && !sim.IsPet && !sim.IsPerformingAService && sim.Household != null && sim.Household.LotHome != null)
                        {
                            // We need to skip our active household sims.
                            if (sim.IsInActiveHousehold) { continue; }
                            TownieManagerRPG.TownieDegreeFixer(sim, false);
                        }
                    }
                }

                if (TownieManagerRPG.assignedDegree.Count > 0)
                {
                    ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sims as Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");

                    switch (XmlWriter)
                    {
                        case ThreeButtonDialog.ButtonPressed.FirstButton:
                            RPGManagerUtil.WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Degree] Log", null, TownieManagerRPG.GetTownieNamesDegreesInStringFormat(true));
                            SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
                            break;
                        case ThreeButtonDialog.ButtonPressed.SecondButton:
                            string TownieData = TownieManagerRPG.GetTownieNamesDegreesInStringFormat(false);

                            string[] subs = TownieData.Split(new string[] { "[CUT]" }, StringSplitOptions.None);

                            foreach (string split in subs)
                            {
                                StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
                                StyledNotification.Show(format);
                            }
                            break;
                        case ThreeButtonDialog.ButtonPressed.ThirdButton:
                            break;
                    }
                    TownieManagerRPG.assignedSKills.Clear();
                }
                else
                {
                    SimpleMessageDialog.Show("Lyralei's RPG Manager", "The Townie manager of the mod tried to find any townies that need degrees assigned, but couldn't find any good candidates! (or, there are no sims in your world!)");
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RunTownieDegree>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Degree Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RunTownieCareer : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                bool TownieJobChecker = TwoButtonDialog.Show("Would you Like RPG Manager run over all sims in town and check if unemployed sims can have a good job, depending on their traits/skills?", "Yes please!", "No, next!");

                if (TownieJobChecker)
                {
                    TownieManagerRPG.UserCanConfirmJob = TwoButtonDialog.Show("Before the RPG manager begins, there is a chance that townies are qualified for multiple jobs. Do you want to manually choose the jobs in those cases?", "Yep!", "Nah, assign randomly");

                    Sim[] sims = Sims3.Gameplay.Queries.GetObjects<Sim>();
                    for (int i = 0; i < sims.Length; i++)
                    {
                        Sim sim = sims[i];
                        if (sim != null || sim.SimDescription != null || sim.SimDescription.CreatedSim != null)
                        {
                            if (sim.InWorld && !sim.IsPet && !sim.IsPerformingAService && sim.Household != null && sim.Household.LotHome != null)
                            {
                                // We need to skip our active household sims.
                                if (sim.IsInActiveHousehold) { continue; }
                                TownieManagerRPG.TownieJobFixer(sim);
                            }
                        }
                    }

                    if (TownieManagerRPG.assignedJob.Count > 0)
                    {
                        ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sims gotten a job as a Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");

                        switch (XmlWriter)
                        {
                            case ThreeButtonDialog.ButtonPressed.FirstButton:
                                RPGManagerUtil.WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Jobs] Log", null, TownieManagerRPG.GetTownieNamesJobsInStringFormat(true));
                                SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
                                break;
                            case ThreeButtonDialog.ButtonPressed.SecondButton:
                                string TownieData = TownieManagerRPG.GetTownieNamesJobsInStringFormat(false);

                                string[] subs = TownieData.Split(new string[] { "[CUT]" }, StringSplitOptions.None);

                                foreach (string split in subs)
                                {
                                    StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
                                    StyledNotification.Show(format);
                                }
                                break;
                            case ThreeButtonDialog.ButtonPressed.ThirdButton:
                                break;
                        }
                        TownieManagerRPG.assignedDegree.Clear();
                    }
                    else
                    {
                        SimpleMessageDialog.Show("Lyralei's RPG Manager", "The Townie manager of the mod tried to find any townies that need a Jobs, but couldn't find any good candidates! (or, there are no sims in your world!)");
                    }
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RunTownieDegree>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Career Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }


        public class RunTownieGenderPreference : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                bool GenderPreferenceChecker = TwoButtonDialog.Show("[Gender Preference Manager]" + '\n' + '\n' + "Would You like The RPG Manager let you handle Gender Preferences in town?", "Yes please!", "No, next!");

                if (GenderPreferenceChecker)
                {
                    bool GenderPreferenceCheckerRandom = TwoButtonDialog.Show("[Gender Preference Manager]" + '\n' + '\n' + "Great! Would you like to do this manually, or have RPG Manager handle it? (You'll be able to state the percentages of same-sex or straight preferences. Sims with a preference already won't be overridden.)", "Manually please", "Randomize it!");

                    if (GenderPreferenceCheckerRandom)
                    {
                        List<Sim> townies = TownieManagerRPG.TowniePicker("Gender Preference", -1);
                        if (townies != null && townies.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();

                            for (int i = 0; i < townies.Count; i++)
                            {
                                if (townies[i] != null)
                                {
                                    if (i == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
                                    {
                                        sb.AppendLine("[CUT]");
                                    }

                                    ActiveSimsManagerRPG.DoSexualOrientation(townies[i].SimDescription);

                                    sb.AppendLine(townies[i].SimDescription.FullName);

                                    sb.AppendLine("     Preference for Male: " + townies[i].SimDescription.mGenderPreferenceMale.ToString());
                                    sb.AppendLine("     Preference for Female: " + townies[i].SimDescription.mGenderPreferenceFemale.ToString());

                                    if (townies[i].SimDescription.mGenderPreferenceMale > townies[i].SimDescription.mGenderPreferenceFemale)
                                    {
                                        sb.AppendLine("			Story Progression's potential partner decision: Male");
                                    }
                                    else if (townies[i].SimDescription.mGenderPreferenceFemale > townies[i].SimDescription.mGenderPreferenceMale)
                                    {
                                        sb.AppendLine("			Story Progression's potential partner decision: Female");
                                    }
                                    else
                                    {
                                        sb.AppendLine("			Story Progression's potential partner decision: Both");
                                    }
                                    sb.AppendLine("");
                                    sb.AppendLine("------------");
                                    sb.AppendLine("");
                                }
                            }

                            ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sim's Gender Preferences Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");
                            string[] subs = sb.ToString().Split(new string[] { "[CUT]" }, StringSplitOptions.None);

                            switch (XmlWriter)
                            {
                                case ThreeButtonDialog.ButtonPressed.FirstButton:

                                    string res = String.Concat(subs);

                                    RPGManagerUtil.WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Gender Preference] Log", null, res);
                                    SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
                                    break;
                                case ThreeButtonDialog.ButtonPressed.SecondButton:
                                    foreach (string split in subs)
                                    {
                                        StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
                                        StyledNotification.Show(format);
                                    }
                                    break;
                                case ThreeButtonDialog.ButtonPressed.ThirdButton:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        int MaleSameSex = 10;
                        string Level = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will have a male same-sex preference.", "10", true);

                        if (!int.TryParse(Level, out MaleSameSex))
                        {
                            SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
                        }

                        int FemaleSameSex = 10;
                        string Level2 = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will have a female same-sex preference.", "10", true);

                        if (!int.TryParse(Level2, out FemaleSameSex))
                        {
                            SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
                        }

                        int Bisexual = 10;
                        string Level3 = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will have a preference for both genders.", "10", true);

                        if (!int.TryParse(Level3, out Bisexual))
                        {
                            SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
                        }

                        int Straight = 10;
                        string Level4 = StringInputDialog.Show("RPG Manager - [Gender Preference] ", "Please Input the chance that a sim that's single, will only have a preference for the opposite sex.", "10", true);

                        if (!int.TryParse(Level4, out Straight))
                        {
                            SimpleMessageDialog.Show("RPG Manager - [Gender Preference]", "You didn't enter a number! Will default to 10");
                        }

                        StringBuilder sb = new StringBuilder();
                        int index = 0;

                        Sim[] simsInWorld = Queries.GetObjects<Sim>();
                        foreach (Sim friend in simsInWorld)
                        {
                            if (friend != null && friend.SimDescription != null && friend.SimDescription.CreatedSim != null)
                            {
                                if (friend.InWorld && !friend.IsPet && !friend.IsPerformingAService && friend.Household.LotHome != null && !friend.IsInActiveHousehold)
                                {
                                    if (friend.SimDescription.ChildOrBelow) { continue; }

                                    if (friend.SimDescription.mGenderPreferenceMale == 0 && friend.SimDescription.mGenderPreferenceFemale == 0)
                                    {
                                        if (index == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
                                        {
                                            sb.AppendLine("[CUT]");
                                        }

                                        if (MaleSameSex != 0)
                                        {
                                            if (friend.IsMale && RandomUtil.RandomChance(MaleSameSex))
                                            {
                                                friend.SimDescription.mGenderPreferenceMale = 200;
                                            }
                                        }

                                        if (FemaleSameSex != 0)
                                        {
                                            if (friend.IsMale && RandomUtil.RandomChance(FemaleSameSex))
                                            {
                                                friend.SimDescription.mGenderPreferenceFemale = 200;
                                            }
                                        }

                                        if (Bisexual != 0)
                                        {
                                            if (friend.IsMale && RandomUtil.RandomChance(Bisexual))
                                            {
                                                friend.SimDescription.mGenderPreferenceFemale = 200;
                                                friend.SimDescription.mGenderPreferenceMale = 200;
                                            }
                                        }

                                        if (Straight != 0)
                                        {
                                            if (RandomUtil.RandomChance(Straight))
                                            {
                                                if (friend.IsMale)
                                                {
                                                    friend.SimDescription.mGenderPreferenceFemale = 200;
                                                }
                                                else
                                                {
                                                    friend.SimDescription.mGenderPreferenceMale = 200;
                                                }
                                            }
                                        }

                                        sb.AppendLine(friend.SimDescription.FullName);

                                        sb.AppendLine("     Preference for Male: " + friend.SimDescription.mGenderPreferenceMale.ToString());
                                        sb.AppendLine("     Preference for Female: " + friend.SimDescription.mGenderPreferenceFemale.ToString());

                                        if (friend.SimDescription.mGenderPreferenceMale > friend.SimDescription.mGenderPreferenceFemale)
                                        {
                                            sb.AppendLine("			Story Progression's potential partner decision: Male");
                                        }
                                        else if (friend.SimDescription.mGenderPreferenceFemale > friend.SimDescription.mGenderPreferenceMale)
                                        {
                                            sb.AppendLine("			Story Progression's potential partner decision: Female");
                                        }
                                        else
                                        {
                                            sb.AppendLine("			Story Progression's potential partner decision: Both");
                                        }
                                        sb.AppendLine("");
                                        sb.AppendLine("------------");
                                        sb.AppendLine("");

                                        index++;
                                    }
                                }
                            }
                        }
                        ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of sim's Gender Preferences Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");
                        string[] subs = sb.ToString().Split(new string[] { "[CUT]" }, StringSplitOptions.None);

                        switch (XmlWriter)
                        {
                            case ThreeButtonDialog.ButtonPressed.FirstButton:

                                string res = String.Concat(subs);

                                RPGManagerUtil.WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Gender Preference] Log", null, res);
                                SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");
                                break;
                            case ThreeButtonDialog.ButtonPressed.SecondButton:
                                foreach (string split in subs)
                                {
                                    StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
                                    StyledNotification.Show(format);
                                }
                                break;
                            case ThreeButtonDialog.ButtonPressed.ThirdButton:
                                break;
                        }

                    }
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RunTownieGenderPreference>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Gender Preference";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RunTowniePetAdoption : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                bool PetsChecker = TwoButtonDialog.Show("[Pet Manager]" + '\n' + '\n' + "Would You like to automatically adopt an animal for certain townie households? You'll be able to choose which household AND which animal(s)", "Yes please!", "No, next!");

                if (BinManagerRPG.mHasFullAdoptionRunBefore)
                {
                    bool refreshBin = TwoButtonDialog.Show("[Pet Manager]" + '\n' + '\n' + "The Pet Adoption bin still seems to have previously filled bin pets. If you saved new pets to your bin prior to the RPG Manager getting all bin pets, then the newest pets won't be added. Would you like the RPG Manager to refresh the bin, or continue on with the cached bin pets?", "Use cached Pets", "Refresh list");
                    if (!refreshBin)
                    {
                        BinManagerRPG.mHasFullAdoptionRunBefore = false;
                    }
                }

                if (PetsChecker)
                {
                    List<Household> townies = TownieManagerRPG.TownieHouseholdPicker("Pet ", -1);
                    if (townies != null && townies.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        for (int i = 0; i < townies.Count; i++)
                        {
                            if (townies[i] != null)
                            {
                                if (i == RPGManagerUtil.kHowManyEntriesCanBeShownOnNotification)
                                {
                                    sb.AppendLine("[CUT]");
                                }
                                SimDescription simDescription = BinManagerRPG.ShowAdoptPetPicker();
                               
                                if (simDescription != null)
                                {
                                    sb.AppendLine("The " + townies[i].Name + "'s Adopted: ");

                                    PetAdoption.GetPetOutOfPool(simDescription);

                                    townies[i].AddSilent(simDescription);
                                    simDescription.OnHouseholdChanged(townies[i], false);
                                    townies[i].OnMemberChanged(townies[i].Sims[0].SimDescription, simDescription.CreatedSim);

                                    simDescription.Instantiate(townies[i].LotHome);

                                    //townies[i].Add(simDescription);
                                    simDescription.IsNeverSelectable = false;

                                    sb.AppendLine("		- " + simDescription.FullName + " (" + simDescription.Species + ")");
                                    sb.AppendLine("");
                                    sb.AppendLine("");
                                }
                            }
                        }

                        ThreeButtonDialog.ButtonPressed XmlWriter = ThreeButtonDialog.Show("Success! Would you like to write the Log of households adopted pets as a Text file, In-Game notification or not write any Log?", "Text File please!", "In-game Notification", "No, next!");
                        string[] subs = sb.ToString().Split(new string[] { "[CUT]" }, StringSplitOptions.None);

                        switch (XmlWriter)
                        {
                            case ThreeButtonDialog.ButtonPressed.FirstButton:

                                string res = String.Concat(subs);

                                RPGManagerUtil.WriteErrorXMLFile("Lyralei's RPG Manager - Townie Manager [Pet Adoption] Log", null, res);
                                SimpleMessageDialog.Show("Lyralei's RPG Manager", "Log generated as XML/Text file. Please go to your Documents/The Sims 3/");

                                break;
                            case ThreeButtonDialog.ButtonPressed.SecondButton:
                                foreach (string split in subs)
                                {
                                    StyledNotification.Format format = new StyledNotification.Format(split, StyledNotification.NotificationStyle.kSystemMessage);
                                    StyledNotification.Show(format);
                                }
                                break;
                            case ThreeButtonDialog.ButtonPressed.ThirdButton:
                                break;
                        }
                    }
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RunTowniePetAdoption>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Pet Adoption";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }



        public class RerunActorsRPG : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                RPGManagerUtil.RunActiveHouseholdRPGFixer();
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsRPG>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "RPG Manager's Active Household";
                }

                public override string[] GetPath(bool isFemale)
                {

                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunSexualOrientation : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoSexualOrientation(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunSexualOrientation>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Gender Preference";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsSkills : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach(Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoSkilling(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsSkills>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Skilling Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsDegrees : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoDegree(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsDegrees>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Degree Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsCareer : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoCareer(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsCareer>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Career Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsMoney : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {

                ActiveSimsManagerRPG.DoMoneySituation(base.Actor.SimDescription);
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsMoney>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Money Situation Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsFriendships : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoFriendship(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsFriendships>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Friendship Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsLovers : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoLovers(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsLovers>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Relationship Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsEnemies : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoEnemies(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsEnemies>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Enemies Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsKnownNeighbors : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoKnownNeighbors(sim.SimDescription);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsKnownNeighbors>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Known Neighbors Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

        public class RerunActorsDoPregnancy : ImmediateInteraction<Sim, CityHall>
        {
            public static readonly InteractionDefinition Singleton = new Definition();

            public override bool Run()
            {
                foreach (Sim sim in base.Actor.Household.Sims)
                {
                    ActiveSimsManagerRPG.DoPregnancy(sim.SimDescription, false, 0);
                }
                return true;
            }

            public sealed class Definition : ImmediateInteractionDefinition<Sim, CityHall, RerunActorsDoPregnancy>
            {
                public override string GetInteractionName(Sim a, CityHall target, InteractionObjectPair interaction)
                {
                    return "Pregnancy Manager";
                }

                public override string[] GetPath(bool isFemale)
                {
                    return new string[3]
                    {
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:1"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:2Alt"),
                        Localization.LocalizeString("Lyralei/Gameplay/RPGManager/Interaction/Path:3"),
                    };
                }
                public override bool Test(Sim a, CityHall target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
                {
                    return true;
                }
            }
        }

    }
}
