namespace DataLayer.Base.Response;

public class BaseResponse
{
    public long id { get; set; }
	public string name { get; set; }
	public string slug { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}