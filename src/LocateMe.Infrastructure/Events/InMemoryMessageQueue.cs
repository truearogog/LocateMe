using System.Threading.Channels;
using LocateMe.Core;

namespace LocateMe.Infrastructure.Events;

internal sealed class InMemoryMessageQueue
{
    private readonly Channel<IDomainEvent> _channel = Channel.CreateUnbounded<IDomainEvent>();

    public ChannelWriter<IDomainEvent> Writer => _channel.Writer;
    public ChannelReader<IDomainEvent> Reader => _channel.Reader;
}
