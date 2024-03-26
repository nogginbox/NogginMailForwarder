namespace Nogginbox.MailForwarder.Server.MailRules;

public static class IMessageRuleExtensions
{
    public static IEnumerable<IMessageRule> OrderByRuleType(this IEnumerable<IMessageRule> rules)
    {
        return rules.OrderBy(r => r.ForwardAddress != null);
    }
}