using System.Collections.Generic;
using Tools.Service.Model;

namespace Tools.Service.Interface
{
    public interface IContentService
    {
        IList<WidgetContentViewModel> GetTopContent();
        IList<WidgetContentViewModel> GetTopContent(string categoryUrl);
    }
}