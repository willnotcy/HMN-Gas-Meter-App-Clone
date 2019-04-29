namespace HMNGasApp.WebServices
{
    public interface IUserContext
    {
        string Caller { get; set; }
        string Company { get; set; }
#pragma warning disable IDE1006 // Naming Styles
        string functionName { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        int Logg { get; set; }
        int MaxRows { get; set; }
#pragma warning disable IDE1006 // Naming Styles
        string securityKey { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        int StartRow { get; set; }
    }
}