using FluentValidation;
using Web3raffle.Models.Requests;

namespace Web3raffle.Api.RequestValidators;

public class Web3RaffleRequestModelValidator : Validator<Web3RaffleRequestModel>
{
	public Web3RaffleRequestModelValidator()
	{
		this.RuleFor(x => x.ProjectId)
			.NotEmpty()
			.WithMessage("ProjectId is required!");
		this.RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Name is required!");
	}
}