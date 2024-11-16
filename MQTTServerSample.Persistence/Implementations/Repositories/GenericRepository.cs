using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MQTTServerSample.Application.Contracts.Repositories;
using MQTTServerSample.Persistence.Data;
using MQTTServerSample.Domain.Bases;

namespace MQTTServerSample.Persistence.Implementations.Repositories;


public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseTable
{
    #region Constructor

    private readonly MQTTServerSampleDbContext _context;
    private DbSet<TEntity> _dbset;

    public GenericRepository(MQTTServerSampleDbContext context)
    {
        _context = context;
        _dbset = context.Set<TEntity>();
    }


    #endregion


    #region Create

    public async Task<TEntity> Create(TEntity entity)
    {
        entity.UserId = entity.CreatorUserId;
        entity.CreatedDate = DateTime.Now;
        var savedObj = await _dbset.AddAsync(entity);
        _context.SaveChanges();
        return savedObj.Entity;
    }

    #endregion

    #region Delete

    public async Task<bool> Delete(Guid id)
    {
        var entity = await _dbset.FindAsync(id);
        if (entity != null) _dbset.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> SoftDelete(Guid id, string cUserId)
    {
        var record = await GetById(id);
        if (record != null)
        {
            record.IsDeleted = true;
            record.IsActive = false;
            record.UserId = cUserId;
            record.ModifiedDate = DateTime.Now;

            var updateRecord = await Update(record);
            if (updateRecord.IsDeleted)
                return true;

        }

        return false;
    }
    #endregion

    #region GetAll

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
    {
        var data = _dbset.Where(predicate);
        return data;
    }
    public IEnumerable<TEntity> GetAllRaw(Expression<Func<TEntity, bool>>? predicate)
    {
        var data = _dbset.Where(predicate);
        return data;
    }



    public virtual IEnumerable<TEntity> GetAll() => _context.Set<TEntity>().OrderByDescending(x => x.CreatedDate);
    public IEnumerable<TEntity> GetAllDeleted(Expression<Func<TEntity, bool>> predicate)
    {
        var data = _dbset.Where(x => x.IsDeleted && x.IsActive).Where(predicate);
        return data;
    }
    #endregion

    #region Get

    public async Task<TEntity?> GetById(Guid id)
    {
        var obj = await _dbset.FindAsync(id);
        return obj;
    }

    #endregion

    #region Save
    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0;
    }


    #endregion

    #region Update
    public async Task<TEntity> Update(TEntity entity)
    {
        entity.ModifiedDate = DateTime.Now;
        var savedObj = _context.Set<TEntity>().Update(entity);
        await Save();
        return savedObj.Entity;
    }


    #endregion

    #region Execute

    public async Task<bool> ExecuteQuery(string query)
    {
        var exec = await _context.Database.ExecuteSqlRawAsync(query);
        return exec > 0;
    }



    #endregion

    #region Exist

    public async Task<bool> Exist(Guid id)
    {
        var result = await _dbset.AnyAsync(x => x.Id == id);
        return result;
    }

    #endregion
}
