using Core.MVP;

namespace ViewInterfaces
{
    public interface IUgolkiRulesListView : IView
    {
        void AddItem(IUgolkiRulesListItemView item);
    }
}