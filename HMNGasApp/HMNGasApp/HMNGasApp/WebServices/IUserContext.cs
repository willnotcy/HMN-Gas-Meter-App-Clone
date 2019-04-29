namespace HMNGasApp.WebServices
{
    public interface IUserContext
    {
        string Caller { get; set; }
        string Company { get; set; }
        string FunctionName { get; set; }
        int Logg { get; set; }
        int MaxRows { get; set; }
        string SecurityKey { get; set; }
        int StartRow { get; set; }
    }
}