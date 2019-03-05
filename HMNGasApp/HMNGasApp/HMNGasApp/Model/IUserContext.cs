namespace HMNGasApp.Model
{
    public interface IUserContext
    {
        string Caller { get; set; }
        string Company { get; set; }
        string functionName { get; set; }
        int Logg { get; set; }
        int MaxRows { get; set; }
        string securityKey { get; set; }
        int StartRows { get; set; }
    }
}