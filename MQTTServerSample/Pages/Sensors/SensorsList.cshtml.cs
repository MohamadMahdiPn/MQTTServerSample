using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MQTTServerSample.Application.Contracts.Sensors;
using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Entities.IdentityModels;
using MQTTServerSample.Domain.Entities.Sensors;

namespace MQTTServerSample.Pages.Sensors
{
    public class SensorsListModel : PageModel
    {
        private readonly ISensorService _sensorService;

        public SensorsListModel(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }


        #region Parameters

        public Paging<SensorDto> PagingUsers { get; set; }
        public int? PageNumber = 1;

        #endregion

        public async Task<IActionResult> OnGet(int? pageNumber)
        {
            PageNumber = pageNumber == null ? 1 : pageNumber;
            int pageSize = 12;

            var data =await _sensorService.GetAllSensors();
            PagingUsers = Paging<SensorDto>.Create(data.DataItems, pageNumber ?? 1, pageSize);
            return Page();
        }
    }
}
