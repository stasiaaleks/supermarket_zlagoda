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
    Task<string> CreateCustomerCard(CustomerCardDto dto);
    Task<string> UpdateByNum(CustomerCardDto dto);
    Task<bool> DeleteByNum(string number);
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
    
    public async Task<string> CreateCustomerCard(CustomerCardDto dto)
    {
        var card = _mapper.Map<CustomerCard>(dto);
        var newNum = await _cardRepo.InsertAsync<string>(card, _queryProvider.CreateSingle);
        return newNum;
    }
    
    public async Task<string> UpdateByNum(CustomerCardDto dto)
    {
        var cardToUpdate = _mapper.Map<CustomerCard>(dto);
        var updatedNum = await _cardRepo.UpdateAsync<string>(cardToUpdate, _queryProvider.UpdateByCardNum);
        return updatedNum;
    }

    public async Task<bool> DeleteByNum(string number)
    {
        var rows = await _cardRepo.DeleteByIdAsync(_queryProvider.DeleteByCardNum, number);
        return rows > 0;
    }
}