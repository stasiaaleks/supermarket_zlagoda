using AutoMapper;
using ShopApp.DAL.Repository;
using ShopApp.Data.DTO;
using ShopApp.Data.Entities;
using ShopApp.Data.QueriesAccess;
using ShopApp.Data.SearchCriteria;

namespace ShopApp.Services;

public interface ICustomerCardService
{
    Task<IEnumerable<CustomerCardDto>> GetAll();
    Task<IEnumerable<CustomerCardDto>> Filter(CustomerCardSearchCriteria criteria);
}

public class CustomerCardService : ICustomerCardService
{
    private readonly IRepository<CustomerCard> _cardRepo;
    private readonly CustomerCardQueryProvider _queryProvider;
    private readonly IMapper _mapper;

    public CustomerCardService(CustomerCardQueryProvider queryProvider, IMapper mapper, IRepository<CustomerCard> cardRepo)
    {
        _queryProvider = queryProvider;
        _mapper = mapper;
        _cardRepo = cardRepo;
    }
    
    public async Task<IEnumerable<CustomerCardDto>> GetAll()
    {
        return await _cardRepo.GetAllAsync<CustomerCardDto>(_queryProvider.GetAll);
    }

    public async Task<IEnumerable<CustomerCardDto>> Filter(CustomerCardSearchCriteria criteria)
    {
        var query = _queryProvider.GetAll;
        var cards = await _cardRepo.FilterByPredicateAsync<CustomerCardDto>(query, criteria);
        return cards;
    }
}