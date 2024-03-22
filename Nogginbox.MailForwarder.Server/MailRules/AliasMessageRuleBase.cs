using System.Text.RegularExpressions;

namespace Nogginbox.MailForwarder.Server.MailRules;

public abstract record AliasMessageRuleBase(string EmailPattern)
{
    protected readonly Regex _aliasRegex = new(WildCardToRegular(EmailPattern), RegexOptions.IgnoreCase);

    protected static string WildCardToRegular(string value) => $"^{Regex.Escape(value).Replace("\\*", ".*")}$";

    public virtual bool IsValid() => !string.IsNullOrEmpty(EmailPattern);
}