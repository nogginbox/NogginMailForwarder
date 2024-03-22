using MimeKit;

namespace Nogginbox.MailForwarder.Server.MailRules;

public interface IMessageRule
{
    /// <summary>
    /// Does address match this rule.
    /// </summary>
    /// <param name="address">The address to try toi match.</param>
    /// <returns></returns>
    bool IsMatch(string address);

    MailboxAddress? ForwardAddress { get; }
}