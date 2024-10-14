using Domain.Logic.Features.Messages;
using Domain.Logic.Features.Messages.Requests;
using Domain.Logic.Integrations.Session;
using Domain.Objects.Database;
using Domain.Objects.ViewModels.Messages;
using Licence.Holder.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Licence.Holder.Application.Controllers
{
    public class MessagesController(
        ILogger<MessagesController> logger,
        IMessagesHandler notificationsHandler,
        ISessionOrchestration sessionOrchestration) : BaseController(sessionOrchestration)
    {
        private readonly ILogger<MessagesController> _logger = logger;
        private readonly IMessagesHandler notificationsHandler = notificationsHandler;

        [Route("messages/index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("messages/inbox")]
        public async Task<IActionResult> Inbox(int? page, CancellationToken cancellationToken)
        {
            return await GetMessages(page, MessageType.Inbound, cancellationToken);
        }

        [Route("messages/sent")]
        public async Task<IActionResult> Sent(int? page, CancellationToken cancellationToken)
        {
            return await GetMessages(page, MessageType.Outbound, cancellationToken);
        }

        [Route("messages/message")]
        public async Task<IActionResult> Message(int id, CancellationToken cancellationToken)
        {
            var query = new GetMessageViewModelRequest()
            {
                UserOktaId = GetOktaUserId(),
                NotificationId = id,
            };

            try
            {
                var model = await notificationsHandler.GetAsync(query, cancellationToken);

                return View(model);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "For company id {companyId} Failed to get message {messageId}", query.OrganisationId, id);
                return RedirectToAction(nameof(MessageNotFound));
            }
        }

        [Route("messages/message/message-not-found")]
        public IActionResult MessageNotFound()
        {
            return View();
        }

        [Route("messages/new-message")]
        public IActionResult Send()
        {
            return View(new SendMessageViewModel());
        }

        [HttpPost]
        [Route("messages/new-message")]
        public async Task<IActionResult> Send(SendMessageViewModel model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Body))
            {
                model.ValidationFailure = true;
                return View(model);
            }

            await notificationsHandler.SendAsync(new SendNotificationQuery()
            {
                UserOktaId = GetOktaUserId(),
                Model = model
            }, cancellationToken);

            return RedirectToAction(nameof(SendConfirmation));
        }

        [Route("messages/new-message/confirmation")]
        public IActionResult SendConfirmation()
        {
            return View();
        }

        [Route("messages/feedback")]
        public IActionResult Feedback()
        {
            Set_Session_FeedbackType(Domain.Objects.Database.FeedbackType.Messages);

            return RedirectToAction("Index", "Feedback");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IActionResult> GetMessages(
            int? page,
            MessageType type,
            CancellationToken cancellationToken)
        {
            var model = await notificationsHandler.GetAsync(
                            new GetMessagesDashboardViewModelRequest()
                            {
                                UserOktaId = GetOktaUserId(),
                                PageNumber = page ?? 1,
                                Type = type
                            },
                            cancellationToken);

            if (model == null)
            {
                var error = new Exception("Failed to get messages");

                _logger.LogError(error, "Failed to get messages");

                throw error;
            }

            return View(model);
        }
    }
}
