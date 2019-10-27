<%@ Page Language="C#"  AutoEventWireup="true" ClassName="CacheViewer" Inherits="Sitecore.sitecore.admin.AdminPage"  %>
<%@ Import Namespace="Sitecore.Web" %>
<%@ Import Namespace="Sitecore.Caching" %>

<script runat="server">

    public class Constants
    {
        public static readonly string CacheData = "cachedata";
        public static readonly string CacheDataSortDirection = "cachedatasortdirection";
        public static readonly string SiteSelectorFieldName = "Name";
        public static readonly string EmptyCacheText = "Cache is currently empty";
        public static readonly string ContentEditorUrl = "/sitecore/shell/Applications/Content Editor.aspx?fo=";
        public static readonly string RegExGuid = @"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?";
        public static readonly string OpenInEditorText = "Click to open this item in the Content Editor";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<SiteInfo> siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
            DropDownListCacheSelector.DataTextField = Constants.SiteSelectorFieldName;
            DropDownListCacheSelector.DataValueField = Constants.SiteSelectorFieldName;
            DropDownListCacheSelector.DataSource = siteInfoList;
            DropDownListCacheSelector.DataBind();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        CheckSecurity(true);
        base.OnInit(e);
    }
        
    protected void BindData()
    {
        WebsiteCacheGrid.DataSource = Sort(GetCacheData());
        WebsiteCacheGrid.DataBind();
    }

    private List<KeyValuePair<string, string>> Sort(Dictionary<string, string> cacheData)
    {
        var direction = ViewState[Constants.CacheDataSortDirection];
        if (direction != null && (SortDirection)direction == SortDirection.Ascending)
        {
            return cacheData.OrderByDescending(x => x.Key).ToList();
        }
        return cacheData.OrderBy(x => x.Key).ToList();
    }
    protected Dictionary<string, string> GetCacheData()
    {
        return GetCacheData(DropDownListCacheSelector.SelectedValue);
    }

    protected Dictionary<string, string> GetCacheData(string siteName)
    {
        string cacheDataString = siteName + Constants.CacheData;
        if (ViewState[cacheDataString] != null)
        {
            return (Dictionary<string, string>)ViewState[cacheDataString];
        }

        var cacheData = new Dictionary<string, string>();
        HtmlCache htmlCache = CacheManager.GetHtmlCache(Sitecore.Sites.SiteContextFactory.GetSiteContext(siteName));
        if (htmlCache != null && htmlCache.InnerCache.Count > 0)
        {
            foreach (var cacheKey in htmlCache.InnerCache.GetCacheKeys())
            {
                cacheData.Add(cacheKey, HttpUtility.HtmlEncode(htmlCache.InnerCache.GetValue(cacheKey)));
            }

            WebsiteCacheGrid.Columns[2].Visible = true;
        }
        else
        {
            WebsiteCacheGrid.Columns[2].Visible = false;
            cacheData.Add(Constants.EmptyCacheText, "-");
        }

        ViewState[cacheDataString] = cacheData;
        return cacheData;
    }

    protected string GetContentEditorItemUrl(string site, string guid)
    {
        return Constants.ContentEditorUrl + guid;
    }

    protected void DropDownListCacheSelector_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        PanelCache.Visible = true;
        PanelRefresh.Visible = true;
    }

    protected void WebsiteCacheGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string cacheKey = e.Row.Cells[0].Text;
            var match = Regex.Match(cacheKey, Constants.RegExGuid);
            if (match.Success)
            {
                string url = GetContentEditorItemUrl(DropDownListCacheSelector.SelectedValue, match.Value);
            
                e.Row.Cells[0].Text = Regex.Replace(
                    e.Row.Cells[0].Text,
                    Regex.Escape(match.Value),
                    m => String.Format("<a href=\"{0}\" title=\"{1}\" target=\"_blank\">{2}</a>",url, Constants.OpenInEditorText, match.Value));
            }

            HtmlButton btn = (HtmlButton)e.Row.FindControl("btnCopy");
            HtmlGenericControl span = (HtmlGenericControl) e.Row.FindControl("spanCacheContents");
            if (btn != null && span != null)
            {
                btn.Attributes.Add("onclick",String.Format("copyTextToClipboard('{0}'); return false;", span.ClientID));
            }
        }
    }

    protected void WebsiteCacheGrid_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        WebsiteCacheGrid.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void WebsiteCacheGrid_OnSorting(object sender, GridViewSortEventArgs e)
    {
        var direction = ViewState[Constants.CacheDataSortDirection];
        if (direction != null && (SortDirection) direction == SortDirection.Ascending)
        {
            ViewState[Constants.CacheDataSortDirection] = SortDirection.Descending;
        }
        else
        {
            ViewState[Constants.CacheDataSortDirection] = SortDirection.Ascending;
        }
        BindData();
    }

    private void ClearCaches()
    {
        ViewState[Constants.CacheDataSortDirection] = null;
        foreach (var item in DropDownListCacheSelector.Items)
        {
            ViewState[item + Constants.CacheData] = null;
        }
    }

    protected void LinkRefresh_OnClick(object sender, EventArgs e)
    {
        ClearCaches();
        BindData();
    }

