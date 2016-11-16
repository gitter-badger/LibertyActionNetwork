using Li.Lan.Common.Services;
using Li.Lan.Common.Validators;
using Li.Lan.Data;
using Li.Lan.Models;
using Li.Lan.Views.Web.Interop;
using Li.Lan.Views.Web.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Controllers
{
    [Authorize]
    public class VoterController : BaseController
    {
        public VoterController(
            IApplicationContextProvider applicationContextProvider,
            IReferenceDictionaryProvider referenceDictionaryProvider,
            ILoggingService loggingService,
            ICommonService commonService,
            IVoterRepository voterRepository)
            : base(
                applicationContextProvider,
                loggingService)
        {
            this.ReferenceDictionaryProvider = referenceDictionaryProvider;
            this.CommonService = commonService;
            this.VoterRepository = voterRepository;
        }

        private IReferenceDictionaryProvider ReferenceDictionaryProvider { get; set; }

        private ICommonService CommonService { get; set; }

        private IVoterRepository VoterRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult View(int id)
        {
            var voterView = this.VoterRepository.SelectVoterView(id);

            if (voterView == null)
                return RedirectToAction("Search");

            var vm = Mapper.Map(voterView);

            var voterIssueTagViews = this.VoterRepository.SelectVoterIssueTagViews(id);

            vm.VoterIssueTagViewModels = voterIssueTagViews.Select(x => Mapper.Map(x)).ToList();

            var perfOptions = new VoterCandidatePreferenceSearchOptions();

            perfOptions.VoterId = vm.VoterId;
            perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaCaucus2012;

            var voterCandidatePreferences2012 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

            perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaCaucus2016;
            var voterCandidatePreferences2016 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

            vm.VoterCandidatePreferencesIowaCaucus2012 = voterCandidatePreferences2012.Select(x => Mapper.Map(x)).ToList();

            vm.VoterCandidatePreferencesIowaCaucus2016 = voterCandidatePreferences2016.Select(x => Mapper.Map(x)).ToList();

            var voterElectionViews = this.VoterRepository.SelectVoterElectionViews(id);

            vm.VoterElectionViewModels = voterElectionViews.Select(x => Mapper.Map(x)).ToList();

            return View(vm);
        }

        public ActionResult Search(string h, string l, string s, int[] p, short[] i)
        {
            var vm = new VoterSearchViewModel();
            vm.VoterSearchOptions = new VoterSearchOptions();

            if (h == "1")
            {
                if (p == null)
                    p = new int[0];

                if (i == null)
                    i = new short[0];

                this.LoggingService.Log(String.Format("l:{0}	s:{1}	p:{2}	i:{3}", l, s, String.Join(";", p), String.Join(";", i)), "Search", "Voter");

                vm.VoterSearchOptions.LastName = l;
                vm.VoterSearchOptions.StreetNameLike = s;
                vm.VoterSearchOptions.PrecinctIds = p.ToList();
                vm.VoterSearchOptions.IssueTags = i.ToList();

                // todo: validate search options
                vm.Results = this.VoterRepository.SelectVoters(vm.VoterSearchOptions);
            }

            vm.Precincts = this.GetPrecinctSelectList();
            vm.IssueTags = this.GetIssueTagSelectList();

            return View(vm);
        }

        public ActionResult Add()
        {
            var vm = new VoterEditViewModel();

            vm.Cities = this.GetCitiesSelectList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VoterViewModel voter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var v = this.MapVoter(voter);

                    var now = this.CommonService.GetCurrentDateTimeUtc();
                    v.ActiveStatus = 1;
                    v.InsertedBy = this.ApplicationContext.UserId;
                    v.InsertedOn = now;
                    v.UpdatedBy = this.ApplicationContext.UserId;
                    v.UpdatedOn = now;

                    this.VoterRepository.InsertVoter(v);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                this.LoggingService.Log(String.Format("Exception:{0}", ex.ToString()), "Add", "Voter", 1);

                ModelState.AddModelError("", "Error while saving voter. Please try again.");
            }

            // if we made it this far there was an issue, display the form again
            var vm = new VoterEditViewModel();
            vm.Cities = this.GetCitiesSelectList();

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var voter = this.VoterRepository.SelectVoter(id);

            if (voter == null)
                return RedirectToAction("Index");

            var vm = Mapper.MapEdit(voter);

            vm.Cities = this.GetCitiesSelectListWithEmpty();
            vm.PhoneNumberTypes = this.GetPhoneNumberTypeSelectListWithEmpty();

            vm.AllIssueTags = this.GetIssueTagSelectList();

            var voterIssueTagViews = this.VoterRepository.SelectVoterIssueTagViews(id);
            vm.VoterIssueTagViewModels = voterIssueTagViews.Select(x => Mapper.Map(x)).ToList();

            vm.AllIowaCaucus2016Candidates = this.GetAllIowaCaucus2016PresidentCandidatesSelectList();

            var perfOptions = new VoterCandidatePreferenceSearchOptions();
            perfOptions.VoterId = vm.VoterId;
            perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaCaucus2016;
            perfOptions.PositionId = ReferenceDictionary.PositionIdPresidnet;
            var voterCandidatePreferences2016 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

            vm.VoterCandidatePreferencesIowaCaucus2016 = voterCandidatePreferences2016.Select(x => Mapper.Map(x)).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VoterEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var voter = this.VoterRepository.SelectVoter(vm.VoterId);

                    if (voter != null)
                    {
                        this.UpdateVoter(voter, vm);

                        this.VoterRepository.UpdateVoter(voter);

                        // todo: update issue tags

                        return RedirectToAction("View", new { Id = vm.VoterId });
                    }
                    else
                    {
                        // id is not valid, redirect to index
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error while saving voter. Please try again.");
                }
            }

            return View(vm);
        }

        [HttpPost]
        public void UpdateVoterIssueTags(UpdateVoterIssueTagsViewModel vm)
        {
            // TODO: ValidateAntiForgeryToken attribute was not working even though it is being sent

            if (!vm.VoterIssueTagViewModels.Any())
                return;

            var voterId = vm.VoterIssueTagViewModels.First().VoterId;

            var userId = this.ApplicationContext.UserId;
            var utc = this.CommonService.GetCurrentDateTimeUtc();

            foreach (var item in vm.VoterIssueTagViewModels)
            {
                item.ActiveStatus = 1;
                item.FromMigrationFlag = 0;
                item.InsertedBy = userId;
                item.InsertedOn = utc;
                item.UpdatedBy = userId;
                item.UpdatedOn = utc;
            }

            this.VoterRepository.UpdateVoterIssueTagsForVoter(voterId, vm.VoterIssueTagViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int voterId)
        {
            var voter = this.VoterRepository.SelectVoter(voterId);

            if (voter != null)
            {
                voter.ActiveStatus = 0;

                this.VoterRepository.UpdateVoter(voter);
            }

            return RedirectToAction("Index");
        }

        public ActionResult CaucusPreparation(string rn)
        {
            var vm = new CaucusPreparationViewModel();

            var voter = this.VoterRepository.SelectVoter(rn);

            if (voter == null)
                return RedirectToAction("Index");

            vm.VoterEditViewModel = Mapper.MapEdit(voter);

            // retrieve CaucusPreparation record if it already exists
            var cp = this.VoterRepository.SelectCaucusPreparation(ReferenceDictionary.ElectionIdIowaCaucus2014, voter.VoterId);

            if (cp != null)
            {
                vm.CaucusPreparationId = cp.CaucusPreparationId;
                vm.ElectionId = cp.ElectionId;

                vm.CallDispositionId = cp.CallDispositionId;

                vm.IsAttending = this.ByteToBool(cp.IsAttending);
                vm.IsCentralCommittee = this.ByteToBool(cp.IsCentralCommittee);
                vm.IsDelegate = this.ByteToBool(cp.IsDelegate);
                vm.IsVolunteer = this.ByteToBool(cp.IsVolunteer);
                vm.Note = cp.Note;
                vm.VoterId = cp.VoterId;
            }
            else
            {
                vm.ElectionId = ReferenceDictionary.ElectionIdIowaCaucus2014;
                vm.VoterId = voter.VoterId;
            }

            var perfOptions = new VoterCandidatePreferenceSearchOptions();

            // Iowa Primary 2014 - Iowa Senate

            perfOptions.VoterId = vm.VoterId;
            perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaPrimaryElection2014;
            perfOptions.PositionId = ReferenceDictionary.PositionIdIowaSenate01;

            var voterCandidatePreferencesIowaSenate2014 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

            if (voterCandidatePreferencesIowaSenate2014.Any())
            {
                vm.IowaPrimary2014CandidateId = voterCandidatePreferencesIowaSenate2014.First().CandidateId;
            }

            // Iowa Caucus 2016 - President

            perfOptions.VoterId = vm.VoterId;
            perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaCaucus2016;
            perfOptions.PositionId = ReferenceDictionary.PositionIdPresidnet;

            var voterCandidatePreferences2016 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

            if (voterCandidatePreferences2016.Any())
            {
                vm.IowaCaucus2016CandidateId = voterCandidatePreferences2016.First().CandidateId;
            }

            vm.AllCallDispositionIds = this.GetAllCallDispositionIds();

            vm.VoterEditViewModel.Cities = this.GetCitiesSelectListWithEmpty();
            vm.VoterEditViewModel.PhoneNumberTypes = this.GetPhoneNumberTypeSelectListWithEmpty();

            vm.All2014IowaPrimarySenateCandidates = this.GetAll2014IowaPrimarySenateCandidatesSelectList();
            vm.All2014IowaPrimarySenateCandidates.Insert(0, new SelectListItem() { Value = "38", Text = "Undecided" });

            vm.All2016IowaCaucusPresidentCandidates = this.GetAllIowaCaucus2016PresidentCandidatesSelectList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaucusPreparation(CaucusPreparationViewModel vm)
        {
            // TODO: Transaction
            try
            {
                var utc = this.CommonService.GetCurrentDateTimeUtc();

                var voter = this.VoterRepository.SelectVoter(vm.VoterId);

                if (voter == null)
                    return RedirectToAction("Index");

                this.UpdateVoter(voter, vm.VoterEditViewModel);
                this.VoterRepository.UpdateVoter(voter);

                vm.VoterEditViewModel = Mapper.MapEdit(voter);

                // retrieve CaucusPreparation record if it already exists
                var cp = this.VoterRepository.SelectCaucusPreparation(ReferenceDictionary.ElectionIdIowaCaucus2014, vm.VoterId);

                if (cp == null)
                {
                    cp = new Lan.Models.CaucusPreparation();

                    cp.ActiveStatus = ReferenceDictionary.ActiveStatusActive;
                    cp.ElectionId = vm.ElectionId;
                    cp.VoterId = vm.VoterId;

                    cp.InsertedBy = this.ApplicationContext.UserId;
                    cp.InsertedOn = utc;
                }

                cp.UpdatedBy = this.ApplicationContext.UserId;
                cp.UpdatedOn = utc;

                cp.CallDispositionId = vm.CallDispositionId;
                
                cp.IsAttending = BoolToByte(vm.IsAttending);
                cp.IsDelegate = BoolToByte(vm.IsDelegate);
                cp.IsCentralCommittee = BoolToByte(vm.IsCentralCommittee);
                cp.IsVolunteer = BoolToByte(vm.IsVolunteer);

                if (!String.IsNullOrWhiteSpace(vm.Note))
                {
                    cp.Note = new string(vm.Note.Take(1000).ToArray());
                }

                if (cp.CaucusPreparationId == 0)
                    this.VoterRepository.InsertCaucusPreparation(cp);
                else
                    this.VoterRepository.UpdateCaucusPreparation(cp);

                // retrieve Candidate Preference if they already exist
                var perfOptions = new VoterCandidatePreferenceSearchOptions();

                // Iowa Primary 2014 - Iowa Senate
                perfOptions.VoterId = vm.VoterId;
                perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaPrimaryElection2014;
                perfOptions.PositionId = ReferenceDictionary.PositionIdIowaSenate01;

                var voterCandidatePreferencesIowaSenate2014 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

                // if there are no current preferences, or it doesn't match
                if (!voterCandidatePreferencesIowaSenate2014.Any()
                    || voterCandidatePreferencesIowaSenate2014.First().CandidateId != vm.IowaPrimary2014CandidateId)
                {
                    var vcp = this.CreateVoterCandidatePreference(vm.VoterId, vm.IowaPrimary2014CandidateId);
                    this.VoterRepository.UpdateVoterCandidatePreference(
                        vm.VoterId,
                        ReferenceDictionary.ElectionIdIowaPrimaryElection2014,
                        new[] { vcp });
                }

                // Iowa Caucus 2016 - President
                perfOptions.VoterId = vm.VoterId;
                perfOptions.ElectionId = ReferenceDictionary.ElectionIdIowaCaucus2016;
                perfOptions.PositionId = ReferenceDictionary.PositionIdPresidnet;

                var voterCandidatePreferences2016 = this.VoterRepository.SelectVoterCandidatePreferenceView(perfOptions);

                // if there are no current preferences, or it doesn't match
                if (!voterCandidatePreferences2016.Any()
                    || voterCandidatePreferences2016.First().CandidateId != vm.IowaCaucus2016CandidateId)
                {
                    var vcp = this.CreateVoterCandidatePreference(vm.VoterId, vm.IowaCaucus2016CandidateId);
                    this.VoterRepository.UpdateVoterCandidatePreference(
                        vm.VoterId,
                        ReferenceDictionary.ElectionIdIowaCaucus2016,
                        new[] { vcp });
                }

                ViewBag.Message = "Save Success";
            }
            catch (Exception ex)
            {
                this.LoggingService.Log(ex.ToString(), "CaucusPreparation", "Voter", 1);

                ViewBag.Message = "Error while saving.  Please try again.";
            }

            vm.AllCallDispositionIds = this.GetAllCallDispositionIds();

            vm.VoterEditViewModel.Cities = this.GetCitiesSelectListWithEmpty();
            vm.VoterEditViewModel.PhoneNumberTypes = this.GetPhoneNumberTypeSelectListWithEmpty();

            vm.All2014IowaPrimarySenateCandidates = this.GetAll2014IowaPrimarySenateCandidatesSelectList();
            vm.All2014IowaPrimarySenateCandidates.Insert(0, new SelectListItem() { Value = "38", Text = "Undecided" });

            vm.All2016IowaCaucusPresidentCandidates = this.GetAllIowaCaucus2016PresidentCandidatesSelectList();

            return View(vm);
        }

        public string PrepWildcard(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                var val = value.Trim('%');
                return String.Format("%{0}%", val.ToUpper());
            }
        }

        private void UpdateVoter(Voter voter, VoterEditViewModel vm)
        {
            voter.NickName = vm.NickName;

            voter.PhoneNumber2 = vm.PhoneNumber2;
            voter.PhoneNumber2Type = vm.PhoneNumber2Type;

            voter.NewAddressLine1 = vm.NewAddressLine1;
            voter.NewAddressLine2 = vm.NewAddressLine2;
            voter.NewCity = vm.NewCity;
            voter.NewStateCode = vm.NewStateCode;
            voter.NewZipCode = vm.NewZipCode;

            voter.FacebookId = vm.FacebookId;
            voter.TwitterId = vm.TwitterId;

            voter.Email = vm.Email;

            voter.UpdatedBy = this.ApplicationContext.UserId;
            voter.UpdatedOn = this.CommonService.GetCurrentDateTimeUtc();
        }

        private VoterCandidatePreference CreateVoterCandidatePreference(int voterId, int candidateId)
        {
            var userId = this.ApplicationContext.UserId;
            var utc = this.CommonService.GetCurrentDateTimeUtc();

            return new VoterCandidatePreference()
            {
                ActiveStatus = ReferenceDictionary.ActiveStatusActive,
                CandidateId = candidateId,
                InsertedBy = userId,
                InsertedOn = utc,
                SupportLevel = ReferenceDictionary.CandidateSupportLevelUnknown,
                Priority = 1,
                UpdatedBy = userId,
                UpdatedOn = utc,
                VoterId = voterId
            };
        }

        private bool? ByteToBool(byte? b)
        {
            if (b.HasValue)
            {
                return b.Value == ReferenceDictionary.ByteTrue;
            }
            else
            {
                return null;
            }
        }

        private byte? BoolToByte(bool? b)
        {
            if (b.HasValue)
            {
                return b.Value ? ReferenceDictionary.ByteTrue : ReferenceDictionary.ByteFalse;
            }
            else
            {
                return null;
            }
        }

        private Voter MapVoter(VoterViewModel voter)
        {
            var v = new Voter();

            v.VoterId = voter.VoterId;
            v.VoterGuid = voter.VoterGuid;
            v.ActiveStatus = voter.ActiveStatus;

            v.StateVoterId = voter.StateVoterId;
            v.FirstName = voter.FirstName;
            v.LastName = voter.LastName;
            v.AddressLine1 = voter.AddressLine1;
            v.AddressLine2 = voter.AddressLine2;
            v.City = voter.City;
            v.StateCode = voter.StateCode;
            v.ZipCode = voter.ZipCode;
            v.PrecinctId = voter.PrecinctId;

            var phoneNumber = PhoneNumberValidator.ValidatePhoneNumber(voter.PhoneNumber);
            v.PhoneNumber = phoneNumber.Result;

            v.FacebookId = voter.FacebookId;
            v.TwitterId = voter.TwitterId;

            return v;
        }

        private List<SelectListItem> GetAllCallDispositionIds()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem() { Value = "0", Text = "Unknown" },
                new SelectListItem() { Value = "1", Text = "Answering Machine" },
                new SelectListItem() { Value = "2", Text = "Bad Number" },
                new SelectListItem() { Value = "3", Text = "Contacted Person" },
                new SelectListItem() { Value = "4", Text = "Disconnected" },
                new SelectListItem() { Value = "5", Text = "Do Not Call" },
                new SelectListItem() { Value = "6", Text = "Wrong Person" },
            };
        }

        private List<IssueTag> GetAllIssueTags()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.IssueTags.ToList();
        }

        private List<SelectListItem> GetPrecinctSelectListWithUnknown()
        {
            var precincts = this.GetPrecinctSelectList();

            precincts.Insert(0, new SelectListItem() { Text = "Unknown", Value = "" });

            return precincts;
        }

        private List<SelectListItem> GetPrecinctSelectList()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.Precincts
                .Select(x => new SelectListItem()
                {
                    Value = x.PrecinctId.ToString(),
                    Text = x.Description
                })
                .ToList();
        }

        private List<SelectListItem> GetCitiesSelectList()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.Cities
                .Select(x => new SelectListItem()
                {
                    Value = x,
                    Text = x,
                })
                .ToList();
        }

        private List<SelectListItem> GetCitiesSelectListWithEmpty()
        {
            var cities = this.GetCitiesSelectList();

            cities.Insert(0, new SelectListItem() { Text = "", Value = "" });

            return cities;
        }

        private List<SelectListItem> GetIssueTagSelectList()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.IssueTags
                .Select(x => new SelectListItem()
                {
                    Value = x.IssueTagId.ToString(),
                    Text = x.Description
                })
                .ToList();
        }

        private List<SelectListItem> GetAllIowaCaucus2016PresidentCandidatesSelectList()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.IowaCaucus2016PresidentCandidates
                .Select(x => new SelectListItem()
                {
                    Value = x.CandidateId.ToString(),
                    Text = x.Name
                })
                .ToList();
        }

        private List<SelectListItem> GetAll2014IowaPrimarySenateCandidatesSelectList()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.IowaPrimary2014SenateCandidates
                .Select(x => new SelectListItem()
                {
                    Value = x.CandidateId.ToString(),
                    Text = x.Name
                })
                .ToList();
        }

        private List<SelectListItem> GetPhoneNumberTypeSelectList()
        {
            var referenceDictionary = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return referenceDictionary.PhoneNumberTypes
                .Select(x => new SelectListItem()
                {
                    Value = x.PhoneNumberTypeId.ToString(),
                    Text = x.Description
                })
                .ToList();
        }

        private List<SelectListItem> GetPhoneNumberTypeSelectListWithEmpty()
        {
            var phoneNumberTypes = this.GetPhoneNumberTypeSelectList();

            phoneNumberTypes.Insert(0, new SelectListItem() { Text = "", Value = "" });

            return phoneNumberTypes;
        }
    }
}