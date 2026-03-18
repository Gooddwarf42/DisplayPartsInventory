namespace WF.Cqrs.Crud.Extensions;

public static class CqrsContextExtensions
{
    public static CqrsContext AddCrud(this CqrsContext source) => source.AddAssembly(typeof(CqrsContextExtensions));
}