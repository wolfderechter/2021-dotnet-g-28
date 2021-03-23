﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _2021_dotnet_g_28.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        #region variables
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISupportManagerRepository _supportManagerRepository;
        private readonly ICompanyRepository _companyRepository;
        #endregion

        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository, IWebHostEnvironment hostingEnvironment, ISupportManagerRepository supportManagerRepository, ICompanyRepository companyRepository)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _hostingEnvironment = hostingEnvironment;
            _supportManagerRepository = supportManagerRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IActionResult> Index(TicketIndexViewModel model)
        {
            //if tempdata is filled with a model -> use this model (gets emptied when read)
            if (TempData["tempModel"] != null)
            {
                model = JsonConvert.DeserializeObject<TicketIndexViewModel>((string)TempData["tempModel"]);
            }

            //get signed in user
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SupportManager"))
            {
                //get support manager connected 
                var supManager = _supportManagerRepository.GetById(user.Id);
                ViewData["GebruikersNaam"] = supManager.FirstName + " " + supManager.LastName;
                model.Tickets = _ticketRepository.GetAll();
            }
            else
            {
                //get contactperson matching with signed in user
                ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);
                ViewData["GebruikersNaam"] = contactPerson.FirstName + " " + contactPerson.LastName;
                model.Tickets = _ticketRepository.GetByContactPersonId(contactPerson.Id);
                ViewData["Notifications"] = contactPerson.Notifications.Where(n => !n.IsRead).ToList();
            }

            //only do this when index gets called for first time
            if (model.CheckBoxItems == null)
            {
                //populate filter with checkbox options and setting 2 selected
                model.CheckBoxItems = new List<StatusModelTicket>();
                foreach (TicketEnum.Status ticketStatus in Enum.GetValues(typeof(TicketEnum.Status)))
                {
                    model.CheckBoxItems.Add(new StatusModelTicket() { Status = ticketStatus, IsSelected = false });
                }
                model.CheckBoxItems.SingleOrDefault(c => c.Status == TicketEnum.Status.Created).IsSelected = true;
                model.CheckBoxItems.SingleOrDefault(c => c.Status == TicketEnum.Status.InProgress).IsSelected = true;

                //insert tickets with status created and in progress
                model.Tickets = _ticketRepository.GetByStatus(new List<TicketEnum.Status> { TicketEnum.Status.Created, TicketEnum.Status.InProgress });
            }
            else
            {
                //reload tickets with new selected status
                List<TicketEnum.Status> statusList = new List<TicketEnum.Status>();
                foreach (var item in model.CheckBoxItems)
                {
                    if (item.IsSelected)
                    {
                        statusList.Add(item.Status);
                    }
                }
                model.Tickets = _ticketRepository.GetByStatus(statusList);
            }

            if(TempData["openTicket"] != null)
            {
                model.OpenedTicket = _ticketRepository.GetBy((int)TempData["openTicket"]);
            }
            
            //writes model to session so that in next request it can get read and put into tempdata
            WriteTicketIndexViewModelToSession(model);
            
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            GetTicketIndexViewModelFromSessionAndPutInTempData();
            ViewData["IsEdit"] = false;
            ViewData["typeTickets"] = TypeTickets();
            if (User.IsInRole("SupportManager"))
            {
                //get support manager connected 
                var user = await _userManager.GetUserAsync(User);
                var supManager = _supportManagerRepository.GetById(user.Id);
                ViewData["Customers"] = GetSelectListCompanies();
            }
            return View(nameof(Edit), new TicketEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketEditViewModel ticketEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //getting company from contactperson or from 
                    Company company;
                    if (!User.IsInRole("SupportManager"))
                    {
                        var user = await _userManager.GetUserAsync(User);
                        ContactPerson contact = _contactPersonRepository.getById(user.Id);
                        company = contact.Company;
                    }
                    else
                    {
                        company = _companyRepository.GetByNr(ticketEditViewModel.CompanyNr);
                    }
                    string uniqueFileName = null;
                    if (ticketEditViewModel.Picture != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "bijlagen");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + ticketEditViewModel.Picture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        ticketEditViewModel.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    var ticket = new Ticket(DateTime.Now, ticketEditViewModel.Title, ticketEditViewModel.Remark, ticketEditViewModel.Description, ticketEditViewModel.Type, TicketEnum.Status.Created, uniqueFileName);
                    _ticketRepository.Add(ticket);
                    company.AddTicket(ticket);
                    _ticketRepository.SaveChanges();
                    TempData["message"] = $"You successfully created a ticket.";
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, the ticket was not created...";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nameof(Edit), ticketEditViewModel);
            }
        }

        public IActionResult Edit(int ticketNr)
        {
            TempData["openTicket"] = ticketNr;
            GetTicketIndexViewModelFromSessionAndPutInTempData();
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ViewData["IsEdit"] = true;
            ViewData["typeTickets"] = TypeTickets();
            return View(new TicketEditViewModel(ticket));
        }

        [HttpPost]
        public IActionResult Edit(int ticketnr, TicketEditViewModel ticketEditViewModel)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    if (ticketEditViewModel.Picture != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "bijlagen");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + ticketEditViewModel.Picture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        ticketEditViewModel.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    Ticket ticket = _ticketRepository.GetBy(ticketnr);
                    ticket.EditTicket(ticketEditViewModel.Title, ticketEditViewModel.Remark, ticketEditViewModel.Description, ticketEditViewModel.Type, uniqueFileName);
                    _ticketRepository.SaveChanges();
                    TempData["message"] = $"You successfully updated the ticket.";
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, ticket was not updated...";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nameof(Edit), ticketEditViewModel);
            }

        }

        public IActionResult Stop(int ticketNr)
        {
            GetTicketIndexViewModelFromSessionAndPutInTempData();
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ticket.Status = TicketEnum.Status.Cancelled;
            _ticketRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction(int ticketNr, string reaction)
        {
            var user = await _userManager.GetUserAsync(User);
            ContactPerson contact = _contactPersonRepository.getById(user.Id);
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ticket.AddReaction(new Reaction(reaction, contact.FirstName + " " + contact.LastName, false, ticketNr));
            _ticketRepository.SaveChanges();
            TempData["message"] = $"Your reaction has been succesfully added";

            TempData["openTicket"] = ticketNr;
            GetTicketIndexViewModelFromSessionAndPutInTempData();

            return RedirectToAction(nameof(Index));
        }

        private SelectList TypeTickets()
        {
            var typeTickets = new List<TicketEnum.Type>();
            foreach (TicketEnum.Type typeTicket in Enum.GetValues(typeof(TicketEnum.Type)))
            {
                typeTickets.Add(typeTicket);
            }
            return new SelectList(typeTickets);
        }

        private SelectList GetSelectListCompanies()
        {
            return new SelectList(
                            _companyRepository.GetAll().OrderBy(c => c.CompanyName),
                            nameof(Company.CompanyNr),
                            nameof(Company.CompanyName));
        }
        
        private async Task<ContactPerson> GetLoggedInContactPerson()
        {
            var user = await _userManager.GetUserAsync(User);
            return _contactPersonRepository.getById(user.Id);
        }

        //this method esures that our filter doesn't reset each time we leave index
        //model gets saved in session when we leave index -> see index
        //in this method session gets read and cleared and put the model in tempdata -> tempdata gets checked at the start of index
        //this method gets called each time when our next request will be index
        public void GetTicketIndexViewModelFromSessionAndPutInTempData()
        {
            var model = ReadTicketIndexViewModelFromSession();
            HttpContext.Session.Clear();
            TempData["tempModel"] = JsonConvert.SerializeObject(model);
        }

        //this method writes a ticketindexviewmodel into session
        private void WriteTicketIndexViewModelToSession(TicketIndexViewModel model)
        {
            HttpContext.Session.SetString("model", JsonConvert.SerializeObject(model));
        }

        //this method reads and returns a ticketindexviewmodel from session
        private TicketIndexViewModel ReadTicketIndexViewModelFromSession()
        {
            return JsonConvert.DeserializeObject<TicketIndexViewModel>(HttpContext.Session.GetString("model"));
        }

    }
}
