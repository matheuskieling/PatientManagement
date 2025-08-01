namespace FicharioDigital.Model.DTO;

public class PageableResponseDto<T>
{
    public int Page  { get; set; }
    public int PageSize  { get; set; }
    public int TotalResults  { get; set; }
    public int TotalPages  { get; set; }
    public List<T> Items { get; set; } = [];
}