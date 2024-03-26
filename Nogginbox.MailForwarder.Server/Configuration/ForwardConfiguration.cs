namespace Nogginbox.MailForwarder.Server.Configuration;

public class ForwardConfiguration
{
	public RuleConfiguration[] Rules { get; set; } = [];

	public string ServerName { get; set; } = "localhost";
}
