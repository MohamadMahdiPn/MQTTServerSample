using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MQTTServerSample.Application.Contracts.Sensors;
using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Enums;

namespace MQTTServerSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
      

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
           
        }

        public async void OnGet()
        {
            //var cUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var sensorDTO = new SensorDto
            //{
            //    CreatedDate = DateTime.Now,
            //    SensorIp = "Test",
            //    SensorName = "Test",
            //    SensorType = SensorType.Temperature,
            //    UserId = cUser
            //};
            //var insertResult = await _sensorService.AddNew(sensorDTO);
        }
    }
}
