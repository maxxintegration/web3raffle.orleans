using Microsoft.AspNetCore.Components;

namespace Web3raffle.Web.Client.Shared;

public partial class Pagination : ComponentBase
{

	[Parameter]
	public EventCallback<string> OnPaging { get; set; }

	[Parameter]
	public int PageSize { get; set; } = 50;

	[Parameter]
	public int? Length { get; set; } = 0;

	public int PageNumber { get; set; } = 1;

	public int PageTotal { get; set; } = 1;

	public int Skip
	{
		get
		{
			this._skip = this.PageNumber > 0 ? (this.PageNumber > this.PageTotal ? this.PageTotal : this.PageNumber) * this.PageSize - this.PageSize : 0;
			this._skip = this._skip < 0 ? 0 : this._skip;

			return this._skip;
		}
	}

	public int Top
	{
		get
		{

			this._top = this.PageNumber >= this.PageTotal ? this.Length!.Value - this._skip : this.PageSize;
			this._top = this._top == 0 ? this.PageSize : this._top;

			return this._top;
		}
	}

	int _skip = 0;
	int _top = 0;
	bool _disabledPrevious = true;
	bool _disabledNext = false;

	public void SetLength(int length)
	{
		this.Length = length;
		this.PageTotal = (int)Math.Ceiling((double)this.Length / this.PageSize);
	}

	public async Task GoToPage(int? pageNumber = 1)
	{
		this.PageNumber = pageNumber!.Value;
		this.PageNumber = this.PageNumber > this.PageTotal ? this.PageTotal : this.PageNumber < 1 ? 1 : this.PageNumber;

		this._disabledPrevious = this.PageNumber <= 1;
		this._disabledNext = this.PageNumber == this.PageTotal;

		if (this.OnPaging.HasDelegate)
			await this.OnPaging.InvokeAsync();
	}



}