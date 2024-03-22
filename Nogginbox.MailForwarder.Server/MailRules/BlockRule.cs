using MimeKit;

namespace Nogginbox.MailForwarder.Server.MailRules;

public record BlockRule(string AliasPattern) : AliasMessageRuleBase(AliasPattern), IMessageRule
{
    public MailboxAddress? ForwardAddress => null;

    public bool IsMatch(string address) => IsValid() && _aliasRegex.IsMatch(address);
}
