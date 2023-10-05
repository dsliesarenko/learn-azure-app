using Microsoft.AspNetCore.Mvc;
using Mvc.StorageAccount.Demo.Data;
using Mvc.StorageAccount.Demo.Interfaces;
using Mvc.StorageAccount.Demo.Models;

namespace Mvc.StorageAccount.Demo.Controllers
{
    public class AttendeeRegistrationController : Controller
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IQueueService _queueService;

        public AttendeeRegistrationController(ITableStorageService tableStorageService, IBlobStorageService blobStorageService, IQueueService queueService)
        {
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
            _queueService = queueService;
        }

        // GET: AttendeeRegistrationController
        public async Task<ActionResult> Index()
        {
            var data = _tableStorageService.GetAttendees();

            foreach (var item in data)
            {
                item.ImageName = _blobStorageService.GetBlobUrl(item.ImageName);
            }

            return View(data);
        }

        // GET: AttendeeRegistrationController/Details/5
        public async Task<ActionResult> Details(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);

            data.ImageName = _blobStorageService.GetBlobUrl(data.ImageName);

            return View(data);
        }

        // GET: AttendeeRegistrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendeeRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AttendeeEntity attendeeEntity, IFormFile file)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                attendeeEntity.RowKey = id;

                if (file?.Length > 0)
                {
                    attendeeEntity.ImageName = await _blobStorageService.UploadBlob(file, id);
                }
                else
                {
                    attendeeEntity.ImageName = "default.jpg";
                }

                await _tableStorageService.UpsertAttendee(attendeeEntity);

                var email = new EmailMessage
                {
                    EmailAddress = attendeeEntity.EmailAddress,
                    TimeStamp = DateTime.UtcNow,
                    Message = $"Hello {attendeeEntity.FirstName} {attendeeEntity.LastName}," +
                    $"\n\r Thank you for registering for this event. " +
                    $"\n\r Your record has been saved for future reference. "
                };

                await _queueService.SendMessage(email);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendeeRegistrationController/Edit/5
        public async Task<ActionResult> Edit(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);

            return View(data);
        }

        // POST: AttendeeRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AttendeeEntity attendeeEntity, IFormFile file)
        {
            try
            {
                if (file?.Length > 0)
                {
                    attendeeEntity.ImageName = await _blobStorageService.UploadBlob(file, attendeeEntity.RowKey, attendeeEntity.ImageName);
                }

                attendeeEntity.PartitionKey = attendeeEntity.Industry;

                await _tableStorageService.UpsertAttendee(attendeeEntity);

                var email = new EmailMessage
                {
                    EmailAddress = attendeeEntity.EmailAddress,
                    TimeStamp = DateTime.UtcNow,
                    Message = $"Hello {attendeeEntity.FirstName} {attendeeEntity.LastName}," +
                   $"\n\r Your record was modified successfully"
                };
                await _queueService.SendMessage(email);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendeeRegistrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendeeRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, string industry)
        {
            try
            {
                var data = await _tableStorageService.GetAttendee(industry, id);
                await _tableStorageService.DeleteAttendee(industry, id);
                await _blobStorageService.RemoveBlob(data.ImageName);

                var email = new EmailMessage
                {
                    EmailAddress = data.EmailAddress,
                    TimeStamp = DateTime.UtcNow,
                    Message = $"Hello {data.FirstName} {data.LastName}," +
                  $"\n\r Your record was removed successfully"
                };
                await _queueService.SendMessage(email);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
