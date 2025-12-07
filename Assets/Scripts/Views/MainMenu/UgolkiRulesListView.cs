using UnityEngine;
using ViewInterfaces;

namespace Views.MainMenu
{
    public class UgolkiRulesListView : BaseView, IUgolkiRulesListView
    {
        [SerializeField]
        private RectTransform _content;

        public void AddItem(IUgolkiRulesListItemView item)
        {
            if (item is not BaseView itemView)
            {
                return;
            }

            Transform itemTransform = itemView.transform;
            itemTransform.SetParent(_content);
            itemTransform.localScale = Vector3.one;
            itemTransform.localEulerAngles = Vector3.zero;
        }
    }
}