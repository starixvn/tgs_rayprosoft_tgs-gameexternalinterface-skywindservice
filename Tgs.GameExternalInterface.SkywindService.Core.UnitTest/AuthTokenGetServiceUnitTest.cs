using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameToken.Client;
using Tgs.GameExternalInterface.SkywindService.Core.ApplicationServices;

namespace Tgs.GameExternalInterface.SkywindService.Core.UnitTest;

[TestClass]
public class AuthTokenGetServiceUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var fakeLogger = NullLogger<ValidateTicketGetService>.Instance;
		var fakeGameTokenClient = Substitute.For<IGameTokenClient>();

		var query = new ValidateTicketQuery(
			Ticket: Guid.NewGuid().ToString("N"));

		var authTokenInfo = new ValidateTicketInfo()
		{
			PlayerCode = "test",
			CurrencyCode = "CNY",
			TestCust = false,
			Country = "CN"
		};

		var gameAuthInfo = new GameAuthInfo()
		{
			Id = query.Ticket,
			PlayerCode = "test",
			Info = JsonSerializer.Serialize(authTokenInfo)
		};

		_ = fakeGameTokenClient
				.GetAuthInfoAsync(
					Arg.Is(query.Ticket),
					cancellationToken: default)
				.Returns(gameAuthInfo);

		var sessionToken = Guid.NewGuid().ToString("N");
		_ = fakeGameTokenClient
				.CreateTokenAsync(
					playerCode: Arg.Is(gameAuthInfo.PlayerCode),
					cancellationToken: default)
				.Returns(sessionToken);

		

		var service = new ValidateTicketGetService(
			fakeLogger,
			fakeGameTokenClient);

		var actual = await service.GetAsync(query).ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(sessionToken, actual.CustSessionId);
	}
}
