using AutoMapper;
using ShopApp.Data.DTO;

namespace ShopApp.MappingProfiles;

public class CheckProfile : Profile
{
    public CheckProfile()
    {
        CreateMap<IEnumerable<CheckWithSaleDto>, IEnumerable<CheckWithSalesListDto>>().ConvertUsing<FlatToGroupedChecksConverter>();
    }
}


public class FlatToGroupedChecksConverter : ITypeConverter<IEnumerable<CheckWithSaleDto>, IEnumerable<CheckWithSalesListDto>>
{
    public IEnumerable<CheckWithSalesListDto> Convert(IEnumerable<CheckWithSaleDto> source, IEnumerable<CheckWithSalesListDto> destination, ResolutionContext context)
    {
        return source
            .GroupBy(x => x.CheckNumber)
            .Select(group =>
            {
                var firstCheck = group.First();

                return new CheckWithSalesListDto
                {
                    CheckNumber = firstCheck.CheckNumber,
                    IdEmployee = firstCheck.IdEmployee,
                    CardNumber = firstCheck.CardNumber,
                    PrintDate = firstCheck.PrintDate,
                    SumTotal = firstCheck.SumTotal,
                    VAT = firstCheck.VAT,
                    Sales = group.Select(sale => new SaleDto
                    {
                        UPC = sale.UPC,
                        ProductName = sale.ProductName,
                        ProductNumber = sale.ProductNumber,
                        SellingPrice = sale.SellingPrice,
                        CheckNumber = firstCheck.CheckNumber
                    }).ToList()
                };
            })
            .ToList();
    }
}