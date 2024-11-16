using AutoMapper;
using MQTTServerSample.Application.Contracts.Redis;
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
    private readonly IGenericRepository<SensorMessage> _sensorMessageRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;
    public SensorService(IGenericRepository<Sensor> sensorRepository, IMapper mapper, IRedisService redisService, IGenericRepository<SensorMessage> sensorMessageRepository)
    {
        _sensorRepository = sensorRepository;
        _mapper = mapper;
        _redisService = redisService;
        _sensorMessageRepository = sensorMessageRepository;
    }
    #endregion


    #region AddNew 
    public async Task<BaseResponse<SensorDto>> AddNew(SensorDto sensorDto)
    {

        var sensor = _mapper.Map<Sensor>(sensorDto);
        sensor.CreatorUserId = sensorDto.UserId;
        sensor.IsActive = true;
        var insert = await _sensorRepository.Create(sensor);
        await SaveListToRedis();
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
        
    }


    #endregion

    #region Redis
    public async Task<BaseResponse<NameIdSensorRedis>> SaveListToRedis()
    {
        var listOfSensors = _sensorRepository.GetAll().Select(x => new NameIdSensorRedis(x)).ToList();
        await _redisService.SaveToCashAsync<List<NameIdSensorRedis>>("SensorsList", listOfSensors, 120);

        return new()
        {
            DataItems = listOfSensors,
            IsSucceeded = true
        };

    }

    public async Task<BaseResponse<NameIdSensorRedis>> ReadListFromRedis()
    {
        var listOfSensors = await _redisService.RetrieveFromCashAsync<List<NameIdSensorRedis>>("SensorsList");

        return new()
        {
            DataItems = listOfSensors,
            IsSucceeded = true
        };

    }

    public async Task<BaseResponse<NameIdSensorRedis>> ReadIdFromNameRedis(string sensorName)
    {
        var listOfSensors = await _redisService.RetrieveFromCashAsync<List<NameIdSensorRedis>>("SensorsList");
        var item = listOfSensors.Find(x => x.SensorName == sensorName);
        return new()
        {
            DataItem = item,
            DataItems = listOfSensors,
            IsSucceeded = true
        };

    }
    #endregion

    #region SensorMessage
    public async Task<BaseResponse<SensorMessageDto>> AddNewMessage(SensorMessageDto sensorMessageDto)
    {
        var sensorMessage = _mapper.Map<SensorMessage>(sensorMessageDto);
        sensorMessage.CreatorUserId = sensorMessageDto.UserId;
        sensorMessage.IsActive = true;

        var isnert = await _sensorMessageRepository.Create(sensorMessage);

        return new()
        {
            IsSucceeded = isnert != null,
            DataItem = _mapper.Map<SensorMessageDto>(sensorMessage),
        };
    }
    #endregion
}
