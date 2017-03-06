namespace DU.Themes.Models
{
    public interface ISortable
    {
        int Take { get; set; }
        int Skip { get; set; }
        int Total { get; set; }
    }
}