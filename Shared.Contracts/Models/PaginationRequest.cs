namespace Shared.Contracts.Models;

public record PaginationRequest(int PageIndex, int PageSize = 10);