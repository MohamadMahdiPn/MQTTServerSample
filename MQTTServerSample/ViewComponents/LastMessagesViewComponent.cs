using Microsoft.AspNetCore.Mvc;

namespace MQTTServerSample.ViewComponents;

public class LastMessagesViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        //var messages = MqttBackgroundService.GetLastMessages();
        return View();
    }
}

