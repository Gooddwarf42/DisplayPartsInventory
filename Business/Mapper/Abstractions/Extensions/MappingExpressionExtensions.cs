using System.Linq.Expressions;
using AutoMapper;

namespace Business.Mapper.Abstractions.Extensions;

public static class MappingExpressionExtensions
{
    public static IMappingExpression<T1, T2> Bind<T1, T2, TMember>(this IMappingExpression<T1, T2> source, Expression<Func<T2, TMember>> destinationMember, Expression<Func<T1, TMember>> mapExpression)
        => source.ForMember
            (
                destinationMember,
                options => options.MapFrom(mapExpression)
            );
}
