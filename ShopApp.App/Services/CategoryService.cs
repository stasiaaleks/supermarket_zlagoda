using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAll();
    Task<IEnumerable<CategoryDto>> GetAllByNum(int number);
    Task<int> CreateCategory(CreateCategoryDto dto);
    Task<int> UpdateByNum(CategoryDto dto);
    Task<bool> DeleteByNum(int number);
    
    Task<IEnumerable<CategoryDto>> Filter(CategorySearchCriteria criteria);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepo;
    private readonly CategoryQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public CategoryService(CategoryQueryProvider queryProvider, IMapper mapper, IRepository<Category> categoryRepo)
    {
        _queryProvider = queryProvider;
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }
    
    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        return await _categoryRepo.GetAllAsync<CategoryDto>(_queryProvider.GetAll);
    }
    

    public async Task<IEnumerable<CategoryDto>> Filter(CategorySearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var categories = await _categoryRepo.FilterByPredicateAsync<CategoryDto>(query, criteria);
        return categories;
    }
    public async Task<IEnumerable<CategoryDto>> GetAllByNum(int number)
    {
        var categories = await _categoryRepo.GetAllAsync<Category>(
            _queryProvider.GetByNum,
            new { CategoryNumber = number }
        );
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<int> CreateCategory(CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        var newId = await _categoryRepo.InsertAsync<int>(category, _queryProvider.CreateSingle);
        return newId;
    }
    
    public async Task<int> UpdateByNum(CategoryDto dto)
    {
        var categoryToUpdate = _mapper.Map<Category>(dto);
        var updatedId = await _categoryRepo.UpdateAsync<int>(categoryToUpdate, _queryProvider.UpdateByNum);
        return updatedId;
    }

    public async Task<bool> DeleteByNum(int number)
    {
        var numberParams = new { CategoryNumber = number };
        var existingEntity = await _categoryRepo.GetSingleAsync(_queryProvider.GetByNum, numberParams);
        if (existingEntity == null) throw new ArgumentException($"No category with number {number} was found.");
        
        var rows = await _categoryRepo.DeleteAsync(_queryProvider.DeleteByNum, new { CategoryNumber = number});
        return rows > 0;
    }


}