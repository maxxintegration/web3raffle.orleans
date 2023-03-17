using FluentValidation;
using Web3raffle.Models.Requests;

namespace Web3raffle.Api.RequestValidators;

public class Web3RaffleProjectRequestModelValidator : Validator<Web3RaffleProjectRequestModel>
{
	public Web3RaffleProjectRequestModelValidator()
	{
		this.RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("'Name' is required!");
	}
}