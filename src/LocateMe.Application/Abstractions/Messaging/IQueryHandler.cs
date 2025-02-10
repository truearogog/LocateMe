using LocateMe.Core;
using MediatR;

namespace LocateMe.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> 
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