</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HTML Cache Viewer</title>
    <link rel="Stylesheet" type="text/css" href="/sitecore/shell/themes/standard/default/WebFramework.css" />
    <style type="text/css">
        .wf-container {
            width: 90%;
            margin-top: 10px;
            overflow: hidden;
        }

        .cache-panel table {
            table-layout:fixed;
            width:100%;
        }

        .cache-panel td {
            word-wrap: break-word;
            padding: 5px;
            color: #333333;
        }

        .cache-panel th {
            padding: 10px;
        }

        .grid-header {
            background-color: #8CACCA;
        }
        
        .grid-odd-row {
            background-color: #D3D3D3;
        }

        .grid-even-row {
            background-color: #ffffff;
        }

        .configuration span {
            padding: 10px;
        }

        .cache-panel {
            margin-top: 10px;
        }

        .refresh-panel {
            margin-top: 10px;
        }

        .grid-pager {
            padding-left: 5px;
            text-align: right;
            display: block;
        }

        h1 {
            margin: 10px;
        }

        form {
            height: 100%;
        }

        #wf-dropshadow-right
        {
            position: absolute;
            margin-left: 90%;
        }
    </style>
    <script>
        function fallbackCopyTextToClipboard(text) {
            var textArea = document.createElement("textarea");
            textArea.value = text;
            document.body.appendChild(textArea);
            textArea.style.position = 'fixed';
            textArea.style.bottom= 0;
            textArea.style.left= 0;
            textArea.focus({ preventScroll: true});
            textArea.select();
            try {
                document.execCommand('copy');
            } catch (e) {
                alert("Unable to copy to clipboard, but the text can be manually copied.");
            }

            document.body.removeChild(textArea);
        }

        function copyTextToClipboard(element) {
            var el = document.getElementById(element);
            if (!el) {
                return;
            }
            if (!navigator.clipboard) {
                fallbackCopyTextToClipboard(el.innerText);
                return;
            }
            navigator.clipboard.writeText(el.innerText).catch(() => {
                alert("Unable to copy to clipboard, but the text can be manually copied.");
            });
        }
        
    </script>
</head>
<body style="min-height: 100%;">
    <form id="formCaches" runat="server">
        <div class="wf-container">
            <div id="wf-dropshadow-left"></div>
            <div id="wf-dropshadow-right"></div>
            <div class="configuration">
                <h1><a href="/sitecore/admin/">Administration Tools</a> - HTML Cache Viewer</h1>
                <span>Select site to view HTML cache: </span>
                <asp:DropDownList ID="DropDownListCacheSelector" runat="server" OnSelectedIndexChanged="DropDownListCacheSelector_OnSelectedIndexChanged" AutoPostBack="true"/>
                <asp:Panel ID="PanelRefresh" runat="server" Visible="False" CssClass="refresh-panel">
                    <span>Data is cached on first load, click <asp:LinkButton runat="server" Text="Refresh" OnClick="LinkRefresh_OnClick" /> to reload from Sitecore</span>
                </asp:Panel>
            </div>
        
            <asp:Panel ID="PanelCache" runat="server" CssClass="cache-panel" Visible="False">
                <asp:GridView ID="WebsiteCacheGrid" Width="100%" runat="server" PageSize="100" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" OnRowDataBound="WebsiteCacheGrid_OnRowDataBound" OnPageIndexChanging="WebsiteCacheGrid_OnPageIndexChanging" OnSorting="WebsiteCacheGrid_OnSorting">  
                    <RowStyle CssClass="grid-odd-row" />
                    <HeaderStyle CssClass="grid-header" />
                    <AlternatingRowStyle CssClass="grid-even-row" />
                    <PagerStyle CssClass="grid-pager" />
                    <Columns>
                        <asp:BoundField DataField="Key" HeaderText="Cache Key" HeaderStyle-Width="30%" SortExpression="Key" ItemStyle-Wrap="True"  />
                        <asp:TemplateField HeaderText="Cache contents" HeaderStyle-Width="62%" ItemStyle-Wrap="True" >
                            <ItemTemplate>
                                <span id="spanCacheContents" runat="server"><%# Eval("Value") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Copy contents" ItemStyle-Wrap="True">
                            <ItemTemplate>
                                <button id="btnCopy" runat="server">Copy</button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
