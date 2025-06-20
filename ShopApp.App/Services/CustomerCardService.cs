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
    Task<string> CreateCustomerCard(CreateCustomerCardDto dto);
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
    
    public async Task<string> CreateCustomerCard(CreateCustomerCardDto dto)
    {
        var number = await _cardRepo.GetNextPrefixedStringId("CARD", _queryProvider.GetSeqNextVal);
        var cardToCreate = _mapper.Map<CustomerCard>(dto);
        cardToCreate.CardNumber = number;
        var createdEntityId = await _cardRepo.InsertAsync<string>(cardToCreate, _queryProvider.CreateSingle);
        return createdEntityId;
    }
    
    public async Task<string> UpdateByNum(CustomerCardDto dto)
    {
        var cardToUpdate = _mapper.Map<CustomerCard>(dto);
        var updatedNum = await _cardRepo.UpdateAsync<string>(cardToUpdate, _queryProvider.UpdateByCardNum);
        return updatedNum;
    }

    public async Task<bool> DeleteByNum(string number)
    {
        var param = new { CardNumber = number };
        var existingEntity = await _cardRepo.GetSingleAsync(_queryProvider.GetByCardNum, param);

        if (existingEntity == null)
            throw new ArgumentException($"No customer card with number {number} was found.");

        var rows = await _cardRepo.DeleteAsync(_queryProvider.DeleteByCardNum, param);

        return rows > 0;
    }

}