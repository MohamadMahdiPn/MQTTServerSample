using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MQTTServerSample.Application.Contracts.Sensors;
using MQTTServerSample.Application.DTOs.Sensors;
using System.ComponentModel.DataAnnotations;

namespace MQTTServerSample.Pages.Sensors
{
    public class SensorDetailsModel : PageModel
    {
        private readonly ISensorService _sensorService;

        public SensorDetailsModel(ISensorService sensorService)
        {
            _sensorService = sensorService;

        }
        #region Parameters

        public string Id { get; set; }
      
        public List<SensorMessageDto> Messages { get; set; }
        public SensorDto SensorItem { get; set; }
      
        #endregion
        public async Task<IActionResult> OnGet(string Id)
        {
            var sensorItem = await _sensorService.GetSensorById(Guid.Parse(Id));
            SensorItem = sensorItem.DataItem;
            var sensorMessages = await _sensorService.GetAllMessagesForSensor(Guid.Parse(sensorItem.DataItem.Id));
            Messages = sensorMessages.DataItems;
            return Page();

        }
    }
}
