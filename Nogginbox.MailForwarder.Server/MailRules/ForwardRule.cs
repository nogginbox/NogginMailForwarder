using MimeKit;

namespace Nogginbox.MailForwarder.Server.MailRules;

public record ForwardRule(string AliasPattern, MailboxAddress ForwardAddress) : AliasMessageRuleBase(AliasPattern), IMessageRule
{
    public ForwardRule(string aliasPattern, string forwardAddress)
        : this(aliasPattern, new MailboxAddress(forwardAddress, forwardAddress)) { }

    public const string RuleType = "forward";

    public bool IsMatch(string address) => IsValid() && _aliasRegex.IsMatch(address);

    public override bool IsValid() => base.IsValid() && ForwardAddress != null;
    
    public override string ToString() => $"Forward: {AliasPattern} => {ForwardAddress}";
}
