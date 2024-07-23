using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Application
{
    public abstract class ApplicationBase<TEntity> where TEntity : EntityBase
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IValidatorService<TEntity> _validatorService;

        public ApplicationBase(IRepository<TEntity> repository,
                               IValidatorService<TEntity> validatorService
                              )
        {
            _repository = repository;
            _validatorService = validatorService;
        }

        public virtual async Task<TEntity> Create(TEntity model)
        {
            if (!await _validatorService.IsValidAsync(ValidatorType.Create, model))
                return Activator.CreateInstance<TEntity>();

            model.Id = Convert.ToInt32(await _repository.Insert(model));

            return model;
        }

        public virtual async Task<bool> Update(int id, TEntity model)
        {
            if (!await _validatorService.IsValidAsync(ValidatorType.Update, model) &&
                !await _validatorService.IsValidValue(nameof(id), id, (x) => x is int intValue && intValue > 0)
               )
                return false;

            var entity = await _repository.GetById(id);
            if (entity.Id == 0)
                return false;

            entity = model;
            model.Id = entity.Id;

            return await _repository.Update(model);
        }

        public virtual async Task<bool> Delete(int id)
        {
            if (!await _validatorService.IsValidValue(nameof(id), id, (x) => x is int intValue && intValue > 0))
                return false;

            var entity = await _repository.GetById(id);
            if (entity.Id == 0)
                return false;

            return await _repository.Delete(entity);
        }

        public virtual async Task<TEntity> Get(int id)
        {
            if (!await _validatorService.IsValidValue(nameof(id), id, (x) => x is int intValue && intValue > 0))
                return Activator.CreateInstance<TEntity>();

            return await _repository.GetById(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _repository.GetAll();
        }
    }
}
