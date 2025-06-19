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
    Task<IEnumerable<CategoryDto>> GetByNum(int number);
    Task<string> CreateCategory(CreateCategoryDto dto);
    Task<string> UpdateByNum(CategoryDto dto);
    Task<bool> DeleteByNum(string number);
    
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
    public async Task<IEnumerable<CategoryDto>> GetByNum(int number)
    {
        var categories = await _categoryRepo.GetAllAsync<Category>(
            _queryProvider.GetByNum,
            new { CategoryNumber = number }
        );
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<string> CreateCategory(CreateCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        var newId = await _categoryRepo.InsertAsync<string>(category, _queryProvider.CreateSingle);
        return newId;
    }
    
    public async Task<string> UpdateByNum(CategoryDto dto)
    {
        var categoryToUpdate = _mapper.Map<Category>(dto);
        var updatedId = await _categoryRepo.UpdateAsync<string>(categoryToUpdate, _queryProvider.UpdateByNum);
        return updatedId;
    }

    public async Task<bool> DeleteByNum(string number)
    {
        var rows = await _categoryRepo.DeleteByIdAsync(_queryProvider.DeleteByNum, number);
        return rows > 0;
    }


}