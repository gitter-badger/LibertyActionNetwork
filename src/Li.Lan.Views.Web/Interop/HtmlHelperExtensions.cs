using Newtonsoft.Json;
using System.Web.Mvc;

public static class HtmlHelperExtensions
{
    public static MvcHtmlString Json<TModel, TObject>(this HtmlHelper<TModel> html, TObject obj)
    {
        return MvcHtmlString.Create(JsonConvert.SerializeObject(obj));
    }
    
    public static SelectList MakeSelection(this SelectList list, object selection)
    {
        return new SelectList(list.Items, list.DataValueField, list.DataTextField, selection);
    }
}