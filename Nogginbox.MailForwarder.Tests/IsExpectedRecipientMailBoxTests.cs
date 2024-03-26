using DnsClient;
using MimeKit;
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
    const string Tld = "nogginbox.co.uk";

    [Fact]
    public async Task NoRulesWillNotDeliverTest()
    {
        // Arrange
        var sessionContext = Substitute.For<ISessionContext>();
        var log = Substitute.For<Logging.ILogger>();
        
        var rules = new List<IMessageRule>();
        var filter = new IsExpectedRecipientMailboxFilter(rules, log);

        var to = new Mailbox($"code@null.{Tld}");
        var from = new Mailbox($"do-not-reply@null.{Tld}");

        // Act
        var result = await filter.CanDeliverToAsync(sessionContext, to, from, CancellationToken.None);

        // Assert
        Assert.Equal(MailboxFilterResult.NoPermanently, result);
    }

    [Theory]
    [InlineData($"code@null1.{Tld}")]
    [InlineData($"richard.g@null2.{Tld}")]
    public async Task AddressesMatchingForwardRulesWillDeliverTest(string email)
    {
        // Arrange
        var sessionContext = Substitute.For<ISessionContext>();
        var log = Substitute.For<Logging.ILogger>();

        var forwardRecipient = new MailboxAddress("Tester 1", $"tester1@null.{Tld}");
        var rules = new List<IMessageRule>
        {
            new ForwardRule($"*@null1.{Tld}", forwardRecipient),
            new ForwardRule($"richard.*@null2.{Tld}", forwardRecipient)
        };
        var filter = new IsExpectedRecipientMailboxFilter(rules, log);

        var to = new Mailbox(email);
        var from = new Mailbox($"do-not-reply@null.{Tld}");

        // Act
        var result = await filter.CanDeliverToAsync(sessionContext, to, from, CancellationToken.None);

        // Assert
        Assert.Equal(MailboxFilterResult.Yes, result);
    }

    [Theory]
    [InlineData($"richard.spam@null.{Tld}")]
    [InlineData($"richard.leaked@null.{Tld}")]
    public async Task AddressesMatchingBlockRulesWillNotDeliverTest(string email)
    {
        // Arrange
        var sessionContext = Substitute.For<ISessionContext>();
        var log = Substitute.For<Logging.ILogger>();

        var forwardRecipient = new MailboxAddress("Tester 1", $"tester1@null.{Tld}");
        var rules = new List<IMessageRule>
        {
            new ForwardRule($"*@null.{Tld}", forwardRecipient),
            new BlockRule($"richard.*@null.{Tld}")
        };
        var filter = new IsExpectedRecipientMailboxFilter(rules, log);

        var to = new Mailbox(email);
        var from = new Mailbox($"do-not-reply@null.{Tld}");

        // Act
        var result = await filter.CanDeliverToAsync(sessionContext, to, from, CancellationToken.None);

        // Assert
        Assert.Equal(MailboxFilterResult.NoPermanently, result);
    }

    [Theory]
    [InlineData($"rich.spam@null.{Tld}")]
    [InlineData($"rich.leaked@null.{Tld}")]
    public async Task AddressesWithMatchingForwardRulesNotMatchingBlockWillDeliverTest(string email)
    {
        // Arrange
        var sessionContext = Substitute.For<ISessionContext>();
        var log = Substitute.For<Logging.ILogger>();

        var forwardRecipient = new MailboxAddress("Tester 1", $"tester1@null.{Tld}");
        var rules = new List<IMessageRule>
    {
        new ForwardRule($"*@null.{Tld}", forwardRecipient),
        new BlockRule($"richard.*@null.{Tld}")
    };
        var filter = new IsExpectedRecipientMailboxFilter(rules, log);

        var to = new Mailbox(email);
        var from = new Mailbox($"do-not-reply@null.{Tld}");

        // Act
        var result = await filter.CanDeliverToAsync(sessionContext, to, from, CancellationToken.None);

        // Assert
        Assert.Equal(MailboxFilterResult.Yes, result);
    }
}
