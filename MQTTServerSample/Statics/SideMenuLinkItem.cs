namespace MQTTServerSample.Statics;

public class SideMenuLinkItem
{
    public SideMenuLinkItem(int id, string link, string title, string linkIcon, bool permission, int parentId)
    {
        Id = id;
        Link = link;
        Title = title;
        LinkIcon = linkIcon;
        Permission = permission;
        ParentId = parentId;
    }
    public bool Permission { get; set; }
    public string Link { get; set; }
    public string Title { get; set; }
    public string LinkIcon { get; set; }
    public int Id { get; set; }
    public int ParentId { get; set; }
}