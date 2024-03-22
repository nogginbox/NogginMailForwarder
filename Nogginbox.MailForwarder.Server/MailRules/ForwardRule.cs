using MimeKit;

namespace Nogginbox.MailForwarder.Server.MailRules;

public record ForwardRule(string EmailPattern, MailboxAddress ForwardAddress) : AliasMessageRuleBase(EmailPattern), IMessageRule
{
    public ForwardRule(string aliasPattern, string forwardAddress)
        : this(aliasPattern, new MailboxAddress(forwardAddress, forwardAddress)) { }

    public bool IsMatch(string address) => IsValid() && _aliasRegex.IsMatch(address);

    public override bool IsValid() => base.IsValid() && ForwardAddress != null; 
}
