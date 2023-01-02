namespace ForumApi.BusinessLogic.DTOs;

public class PostEntityDto
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    
    public string OwnerNickname { get; set; }
    
    public string Text { get; set; }
    
    public DateTime DateOfCreate { get; set; }
    
    public DateTime? DateOfUpdate { get; set; }
}