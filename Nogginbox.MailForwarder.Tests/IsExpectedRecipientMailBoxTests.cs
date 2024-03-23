using DnsClient;
using Nogginbox.MailForwarder.Server.MailboxFilters;
using Nogginbox.MailForwarder.Server.MailRules;
using NSubstitute;
using SmtpServer;
using SmtpServer.Mail;
using SmtpServer.Storage;
using Logging = Microsoft.Extensions.Logging;

namespace Nogginbox.MailForwarder.Tests;

public class IsExpectedRecipientMailBoxTests
{
    [Fact]
    public async Task NoRulesWillNotDeliverTest()
    {
        // Arrange
        var sessionContext = Substitute.For<ISessionContext>();
        var log = Substitute.For<Logging.ILogger>();
        
        var rules = new List<IMessageRule>();
        var filter = new IsExpectedRecipientMailboxFilter(rules, log);

        var to = new Mailbox("code@nogginbox.co.uk");
        var from = new Mailbox("do-not-reply@nogginbox.co.uk");

        // Act
        var result = await filter.CanDeliverToAsync(sessionContext, to, from, CancellationToken.None);

        // Assert
        Assert.Equal(MailboxFilterResult.NoPermanently, result);
    }
}
