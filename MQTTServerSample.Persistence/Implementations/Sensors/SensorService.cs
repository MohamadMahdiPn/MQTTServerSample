using AutoMapper;
using MQTTServerSample.Application.Contracts.Repositories;
using MQTTServerSample.Application.Contracts.Sensors;
using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Entities.Sensors;
using MQTTServerSample.Domain.Responses;

namespace MQTTServerSample.Persistence.Implementations.Sensors;

public class SensorService : ISensorService
{
    #region Constructor
    private readonly IGenericRepository<Sensor> _sensorRepository;
    private readonly IMapper _mapper;
    public SensorService(IGenericRepository<Sensor> sensorRepository, IMapper mapper)
    {
        _sensorRepository = sensorRepository;
        _mapper = mapper;
    }
    #endregion


    #region AddNew 
    public async Task<BaseResponse<SensorDto>> AddNew(SensorDto sensorDto)
    {

        var sensor = _mapper.Map<Sensor>(sensorDto);
        sensor.CreatorUserId = sensorDto.UserId;
        sensor.IsActive = true;
        var insert = await _sensorRepository.Create(sensor);

        return new()
        {
            DataItem = _mapper.Map<SensorDto>(insert),
            IsSucceeded = insert != null,
        };
    }
    #endregion


    #region CheckExists

    public async Task<BaseResponse<SensorDto>> CheckExists(string sensorName)
    {
        var check = _sensorRepository.GetAll(x => x.SensorName == sensorName).FirstOrDefault();
        return new()
        {
            DataItem = _mapper.Map<SensorDto>(check),
            IsSucceeded = check != null,

        };
        #endregion
    }
}
