using MimeKit;
using System.Text.RegularExpressions;

namespace Nogginbox.MailForwarder.Server.MailRules;

public record ForwardRule(string AliasPattern, MailboxAddress ForwardAddress) : IMessageRule
{
    public ForwardRule(string aliasPattern, string forwardAddress)
        : this(aliasPattern, new MailboxAddress(forwardAddress, forwardAddress)) { }


    private readonly Regex _aliasRegex = new(WildCardToRegular(AliasPattern), RegexOptions.IgnoreCase);

    public bool IsMatch(string address) => _aliasRegex.IsMatch(address);

    private static string WildCardToRegular(string value) => $"^{Regex.Escape(value).Replace("\\*", ".*")}$";
}
