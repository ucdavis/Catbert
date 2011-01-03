using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Catbert4.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Catbert4.Controllers
{
    /// <summary>
    /// Controller for the Message class
    /// </summary>
    public class MessageController : ApplicationControllerBase
    {
	    private readonly IRepository<Message> _messageRepository;

        public MessageController(IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }
    
        //
        // GET: /Message/
        public ActionResult Index()
        {
            var messageList = _messageRepository.Queryable.Fetch(x=>x.Application);

            return View(messageList.ToList());
        }

        //
        // GET: /Message/Details/5
        public ActionResult Details(int id)
        {
            var message = _messageRepository.GetNullableById(id);

            if (message == null) return RedirectToAction("Index");

            return View(message);
        }

        //
        // GET: /Message/Create
        public ActionResult Create()
        {
			var viewModel = MessageViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /Message/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Message message)
        {
            var messageToCreate = new Message();

            Mapper.Map(message, messageToCreate);
            
            messageToCreate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _messageRepository.EnsurePersistent(messageToCreate);

                Message = "Message Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = MessageViewModel.Create(Repository);
                viewModel.Message = message;

                return View(viewModel);
            }
        }

        //
        // GET: /Message/Edit/5
        public ActionResult Edit(int id)
        {
            var message = _messageRepository.GetNullableById(id);

            if (message == null) return RedirectToAction("Index");

			var viewModel = MessageViewModel.Create(Repository);
			viewModel.Message = message;

			return View(viewModel);
        }
        
        //
        // POST: /Message/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Message message)
        {
            var messageToEdit = _messageRepository.GetNullableById(id);

            if (messageToEdit == null) return RedirectToAction("Index");

            Mapper.Map(message, messageToEdit);
            
            messageToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _messageRepository.EnsurePersistent(messageToEdit);

                Message = "Message Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = MessageViewModel.Create(Repository);
                viewModel.Message = message;

                return View(viewModel);
            }
        }
        
        //
        // GET: /Message/Delete/5 
        public ActionResult Delete(int id)
        {
			var message = _messageRepository.GetNullableById(id);

            if (message == null) return RedirectToAction("Index");

            return View(message);
        }

        //
        // POST: /Message/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, Message message)
        {
			var messageToDelete = _messageRepository.GetNullableById(id);

            if (messageToDelete == null) return RedirectToAction("Index");

            _messageRepository.Remove(messageToDelete);

            Message = "Message Removed Successfully";

            return RedirectToAction("Index");
        }
        
    }

	/// <summary>
    /// ViewModel for the Message class
    /// </summary>
    public class MessageViewModel
	{
		public Message Message { get; set; }
	    public IList<Application> Applications { get; set; }

		public static MessageViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new MessageViewModel {Message = new Message()};
		    viewModel.Applications = repository.OfType<Application>().Queryable.OrderBy(x => x.Name).ToList();
            
			return viewModel;
		}
	}
}
