using System.Collections.Generic;
using BennyKok.RuntimeDebug.Actions;
using BennyKok.RuntimeDebug.Components.UI;
using UnityEngine;

namespace BennyKok.RuntimeDebug.Data
{
    public class ListItem
    {
        public List<ListItem> children;
        public BaseDebugAction actionTrigger;
        public ListItemView view;
        public ListView uiList;
        public string fullPath;
        public string groupName;
        public ListItem parent;

        public bool IsGroup => children != null && children.Count > 0;
        public string Name => IsGroup ? groupName : actionTrigger.name;

        public ListItem(BaseDebugAction actionTrigger)
        {
            this.actionTrigger = actionTrigger;
        }

        public ListItem() { }

        public int FindChildIndex(string name)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Name == name) return i;
            }

            return 0;
        }
    }
}