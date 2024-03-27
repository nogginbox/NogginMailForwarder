
using Nogginbox.MailForwarder.Server.MailRules;

namespace Nogginbox.MailForwarder.Server.Configuration;

public readonly record struct RuleConfiguration(string Alias, string? Address, string? Type = null)
{
    public string Alias { get; init; } = Alias;

    public string? Address { get; init; } = Address;

    public string Type { get; init; } = WorkoutType(Address, Type);

    private static string WorkoutType(string? address, string? type)
    {
        type = type?.ToLower();
        return type switch
        {
            ForwardRule.RuleType => type,
            BlockRule.RuleType => type,
            _ => address != null ? ForwardRule.RuleType : BlockRule.RuleType,
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IMessageRule CreateRule()
    {
        return Type switch
        {
            ForwardRule.RuleType => new ForwardRule(Alias, Address),
            BlockRule.RuleType => new BlockRule(Alias),
            _ => throw new NotImplementedException(),
        };
    }
}
