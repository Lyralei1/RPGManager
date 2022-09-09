using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.PetSystems;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.SimIFace.CustomContent;
using Sims3.UI;
using Sims3.UI.CAS;
using Sims3.UI.GameEntry;
using Sims3.UI.Hud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sims3.Gameplay.Lyralei.RPGManager
{
    public class BinManagerRPG
    {

        public static bool mHasDoneHouseholdDownload = false;
        public static List<ExportBinContents> HouseholdPicker(List<ExportBinContents> Allhouseholds, string HeaderText, int amountOfAccepted)
        {
            List<ObjectPicker.HeaderInfo> list = new List<ObjectPicker.HeaderInfo>();
            list.Add(new ObjectPicker.HeaderInfo(HeaderText, HeaderText, 256));
            List<ObjectPicker.TabInfo> list2 = new List<ObjectPicker.TabInfo>();
            list2.Add(new ObjectPicker.TabInfo(string.Empty, string.Empty, new List<ObjectPicker.RowInfo>()));
            List<ObjectPicker.RowInfo> list3 = new List<ObjectPicker.RowInfo>();

            try
            {
                RPGManagerUtil.DoProgressDialog();
                foreach (ExportBinContents content in Allhouseholds)
                {
                    if (content.mExportBinType == ExportBinType.Household)
                    {
                        bool flag = content.IsLoaded();
                        if (!flag && !mHasDoneHouseholdDownload)
                        {
                            content.Import(false);
                        }

                        if (content.Household != null && content.Household.Sims != null && !string.IsNullOrEmpty(content.Household.Name))
                        {
                            List<ObjectPicker.ColumnInfo> list4 = new List<ObjectPicker.ColumnInfo>();

                            ResourceKey objectDescKey = new ResourceKey(content.Household.HouseholdId, 188367198u, 0u);
                            list4.Add(new ObjectPicker.ThumbAndTextColumn(new ThumbnailKey(objectDescKey, ThumbnailSize.Medium), content.Household.Name));
                            ObjectPicker.RowInfo item = new ObjectPicker.RowInfo(content, list4);
                            list2[0].RowInfo.Add(item);
                        }
                    }
                }
                mHasDoneHouseholdDownload = true;
                RPGManagerUtil.DoCloseProgressDialog();
            }
            catch (Exception ex)
            {
                RPGManagerUtil.printException(ex);
            }
            finally
            {
                RPGManagerUtil.DoCloseProgressDialog();
            }

            if (list2.Count > 0)
            {
                
                List<ObjectPicker.RowInfo> list5 = ObjectPickerDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObject/header:Household"), Localization.LocalizeString("Ui/Caption/ObjectPicker:OK"), Localization.LocalizeString("Ui/Caption/ObjectPicker:Cancel"), list2, list, amountOfAccepted, list3, false);
                if (list5 != null)
                {
                    if (list5.Count > 0)
                    {
                        List<ExportBinContents> chosenHouseholds = new List<ExportBinContents>();
                        foreach (ObjectPicker.RowInfo item2 in list5)
                        {
                            ExportBinContents householdChosen = item2.Item as ExportBinContents;
                            chosenHouseholds.Add(householdChosen);
                        }
                        return chosenHouseholds;
                    }
                }
            }
            return new List<ExportBinContents>();
        }


        public static void UpdateFamilyFundsToUIInfo(ulong householdId, int newFunds, ulong ContentId)
        {
            ExportBinContents exportBinContents = (ExportBinContents)BinModel.Singleton.FindExportBinInfo(ContentId);
            if (exportBinContents != null)
            {

                //if(PlayFlowHouseholdPanel.Singleton == null) { return; }

                exportBinContents.Household.ModifyFamilyFunds(newFunds);
                exportBinContents.UIBinInfo.mHouseholdFunds = newFunds;
                exportBinContents.mHouseholdWealth = newFunds;

                if (EditTownLibraryPanel.Instance != null) { EditTownLibraryPanel.Instance.mGrid.Clear();  EditTownLibraryPanel.Instance.PopulateGrid(); }

                if (PlayFlowHouseholdPanel.Singleton != null) { PlayFlowHouseholdPanel.Singleton.SetHouseholdInfo(exportBinContents.HouseholdId, exportBinContents.HouseholdName, exportBinContents.HouseholdFunds, exportBinContents.HouseholdDifficulty); }
            }
        }

        private static string GetProperString(PetPoolType type)
        {
            if(type == PetPoolType.AdoptCat)
            {
                return Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObject/progress:cats");
            }
            else if(type == PetPoolType.AdoptDog)
            {
                return Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObject/progress:dogs");
            }
            else if(type == PetPoolType.AdoptHorse)
            {
                return Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObject/progress:horses");
            }
            return "";
        }

        public static void BetterFillMiniList(PetPoolType type, ref Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>> data, bool mShowAllBinItems)
        {
            bool errorThrown = false;
            try
            {

                ProgressDialog.Show(Localization.LocalizeString("Lyralei/Gameplay/RPGManager/DialogueObject/progress:main") + GetProperString(type));


                if (CASLogic.Instance == null) { return; }


                StringBuilder sb = new StringBuilder();
                bool mWasAddedPool = false;

                if (type == PetPoolType.AdoptDog)
                {
                    List<SimDescription> mBinDogs = GetSimDescriptions(CASAgeGenderFlags.Dog, mShowAllBinItems);

                    if (mBinDogs != null && mBinDogs.Count > 0)
                    {
                        List<IMiniSimDescription> list = new List<IMiniSimDescription>();
                        foreach (SimDescription item in mBinDogs)
                        {
                            mWasAddedPool = ForceAddPet(PetPoolType.AdoptDog, item);
                            list.Add(item);

                            sb.AppendLine(item.FullName + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/PetAdoption:Added") + mWasAddedPool.ToString());
                            mWasAddedPool = false;
                        }

                        data.Add(CASAgeGenderFlags.Dog, list);
                    }

                }

                if (type == PetPoolType.AdoptCat)
                {
                    List<SimDescription> mBinCats = GetSimDescriptions(CASAgeGenderFlags.Cat, mShowAllBinItems);
                    if (mBinCats != null && mBinCats.Count > 0)
                    {
                        List<IMiniSimDescription> list = new List<IMiniSimDescription>();
                        foreach (SimDescription item in mBinCats)
                        {
                            mWasAddedPool = ForceAddPet(PetPoolType.AdoptCat, item);
                            list.Add(item);
                            sb.AppendLine(item.FullName + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/PetAdoption:Added") + mWasAddedPool.ToString());
                            mWasAddedPool = false;

                        }
                        data.Add(CASAgeGenderFlags.Cat, list);
                    }

                }

                if (type == PetPoolType.AdoptHorse)
                {
                    List<SimDescription> mBinHorses = GetSimDescriptions(CASAgeGenderFlags.Horse, mShowAllBinItems);


                    if (mBinHorses != null && mBinHorses.Count > 0)
                    {
                        List<IMiniSimDescription> list = new List<IMiniSimDescription>();
                        foreach (SimDescription item in mBinHorses)
                        {
                            mWasAddedPool = ForceAddPet(PetPoolType.AdoptHorse, item);
                            list.Add(item);
                            sb.AppendLine(item.FullName + Localization.LocalizeString("Lyralei/Gameplay/RPGManager/xmlWriter/PetAdoption:Added") + mWasAddedPool.ToString());
                            mWasAddedPool = false;
                        }
                        data.Add(CASAgeGenderFlags.Horse, list);
                    }
                }
                //RPGManagerUtil.print(sb.ToString());
                ProgressDialog.Close();
            }
            catch (Exception ex)
            {
                RPGManagerUtil.WriteErrorXMLFile("RPG_Manager_Error_[Pet Manager]", ex, null);
                errorThrown = true;
            }
            finally
            {
                if(errorThrown)
                {
                    ProgressDialog.Close();
                }
            }
        }


        public static List<SimDescription> GetSimDescriptions(CASAgeGenderFlags ageGender, bool mShowAllBinItems)
        {
            ResKeyTable SimDescriptions = DownloadContent.GetImportedSimResourceKeys(false);
            if (SimDescriptions == null)
            {
                return null;
            }

            List<SimDescription> arrayList = new List<SimDescription>();
            for (int i = 0; i < SimDescriptions.Count; i++)
            {
                ResourceKeyContentCategory resourceKeyContentCategory = ResourceKeyContentCategory.kInstalled;
                SimDescription simDescription = CASLogic.Instance.LoadSimDescription(SimDescriptions[i], ref resourceKeyContentCategory);

                if(simDescription == null) { continue; }
                SimDescription CopiedDescription = simDescription;

                // Only show custom pets...
                if (!mShowAllBinItems)
                {
                    if (string.IsNullOrEmpty(simDescription.FirstName) || string.IsNullOrEmpty(simDescription.LastName)) { continue; }
                }
                else // Show All Bin pets...
                {
                    CopiedDescription = GeneticsPet.MakePet(simDescription.Age, simDescription.Gender, simDescription.Species, simDescription.GetOutfit(OutfitCategories.Everyday, 0).Key);
                }


                //SimDescription CopiedDescription = GeneticsPet.MakePet(simDescription.Age, simDescription.Gender, simDescription.Species, simDescription.GetOutfit(OutfitCategories.Everyday, 0).Key);
                //if (simDescription != null) { RPGManagerUtil.print(simDescription.FullName); }
                //SimDescription CopiedDescription = simDescription;

                if (CopiedDescription != null && CASLogic.Instance.GetSpecies(CopiedDescription.Species) == ageGender)
                {
                    if(CopiedDescription.PetManager == null)
                    {
                        CopiedDescription.CreatePetManager();
                    }
                    CopiedDescription.Fixup();

                    // Make pet already takes care for random names, therefore if we had a sim with a user-set name, we reapply it.
                    if (!string.IsNullOrEmpty(simDescription.FirstName) || !string.IsNullOrEmpty(simDescription.FirstName))
                    {
                        CopiedDescription.FirstName = simDescription.FirstName;
                        CopiedDescription.LastName = simDescription.LastName;
                    }
                    if (simDescription.NumTraits > 0)
                    {
                        CopiedDescription.TraitManager = new TraitManager(simDescription.TraitManager);
                        CopiedDescription.TraitManager.SetSimDescription(CopiedDescription);
                    }
                    else
                    {
                        CopiedDescription.TraitManager.AddRandomTrait(RandomUtil.GetInt(PetAdoption.PetTraitNum[0], PetAdoption.PetTraitNum[1]));
                    }

                    //if (!string.IsNullOrEmpty(simDescription.FullName)) { RPGManagerUtil.print(simDescription.FullName); }
                    //CASLogic.Instance.LoadSim(SimDescriptions[i]);
                    arrayList.Add(CopiedDescription);
                }
            }
            return arrayList;
        }

        public static bool ForceAddPet(PetPoolType type, SimDescription mDescription)
        {
            PetPool petPool;
            if (PetPoolManager.TryGetPetPool(type, out petPool))
            {
                if ((petPool.mSimDescriptionIds == null || !petPool.mSimDescriptionIds.Contains(mDescription.SimDescriptionId)) && !Household.PetHousehold.Contains(mDescription))
                {
                    //Lazy.Allocate(ref petPool.mSimDescriptionIds);
                    petPool.mSimDescriptionIds.Add(mDescription.SimDescriptionId);

                    if (Household.PetHousehold != null)
                    {
                        Household.PetHousehold.AddSilent(mDescription);
                        mDescription.OnHouseholdChanged(Household.PetHousehold, false);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool mHasFullAdoptionRunBefore = false;

        public static SimDescription ShowAdoptPetPicker()
        {
            try
            {
                //bool mDoCustomOnly = TwoButtonDialog.Show("Before RPG manager will gather all pets, it will grab a few random pets first and then check for Bin pets. Would you like to load in EA's bin pets as well, or just your own custom ones? (They need to have at least names! Otherwise they'll be treated as a EA pet.)",  "All Bin Pets!", "My Custom Pets Only!");
                
                Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>> petdata = new Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>>();
                Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>> petdata1 = new Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>>();
                Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>> Combined = new Dictionary<CASAgeGenderFlags, List<IMiniSimDescription>>();

                //ProgressDialog.Show("RPG Manager Is Thinking...");

                    PetAdoption.FillMiniList(PetPoolType.AdoptCat, ref petdata);
                    PetAdoption.FillMiniList(PetPoolType.AdoptDog, ref petdata);
                    PetAdoption.FillMiniList(PetPoolType.AdoptHorse, ref petdata);

                //Since the thing has run before, this should work :)
                if(!mHasFullAdoptionRunBefore)
                {
                    bool mDoCustomOnly = true;
                    // Fill Adoption Picker with bin animals...
                    BetterFillMiniList(PetPoolType.AdoptDog, ref petdata1, mDoCustomOnly);
                    BetterFillMiniList(PetPoolType.AdoptCat, ref petdata1, mDoCustomOnly);
                    BetterFillMiniList(PetPoolType.AdoptHorse, ref petdata1, mDoCustomOnly);
                    mHasFullAdoptionRunBefore = true;
                }


                    foreach (KeyValuePair<CASAgeGenderFlags, List<IMiniSimDescription>> kpv in petdata)
                    {
                        if (petdata1.ContainsKey(kpv.Key))
                        {
                            List<IMiniSimDescription> sims = new List<IMiniSimDescription>();
                            sims.AddRange(kpv.Value);
                            sims.AddRange(petdata1[kpv.Key]);
                            Combined.Add(kpv.Key, sims);
                        }
                    }

                //ProgressDialog.Close();

                if (Combined.Count > 0)
                {
                    return PetAdoptionDialog.Show(Combined, true) as SimDescription;
                }
                else
                {
                    return PetAdoptionDialog.Show(petdata, true) as SimDescription;
                }
            }
            catch (Exception ex)
            {
                RPGManagerUtil.WriteErrorXMLFile("RPGManager_PetAdoption_Error_", ex, null);
            }
            finally
            {
                ProgressDialog.Close();
            }
            return null;
        }
    }
}
