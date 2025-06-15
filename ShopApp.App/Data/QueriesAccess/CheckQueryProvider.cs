namespace ShopApp.Data.QueriesAccess;

public class CheckQueryProvider: IQueryByNamespaceProvider
{
    private const string FolderName = "Check";
    private const string GetAllQuery = "get_all";
    private const string GetAllWithSalesByPeriodQuery = "get_all_with_sales_by_period";
    private const string GetAllWithSalesByPeriodAndEmployeeQuery = "get_all_by_empl_and_period_with_sales";
    private const string GetSumByPeriodQuery = "get_sum_by_period";
    private const string GetSumByPeriodAndEmployeeQuery = "get_sum_by_employee_period";
    private const string GetByNumQuery = "get_by_number";
    private const string GetByTodayQuery = "get_by_today";
    private const string GetSeqNextValQuery = "get_seq_nextval";
    
    private const string CreateSingleQuery = "create_single";
    private const string DeleteByNumQuery = "delete_by_number";
    

    public string GetAll => GetNamespace(GetAllQuery);
    public string GetAllWithSalesByPeriod => GetNamespace(GetAllWithSalesByPeriodQuery);
    public string GetAllWithSalesByPeriodAndEmployee => GetNamespace(GetAllWithSalesByPeriodAndEmployeeQuery);
    public string GetSumByPeriod => GetNamespace(GetSumByPeriodQuery);
    public string GetSumByPeriodAndEmployee => GetNamespace(GetSumByPeriodAndEmployeeQuery);
    public string CreateSingle => GetNamespace(CreateSingleQuery);
    public string DeleteByNum => GetNamespace(DeleteByNumQuery);
    public string GetByNum => GetNamespace(GetByNumQuery);
    public string GetByToday => GetNamespace(GetByTodayQuery);
    
    public string GetSeqNextVal => GetNamespace(GetSeqNextValQuery);


    
    public string GetNamespace(string fileName)
    {
        return $"{FolderName}.{fileName}.sql";
    }
}

