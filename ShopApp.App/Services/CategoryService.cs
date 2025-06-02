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
}