using LocateMe.Core;
using MediatR;

namespace LocateMe.Application.Abstractions.Messaging;

public interface IQuery<TResponse> 
    : IRequest<Result<TResponse>>;
