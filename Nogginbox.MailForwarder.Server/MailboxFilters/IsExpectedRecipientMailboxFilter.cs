using Microsoft.Extensions.Logging;
using Nogginbox.MailForwarder.Server.MailRules;
using SmtpServer;
using SmtpServer.Mail;
using SmtpServer.Storage;
using Logging = Microsoft.Extensions.Logging;

namespace Nogginbox.MailForwarder.Server.MailboxFilters;

/// <summary>
/// Checks to see if the intended recipient of this email is in the configured forward rules.
/// </summary>
public class IsExpectedRecipientMailboxFilter : IMailboxFilter
{
	private readonly IReadOnlyList<IMessageRule> _rules;
	private readonly Logging.ILogger _log;

	public IsExpectedRecipientMailboxFilter(IReadOnlyList<IMessageRule> rules, Logging.ILogger log)
	{
		_rules = rules;
		_log = log;
	}

	public Task<MailboxFilterResult> CanAcceptFromAsync(ISessionContext context, IMailbox @from, int size, CancellationToken cancellationToken)
	{
		return Task.FromResult(MailboxFilterResult.Yes);
	}

	public Task<MailboxFilterResult> CanDeliverToAsync(ISessionContext context, IMailbox to, IMailbox @from, CancellationToken token)
	{
		if (!_rules.Any(r => r.IsMatch(to.AsAddress())))
		{
			_log.LogWarning("Filter by recipient - NO, can not deliver to {recipient}", to.AsAddress());
			return Task.FromResult(MailboxFilterResult.NoPermanently);
		}

		_log.LogInformation("Filter by recipient - YES, can deliver to {recipient}", to.AsAddress());
		return Task.FromResult(MailboxFilterResult.Yes);
	}
}
