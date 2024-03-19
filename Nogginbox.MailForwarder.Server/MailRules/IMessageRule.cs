namespace Nogginbox.MailForwarder.Server.MailRules;

public interface IMessageRule
{
    bool IsMatch(string address);
}